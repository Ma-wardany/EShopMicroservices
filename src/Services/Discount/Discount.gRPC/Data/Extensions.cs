using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Data
{
    public static class Extensions
    {
        public static IApplicationBuilder useMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();

            dbContext.Database.MigrateAsync();

            return app;
        }
    }
}
