namespace PortalJob.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File tidak valid.");
            }

            // Validasi ekstensi file (hanya gambar yang diizinkan)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Format file tidak didukung. Hanya gambar diperbolehkan.");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{folderName}/{fileName}";
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
