using Application.ViewModels.AccountVMs;
using Domain.Entities;

namespace Application.IServices
{
    public interface IAuthService
    {
        string GenerateToken(Account account);
        string GenerateToken(Account account, User user);
        string GenerateRandomString();
        string GenerateOtp();
        HashAndSalt HashPassword(string password);
        bool ComparePassword(string password, string accountPassword, string salt);   
    }
}
