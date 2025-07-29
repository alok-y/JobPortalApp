using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobPortalReal.Models;
using JobPortalReal.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Json;

namespace JobPortalReal.Controllers
{
    [Authorize(Roles = "Jobseeker")]
    public class JobseekerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobService _jobService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;





        public JobseekerController(UserManager<ApplicationUser> userManager, IJobService jobService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _jobService = jobService;
            _configuration = configuration;
            _httpClient = new HttpClient();
            _webHostEnvironment = webHostEnvironment;
        }










        // GET: /Jobseeker/Dashboard
        public async Task<IActionResult> JobStatus()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var applications = await _jobService.GetApplicationsByJobseekerAsync(currentUser.Id);
            return View(applications);
        }








        public async Task<IActionResult> Dashboard(string searchQuery, int? minSalary, int? maxSalary, int page = 1, int pageSize = 5)
        {
            var jobs = await _jobService.GetAllJobsAsync();
            // Apply search filter
            if (!string.IsNullOrEmpty(searchQuery))
            {
                jobs = jobs.Where(j => j.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }
            // Apply salary range filter
            if (minSalary.HasValue)
            {
                jobs = jobs.Where(j => j.Salary >= minSalary.Value);
            }
            if (maxSalary.HasValue)
            {
                jobs = jobs.Where(j => j.Salary <= maxSalary.Value);
            }
            // Pagination
            var paginatedJobs = jobs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)jobs.Count() / pageSize);
            ViewBag.SearchQuery = searchQuery;
            ViewBag.MinSalary = minSalary;
            ViewBag.MaxSalary = maxSalary;
            return View(paginatedJobs);
        }







        // GET: /Jobseeker/ViewJobs
        public async Task<IActionResult> ViewJobs()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return View(jobs);
        }






        // GET: /Jobseeker/ApplyJob/{id}
        [HttpGet]
        public async Task<IActionResult> ApplyJob(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }










        // POST: /Jobseeker/ApplyJob
        [HttpPost]  
        public async Task<IActionResult> ApplyJob(int id, string coverLetter, IFormFile? file)
        {
            // You won't have access to properties and methods specific to ApplicationUser unless you cast it.
            //If you don't cast the result at all, you will have to work with the base type returned by GetUserAsync, which is typically IdentityUser
            var currentUser = await _userManager.GetUserAsync(User) as ApplicationUser;
            var job = await _jobService.GetJobByIdAsync(id);
            // It is used to get the root path of the web server's public directory
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            // Initialize filename with a default value
            string filename = string.Empty; 

            if (file != null)
            {
                var random = new Random();
                // Generates a random number between 1000 and 9999
                var randomNumber = random.Next(1000, 9999); 
                filename = currentUser.FullName + randomNumber.ToString() + Path.GetExtension(file.FileName);
                
                string ResumePath = Path.Combine(wwwRootPath, @"uploads\resumes");
                //creates a new file stream for writing to the specified path. If the file already exists, it will be overwritten.
                using (var fileStream = new FileStream(Path.Combine(ResumePath, filename), FileMode.Create))
                {
                    file.CopyTo(fileStream);//copies the contents of the uploaded file (file) to the file stream (fileStream).
                }
            }

            var application = new JobApplication
            {
                JobId = id,
                JobTitle = job.Title,
                JobseekerName = currentUser.FullName,
                JobseekerId = currentUser.Id,
                CoverLetter = coverLetter,
                Status = "Pending",
                AppliedDate = DateTime.Now,
                ResumeUrl = file != null ? @"\uploads\resumes\" + filename : string.Empty // Ensure ResumeUrl is set correctly
            };

            var result = await _jobService.AddJobApplicationAsync(application);
            if (result)
            {
                return RedirectToAction("Dashboard");
            }

            return View("Error");
        }











        //NOTIFICATION
        public async Task<IActionResult> Notifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Notification/user/{userId}";
            var response = await _httpClient.GetStringAsync(apiUrl);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var notifications = JsonSerializer.Deserialize<IEnumerable<Notification>>(response, options);
            return View(notifications);
        }





    }
}
