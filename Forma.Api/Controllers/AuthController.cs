using System.Diagnostics;
using Forma.Api.Models;
using Forma.Api.Requests;
using Forma.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Forma.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
                return ErrorResponse("email", "email_already_exists", Conflict);

            var hashedPassword = PasswordHasher.Hash(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = hashedPassword,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Signup), new { user.Id }, new { user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !PasswordHasher.Verify(request.Password, user.Password))
                return ErrorResponse("email", "invalid_credentials");

            // Replace with JWT token later
            return Ok(new { user.Id, user.Email, user.FirstName });
        }
    }
}
