using Forma.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forma.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public FieldTypesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<FieldTypesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FieldType>>> Get()
        {
            var fieldTypesList = await _dbContext.FieldTypes.ToListAsync();
            return fieldTypesList;
        }

        // GET api/<FieldTypesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FieldType>> Get(int id)
        {
            var fieldTypeItem = await _dbContext.FieldTypes.FindAsync(id);
            
            if (fieldTypeItem == null)
                return NotFound();

            return fieldTypeItem;
        }
    }
}
