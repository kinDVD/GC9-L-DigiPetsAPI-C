using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAPI.Models;

namespace PetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DigiPetsController : ControllerBase
    {
        DigiPetsDbContext dbContext = new DigiPetsDbContext();

        [HttpGet()]
        public IActionResult GetAll()
        {
            List<DigiPet> result = dbContext.DigiPets.ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            DigiPet result = dbContext.DigiPets.FirstOrDefault(d => d.Id == id);
            if (result == null)
            {
                return NotFound("This is not pets you're looking for.");
            }
            return Ok(result);
        }
    }
}
