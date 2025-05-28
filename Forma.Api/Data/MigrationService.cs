using Microsoft.EntityFrameworkCore;

namespace Forma.Api.Data
{
    public class MigrationService
    {
        public static void InitializeMigration(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            serviceScope.ServiceProvider.GetService<ApplicationDbContext>()!.Database.Migrate();
        }
    }
}
