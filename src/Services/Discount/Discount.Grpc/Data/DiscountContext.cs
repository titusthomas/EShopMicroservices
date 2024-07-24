using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext:DbContext
    {
        public DiscountContext(DbContextOptions<DiscountContext> options):base(options) 
        {
            
        }
        public DbSet<Coupon> Coupons { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id=1,ProductName="Iphone X",Description="Iphone description",Amount=500},
                new Coupon { Id = 2, ProductName = "Samsung 10", Description = "Samsung 10 description", Amount = 450 }
                );
            //base.OnModelCreating(modelBuilder);
        }
    }
}
