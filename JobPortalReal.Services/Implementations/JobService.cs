using JobPortalReal.Models;
using JobPortalReal.DataAccess.Data;
using JobPortalReal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortalReal.Services.Implementations
{
    public class JobService : IJobService
    {
        private readonly JobPortalContext _context;

        public JobService(JobPortalContext context)
        {
            _context = context;
        }

        // Job Management
        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task<Job> GetJobByIdAsync(int id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public async Task<bool> AddJobAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateJobAsync(Job job)
        {
            _context.Jobs.Update(job);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteJobAsync(int jobId)
        {
            var job = await GetJobByIdAsync(jobId);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        // Job Application Management
        public async Task<IEnumerable<JobApplication>> GetApplicationsByJobIdAsync(int jobId)
        {
            return await _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.JobId == jobId)
                .ToListAsync();
        }

        public async Task<bool> UpdateJobApplicationAsync(JobApplication application)
        {
           _context.JobApplications.Update(application); // Update the application
           return await _context.SaveChangesAsync() > 0; // Save changes and check if it succeeded
        }

        public async Task<bool> AddJobApplicationAsync(JobApplication application)
        {
            await _context.JobApplications.AddAsync(application);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<JobApplication> GetApplicationByIdAsync(int applicationId)
        {
            return await _context.JobApplications
            .Include(a => a.Job).Include(a=>a.applicationUser)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        }





        public async Task<int> GetTotalJobsAsync()
        {
            return await _context.Jobs.CountAsync();
        }

        public async Task<int> GetTotalApplicationsAsync()
        {
            return await _context.JobApplications.CountAsync();
        }





        public async Task<IEnumerable<Job>> GetJobsByEmployerAsync(string employerName)
        {
            return await _context.Jobs
                .Where(j => j.PostedBy == employerName)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetApplicationsByJobseekerAsync(string jobseekerId)
        {
            return await _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.JobseekerId == jobseekerId)
                .ToListAsync();
        }
    }
}
