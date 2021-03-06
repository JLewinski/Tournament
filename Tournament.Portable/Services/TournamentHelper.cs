﻿using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Portable.Data;
using Tournament.Portable.Models;

namespace Tournament.Portable.Services
{
    /// <summary>
    /// The tournament helper is in charge of handling changes to the tournament.
    /// </summary>
    public static class TournamentHelper
    {
        /// <summary>
        /// Will update the tournament and everything inside it. If the round is over it
        /// will also create and add the new matches for the next round.
        /// </summary>
        /// <param name="tour">
        /// The tournament being updated.
        /// </param>
        /// <param name="db">
        /// The database where the tournament is being updated
        /// </param>
        public static void UpdateTournament(Tournee tour, ITournamentContextBase db = null)
        {
            var teams = new List<Team>();

            tour.Matches = tour.Matches.Where(match => match.Round <= tour.CurrentRound).ToList();

            foreach (var match in tour.Matches)
            {
                foreach (var team in match.Teams)
                {
                    team.IsEliminated = match.WinnerId != null && team.Id != match.WinnerId;
                    if (team.IsEliminated || match.Round == tour.CurrentRound) teams.Add(team);
                }
            }

            tour.Teams = teams;
            db?.UpdateRange(tour.Matches);


            // If all the matches in the current round are finished go to the next round
            if (tour.Matches.All(m => m.WinnerId != null))
            {
                // Go to next round
                tour.CurrentRound++;
                db?.Update(tour);

                NextRound(tour);

                db?.Update(tour);
            }
        }

        /// <summary>
        /// Finish making a new tournament.
        /// </summary>
        /// <param name="tour">
        /// The tour.
        /// </param>
        public static void MakeNewTournament(Models.Tournee tour)
        {
            if (tour.GamesPerMatch < 1) tour.GamesPerMatch = 1;
            if (tour.TeamsPerMatch < 2) tour.TeamsPerMatch = 2;
            if (tour.CurrentRound < 1) tour.CurrentRound = 1;

            tour.Matches = new List<Match>();
            NextRound(tour);
        }

        /// <summary>
        /// Creates and adds the new matches and connections (MatchTeams) to the tournament.
        /// It will also set the tournament to finished if it is finished.
        /// It will not add anything to the database.
        /// </summary>
        /// <param name="tour">The Tournament</param>
        public static void NextRound(Models.Tournee tour)
        {
            var rand = new Random();

            // Get the players that are not out of the tournament
            var plrs = new Stack<Team>(tour.Teams?.Where(player => !player.IsEliminated).OrderBy(p => rand.Next(500)));

            tour.IsFinished = plrs.Count == 1;

            if (tour.IsFinished) return;

            for (var counter = 1; plrs.Any(); counter++)
            {
                // Display name of the new match
                var matchName = counter < 10 ? $"0{counter}" : $"{counter}";

                // Create the new match
                var tempMatch = new Match
                {
                    Id = Guid.NewGuid().ToString(),
                    Tournament = tour,
                    Round = tour.CurrentRound,
                    DisplayName = $"Match {matchName}",
                };

                // players in match
                var tempPlrs = new List<Team>();

                // connections to players
                var tempCons = new List<MatchTeam>();

                // set up players and connections
                for (var i = 0; i < tour.TeamsPerMatch && plrs.Any(); i++)
                {
                    // Get the player
                    var tempP = plrs.Pop();

                    // add player to temp list
                    tempPlrs.Add(tempP);

                    // create new connection and add it to the list
                    tempCons.Add(new MatchTeam()
                    {
                        Team = tempP,
                        Match = tempMatch
                    });
                }

                // Add players and connections
                // tempMatch.Teams = tempPlrs;
                tempMatch.Connections = tempCons;

                // Add match to list to add to tournament
                tour.Matches.Add(tempMatch);

                // TODO: Think about adding the games as well
            }

        }

        // IDK what this is :(
        public static void EnumerateTeams(Models.Tournee tour)
        {
            foreach (var match in tour.Matches)
            {
                // match.Teams = match.Connections.Select(team => team.Team).ToList();
                tour.Teams.AddRange(match.Teams);
            }
        }
    }
}
