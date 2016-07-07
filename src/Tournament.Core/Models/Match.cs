using System.Collections.Generic;
using System.Linq;

namespace Tournament.Core.Models
{
    public class Match : IModel
    {
        private IList<Team> _teams;
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ICollection<MatchTeam> Connections { get; set; }

        public IList<Team> Teams
        {
            get { return _teams ?? (_teams = Connections?.Select(team => team.Team).ToList()); }
            set { _teams = value; }
        }

        public ICollection<Game> Games { get; set; }
        public Tournament Tournament { get; set; }
        public int Round { get; set; }
        public string WinnerId { get; set; }
    }
}