using Microsoft.EntityFrameworkCore;

namespace ProductsAndCatagories.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Associations> Associations { get; set; }
    }
}