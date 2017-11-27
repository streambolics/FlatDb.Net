using Microsoft.EntityFrameworkCore;

namespace FlatDatabase.Models
{
    public class FlatDbContext : DbContext
    {
        public FlatDbContext(DbContextOptions<FlatDbContext> options)
        : base(options)
        {

        }

        public FlatDbContext()
        : base()
        {

        }

        public DbSet<ItemModel> Item { get; set; }

    }
}