using System.ComponentModel.DataAnnotations;
using Forma.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forma.Api.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : ITimestampedEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required, MinLength(6)]
        public required string Password { get; set; }
        [Required]
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}