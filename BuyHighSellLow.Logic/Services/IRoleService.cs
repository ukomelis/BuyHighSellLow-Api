using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.Services
{
    public interface IRoleService
    {
        Task CreateRole(string roleName);
        Task DeleteRole(string roleName);
        Task AddRoleToUser(string userEmail, string role);
        Task RemoveRoleFromUser(string userEmail, string role);
    }
}