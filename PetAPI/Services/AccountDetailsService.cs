using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetAPI.Models;

namespace PetAPI.Services
{
    public class AccountDetailsService
    {
        private readonly HttpClient _httpClient;

        public AccountDetailsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8080/AccountDetails");
        }
        public async Task<AccountDetails>GetAccountDetails(long id, string apiKey)
        {
            AccountDetails result = await _httpClient.GetFromJsonAsync<AccountDetails>($"?username={id}n&apiKey={apiKey}");
            
            return result;
        }


        //public async Task<int> GetIdFromApiKey(string key)
        //{
        //    HttpResponseMessage response = await client.GetAsync($"Accounts/by-key/{key}");
        //    if (!response.IsSuccessStatusCode)
        //        return 0;
        //    Account account = await response.Content.ReadFromJsonAsync<Account>();
        //    return account.Id;
        //}
    }
}