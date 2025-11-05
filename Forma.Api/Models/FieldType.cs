using Microsoft.EntityFrameworkCore;

namespace Forma.Api.Models
{
    public enum FieldTypeEnum
    {
        Text = 1,
        Paragraph = 2,
        Number = 3,
        Checkbox = 4,
        Toggle = 5,
        Dropdown = 6,
        Date = 7
    }
    
    [Index(nameof(Name), IsUnique = true)]
    public class FieldType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required FieldTypeEnum FieldTypeId { get; set; }
    }
}
