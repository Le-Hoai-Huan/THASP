using Microsoft.EntityFrameworkCore;

namespace WebBanHangRestApi.model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
