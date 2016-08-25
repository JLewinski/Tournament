using System.Collections.Generic;

namespace Tournament.Portable.Models
{
    public class Team : IModel
    {
        //private ICollection<Match> _matches;
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsEliminated { get; set; }

        public Tournament Tournament { get; set; }

        public ICollection<Game> Games { get; set; }
        //public ICollection<MatchTeam> Connections { get; set; }

        //public ICollection<Match> Matches => _matches ?? (_matches = Connections.Select(team => team.Match).ToList());

        public ICollection<Player> Players { get; set; }
    }
}