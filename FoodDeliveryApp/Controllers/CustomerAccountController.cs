using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FoodDeliveryApp.Data;

namespace FoodDeliveryApp.Controllers
{
    public class CustomerAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerAccountController(ApplicationDbContext ctx) => _context = ctx;
        // POST: /CustomerAccount/Register 
        public IActionResult Register(CustomerRegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Vérification unicité de l'email 
            bool emailExists = _context.Customers.AsNoTracking()
                .Any(c => c.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("", "Email already in use.");
                return View(model);
            }

            var newCustomer = new Customer
            {
                CustomerName = model.CustomerName,
                Email = model.Email,
                Password = model.Password,  // ⚠️ En prod : hasher ! 
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };

            _context.Customers.Add(newCustomer);
            _context.SaveChanges();
            return RedirectToAction("Login");  // Redirection vers la connexion 
        }

        // POST: /CustomerAccount/Login 
        public async Task<IActionResult> Login(CustomerLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var customer = _context.Customers
                .FirstOrDefault(c => c.Email == model.Email && c.Password ==
        model.Password);
            if (customer == null)
            {
                ModelState.AddModelError("", "Invalid Customer Credentials."); return
        View(model);
            }

            var claims = new List<Claim> {
        new Claim(ClaimTypes.Name, customer.CustomerName),
        new Claim(ClaimTypes.Email, customer.Email),
        new Claim("CustomerId", customer.CustomerId.ToString()),
        new Claim(ClaimTypes.Role, "Customer")  // ← Rôle "Customer" 
    };
            var identity = new ClaimsIdentity(claims,
        CookieAuthenticationDefaults.AuthenticationScheme);
            await
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("MyOrders", "Customer");
        }
    }
}
