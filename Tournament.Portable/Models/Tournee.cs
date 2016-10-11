using System.Collections.Generic;

namespace Tournament.Portable.Models
{
    public class Tournee
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public int CurrentRound { get; set; }

        public bool IsFinished { get; set; }

        public int TeamsPerMatch { get; set; }

        public int GamesPerMatch { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        private IUser user;

        public IUser User
        {
            get
            {
                return this.user;
            }

            set
            {
                this.user = value;
                UserId = value?.Id;
                UserName = value?.UserName;
            }
        }

        public List<Team> Teams { get; set; }

        public List<Match> Matches { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}