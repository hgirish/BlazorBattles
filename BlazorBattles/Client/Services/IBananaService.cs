using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public   interface IBananaService
    {
        int Bananas { get; set; }
        void EatBananas(int amount);
        event Action OnChange;
        Task AddBananas(int amount);
        Task GetBananas();
    }
}
