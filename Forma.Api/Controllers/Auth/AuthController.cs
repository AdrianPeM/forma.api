using Forma.Api.Constants;
using Forma.Api.Models;
using Forma.Api.Requests;
using Forma.Api.Responses;
using Forma.Api.Services;
using Forma.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forma.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext dbContext, JwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
                return ErrorResponse("email", ErrorCodes.AlreadyExists, Conflict);

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

            return CreatedAtAction(nameof(Signup), new { user.Id }, new SignupResponse { Id = user.Id });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _jwtService.Authenticate(request);

            if (result is null)
                return Unauthorized();

            return Ok(result);
        }
    }
}
