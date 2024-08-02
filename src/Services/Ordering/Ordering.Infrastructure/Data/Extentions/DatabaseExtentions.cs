using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data.Extentions;

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class DatabaseExtentions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            var scope=app.Services.CreateScope();
            var context=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedAsync(context);
        }
        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedCustomer(context);
            await SeedProductAsync(context);
            await SeedOrdersWithItemsAsync(context);
        }
        private static async Task SeedCustomer(ApplicationDbContext context)
        {
            if (!await context.Customers.AnyAsync()) 
            { 
                await context.Customers.AddRangeAsync(InitialData.Customers);
                await context.SaveChangesAsync();
            }

        }
        private static async Task SeedProductAsync(ApplicationDbContext context)
        {
            if (!await context.Products.AnyAsync()) 
            { 
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }

        }
        private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
        {
            if (!await context.Orders.AnyAsync()) 
            { 
                await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
                await context.SaveChangesAsync();
            }

        }
    }
}
