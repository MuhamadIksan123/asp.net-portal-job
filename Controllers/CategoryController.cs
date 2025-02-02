using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalJob.Data;
using PortalJob.Models;
using PortalJob.Payload.Request;

namespace PortalJob.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.OrderByDescending(c => c.Id).ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Store(CategoryRequest model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = model.Name,
                    Slug = model.Name.ToLower().Replace(" ", "-"),
                };

                if (model.Icon != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Icon.FileName);
                    string filePath = Path.Combine(_environment.WebRootPath, "uploads/icons", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Icon.CopyToAsync(fileStream);
                    }
                    category.Icon = "/uploads/icons/" + fileName;
                }

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var model = new Category
            {
                Name = category.Name,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryRequest model)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            if (ModelState.IsValid)
            {
                category.Name = model.Name;
                category.Slug = model.Name.ToLower().Replace(" ", "-");

                if (model.Icon != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Icon.FileName);
                    string filePath = Path.Combine(_environment.WebRootPath, "uploads/icons", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Icon.CopyToAsync(fileStream);
                    }
                    category.Icon = "/uploads/icons/" + fileName;
                }

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Edit", model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
