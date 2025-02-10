using System.ComponentModel.DataAnnotations;

namespace PortalJob.Payload.Request
{
    public class CompanyRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IFormFile? LogoFile { get; set; }

        [Required]
        [StringLength(300)]
        public string About { get; set; }

        [Required]
        public string EmployerId { get; set; }
    }
}
