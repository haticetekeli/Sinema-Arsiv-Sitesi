using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SinemaArsivSitesi.Data.DbContext;

namespace SinemaArsivSitesi.Data
{
    public static class ServiceRegistration
    {
        public static void DataAccessRegistration(this IServiceCollection services)
        {
            var connectionString = "Data Source=SinemaArsivSitesi.db";

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(connectionString);
                options.EnableSensitiveDataLogging();
            });
        }
    }
}
