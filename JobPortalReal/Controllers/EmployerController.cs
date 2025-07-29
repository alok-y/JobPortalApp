using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobPortalReal.Models;
using JobPortalReal.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using JobPortalReal.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace JobPortalReal.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobService _jobService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;




        public EmployerController(UserManager<ApplicationUser> userManager, IJobService jobService, IConfiguration configuration)
        {
            _userManager = userManager;
            _jobService = jobService;
            _configuration = configuration;
            _httpClient = new HttpClient();

        }







        // GET: /Employer/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var currentUser = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userName = currentUser.FullName;
            IEnumerable<Job> joblist = await _jobService.GetAllJobsAsync();
            var jobs = await _jobService.GetJobsByEmployerAsync(userName);
            return View(jobs);
        }







        // GET: /Employer/ViewApplications/{jobId}
        [HttpGet]
        public async Task<IActionResult> ViewApplications(int jobId)
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            ViewData["ApiBaseUrl"] = apiBaseUrl;


            var currentUser = await _userManager.GetUserAsync(User) as ApplicationUser;
            var userName = currentUser.FullName;
            // Get jobs posted by the employer
            var jobsPosted = await _jobService.GetJobsByEmployerAsync(userName);
            // Get applications for these jobs
            var applications = new List<JobApplication>();
            foreach (var job in jobsPosted)
            {
                var jobApplications = await _jobService.GetApplicationsByJobIdAsync(job.Id);
                applications.AddRange(jobApplications);
            }
            return View(applications);
        }








        // POST: /Employer/ApproveApplication/{id}
        [HttpPost]
        public async Task<IActionResult> ApproveApplication(int id)
        {
            var application = await _jobService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            application.Status = "Approved";
            await _jobService.UpdateJobApplicationAsync(application);

            var notifyDto = new NotificationDto
            {
                UserId = application.JobseekerId,
                Message = $"Your interview for '{application.Job.Title}' is scheduled on {DateTime.Now.AddDays(3):MMMM dd, yyyy}."
            };

            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Notification";
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var content = new StringContent(JsonSerializer.Serialize(notifyDto, options), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            return RedirectToAction("ViewApplications");

        }










        // POST: /Employer/RejectApplication/{id}
        [HttpPost]
        public async Task<IActionResult> RejectApplication(int id)
        {
            var application = await _jobService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            application.Status = "Rejected";
            await _jobService.UpdateJobApplicationAsync(application);

            //Send notification via Api

            var notifyDto = new NotificationDto
            {
                UserId = application.JobseekerId,
                Message = $"We regret to inform you that your application for '{application.Job.Title}' has been rejected."
            };

            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Notification";
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase //to match the expected format of api
            };
            var content = new StringContent(JsonSerializer.Serialize(notifyDto, options), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            return RedirectToAction("ViewApplications");
        }











        // GET: /Employer/PostJob
        [HttpGet]
        public IActionResult PostJob()
        {
            return View();
        }












        // POST: /Employer/PostJob
        [HttpPost]
        public async Task<IActionResult> PostJob(Job job)
        {
            if (ModelState.IsValid)
            {
                
                var currentUser = await _userManager.GetUserAsync(User) as ApplicationUser;
                var userName = currentUser.FullName ;

                job.PostedBy = userName;

                var result = await _jobService.AddJobAsync(job);

                if (result)
                {
                    return RedirectToAction("Dashboard");
                }
                ModelState.AddModelError("", "Failed to post the job. Please try again.");
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }
            return View(job);
        }











        // GET: /Employer/EditJob/{id}
        [HttpGet]
        public async Task<IActionResult> EditJob(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }












        // POST: /Employer/EditJob/{id}
        [HttpPost]
        public async Task<IActionResult> EditJob(Job job)
        {
            if (ModelState.IsValid)
            {
                var result = await _jobService.UpdateJobAsync(job);
                if (result)
                {
                    return RedirectToAction("Dashboard");
                }
                ModelState.AddModelError("", "Failed to update the job. Please try again.");
            }
            return View(job);
        }













        // POST: /Employer/DeleteJob
        [HttpPost]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var result = await _jobService.DeleteJobAsync(id);
            if (result)
            {
                return RedirectToAction("Dashboard");
            }
            return View("Error");
        }




    }
}


