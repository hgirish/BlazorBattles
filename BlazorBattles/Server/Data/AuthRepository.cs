using BlazorBattles.Shared;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Data
{
    public class AuthRepository : IAuthRepository
    {
        public Task<string> Login(string email, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> Register(User user, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UserExists(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}
