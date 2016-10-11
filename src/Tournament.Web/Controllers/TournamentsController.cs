using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Core.v3;
using Tournament.Web.Data;
using Tournament.Web.Models;

namespace Tournament.Web.Controllers
{
    using Tournament.Portable.Models;

    [Produces("application/json")]
    public class TournamentsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public TournamentsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        [HttpGet]
        public IEnumerable<Tournee> GetAll()
        {
            return this.context.Tournaments.ToList();
        }

        [HttpGet]
        public string Get(string id)
        {
            var tournament =
                this.context.Tournaments
                    .Include(tournament1 => tournament1.Matches)
                    .ThenInclude(match => match.Connections)
                    .ThenInclude(team => team.Team)
                    .Single(tournament1 => tournament1.Id == id);

            // TODO: Check if Match has multiple copies of the same teams in Teams
            return tournament.ToJson();
        }

        [HttpGet]
        public IEnumerable<Tournee> GetFromUser(string userId)
        {
            return this.context.Tournaments.Where(tournament => tournament.UserId == userId).ToList();
        }

        [HttpPost]
        [AcceptVerbs("application/json")]
        public async Task<IActionResult> Create([FromBody]Tournee tournament)
        {
            // Make sure the tournament is passed by ref
            var formattedResult = await FormatTournament(tournament);

            if (formattedResult != null) return formattedResult;

            try
            {
                this.context.Tournaments.Add(tournament);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return new CreatedAtActionResult("GetById", "Tournaments", new { id = tournament.Id }, tournament);
        }

        [HttpPut]
        [AcceptVerbs("application/json")]
        public async Task<IActionResult> Update([FromBody]Tournee tournament)
        {
            // TODO: Test to see if tournament needs to be formatted.
            var formattedResult = await FormatTournament(tournament);

            if (formattedResult != null) return formattedResult;

            try
            {
                this.context.Update(tournament); // TODO: see if I need to update Matches and Teams as well
                this.context.SaveChanges();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return new NoContentResult();
        }

        [HttpDelete]
        [AcceptVerbs("application/json")]
        public async void Delete([FromBody]Tournee tournament)
        {
            if (await IsTournamentValid(tournament))
            {
                tournament = this.context.Tournaments.Single(t => t.Id == tournament.Id);
                this.context.Remove(tournament);
                this.context.SaveChanges();
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return this.signInManager.IsSignedIn(HttpContext.User) ? this.userManager.GetUserAsync(HttpContext.User) : Task.FromResult<ApplicationUser>(null);
        }

        /// <summary>
        /// Returns whether or not the tournament is valid and is owned by the current user.
        /// </summary>
        /// <param name="tour">Tournament to be deleted</param>
        /// <returns>Success or not</returns>
        private async Task<bool> IsTournamentValid([FromBody]Tournee tour)
        {
            if (!ModelState.IsValid) return false;
            try
            {
                tour.User =
                    this.context.Tournaments.Where(t => t.Id == tour.Id).Select(t => t.User).Single();
            }
            catch (Exception)
            {
                return false;
            }

            return tour.UserId == null || tour.UserId == (await GetCurrentUserAsync())?.Id;
        }

        private async Task<IActionResult> FormatTournament(Tournee tournament)
        {
            if (tournament == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            tournament.User = await GetCurrentUserAsync();
            foreach (var match in tournament.Matches)
            {
                if (match.Teams.Count == match.Connections.Count) continue;

                // ERROR! TODO: Figure out how to pass tournament without screwing up Match.Teams and Match.Connections
                match.Teams = null;
                foreach (var matchConnection in match.Connections)
                {
                    matchConnection.TeamId = matchConnection.Team.Id;
                    matchConnection.Team = null;
                    matchConnection.MatchId = match.Id;
                }
            }

            return null;
        }
    }
}