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
    [Produces("application/json")]
    public class TournamentsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TournamentsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

        }

        [HttpGet]
        public IEnumerable<Core.Models.Tournament> GetAll()
        {
            return _dbContext.Tournaments.ToList();
        }

        [HttpGet]
        public string Get(string id)
        {
            var tournament =
                _dbContext.Tournaments
                    .Include(tournament1 => tournament1.Matches)
                    .ThenInclude(match => match.Connections)
                    .ThenInclude(team => team.Team)
                    .Single(tournament1 => tournament1.Id == id);

            return tournament.ToJson();
        }

        [HttpGet]
        public IEnumerable<Core.Models.Tournament> GetFromUser(string userId)
        {
            return _dbContext.Tournaments.Where(tournament => tournament.UserId == userId).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Core.Models.Tournament tournament)
        {
            if (tournament == null)
            {
                return BadRequest();
            }

            tournament.User = await GetCurrentUserAsync();

            try
            {
                _dbContext.Tournaments.Add(tournament);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return new CreatedAtActionResult("GetById", "Tournaments", new { id = tournament.Id }, tournament);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]Core.Models.Tournament tournament)
        {
            if (!await IsTournamentValid(tournament)) return BadRequest();


            try
            {
                _dbContext.Update(tournament); //TODO: see if I need to update Matches and Teams as well
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return new NoContentResult();
        }

        [HttpDelete]
        public async void Delete(Core.Models.Tournament tournament)
        {
            if (await IsTournamentValid(tournament))
            {
                _dbContext.Remove(tournament); //TODO: see if I need to update Matches and Teams as well
                _dbContext.SaveChanges();
            }
        }


        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _signInManager.IsSignedIn(HttpContext.User) ? _userManager.GetUserAsync(HttpContext.User) : Task.FromResult<ApplicationUser>(null);
        }

        /// <summary>
        /// Returns whether or not the tournament is valid and is owned by the current user.
        /// </summary>
        /// <param name="tour"></param>
        /// <returns></returns>
        private async Task<bool> IsTournamentValid(Core.Models.Tournament tour)
        {
            if (!ModelState.IsValid) return false;
            try
            {
                tour.User =
                    _dbContext.Tournaments.Where(t => t.Id == tour.Id).Select(t => t.User).Single();
            }
            catch (Exception)
            {
                return false;
            }
            return tour.UserId == null || tour.UserId == (await GetCurrentUserAsync())?.Id;
        }
    }
}