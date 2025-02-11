using Microsoft.AspNetCore.Identity;

namespace PortalJob.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Occupation { get; set; }
        public int Experience { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<JobCandidate> JobApplications { get; set; }
    }
}
