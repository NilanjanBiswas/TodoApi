using Microsoft.EntityFrameworkCore;
using TodoApi.DatabaseContext;

namespace TodoApi.Extensions
{
    public static class DatabaseServiceExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var conn = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing DefaultConnection");


            services.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseSqlServer(conn));


            // Note: do NOT run migrations here - we will run them on startup in SeedDatabaseAsync to allow scoped services.


            return services;
        }


        /// <summary>
        /// Run this during app startup to apply migrations and seed demo data.
        /// Example usage in Program.cs: await app.Services.SeedDatabaseAsync();
        /// </summary>
        public static async Task SeedDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var scoped = scope.ServiceProvider;


            var db = scoped.GetRequiredService<ApplicationDbContext>();


            // apply pending migrations
            await db.Database.MigrateAsync();


            // run seeding logic
            var seeder = new DbSeeder(db);
            await seeder.SeedAsync();
        }
    }
}

