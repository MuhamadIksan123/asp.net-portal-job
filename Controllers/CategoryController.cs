using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalJob.Data;
using PortalJob.Models;
using PortalJob.Payload.Request;
using PortalJob.Services;

namespace PortalJob.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService;

        public CategoryController(ApplicationDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.OrderByDescending(c => c.Id).ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View(new CategoryRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Store(CategoryRequest request)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = request.Name,
                    Slug = request.Name.ToLower().Replace(" ", "-"),
                };

                if (request.IconFile != null)
                {
                    category.Icon = await _fileService.SaveFileAsync(request.IconFile, "icons");
                }

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Create", request); // Pastikan model yang dikirim tetap CategoryStoreRequest
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var model = new CategoryRequest
            {
                Name = category.Name
            };

            ViewData["CategoryId"] = id;
            ViewData["CurrentIcon"] = category.Icon; // Simpan ikon lama

            return View(model); // Pastikan model yang dikembalikan adalah CategoryUpdateRequest
        }



        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryRequest model)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            // Hapus validasi jika IconFile kosong
            ModelState.Remove("IconFile");

            if (ModelState.IsValid)
            {
                category.Name = model.Name;
                category.Slug = model.Name.ToLower().Replace(" ", "-");

                // Cek apakah pengguna mengunggah ikon baru
                if (model.IconFile != null)
                {
                    // Hapus ikon lama jika ada
                    if (!string.IsNullOrEmpty(category.Icon))
                    {
                        await _fileService.DeleteFileAsync(category.Icon);
                    }

                    // Simpan ikon baru
                    category.Icon = await _fileService.SaveFileAsync(model.IconFile, "icons");
                }

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["CategoryId"] = id;
            ViewData["CurrentIcon"] = category.Icon; // Tetap kirim ikon lama jika ada error
            return View("Edit", model);
        }




        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            // Hapus file gambar terkait jika ada
            if (!string.IsNullOrEmpty(category.Icon))
            {
                await _fileService.DeleteFileAsync(category.Icon);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
