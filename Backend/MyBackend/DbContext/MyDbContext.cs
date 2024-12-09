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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Trigger> Triggers { get; set; }

        public DbSet<Vtt> Vtts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MyBackend.db");
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>()
                .HasMany(lol => lol.Players)
                .WithMany(kek => kek.Sessions);
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
