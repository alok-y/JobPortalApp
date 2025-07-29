using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobPortalReal.Models;
using JobPortalReal.Services.Interfaces;
using System.Threading.Tasks;

namespace JobPortalReal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJobService _jobService;

        public AdminController(IUserService userService, IJobService jobService)
        {
            _userService = userService;
            _jobService = jobService;
        }

        // GET: /Admin/Dashboard
        // GET: /Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // Fetch metrics from the services
            var totalUsers = (await _userService.GetAllUsersAsync()).Count();
            var totalJobs = await _jobService.GetTotalJobsAsync();
            var totalApplications = await _jobService.GetTotalApplicationsAsync();

            // Pass data to the view using a model
            var model = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalJobs = totalJobs,
                TotalApplications = totalApplications
            };

            return View(model);
        }


        // GET: /Admin/ManageUsers
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var usersWithRoles = new List<(ApplicationUser User, IEnumerable<string> Roles)>();

            foreach (var user in users)
            {
                var roles = await _userService.GetUserRolesAsync(user);
                usersWithRoles.Add((user, roles));
            }

            return View(usersWithRoles);
        }


        // GET: /Admin/ManageJobs
        public async Task<IActionResult> ManageJobs()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return View(jobs);
        }

        // POST: /Admin/DeleteUser/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id); // Add this method in UserService
            if (result)
            {
                return RedirectToAction("ManageUsers");
            }
            return View("Error");
        }

        // POST: /Admin/DeleteJob/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var result = await _jobService.DeleteJobAsync(id);
            if (result)
            {
                return RedirectToAction("ManageJobs");
            }
            return View("Error");
        }


    }
}
