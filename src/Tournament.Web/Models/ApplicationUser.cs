using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Tournament.Core.Models;

namespace Tournament.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IUser
    {
        public ICollection<Core.Models.Tournament> Tournaments { get; set; }
        public string Description { get; set; }
    }
}
