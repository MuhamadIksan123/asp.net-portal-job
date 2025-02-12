using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalJob.Data;
using PortalJob.Models;
using PortalJob.Payload.Request;
using PortalJob.Services;
using System.Security.Claims;

namespace PortalJob.Controllers
{
    public class CompanyJobController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService;

        public CompanyJobController(ApplicationDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobs = await _context.CompanyJobs
                                      .Where(j => j.Company.EmployerId == employerId)
                                      .Include(j => j.Company)
                                      .Include(j => j.Category)
                                      .ToListAsync();

            return View("Index", jobs);
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = _context.Categories.ToList();
            return View("Create", new CompanyJobRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Store(CompanyJobRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = _context.Categories.ToList();
                return View("Create", request);
            }

            var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.EmployerId == employerId);

            if (company == null)
            {
                ViewData["ErrorMessage"] = "Anda belum memiliki perusahaan yang terdaftar.";
                ViewData["Categories"] = _context.Categories.ToList();
                return View("Create", request);
            }

            var job = new CompanyJob
            {
                Name = request.Name,
                Slug = request.Name.ToLower().Replace(" ", "-"),
                Type = request.Type,
                Location = request.Location,
                SkillLevel = request.SkillLevel,
                Salary = request.Salary,
                About = request.About,
                IsOpen = true,
                CompanyId = company.Id,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (request.ThumbnailFile != null)
            {
                job.Thumbnail = await _fileService.SaveFileAsync(request.ThumbnailFile, "thumbnails");
            }

            _context.CompanyJobs.Add(job);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var job = await _context.CompanyJobs.FindAsync(id);
            if (job == null) return NotFound();

            var request = new CompanyJobRequest
            {
                Name = job.Name,
                Type = job.Type,
                Location = job.Location,
                SkillLevel = job.SkillLevel,
                Salary = job.Salary,
                About = job.About,
                CategoryId = job.CategoryId
            };

            ViewData["Categories"] = _context.Categories.ToList();
            ViewData["JobId"] = id;
            ViewData["CurrentThumbnail"] = job.Thumbnail;

            return View("Edit", request);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CompanyJobRequest request)
        {
            var job = await _context.CompanyJobs.FindAsync(id);
            if (job == null) return NotFound();

            if (ModelState.IsValid)
            {
                job.Name = request.Name;
                job.Type = request.Type;
                job.Location = request.Location;
                job.SkillLevel = request.SkillLevel;
                job.Salary = request.Salary;
                job.About = request.About;
                job.CategoryId = request.CategoryId;
                job.UpdatedAt = DateTime.UtcNow;

                if (request.ThumbnailFile != null)
                {
                    if (!string.IsNullOrEmpty(job.Thumbnail))
                    {
                        await _fileService.DeleteFileAsync(job.Thumbnail);
                    }

                    job.Thumbnail = await _fileService.SaveFileAsync(request.ThumbnailFile, "thumbnails");
                }

                _context.CompanyJobs.Update(job);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["Categories"] = _context.Categories.ToList();
            return View("Edit", request);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var job = await _context.CompanyJobs.FindAsync(id);
            if (job == null) return NotFound();

            if (!string.IsNullOrEmpty(job.Thumbnail))
            {
                await _fileService.DeleteFileAsync(job.Thumbnail);
            }

            _context.CompanyJobs.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
