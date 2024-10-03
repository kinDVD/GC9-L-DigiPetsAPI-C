using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetAPI.Services
{
    public class DigiPetsService
    {
        private readonly HttpClient _httpClient;

        public DigiPetsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8080/CarDetails");
        }
        public async Task<CarDetails>GetCarDetails(string make, string model)
        {
            CarDetails result = await _httpClient.GetFromJsonAsync<CarDetails>($"?make={make}&model={model}");
            return result;
        }
    }
}