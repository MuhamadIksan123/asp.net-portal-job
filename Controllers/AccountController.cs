using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortalJob.Models;
using PortalJob.Services;
using PortalJob.Payload.Request;
using Microsoft.EntityFrameworkCore;

namespace PortalJob.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly FileService _fileService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, FileService fileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterRequest()); // Mengirim model kosong agar tidak null di view
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request); // Tampilkan kembali form jika validasi gagal
            }

            // Pastikan Avatar diunggah
            if (request.AvatarFile == null || request.AvatarFile.Length == 0)
            {
                ModelState.AddModelError("AvatarFile", "Avatar wajib diunggah.");
                return View(request);
            }

            // Upload avatar
            string avatarPath = await _fileService.SaveFileAsync(request.AvatarFile, "avatars");

            var user = new User
            {
                UserName = request.Email, // Username default menggunakan email
                Email = request.Email,
                Avatar = avatarPath,
                Occupation = request.Occupation,
                Experience = request.Experience,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Category");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(request);
        }





        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                // Cek apakah email valid
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Email tidak ditemukan.");
                    return View();
                }

                // Cek apakah password sesuai
                var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Home", "User");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Password tidak sesuai.");
                }
            }

            // Jika ada kesalahan atau validasi gagal
            ModelState.AddModelError(string.Empty, "Terjadi kesalahan pada login, coba lagi.");
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

        [Authorize(Roles = "SuperAdmin")] // Hanya SuperAdmin yang bisa mengubah role
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User tidak ditemukan.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles); // Hapus role lama
            var result = await _userManager.AddToRoleAsync(user, newRole); // Tambah role baru

            if (result.Succeeded)
            {
                return RedirectToAction("ManageUsers"); // Redirect ke halaman manajemen pengguna
            }

            ModelState.AddModelError("", "Gagal mengubah role.");
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

    }
}
