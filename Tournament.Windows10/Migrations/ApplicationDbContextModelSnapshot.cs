using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Tournament.Windows10.Data;

namespace Tournament.Windows10.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Tournament.Portable.Models.Game", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Description");

                    b.Property<string>("MatchId");

                    b.Property<string>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("TeamId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Tournament.Portable.Models.Match", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Description");

                    b.Property<string>("DisplayName");

                    b.Property<int>("Round");

                    b.Property<string>("TournamentId");

                    b.Property<string>("WinnerId");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Tournament.Portable.Models.MatchTeam", b =>
                {
                    b.Property<string>("MatchId");

                    b.Property<string>("TeamId");

                    b.HasKey("MatchId", "TeamId");

                    b.HasIndex("MatchId");

                    b.HasIndex("TeamId");

                    b.ToTable("MatchTeams");
                });

            modelBuilder.Entity("Tournament.Portable.Models.Player", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Tournament.Portable.Models.Team", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Description");

                    b.Property<bool>("IsEliminated");

                    b.Property<string>("Name");

                    b.Property<string>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Tournament.Portable.Models.Tournament", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("CurrentRound");

                    b.Property<string>("Description");

                    b.Property<string>("DisplayName");

                    b.Property<int>("GamesPerMatch");

                    b.Property<bool>("IsFinished");

                    b.Property<int>("TeamsPerMatch");

                    b.HasKey("Id");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("Tournament.Portable.Models.Game", b =>
                {
                    b.HasOne("Tournament.Portable.Models.Match", "Match")
                        .WithMany("Games")
                        .HasForeignKey("MatchId");

                    b.HasOne("Tournament.Portable.Models.Team", "Team")
                        .WithMany("Games")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tournament.Portable.Models.Match", b =>
                {
                    b.HasOne("Tournament.Portable.Models.Tournament", "Tournament")
                        .WithMany("Matches")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tournament.Portable.Models.MatchTeam", b =>
                {
                    b.HasOne("Tournament.Portable.Models.Match", "Match")
                        .WithMany("Connections")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tournament.Portable.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("Tournament.Portable.Models.Player", b =>
                {
                    b.HasOne("Tournament.Portable.Models.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("Tournament.Portable.Models.Team", b =>
                {
                    b.HasOne("Tournament.Portable.Models.Tournament", "Tournament")
                        .WithMany("Teams")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
