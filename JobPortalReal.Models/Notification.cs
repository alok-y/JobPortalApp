using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalReal.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; } // FK to ApplicationUser

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        public virtual ApplicationUser User { get; set; }
    }
}
