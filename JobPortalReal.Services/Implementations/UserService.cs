using JobPortalReal.Models;
using JobPortalReal.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalReal.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return _userManager.Users.ToList();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            return usersInRole;
        }

        //public async Task<string> GetUserIdAsync(ClaimsPrincipal userPrincipal)
        //{
        //    var user = await _userManager.GetUserAsync(userPrincipal) as ApplicationUser;
        //    if (user == null)
        //    {
        //        throw new NotImplementedException("User not found.");
        //    }

        //    var userId = user.Id;
        //    return userId;
        //}
    }
}
