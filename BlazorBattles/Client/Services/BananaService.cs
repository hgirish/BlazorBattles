using System;

namespace BlazorBattles.Client.Services
{
    public class BananaService: IBananaService
    {
        public int Bananas { get; set; } = 1000;
        public void EatBananas(int amount)
        {
            Bananas -= amount;
            BananasChanged();
        }
        public event Action OnChange;
        void BananasChanged() => OnChange.Invoke();

        public void AddBananas(int amount)
        {
            Bananas += amount;
            BananasChanged();
        }
    }
}
