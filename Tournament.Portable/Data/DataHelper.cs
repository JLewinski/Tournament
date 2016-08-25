using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Tournament.Portable.Models;

namespace Tournament.Portable.Data
{
    public static class DataHelper
    {
        public static void CreateTournamentModels(ModelBuilder builder)
        {
            //Many - Many relationships use an extra class to hold the Connection
            /*builder.Entity<Team>().Ignore(team => team.Matches);*/
            builder.Entity<Match>().Ignore(match => match.Teams);
            builder.Entity<MatchTeam>().HasKey(team => new { team.MatchId, team.TeamId });

            builder.Entity<Models.Tournament>()
                .HasMany(t => t.Teams)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Team>()
                .HasOne(team => team.Tournament)
                .WithMany(tournament => tournament.Teams)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Match>()
                .HasOne(match => match.Tournament)
                .WithMany(tournament => tournament.Matches)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MatchTeam>()
                .HasOne(matchTeam => matchTeam.Match)
                .WithMany(match => match.Connections)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MatchTeam>()
                .HasOne(con => con.Team)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Game>()
                .HasOne(game => game.Team)
                .WithMany(team => team.Games)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
