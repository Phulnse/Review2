using Application.IServices;
using Application.ViewModels.AccountVMs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private const int keySize = 64;
        private const int iterations = 350000;
        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Account account)
        {
            var claims = new List<Claim>
            {
                new("email", account.Email),
                new("role", account.RoleName),
            };            

            var signinCredentials = GetSigninCredentials();
            var tokenOptions = GenerateTokenOptions(signinCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigninCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("JWTSettings:SecretKey")!);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
                jwtSettings.GetSection("Issuer").Value,
                jwtSettings.GetSection("Audience").Value,
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

        public string GenerateRandomString()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateOtp()
        {
            return new Random().Next(1000000).ToString("D6");
        }

        public HashAndSalt HashPassword(string password)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            var salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                                                salt,
                                                iterations,
                                                hashAlgorithm,
                                                keySize);

            return new HashAndSalt
            {
                HashPassword = Convert.ToHexString(hash),
                Salt = Convert.ToHexString(salt),
            };
        }

        public bool ComparePassword(string password, string accountPassword, string salt)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                                                Convert.FromHexString(salt),
                                                iterations,
                                                hashAlgorithm,
                                                keySize);

            return CryptographicOperations.FixedTimeEquals(hash, Convert.FromHexString(accountPassword));
        }

        public string GenerateToken(Account account, User user)
        {
            var role = RoleEnum.User.ToString();
            if (user.IsDean)
                role = "Dean";
            var claims = new List<Claim>
            {
                new("email", account.Email),
                new("role", role),
                new("userid", user.Id.ToString()),
                new("fullname", user.FullName),
            };

            var signinCredentials = GetSigninCredentials();
            var tokenOptions = GenerateTokenOptions(signinCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
