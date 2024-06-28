namespace Application.ViewModels.AccountVMs
{
    public class ChangePasswordReq
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}
