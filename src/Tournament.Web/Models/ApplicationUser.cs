using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Tournament.Portable.Models;

namespace Tournament.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IUser
    {
        public ICollection<Portable.Models.Tournee> Tournaments { get; set; }
        public string Description { get; set; }
    }
}
