using System.ComponentModel.DataAnnotations;

namespace PortalJob.Payload.Request
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Avatar wajib diunggah.")]
        public IFormFile AvatarFile { get; set; }

        public string Occupation { get; set; }

        public int Experience { get; set; }

        public string Role { get; set; }
    }
}
