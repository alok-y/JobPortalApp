using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalReal.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Job")]
        public int JobId { get; set; }

        //[Required]
        [StringLength(100)]
        public string JobTitle { get; set; }


        [Required]
        public string JobseekerId { get; set; } // FK to ApplicationUser

        [Required]
        [StringLength(150)]
        public string JobseekerName { get; set; }

        [StringLength(1000)]
        public string CoverLetter { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        [Required]
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [ValidateNever]
        public string ResumeUrl { get; set; }

        public virtual Job Job { get; set; }


        [ForeignKey("JobseekerId")]
        [ValidateNever]
        public ApplicationUser applicationUser { get; set; }
    }

}
