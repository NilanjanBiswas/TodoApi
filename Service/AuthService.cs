using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApi.DatabaseContext;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<User> _hasher;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _hasher = new PasswordHasher<User>();
            _config = config;
        }

        public async Task<AuthResponseDto> SignupAsync(SignupDto dto)
        {
            var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return new AuthResponseDto { Token = string.Empty, ExpiresAt = DateTime.MinValue };

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            // generate token
            return GenerateToken(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return new AuthResponseDto { Token = string.Empty, ExpiresAt = DateTime.MinValue };

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verify == PasswordVerificationResult.Failed) return new AuthResponseDto { Token = string.Empty, ExpiresAt = DateTime.MinValue };

            return GenerateToken(user);
        }

        private AuthResponseDto GenerateToken(User user)
        {
            var jwtSection = _config.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key missing"));
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var expiresInMinutes = int.Parse(jwtSection["ExpiresInMinutes"] ?? "60");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthResponseDto { Token = tokenHandler.WriteToken(token), ExpiresAt = tokenDescriptor.Expires!.Value };
        }
    }
}
