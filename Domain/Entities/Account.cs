namespace Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ForgotOtp { get; set; }
        public DateTime? ForgotOtpCreatedAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenCreatedAt { get; set; }
        public string RoleName { get; set; }
        public string Salt { get; set; }
        public bool IsActive { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
        public Staff Staff { get; set; }
    }
}
