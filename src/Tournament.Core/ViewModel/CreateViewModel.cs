﻿using System;
using System.Collections.Generic;
using Tournament.Core.Models;

namespace Tournament.Core.ViewModel
{
    public class CreateViewModel : ICreateViewModel
    {
        public CreateViewModel()
        {
            NumberTeams = 16;
            Tournament = new Models.Tournament
            {
                Id = Guid.NewGuid().ToString(),
                DisplayName = "Tournament Name",
                Teams = new List<Team>(),
                CurrentRound = 1,
                GamesPerMatch = 1,
                TeamsPerMatch = 2
            };

            for (var i = 0; i < 128; i++)
            {
                Tournament.Teams.Add(new Team
                {
                    Name = "Player " + (i + 1),
                    Tournament = Tournament,
                    IsEliminated = false
                });
            }
        }
        public int NumberTeams { get; set; }
        public Models.Tournament Tournament { get; set; }
    }
}