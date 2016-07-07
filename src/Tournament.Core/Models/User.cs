using System.Collections.Generic;

namespace Tournament.Core.Models
{
    public class User : IUser
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public ICollection<Tournament> Tournaments { get; set; }
    }
}