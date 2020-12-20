using System.Collections.Generic;
using System.Threading.Tasks;
using BuyHighSellLow.DataAccess.Models.Identity;
using BuyHighSellLow.Logic.Models.Requests;

namespace BuyHighSellLow.Logic.Services
{
    public interface IUserService
    {
        Task<string> SignIn(UserLoginRequest requestModel);
        Task SignOut();
        Task RegisterUser(UserDetailsRequest requestModel);
        Task EditUser(UserDetailsRequest requestModel);
        Task<BHSLUser> GetUser(string username);
        Task<decimal> GetUserBalance(string username);
        Task AddStocksToAccount(List<StockOrder> orders, string username, decimal totalPrice);
        Task RemoveStocksFromAccount(List<StockOrder> orders, string username, decimal totalPrice);
    }
}