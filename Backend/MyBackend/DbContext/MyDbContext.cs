using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBackend.Entities;

namespace MyBackend.DbContext
{
    public class MyDbContext : IdentityDbContext<User> //Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Gm> Gms { get; set; }
        public DbSet<GameSystem> Systems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MyBackend.db");
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>()
                .HasMany(lol => lol.Players)
                .WithMany(kek => kek.Sessions)
                .UsingEntity(bur => bur.HasData(new[]
                {
                new
                {
                    SessionsId = "1",
                    PlayersId = "1",
                },
                new
                {
                    SessionsId = "1",
                    PlayersId = "2",
                },
                new
                {
                    SessionsId = "2",
                    PlayersId = "2",
                },
                new
                {
                    SessionsId = "2",
                    PlayersId = "3",
                },
                new
                {
                    SessionsId = "3",
                    PlayersId = "3",
                },
                new
                {
                    SessionsId = "3",
                    PlayersId = "4",
                },
                new
                {
                    SessionsId = "4",
                    PlayersId = "1",
                },
                new
                {
                    SessionsId = "4",
                    PlayersId = "2",
                },
                new
                {
                    SessionsId = "4",
                    PlayersId = "3",
                },
                new
                {
                    SessionsId = "4",
                    PlayersId = "4",
                }

                }));

            modelBuilder.Entity<Gm>()
                .HasData(
                new Gm
                {
                    Id = "1",
                    Name = "Gm1"
                },
                new Gm
                {
                    Id = "2",
                    Name = "Gm2"
                });

            modelBuilder.Entity<Player>()
                .HasData(
                new Player
                {
                    Id = "1",
                    Name = "Player1"
                },
                new Player
                {
                    Id = "2",
                    Name = "Player2"
                },
                new Player
                {
                    Id = "3",
                    Name = "Player3"
                },
                new Player
                {
                    Id = "4",
                    Name = "Player4"
                });
            modelBuilder.Entity<GameSystem>()
                .HasData(
                new GameSystem()
                {
                    Id = "1",
                    Name = "d&d 5e"
                },
                new GameSystem()
                {
                    Id = "2",
                    Name = "Veasen"
                },
                new GameSystem()
                {
                    Id = "3",
                    Name = "Things from the Flood"
                },
                new GameSystem()
                {
                    Id = "4",
                    Name = "DC20"
                });
            modelBuilder.Entity<Session>()
                .HasData(
                new Session()
                {
                    Id = "1",
                    GmId = "1",
                    SystemId = "1",
                    Title = "title1",
                    Tags = "tag1, tag2, tag3",
                    Description = "Description",
                    MaxNumberOfPlayers = 4,
                },
                new Session()
                {
                    Id = "2",
                    GmId = "1",
                    SystemId = "2",
                    Title = "title2",
                    Tags = "tag1, tag2, tag3",
                    Description = "Description",
                    MaxNumberOfPlayers = 4,
                },
                new Session()
                {
                    Id = "3",
                    GmId = "2",
                    SystemId = "3",
                    Title = "title3",
                    Tags = "tag1, tag2, tag3",
                    Description = "Description",
                    MaxNumberOfPlayers = 4,
                },
                new Session()
                {
                    Id = "4",
                    GmId = "2",
                    SystemId = "4",
                    Title = "title4",
                    Tags = "tag2, tag3",
                    Description = "Description",
                    MaxNumberOfPlayers = 6,
                });
            
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
