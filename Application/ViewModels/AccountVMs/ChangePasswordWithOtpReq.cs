namespace Application.ViewModels.AccountVMs
{
    public class ChangePasswordWithOtpReq
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string Password { get; set; }
    }
}
