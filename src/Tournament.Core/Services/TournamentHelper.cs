using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Core.Data;
using Tournament.Core.Models;

namespace Tournament.Core.Services
{
    public static class TournamentHelper
    {
        /// <summary>
        /// Will update the tournament and everything inside it. If the round is over it
        /// will also create and add the new matches for the next round.
        /// </summary>
        /// <param name="tour"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static void UpdateTournament(Models.Tournament tour, ITournamentContextBase db = null)
        {
            var teams = new List<Team>();

            foreach (var match in tour.Matches)
            {
                if (match.Round > tour.CurrentRound)
                {
                    tour.Matches.Remove(match);
                }
                else
                {
                    foreach (var team in match.Teams)
                    {
                        team.IsEliminated = (match.WinnerId != null && team.Id != match.WinnerId);
                        if (team.IsEliminated || match.Round == tour.CurrentRound) teams.Add(team);
                    }
                }
            }
            tour.Teams = teams;
            db?.UpdateRange(tour.Matches);
            
            
            //If all the matches in the current round are finished go to the next round
            if (tour.Matches.All(m => m.WinnerId != null))
            {
                //Go to next round
                tour.CurrentRound++;
                db?.Update(tour);

                NextRound(tour);

                db?.Update(tour);
            }
        }

        public static void MakeNewTournament(Models.Tournament tour)
        {
            tour.Matches = new List<Match>();
            NextRound(tour);
        }

        /// <summary>
        /// Creates and adds the new matches and connections (MatchTeams) to the tournament.
        /// It will also set the tournament to finished if it is finished.
        /// It will not add anything to the database.
        /// </summary>
        /// <param name="tour"></param>
        public static void NextRound(Models.Tournament tour)
        {
            var rand = new Random();

            //Get the players that are not out of the tournament
            var plrs = new Stack<Team>(tour.Teams?.Where(player => !player.IsEliminated).OrderBy(p => rand.Next(500)));

            tour.IsFinished = plrs.Count == 1;

            if (tour.IsFinished) return;

            for (var counter = 1; plrs.Any(); counter++)
            {
                //Display name of the new match
                var mname = counter < 10 ? $"0{counter}" : $"{counter}";

                //Create the new match
                var tempMatch = new Match
                {
                    Id = Guid.NewGuid().ToString(),
                    Tournament = tour,
                    Round = tour.CurrentRound,
                    DisplayName = $"Match {mname}",
                };

                //players in match
                var tempPlrs = new List<Team>();

                //connections to players
                var tempCons = new List<MatchTeam>();

                //set up players and connections
                for (var i = 0; i < tour.TeamsPerMatch && plrs.Any(); i++)
                {
                    //Get the player
                    var tempP = plrs.Pop();

                    //add player to temp list
                    tempPlrs.Add(tempP);

                    //create new connection and add it to the list
                    tempCons.Add(new MatchTeam()
                    {
                        Team = tempP,
                        Match = tempMatch
                    });
                }

                //Add players and connections
                //tempMatch.Teams = tempPlrs;
                tempMatch.Connections = tempCons;

                //Add match to list to add to tournament
                tour.Matches.Add(tempMatch);
                //TODO: Think about adding the games as well
            }
            
        }

        public static void EnumerateTeams(Models.Tournament tour)
        {
            foreach (var match in tour.Matches)
            {
                //match.Teams = match.Connections.Select(team => team.Team).ToList();
                tour.Teams.AddRange(match.Teams);
            }
        }
    }
}
