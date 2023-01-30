using Microsoft.EntityFrameworkCore;
using Places.Models;

namespace Places
{
    public class PlacesContext:DbContext 
    {
        public PlacesContext(DbContextOptions<PlacesContext> options):base(options)
        {

        }
        public DbSet<UserProfile> UserProfile { get; set; }
    }
}
