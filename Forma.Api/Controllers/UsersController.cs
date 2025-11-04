using Forma.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forma.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly ApplicationDbContext _dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var usersList = await _dbContext.Users.ToListAsync();
            return Ok(usersList);
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest("ID mismatch");

            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Email = updatedUser.Email;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
