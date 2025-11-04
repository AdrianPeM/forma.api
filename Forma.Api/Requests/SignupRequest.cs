using System.ComponentModel.DataAnnotations;

namespace Forma.Api.Requests
{
    public class SignupRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;
        [Required, MinLength(6)]
        public string Password { get; set; } = default!;
        [Required]
        public string FirstName { get; set; } = default!;
        public string? LastName { get; set; }
    }
}
