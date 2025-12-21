using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FPT_Booking_BE.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<string> Login(LoginRequest request)
        {
            var user = await _userRepo.GetUserByEmailAsync(request.Email);

            if (user == null || user.PasswordHash != request.Password)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.RoleName), 
                    new Claim("FullName", user.FullName),
                    new Claim("UserId", user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<string?> LoginWithGoogle(string idToken)
        {
            try
            {
                string email;
                string name;

                // Check if this is an Access Token (starts with ya29.) or ID Token
                if (idToken.StartsWith("ya29.") || idToken.StartsWith("1//"))
                {
                    // This is an Access Token - call Google UserInfo API
                    Console.WriteLine("Detected Google Access Token, calling UserInfo API...");
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {idToken}");
                    
                    var response = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Google UserInfo API failed: {response.StatusCode}");
                        throw new Exception("Không thể xác thực token với Google");
                    }

                    var userInfo = await response.Content.ReadFromJsonAsync<GoogleUserInfo>();
                    
                    if (userInfo == null || string.IsNullOrEmpty(userInfo.Email))
                    {
                        throw new Exception("Không thể lấy thông tin người dùng từ Google");
                    }

                    email = userInfo.Email;
                    name = userInfo.Name ?? userInfo.Email;
                    Console.WriteLine($"Google UserInfo API successful for: {email}");
                }
                else
                {
                    // This is an ID Token - validate using JWT
                    Console.WriteLine("Detected Google ID Token, validating JWT...");
                    var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _config["Google:ClientId"] }
                    });

                    email = payload.Email;
                    name = payload.Name;
                }

                if (!email.EndsWith("@fpt.edu.vn", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Chỉ chấp nhận email @fpt.edu.vn");
                }

                var user = await _userRepo.GetUserByEmailAsync(email);

                if (user == null)
                {
                    user = new User
                    {
                        Email = email,
                        FullName = name,
                        IsActive = true,
                        RoleId = 3
                    };
                    await _userRepo.CreateUserAsync(user);
                    Console.WriteLine($"Created new user: {email}");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.RoleName),
                    new Claim("FullName", user.FullName),
                    new Claim("UserId", user.UserId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _config["Jwt:Issuer"],
                    Audience = _config["Jwt:Audience"]
                };

                Console.WriteLine($"JWT token created successfully for user: {user.Email}");
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google login error: {ex.Message}");
                return null;
            }
        }
    }

    // Helper class for Google UserInfo API response
}