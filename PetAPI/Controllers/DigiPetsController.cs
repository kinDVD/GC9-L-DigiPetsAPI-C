using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpPost()]
        public IActionResult AddPet([FromBody] DigiPet newPet)
        {
            newPet.Id = 0;
            dbContext.DigiPets.Add(newPet);
            dbContext.SaveChanges();
            return Created($"/api/DigiPets/{newPet.Id}", newPet);
        }

        [HttpDelete()]
        public IActionResult DeletePet(int id)
        {
            DigiPet result = dbContext.DigiPets.FirstOrDefault(d => d.Id == id);
            if(result == null) { return NotFound("This is not the pet you're looking for."); }
            else { dbContext.DigiPets.Remove(result); dbContext.SaveChanges(); return NoContent(); }
        }

        [HttpPut("{id}/Heal")]
        public IActionResult HealPet(int id)
        {
            DigiPet result = dbContext.DigiPets.FirstOrDefault(d => d.Id == id);
            
            if (result.Id != id) { return BadRequest("IDs don't match"); }
            if (dbContext.DigiPets.Any(d => d.Id == id) == false) { return NotFound("No matching IDs"); }
            
            Random rnd = new Random();
            decimal health = new decimal(rnd.NextDouble() * (0.3 - 0.1) + 0.1);
            
            result.Health += health;
            if (result.Health > 1)
            {
                result.Health = 1;
            }
            
            dbContext.DigiPets.Update(result);
            dbContext.SaveChanges();
            return Ok(result);
        }

        [HttpPut("{id}/Train")]
        public IActionResult TrainPet(int id)
        {
            DigiPet result = dbContext.DigiPets.FirstOrDefault(d => d.Id == id);

            if (result.Id != id) { return BadRequest("IDs don't match"); }
            if (dbContext.DigiPets.Any(d => d.Id == id) == false) { return NotFound("No matching IDs"); }

            Random rnd = new Random();
            int strength = rnd.Next(1, 4);

            result.Strength += strength;

            dbContext.DigiPets.Update(result);
            dbContext.SaveChanges();
            return Ok(result);

        }

        [HttpPut("{id}/Battle")]
        public IActionResult Battle(int id, int opponentId)
        {
            bool win = false;
            DigiPet userPet = dbContext.DigiPets.FirstOrDefault(p => p.Id == id);
            DigiPet opponentPet = dbContext.DigiPets.FirstOrDefault(p => p.Id == opponentId);

            Random rnd = new Random();

            decimal? userAttackPower = userPet.Health * userPet.Strength * userPet.Experience * (new decimal (rnd.NextDouble()));
            decimal? opponentAttackPower = opponentPet.Health * opponentPet.Strength * opponentPet.Experience * (new decimal(rnd.NextDouble()));

            if (userAttackPower > opponentAttackPower)
            {
                win = true;
            }

            decimal? damage = userAttackPower + opponentAttackPower;
            decimal? userDamagePercentage = userAttackPower / damage;
            decimal? opponentDamangePercentage = opponentAttackPower / damage;

            userPet.Health = userPet.Health * userDamagePercentage;
            opponentPet.Health = opponentPet.Health * opponentDamangePercentage;



        }
    }
}
