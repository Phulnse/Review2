namespace Application.IServices
{
    public interface IEmailService
    {
        void SendOtpEmailAsync(string email, string subject, string message);
        void SendNotifyEmailAsync(string email, string subject, string message, string action);
        void SendAccountInforToEmailAsync(string email, string subject, string message);
    }
}
