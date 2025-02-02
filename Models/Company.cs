    namespace PortalJob.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Slug { get; set; }
        public string About { get; set; }
        public string EmployerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User Employer { get; set; }
        public ICollection<CompanyJob> Jobs { get; set; }


    }
}
