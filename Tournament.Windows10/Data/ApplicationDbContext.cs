using Microsoft.EntityFrameworkCore;
using Tournament.Portable.Data;

namespace Tournament.Windows10.Data
{
    public class ApplicationDbContext : TournamentContext
    {
        public ApplicationDbContext() : base(new DbContextOptions<TournamentContext>())
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("FileName=Tournament.db");
        }
    }
}