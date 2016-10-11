using System;
using System.Collections.Generic;
using Tournament.Portable.Models;

namespace Tournament.Portable.DesignTimeData
{
    public static class Factory
    {
        private static string[] teamNames = new string[] {"Cool Dudes", "Cool Gals", "Incredible", "Galaxy", "Mr. Chibbs"};

        public static Models.Tournee CreateTournament()
        {
            var tour = new Models.Tournee()
            {
                CurrentRound = 1,
                Description = "Generated Tournament",
                DisplayName = "Generated Tournament",
                GamesPerMatch = 1,
                Id = Guid.NewGuid().ToString(),
                IsFinished = false,
                Matches = new List<Match>(),
                Teams = new List<Team>(),
                TeamsPerMatch = 2,
                User = null,
                UserId = "1234",
                UserName = "Generated NULL"
            };
            return tour;
        }

        public static Team CreateTeam()
        {
            var team = new Team();
            return team;
        }

        public static Match CreateMatch()
        {
            var match = new Match();
            return match;
        }

        public static Game CreateGame()
        {
            var game = new Game();
            return game;
        }
    }
}