using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

using JobPortalReal.Models;

namespace JobPortalReal.DataAccess.Data

{

    public class JobPortalContext : IdentityDbContext<ApplicationUser>

    {

        public JobPortalContext(DbContextOptions<JobPortalContext> options) : base(options)

        {

        }

        // Add DbSets for other entities

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>(entity =>

            {

                entity.Property(e => e.Salary)

                      .HasColumnType("decimal(18,2)"); // Specify precision and scale

                // Ensure 'PostedBy' is required

            });

            modelBuilder.Entity<JobApplication>()

                .HasOne(ja => ja.Job)

                .WithMany(j => j.Applications)

                .HasForeignKey(ja => ja.JobId)

                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

        }

    }

}

