using System.Collections.Generic;

namespace Tournament.Portable.Models
{
    public class User : IUser
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public ICollection<Tournee> Tournaments { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}