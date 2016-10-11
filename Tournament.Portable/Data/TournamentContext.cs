namespace Tournament.Portable.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class TournamentContext : DbContext, ITournamentContextBase
    {
        public TournamentContext(DbContextOptions<TournamentContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DataHelper.CreateTournamentModels(modelBuilder);
            modelBuilder.Entity<Tournee>().Ignore(t => t.UserName);
            modelBuilder.Entity<Tournee>().Ignore(t => t.UserId);
            modelBuilder.Entity<Tournee>().Ignore(t => t.User);
        }

        public DbSet<Tournee> Tournaments { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<MatchTeam> MatchTeams { get; set; }
    }
}