using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Controllers
{
    // [Authorize] ← Redirige vers /Home/Index si non connecté 
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext ctx) => _context = ctx;

        // GET: /Customer/PlaceOrder → Charge la liste des plats disponibles 
        [HttpGet]
        public IActionResult PlaceOrder()
        {
            var customerIdClaim = User.FindFirst("CustomerId");
            if (customerIdClaim == null) return RedirectToAction("Login", "CustomerAccount");

            var items = _context.Items.AsNoTracking()
                .Where(i => i.IsAvailable)   // Seulement les plats disponibles 
                .ToList();

            var model = new MultipleOrderItemViewModel
            {
                Items = items.Select(i => new OrderItemViewModel
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Price = i.Price,
                    IsAvailable = i.IsAvailable,
                    Quantity = 0  // Par défaut : 0 = non sélectionné 
                }).ToList()
            };
            return View(model);
        }

        // POST: /Customer/PlaceOrder → Crée la commande en base 
        [HttpPost]
        public IActionResult PlaceOrder(MultipleOrderItemViewModel model)
        {
            var customerIdClaim = User.FindFirst("CustomerId");
            if (customerIdClaim == null) return RedirectToAction("Login", "CustomerAccount");
            int customerId = int.Parse(customerIdClaim.Value);

            // On ne garde que les plats avec quantité > 0 
            var selectedItems = model.Items.Where(x => x.Quantity >
    0).ToList();
            if (!selectedItems.Any())
            {
                ModelState.AddModelError("", "No items selected."); return
    View(model);
            }

            var order = new Order
            {
                CustomerId = customerId,
                OrderDateTime = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            foreach (var sel in selectedItems)
            {
                var dbItem = _context.Items.FirstOrDefault(i => i.ItemId ==
    sel.ItemId && i.IsAvailable);
                if (dbItem == null) continue;  // Sécurité : ignorer si plat retiré entre temps


                order.OrderItems.Add(new OrderItem
                {
                    ItemId = dbItem.ItemId,
                    Quantity = sel.Quantity,
                    UnitPrice = dbItem.Price  // Prix réel depuis la BDD ! 
                });
            }

            order.Price = order.OrderItems.Sum(oi => oi.UnitPrice *
    oi.Quantity);
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("MyOrders");
        }

        // GET: /Customer/MyOrders → Historique des commandes du client connecté
        [HttpGet]
        public IActionResult MyOrders()
        {
            var customerIdClaim = User.FindFirst("CustomerId");
            if (customerIdClaim == null) return RedirectToAction("Login", "CustomerAccount");
            int customerId = int.Parse(customerIdClaim.Value);

            var orders = _context.Orders.AsNoTracking()
                .Include(o => o.OrderItems)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDateTime)
                .ToList();

            // Noms des plats, pour l'affichage
            ViewBag.ItemNames = _context.Items.AsNoTracking()
                .ToDictionary(i => i.ItemId, i => i.ItemName);

            return View(orders);
        }
    }
}


