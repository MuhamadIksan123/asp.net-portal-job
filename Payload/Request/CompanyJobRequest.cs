using System.ComponentModel.DataAnnotations;

namespace PortalJob.Payload.Request
{
    public class CompanyJobRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string SkillLevel { get; set; }

        [Required]
        public string Salary { get; set; }

        public IFormFile ThumbnailFile { get; set; }

        [Required]
        public string About { get; set; }

        [Required]
        public bool IsOpen { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<string> JobResponsibilities { get; set; } = new List<string>();
        public List<string> JobQualifications { get; set; } = new List<string>();
    }
}
