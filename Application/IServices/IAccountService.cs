using Application.ViewModels.AccountVMs;

namespace Application.IServices
{
    public interface IAccountService
    {
        Task<LoginRes?> LoginAsync(LoginInfor login);
        IEnumerable<string> GetInactiveEmails();
        Task ProvideAccountForUserViaEmail(IEnumerable<string> emails);
        Task<bool> ChangePassword(ChangePasswordReq req);
        Task<bool> ChangePassword(ChangePasswordWithOtpReq req);
        Task RequestOtp(AccountVM account);
    }
}
