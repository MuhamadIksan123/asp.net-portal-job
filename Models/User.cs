using Microsoft.AspNetCore.Identity;

namespace PortalJob.Models
{
    public class User : IdentityUser
    {
        public string Avatar { get; set; }
        public string Occupation { get; set; }
        public string Experience { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<JobCandidate> JobApplications { get; set; }
    }
}
