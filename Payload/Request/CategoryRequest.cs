using System.ComponentModel.DataAnnotations;

namespace PortalJob.Payload.Request
{
    public class CategoryRequest
    {
        public string Name { get; set; }

        [Display(Name = "Category Icon")]
        public IFormFile? IconFile { get; set; } // Pastikan ini nullable (?)
    }
}
