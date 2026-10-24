using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FoodDeliveryApp.Controllers
{
    [Authorize(Roles = "Admin")]  // Seul le rôle Admin peut accéder 
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext ctx) => _context = ctx;

        // GET: /Admin/Products 
        public IActionResult Products()
        {
            var items = _context.Items.AsNoTracking().ToList();
            return View(items);
        }

        // GET: /Admin/Orders — Page principale avec 3 onglets 
        public IActionResult Orders()
        {
            // ÉTAPE 1 : Auto-rejet des commandes Pending depuis plus de 5 minutes
            AutoRejectOldPendingOrders();

            // ÉTAPE 2 : Filtrer uniquement les commandes d'aujourd'hui 
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var ordersToday = _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Item)
                .Where(o => o.OrderDateTime >= today && o.OrderDateTime <tomorrow)
                .OrderByDescending(o => o.OrderDateTime)
                .ToList();

            // ÉTAPE 3 : Grouper par statut 
            ViewBag.PendingOrders =
            MapToAdminOrdersViewModel(ordersToday.Where(o => o.Status == OrderStatus.Pending));
                    ViewBag.AcceptedOrders =
            MapToAdminOrdersViewModel(ordersToday.Where(o => o.Status == OrderStatus.Accepted));
                    ViewBag.RejectedOrders =
            MapToAdminOrdersViewModel(ordersToday.Where(o => o.Status == OrderStatus.Rejected));
            return View();
        }

        // POST: /Admin/AcceptOrder 
        [HttpPost]

        public IActionResult AcceptOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null && order.Status == OrderStatus.Pending)
            { order.Status = OrderStatus.Accepted; _context.SaveChanges(); }
            return RedirectToAction("Orders");
        }

        // POST: /Admin/RejectOrder 
        [HttpPost]
        public IActionResult RejectOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null && order.Status == OrderStatus.Pending)
            { order.Status = OrderStatus.Rejected; _context.SaveChanges(); }
            return RedirectToAction("Orders");
        }

        // Rejet automatique des commandes Pending depuis plus de 5 minutes 
        private void AutoRejectOldPendingOrders()
        {
            var fiveMinutesAgo = DateTime.Now.AddMinutes(-5);
            var oldPending = _context.Orders
                .Where(o => o.Status == OrderStatus.Pending
                         && o.OrderDateTime < fiveMinutesAgo)
                .ToList();
            foreach (var order in oldPending)
                order.Status = OrderStatus.Rejected;
            _context.SaveChanges();
        }

        // Helper : conversion Orders → AdminOrdersViewModel 
        private List<AdminOrdersViewModel>
        MapToAdminOrdersViewModel(IEnumerable<Order> orders)
        {
            return orders.Select(o => new AdminOrdersViewModel
            {
                OrderId = o.OrderId,
                CustomerName = o.Customer.CustomerName,
                CustomerEmail = o.Customer.Email,
                Items = o.OrderItems.Select(oi =>
                    $"{oi.Item.ItemName} x{oi.Quantity} (${oi.UnitPrice})").ToList(),
                TotalPrice = o.Price,
                OrderDateTime = o.OrderDateTime.ToString("g")
            }).ToList();
        }
    }
} 
    

