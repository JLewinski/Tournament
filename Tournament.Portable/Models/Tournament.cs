using System.Collections.Generic;

namespace Tournament.Portable.Models
{
    public class Tournament
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

        private IUser _user;
        public IUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                UserId = value?.Id;
                UserName = value?.UserName;
            }
        }

        public List<Team> Teams { get; set; }
        public ICollection<Match> Matches { get; set; }
    }
}