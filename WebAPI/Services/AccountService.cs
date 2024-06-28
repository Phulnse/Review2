using Application.IServices;
using Application.ViewModels.AccountVMs;
using Domain.Enums;
using Domain.Interfaces;
using Org.BouncyCastle.Ocsp;

namespace WebAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        public AccountService(IUnitOfWork unitOfWork, IAuthService authService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _emailService = emailService;
        }

        public async Task<bool> ChangePassword(ChangePasswordReq req)
        {
            var account = await _unitOfWork.Account.GetAccountByEmailAsync(req.Email);

            if (account != null 
                && account.IsActive 
                && _authService.ComparePassword(req.OldPassword, account.Password, account.Salt!) 
                && !req.OldPassword.Equals(req.Password))
            {
                var hashAndSalt = _authService.HashPassword(req.Password);
                account.Password = hashAndSalt.HashPassword;
                account.Salt = hashAndSalt.Salt;
                await _unitOfWork.Save();
                return true;
            }

            return false;
        }

        public async Task<bool> ChangePassword(ChangePasswordWithOtpReq req)
        {
            var account = await _unitOfWork.Account.GetAccountByEmailAsync(req.Email);

            if (account != null 
                && account.IsActive 
                && !_authService.ComparePassword(req.Password, account.Password, account.Salt!)
                && account.ForgotOtp != null
                && req.Otp.Equals(account.ForgotOtp)
                && DateTime.Compare(account.ForgotOtpCreatedAt!.Value.AddMinutes(2), DateTime.Now) > 0)
            {
                var hashAndSalt = _authService.HashPassword(req.Password);
                account.Password = hashAndSalt.HashPassword;
                account.Salt = hashAndSalt.Salt;
                account.ForgotOtp = null;
                account.ForgotOtpCreatedAt = null;
                await _unitOfWork.Save();
                return true;
            }

            return false;
        }

        public IEnumerable<string> GetInactiveEmails()
        {
            return _unitOfWork.Account.GetInactiveEmails();
        }

        public async Task<LoginRes?> LoginAsync(LoginInfor login)
        {
            LoginRes loginRes = new LoginRes();
            var account = await _unitOfWork.Account.GetAccountByEmailAsync(login.Email);

            if (account == null || !account.IsActive || !_authService.ComparePassword(login.Password, account.Password, account.Salt!))
            {
                return null;
            }

            if (account.RoleName.Equals(RoleEnum.Staff.ToString()) || account.RoleName.Equals(RoleEnum.Admin.ToString()))
                loginRes.Token = _authService.GenerateToken(account);
            else
            {
                var user = await _unitOfWork.User.GetUserByEmailAsync(login.Email);
                if (user == null)
                {
                    loginRes.Token = _authService.GenerateToken(account);
                }
                else
                {
                    loginRes.Token = _authService.GenerateToken(account, user);
                }
            }            

            return loginRes;
        }

        public async Task ProvideAccountForUserViaEmail(IEnumerable<string> emails)
        {
            var accounts = _unitOfWork.Account.GetAccountByEmails(emails);
            foreach (var account in accounts)
            {
                var password = _authService.GenerateRandomString();
                var hashAndSalt = _authService.HashPassword(password);
                account.Password = hashAndSalt.HashPassword;
                account.Salt = hashAndSalt.Salt;
                account.IsActive = true;

                _emailService.SendAccountInforToEmailAsync(account.Email, "[Norepy] Cung cấp tài khoản truy cập hệ thống SRMS", password);
            }

            _unitOfWork.Account.UpdateBulkAccount(accounts);
            await _unitOfWork.Save();
        }

        public async Task RequestOtp(AccountVM accountVM)
        {
            var account = await _unitOfWork.Account.GetAccountByEmailAsync(accountVM.Email);
            var otp = _authService.GenerateOtp();
            account!.ForgotOtp = otp;
            account!.ForgotOtpCreatedAt = DateTime.Now;
            await _unitOfWork.Save();

            _emailService.SendOtpEmailAsync(account!.Email, "[Noreply] Mã xác thực thay đổi mật khẩu hệ thống SRMS", otp);           
        }
    }
}
