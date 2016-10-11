using System.Collections.Generic;
using System.Linq;

namespace Tournament.Portable.Models
{
    public class Match : IModel
    {
        private IList<Team> teams;

        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public ICollection<MatchTeam> Connections { get; set; }

        public IList<Team> Teams
        {
            get
            {
                return this.teams ?? (this.teams = this.Connections?.Select(connection => connection.Team).ToList());
            }

            set
            {
                this.teams = value;
            }
        }

        public ICollection<Game> Games { get; set; }

        public Tournee Tournament { get; set; }

        public int Round { get; set; }

        public string WinnerId { get; set; }
    }
}