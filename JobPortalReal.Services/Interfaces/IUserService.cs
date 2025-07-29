using JobPortalReal.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalReal.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string role);

        //Task<string> GetUserIdAsync(ClaimsPrincipal userPrincipal);

    }
}
