using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortalReal.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public string FullName { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }
    }
}
