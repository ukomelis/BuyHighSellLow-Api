using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BuyHighSellLow.DataAccess.Models.Identity;
using BuyHighSellLow.Logic.Models.Configuration;
using BuyHighSellLow.Logic.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BuyHighSellLow.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<BHSLUser> _userManager;
        private readonly SignInManager<BHSLUser> _signInManager;
        private readonly IConfigurationProvider _configurationProvider;

        public UserService(UserManager<BHSLUser> userManager, SignInManager<BHSLUser> signInManager, IConfigurationProvider configurationProvider)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public async Task<string> SignIn(UserLoginRequest requestModel)
        {
            var result = await _signInManager.PasswordSignInAsync(requestModel.Email, requestModel.Password,
                requestModel.RememberMe, false).ConfigureAwait(false);

            if (result.Succeeded)
            {
                try
                {
                    return GenerateJwtToken(requestModel.Email);
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to generate the JWT token", e);
                }
            }
            else
            {
                throw new Exception($"Failed to sign in an user: {requestModel.Email}, Error: {result}");
            }
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task RegisterUser(UserDetailsRequest requestModel)
        {
            var user = new BHSLUser { UserName = requestModel.Email, Email = requestModel.Email };
            var result = await _userManager.CreateAsync(user, requestModel.Password).ConfigureAwait(false);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
            }
            else
            {
                throw new Exception($"Failed to register an user. {result.Errors}");
            }
        }

        public Task EditUser(UserDetailsRequest requestModel)
        {
            throw new NotImplementedException();
        }

        public async Task<BHSLUser> GetUser(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail).ConfigureAwait(false);
            if (user == null) throw new Exception($"Failed to find the user: {userEmail}");

            return user;
        }

        private string GenerateJwtToken(string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configurationProvider.GetJwtSecret()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Email", email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationProvider.GetJwtSecret()));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configurationProvider.GetJwtIssuer(),
                _configurationProvider.GetJwtAudience(), claims, expires: DateTime.Now.AddDays(1),
                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}