using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PortalJob.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Home() => View();

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin() => View();

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IActionResult Manager() => View();
    }
}
