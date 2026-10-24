using FoodDeliveryApp.Data;
using FoodDeliveryApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDeliveryApp.Controllers
{
    public class AdminAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminAccountController(ApplicationDbContext ctx) => _context =
    ctx;

        // GET: /AdminAccount/Login → Affiche le formulaire 
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /AdminAccount/Login → Vérifie + crée la session 
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var admin = _context.Admins
                .FirstOrDefault(a => a.Email == model.Email && a.Password ==
            model.Password);

            if (admin == null)
            {
                ModelState.AddModelError("", "Invalid Admin Credentials.");
                return View(model);
            }

            // Construction des claims → informations stockées dans le cookie 
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, admin.Name),
            new Claim(ClaimTypes.Email, admin.Email),
            new Claim("AdminId", admin.AdminId.ToString()),
            new Claim(ClaimTypes.Role, "Admin")  // ← Rôle critique pour [Authorize]
        };

            var identity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme);
            await
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("Orders", "Admin");
        }

        // /AdminAccount/Logout → Supprime le cookie 
        public async Task<IActionResult> Logout()
        {
            await
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");

        }
    }
}
