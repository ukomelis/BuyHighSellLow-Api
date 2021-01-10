using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BuyHighSellLow.DataAccess;
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
        private readonly BHSLContext _context;
        private readonly IStocksService _stocksService;

        public UserService(UserManager<BHSLUser> userManager, SignInManager<BHSLUser> signInManager, IConfigurationProvider configurationProvider, BHSLContext context, IStocksService stocksService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _stocksService = stocksService ?? throw new ArgumentNullException(nameof(stocksService));
        }

        public async Task<string> SignIn(UserLoginRequest requestModel)
        {
            var result = await _signInManager.PasswordSignInAsync(requestModel.Username, requestModel.Password,
                requestModel.RememberMe, false).ConfigureAwait(false);

            if (result.Succeeded)
            {
                try
                {
                    return GenerateJwtToken(requestModel.Username);
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to generate the JWT token", e);
                }
            }
            else
            {
                throw new Exception($"Failed to sign in an user: {requestModel.Username}, Error: {result}");
            }
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);
        }

        public async Task RegisterUser(UserDetailsRequest requestModel)
        {
            var user = new BHSLUser {
                UserName = requestModel.Username,
                Email = requestModel.Email,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                Balance = 0
            };
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

        public async Task<BHSLUser> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username).ConfigureAwait(false);
            if (user == null) throw new Exception($"Failed to find the user: {username}");

            return user;
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configurationProvider.GetJwtSecret()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("username", username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationProvider.GetJwtSecret()));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configurationProvider.GetJwtIssuer(),
                _configurationProvider.GetJwtAudience(), claims, expires: DateTime.Now.AddDays(1),
                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<decimal> GetUserBalance(string username)
        {
            try
            {
                var user = await _context.Users.FindAsync(username);

                return user.Balance;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get user balance: {username}", ex);
            }
        }

        public async Task AddStocksToAccount(List<StockOrder> orders, string username, decimal totalPrice)
        {
            try
            {
                await _stocksService.AddStocksToAccount(orders, username).ConfigureAwait(false);
                var user = _context.Users.FirstOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (user == null) throw new Exception($"Failed to find the user: {username}");
                user.Balance += totalPrice;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add stocks to account!", ex);
            }
        }

        public async Task RemoveStocksFromAccount(List<StockOrder> orders, string username, decimal totalPrice)
        {
            try
            {
                await _stocksService.RemoveStocks(orders, username).ConfigureAwait(false);
                var user = _context.Users.FirstOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (user == null) throw new Exception($"Failed to find the user: {username}");
                user.Balance -= totalPrice;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to remove stocks from account!", ex);
            }
        }
    }
}