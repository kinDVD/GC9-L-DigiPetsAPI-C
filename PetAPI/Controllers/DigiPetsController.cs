using Microsoft.AspNetCore.Mvc;
using PetAPI.Models;
using PetAPI.Services;

namespace PetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DigiPetsController : ControllerBase
    {
        private readonly AccountDetailsService _accService;
        DigiPetsDbContext dbContext = new DigiPetsDbContext();


        public DigiPetsController(AccountDetailsService service)
        {
            _accService = service;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            List<DigiPet> result = dbContext.DigiPets.ToList();
            for (int i = 0; i < result.Count; i++)
            {
                try
                {
                    result[i].details = await _accService.GetAccountDetails(result[i].Id, result[i].details.apiKey);
                }
                catch (Exception e)
                {
                    result[i].details = null;
                }
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            DigiPet result = dbContext.DigiPets.FirstOrDefault(d => d.Id == id);
            if (result == null)
            {
                return NotFound("This is not pets you're looking for.");
            }
            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> AddPet([FromBody] DigiPet newPet,[FromHeader] string apiKey)
        {
            AccountDetails accountDetails = await _accService.GetAccountDetailsByKey(apiKey);
            if (accountDetails == null)
            {
                return Unauthorized("That's no API Key!");
            }
            newPet.Id = 0;
            dbContext.DigiPets.Add(newPet);
            dbContext.SaveChanges();
            return Created($"/api/DigiPets/{newPet.Id}", newPet);
        }

        [HttpDelete()]
        public async Task<IActionResult> DeletePet(int id, string apiKey)
        {
            AccountDetails accountDetails = await _accService.GetAccountDetailsByKey(apiKey);
            if (accountDetails == null)
            {
                return Unauthorized("That's no API Key!");
            }
            DigiPet result = dbContext.DigiPets.FirstOrDefault(d => d.Id == id);
            if(result == null) { return NotFound("This is not the pet you're looking for."); }
            if(accountDetails.id != result.AccountId)
            {
                return Forbid();
            }
            else { dbContext.DigiPets.Remove(result); dbContext.SaveChanges(); return NoContent(); }
        }

        [HttpPut("{id}/Heal")]
        public async Task<IActionResult> HealPet(int id, string apiKey)
        {
            DigiPet result = dbContext.DigiPets.FirstOrDefault(d => d.Id == id);
            AccountDetails accountDetails = await _accService.GetAccountDetailsByKey(apiKey);
            if (accountDetails == null)
            {
                return Unauthorized("That's no API Key!");
            }
            if (result.Id != id) { return BadRequest("IDs don't match"); }
            if (dbContext.DigiPets.Any(d => d.Id == id) == false) { return NotFound("No matching IDs"); }
            if (accountDetails.id != result.AccountId)
            {
                return Forbid();
            }
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
        public async Task<IActionResult> TrainPet(int id, string apiKey)
        {
            DigiPet result = dbContext.DigiPets.FirstOrDefault(d => d.Id == id);
            AccountDetails accountDetails = await _accService.GetAccountDetailsByKey(apiKey);
            if (accountDetails == null)
            {
                return Unauthorized("That's no API Key!");
            }
            if (result.Id != id) { return BadRequest("IDs don't match"); }
            if (accountDetails.id != result.AccountId)
            {
                return Forbid();
            }
            if (dbContext.DigiPets.Any(d => d.Id == id) == false) { return NotFound("No matching IDs"); }
            Random rnd = new Random();
            int strength = rnd.Next(1, 4);

            result.Strength += strength;

            dbContext.DigiPets.Update(result);
            dbContext.SaveChanges();
            return Ok(result);

        }

        [HttpPut("{id}/Battle")]
        public async Task<IActionResult> Battle(int id, int opponentId, string apiKey)
        {
            bool win = false;

            DigiPet userPet = dbContext.DigiPets.FirstOrDefault(p => p.Id == id);
            DigiPet opponentPet = dbContext.DigiPets.FirstOrDefault(p => p.Id == opponentId);
            AccountDetails accountDetails = await _accService.GetAccountDetailsByKey(apiKey);
            
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

            userPet.Experience += 1;
            opponentPet.Experience += 1;

            if(win = true)
            {
                return Ok($"{userPet} won!");
            }
            else if (win = false)
            {
                return Ok($"{userPet} lost..."); 
            }
            else
            {
                return Ok();
            }
            

        }
    }
}
