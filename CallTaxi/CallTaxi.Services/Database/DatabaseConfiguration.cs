using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CallTaxi.Services.Database
{
    public static class DatabaseConfiguration
    {
        public static void AddDatabaseServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CallTaxiDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        public static void AddDatabaseCallTaxi(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CallTaxiDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}