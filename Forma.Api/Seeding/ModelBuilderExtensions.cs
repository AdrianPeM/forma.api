using Microsoft.EntityFrameworkCore;

namespace Forma.Api.Seeding
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedFieldTypes();
        }
    }
}
