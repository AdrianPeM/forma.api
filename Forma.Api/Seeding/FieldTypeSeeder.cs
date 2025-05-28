using Forma.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Forma.Api.Seeding
{
    public static class FieldTypeSeeder
    {
        public static void SeedFieldTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FieldType>().HasData(
                new FieldType { Id = 1, Name = "Text", FieldTypeId = FieldTypeEnum.Text },
                new FieldType { Id = 2, Name = "Paragraph", FieldTypeId = FieldTypeEnum.Paragraph },
                new FieldType { Id = 3, Name = "Number", FieldTypeId = FieldTypeEnum.Number },
                new FieldType { Id = 4, Name = "Checkbox", FieldTypeId = FieldTypeEnum.Checkbox },
                new FieldType { Id = 5, Name = "Toggle", FieldTypeId = FieldTypeEnum.Toggle },
                new FieldType { Id = 6, Name = "Dropdown", FieldTypeId = FieldTypeEnum.Dropdown },
                new FieldType { Id = 7, Name = "Date", FieldTypeId = FieldTypeEnum.Date }
            );
        }
    }
}
