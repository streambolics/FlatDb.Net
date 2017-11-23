using Microsoft.EntityFrameworkCore;

namespace FlatDatabase.Models
{
    public class FlatDbContext : DbContext
    {
        public FlatDbContext(DbContextOptions<FlatDbContext> options)
        : base(options)
        {

        }

        public DbSet<FlatDatabase.Models.Item> Item { get; set; }

    }
}