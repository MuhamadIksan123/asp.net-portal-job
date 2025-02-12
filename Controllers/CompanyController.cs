using Microsoft.AspNetCore.Mvc;
using PortalJob.Data;
using PortalJob.Models;
using PortalJob.Payload.Request;
using PortalJob.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace PortalJob.Controllers
{
    public class CompanyController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService;

        public CompanyController(ApplicationDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = await _context.Companies
                                        .FirstOrDefaultAsync(c => c.EmployerId == userId);

            if (company == null)
            {
                return View("Create", new CompanyRequest());
            }

            return View("Index", company);
        }

        public IActionResult Create()
        {
            return View(new CompanyRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Store(CompanyRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", request);
            }

            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Ambil EmployerId dari user login

            if (string.IsNullOrEmpty(employerId))
            {
                ViewData["ErrorMessage"] = "User tidak terautentikasi.";
                return View("Create", request);
            }

            var company = new Company
            {
                Name = request.Name,
                About = request.About,
                EmployerId = employerId, // Set EmployerId dari user yang login
                Slug = request.Name.ToLower().Replace(" ", "-"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Proses Upload Logo jika ada
            if (request.LogoFile != null)
            {
                company.Logo = await _fileService.SaveFileAsync(request.LogoFile, "logos");
            }

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();


            var model = new CompanyRequest
            {
                Name = company.Name,
                About = company.About,
            };

            ViewData["CompanyId"] = id;
            ViewData["CurrentLogo"] = company.Logo; // Simpan ikon lama

            return View(model); // Pastikan model yang dikembalikan benar
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CompanyRequest request)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();

            if (ModelState.IsValid)
            {
                company.Name = request.Name;
                company.About = request.About;
                company.Slug = request.Name.ToLower().Replace(" ", "-");
                company.UpdatedAt = DateTime.UtcNow;

                // Cek apakah ada logo baru yang diunggah
                if (request.LogoFile != null)
                {
                    // Hapus logo lama jika ada
                    if (!string.IsNullOrEmpty(company.Logo))
                    {
                        await _fileService.DeleteFileAsync(company.Logo);
                    }

                    // Simpan logo baru
                    company.Logo = await _fileService.SaveFileAsync(request.LogoFile, "logos");
                }

                _context.Companies.Update(company);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Edit", request);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();

            // Hapus logo jika ada
            if (!string.IsNullOrEmpty(company.Logo))
            {
                await _fileService.DeleteFileAsync(company.Logo);
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
