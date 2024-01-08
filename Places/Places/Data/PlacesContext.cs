using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Places.Models;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Places.Data
{
    public class PlacesContext : DbContext
    {
        public PlacesContext(DbContextOptions<PlacesContext> options) : base(options)
        {

        }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Models.Connection> Connections { get; set; }
        public DbSet<Event> Events { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Location>()
                .HasMany<UserProfile>(l => l.UserProfiles)
                .WithOne(up => up.UserLocation)
                .HasForeignKey(s => s.CurrentLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Location>()
               .HasMany<Event>(l => l.Events)
               .WithOne(el => el.EventLocation)
               .HasForeignKey(s => s.EventLocationId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Places.Models.Connection>()
                .Ignore(c => c.Receiver);

            modelBuilder.Entity<Places.Models.Connection>()
                .HasOne<UserProfile>(s => s.Sender)
                .WithMany(g => g.Connections)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);



        }
    }
}
