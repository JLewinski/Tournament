using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Tournament.Portable.Data;
using Tournament.Portable.Models;
using Tournament.Web.Models;

namespace Tournament.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, ITournamentContextBase
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            DataHelper.CreateTournamentModels(builder);
            builder.Entity<Portable.Models.Tournament>()
                .HasOne(t => (ApplicationUser) t.User)
                .WithMany(u => u.Tournaments)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Portable.Models.Tournament> Tournaments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<MatchTeam> MatchTeams { get; set; }
    }
}
