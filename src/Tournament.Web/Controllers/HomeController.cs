using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.ViewModel;
using Tournament.Web.Data;
using Tournament.Web.Models;
using static Tournament.Core.Services.TournamentHelper;

namespace Tournament.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _db = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            //Gets all the tournaments
            var tournaments = _db.Tournaments
                .OrderByDescending(t => t.CurrentRound)
                .ToList();
            return View(tournaments);
        }

        public async Task<IActionResult> MyTournaments()
        {
            var userId = (await GetCurrentUserAsync())?.Id;

            //gets all the tournaments connected to the user
            var tournaments = _db.Tournaments
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CurrentRound)
                .ToList();

            return View(nameof(Index), tournaments);
        }

        public IActionResult Create() => View(new CreateViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel vm)
        {
            
            if (ModelState.IsValid)
            {
                //The page uploaded hidden teams which were connected to the tournament so this removes them.
                if(vm.NumberTeams < vm.Tournament.Teams.Count)
                    vm.Tournament.Teams.RemoveRange(vm.NumberTeams, vm.Tournament.Teams.Count - vm.NumberTeams);

                //Connects the user to the tournament
                vm.Tournament.User = await GetCurrentUserAsync();

                foreach (var team in vm.Tournament.Teams)
                {
                    team.Id = Guid.NewGuid().ToString();
                }

                MakeNewTournament(vm.Tournament);
                _db.Add(vm.Tournament);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            PrintErrors();

            return View(vm);
        }

        public IActionResult Edit(string id) => View(_db.Tournaments.Single(t => t.Id == id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Core.Models.Tournament tour)
        {
            if (await IsTournamentValid(tour))
            {
                _db.Update(tour);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            PrintErrors();
            return View(tour);
        }

        public IActionResult Delete(string id) => View(_db.Tournaments.Single(t => t.Id == id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Core.Models.Tournament tour)
        {
            if (await IsTournamentValid(tour))
            {
                _db.Remove(tour);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            //If it gets here something went wrong
            return View(tour);
        }

        public IActionResult Round(string id, int round = 0)
        {
            var tour =
                _db.Tournaments.Include(tournament => tournament.Matches)
                    .ThenInclude(match => match.Connections)
                    .ThenInclude(team => team.Team)
                    .Single(t => t.Id == id);

            if (round == 0) round = tour.CurrentRound;

            tour.Matches = tour.Matches.OrderBy(match => match.DisplayName).ToList();

            return View(new RoundViewModel(tour, round));
        }

        [HttpPost]
        public async Task<IActionResult> Round(RoundViewModel vm)
        {
            var tour = vm.Tournament;

            if (!await IsTournamentValid(tour)) return View(vm);

            UpdateTournament(tour, _db);

            //Check to see if tour has the same matches or not
            _db.SaveChanges();

            if (tour.IsFinished)
            {
                //Go to finished page
            }

            return View(new RoundViewModel(tour));
        }

        public IActionResult Error()
        {
            return View();
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
                    _db.Tournaments.Where(t => t.Id == tour.Id).Select(t => t.User).Single();
            }
            catch (Exception)
            {
                return false;
            }
            return tour.UserId == null || tour.UserId == (await GetCurrentUserAsync())?.Id;
        }

        private void PrintErrors()
        {
            var errors = ModelState.Keys.SelectMany(key => ModelState[key].Errors);
            Console.Out.WriteLine("Error: ");
            foreach (var e in errors)
            {
                Console.Out.WriteLine(e.ErrorMessage);
            }
        }
    }
}
