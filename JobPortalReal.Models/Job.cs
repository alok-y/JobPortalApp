using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortalReal.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        
        [ValidateNever]
        public string? PostedBy { get; set; } 


        //public virtual ICollection<JobApplication> Applications { get; set; }
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }
}
