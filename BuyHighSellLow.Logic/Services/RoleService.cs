using System;
using System.Threading.Tasks;
using BuyHighSellLow.DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace BuyHighSellLow.Logic.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        private readonly UserManager<BHSLUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, IUserService userService, UserManager<BHSLUser> userManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task CreateRole(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName).ConfigureAwait(false)) throw new Exception($"Failed to add the role {roleName}, role already exists");

            var role = new IdentityRole { Name = roleName };
            await _roleManager.CreateAsync(role).ConfigureAwait(false);
        }

        public async Task DeleteRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName).ConfigureAwait(false)) throw new Exception($"Failed to delete the role {roleName}, role doesn't exists");

            var role = new IdentityRole { Name = roleName };
            await _roleManager.DeleteAsync(role).ConfigureAwait(false);
        }

        public async Task AddRoleToUser(string userEmail, string role)
        {
            var user = await _userService.GetUser(userEmail).ConfigureAwait(false);

            var result = await _userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
            if (!result.Succeeded) throw new Exception($"Failed to add role '{role}' to user: {userEmail}");
        }

        public async Task RemoveRoleFromUser(string userEmail, string role)
        {
            var user = await _userService.GetUser(userEmail).ConfigureAwait(false);

            var result = await _userManager.RemoveFromRoleAsync(user, role).ConfigureAwait(false);
            if (!result.Succeeded) throw new Exception($"Failed to add role '{role}' to user: {userEmail}");
        }
    }
}