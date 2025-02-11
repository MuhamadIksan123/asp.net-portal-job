using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortalJob.Models;

namespace PortalJob.Data
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyJob> CompanyJobs { get; set; }
        public DbSet<JobCandidate> JobCandidates { get; set; }
        public DbSet<JobResponsibility> JobResponsibilities { get; set; }
        public DbSet<JobQualification> JobQualifications { get; set; }
        public new DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>()
                .HasOne(company => company.Employer)
                .WithMany()
                .HasForeignKey(company => company.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompanyJob>()
                .HasOne(job => job.Company)
                .WithMany(company => company.Jobs)
                .HasForeignKey(job => job.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<CompanyJob>()
                .HasOne(job => job.Category)
                .WithMany(category => category.Jobs)
                .HasForeignKey(job => job.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobResponsibility>()
                .HasOne(jr => jr.CompanyJob)
                .WithMany(cj => cj.JobResponsibilities)
                .HasForeignKey(jr => jr.CompanyJobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobResponsibility>()
                .HasOne(jr => jr.CompanyJob)
                .WithMany(cj => cj.JobResponsibilities)
                .HasForeignKey(jr => jr.CompanyJobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobCandidate>()
                .HasOne(jc => jc.Job)
                .WithMany(cj => cj.Candidates)
                .HasForeignKey(jc => jc.CompanyJobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobCandidate>()
                .HasOne(jc => jc.Profile)
                .WithMany(u => u.JobApplications)
                .HasForeignKey(jc => jc.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);


            // Menambahkan konfigurasi untuk setiap model
            modelBuilder.Entity<Category>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>()
                .Property(c => c.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Company>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Company>()
                .Property(c => c.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<CompanyJob>()
                .Property(cj => cj.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<CompanyJob>()
                .Property(cj => cj.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<JobCandidate>()
                .Property(jc => jc.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<JobCandidate>()
                .Property(jc => jc.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<JobQualification>()
                .Property(jq => jq.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<JobQualification>()
                .Property(jq => jq.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<JobResponsibility>()
                .Property(jr => jr.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<JobResponsibility>()
                .Property(jr => jr.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
