using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public class BananaService: IBananaService
    {
        private readonly HttpClient _http;

        public BananaService(HttpClient http)
        {
            _http = http;
        }
        public int Bananas { get; set; } = 0;
        public void EatBananas(int amount)
        {
            Bananas -= amount;
            BananasChanged();
        }
        public event Action OnChange;
        void BananasChanged() => OnChange.Invoke();

        public async Task  AddBananas(int amount)
        {
            Bananas += amount;
            var result = await _http.PutAsJsonAsync<int>("api/User/AddBananas", amount);
            Bananas = await result.Content.ReadFromJsonAsync<int>();
            BananasChanged();
        }
        public async Task GetBananas()
        {
            Bananas = await _http.GetFromJsonAsync<int>("api/User/GetBananas");
            BananasChanged();
        }
    }
}
