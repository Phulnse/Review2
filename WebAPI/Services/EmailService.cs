using Application.IServices;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Net.Mail;

namespace WebAPI.Services
{
    public class EmailService : IEmailService
    {
        private const string username = "noreply.nckh@gmail.com";
        private const string pw = "smsbnwsdbxslyewv";
        public async void SendOtpEmailAsync(string email, string subject, string otp)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(username, pw),
            };
            client.UseDefaultCredentials = false;
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(username, "SRMS Supports");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = OtpTemplates(email, otp);

            await client.SendMailAsync(mailMessage);
        }

        public async void SendNotifyEmailAsync(string email, string subject, string message, string action)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(username, pw),
            };
            client.UseDefaultCredentials = false;
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(username, "SRMS Supports");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = NotifyTemplates(email, message, action);

            await client.SendMailAsync(mailMessage);
        }

        public async void SendAccountInforToEmailAsync(string email, string subject, string password)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(username, pw),
            };
            client.UseDefaultCredentials = false;
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(username, "SRMS Supports");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = AccountInforTemplates(email, password);

            await client.SendMailAsync(mailMessage);
        }

        private string OtpTemplates(string email, string otp)
        {
            return @"<html>"
                   + "<body>"
                   + $"<p>Xin chào {email},</p>"
                   + "<p>Bạn đang yêu cầu gửi mã xác thực qua email</p>"
                   + "<p>Mã xác thực của bạn là:</p>"
                   + $"<strong>{otp}</strong>"
                   + $"<p>Mã xác thực có hiệu lực trong vòng 2 phút</p>"
                   + $"<p>Nếu bạn không thực hiện điều này vui lòng bỏ qua</p>"
                   + "</body>"
                   + "</html>";
        }

        private string AccountInforTemplates(string email, string password)
        {
            return @"<html>"
                   + "<body>"
                   + $"<p>Xin chào {email},</p>"
                   + "<p>Bạn đã được cung cấp tài khoản cho hệ thống SRMS</p>"
                   + $"<p>Username: {email}</p>"
                   + $"<p>Password: {password}</p>"
                   + $"<p>Bạn vui lòng đăng nhập vào hệ thống để thay đồi mật khẩu</p>"
                   + "</body>"
                   + "</html>";
        }

        private string NotifyTemplates(string email, string message, string action)
        {           
            if (string.IsNullOrEmpty(action))
                return @"<html>"
                    + "<body>"
                    + $"<p>Xin chào {email},</p>"
                    + $"<p>Trạng thái đề tài của bạn:</p>"
                    + $"<strong>{message}</strong>"
                    + "<p>Bạn vui lòng truy cập vào website để xem chi tiết</p>"
                    + "</body>"
                    + "</html>";
            return @"<html>"
                + "<body>"
                + $"<p>Xin chào {email},</p>"
                + $"<p>Trạng thái đề tài của bạn:</p>"
                + $"<strong>{message}</strong>"
                + $"<p>Bạn cần: </p><strong>{action}</strong>" 
                + "<p>Vui lòng truy cập vào website để xem chi tiết</p>"
                + "</body>"
                + "</html>";
        }       
    }
}
