using JobPortalReal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortalReal.Services.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<Job>> GetAllJobsAsync();
       
        Task<bool> AddJobAsync(Job job);
        Task<bool> UpdateJobAsync(Job job);
        Task<bool> DeleteJobAsync(int jobId);



        Task<IEnumerable<JobApplication>> GetApplicationsByJobIdAsync(int jobId);
        Task<bool> AddJobApplicationAsync(JobApplication application);
        Task<bool> UpdateJobApplicationAsync(JobApplication application);


        Task<int> GetTotalJobsAsync();
        Task<int> GetTotalApplicationsAsync();


        Task<Job> GetJobByIdAsync(int jobId);
        Task<IEnumerable<Job>> GetJobsByEmployerAsync(string employerName);


        Task<IEnumerable<JobApplication>> GetApplicationsByJobseekerAsync(string jobseekerId);
        Task<JobApplication> GetApplicationByIdAsync(int applicationId);

    }
}
