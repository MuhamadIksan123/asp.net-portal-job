namespace PortalJob.Models
{
    public class CompanyJob
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string SkillLevel { get; set; }
        public string Salary { get; set; }
        public string Thumbnail { get; set; }
        public string About { get; set; }
        public bool IsOpen { get; set; }
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Company Company { get; set; }
        public Category Category { get; set; }
        public ICollection<JobResponsibility> JobResponsibilities { get; set; }
        public ICollection<JobQualification> JobQualifications { get; set; }
        public ICollection<JobCandidate> Candidates { get; set; }

    }
}
