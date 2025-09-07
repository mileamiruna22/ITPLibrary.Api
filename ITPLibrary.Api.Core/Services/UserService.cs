using ITPLibrary.Api.Core.Dtos;
using ITPLibrary.Api.Data.Entities;
using ITPLibrary.Api.Data.Repositories;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto userDto)
        {
            if (userDto.Password != userDto.ConfirmedPassword)
            {
                return false;
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.UserEmail);
            if (existingUser != null)
            {
                return false;
            }

            var user = new User
            {
                Email = userDto.UserEmail,
                Password = userDto.Password
            };

            await _userRepository.AddUserAsync(user);
            return true;
        }

        public async Task<string> LoginUserAsync(LoginUserDto userDto)
        {
            var user = await _userRepository.GetUserByEmailAndPasswordAsync(userDto.UserEmail, userDto.Password);
            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> RecoverPasswordAsync(PasswordRecoveryDto recoveryDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(recoveryDto.UserEmail);

            if (user == null)
            {
                return false;
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:Email"]));
            email.To.Add(MailboxAddress.Parse(recoveryDto.UserEmail));
            email.Subject = "Password Recovery";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = $"Your password is: {user.Password}"
            };

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return true;
        }
    }
}