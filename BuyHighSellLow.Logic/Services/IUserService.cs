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
        Task<BHSLUser> GetUser(string userEmail);
    }
}