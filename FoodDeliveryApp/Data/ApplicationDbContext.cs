using FoodDeliveryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Date fixe pour le seed (DateTime.Today changerait à chaque migration)
        private static readonly DateTime SeedDate = new DateTime(2026, 1, 1);

        // Tables de la base de données 
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed : 1 admin 
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    AdminId = 1,
                    Name = "SuperAdmin",
                    Email = "admin@example.com",
                    Password = "admin@123",
                    PhoneNumber = "1234567890",
                    Role = "SuperAdmin"
                }
            );

            // Seed : 2 clients de test 
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    CustomerName = "John Doe",
                    Email = "john@example.com",
                    Password = "john@123",
                    PhoneNumber = "9876543210",
                    Address = "123 Main Street"
                },
                new Customer
                {
                    CustomerId = 2,
                    CustomerName = "Jane Smith",
                    Email = "jane@example.com",
                    Password = "jane@123",
                    PhoneNumber = "5554443322",
                    Address = "456 South Avenue"
                }
            );

            // Seed : 5 plats du menu 
            modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    ItemId = 1,
                    ItemName = "Margherita Pizza",
                    Description = "Classic cheese pizza",
                    Price = 9.99m,
                    IsAvailable = true
                },
                new Item
                {
                    ItemId = 2,
                    ItemName = "Veggie Burger",
                    Description = "Delicious burger with veggie patty",
                    Price = 7.49m,
                    IsAvailable = true
                },
                new Item
                {
                    ItemId = 3,
                    ItemName = "Pepperoni Pizza",
                    Description = "Pizza with pepperoni toppings",
                    Price = 10.99m,
                    IsAvailable = true
                },
                new Item
                {
                    ItemId = 4,
                    ItemName = "Chicken Burger",
                    Description = "Juicy chicken burger",
                    Price = 8.49m,
                    IsAvailable = true
                },
                new Item
                {
                    ItemId = 5,
                    ItemName = "French Fries",
                    Description = "Crispy golden fries",
                    Price = 3.49m,
                    IsAvailable = true
                }
            );

            // Seed : 6 commandes de test (Pending, Accepted, Rejected) 
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    OrderId = 1,
                    CustomerId = 1,
                    OrderDateTime = SeedDate.AddMinutes(10),
                    Status = OrderStatus.Pending,
                    Price = 9.99m
                },
                new Order
                {
                    OrderId = 2,
                    CustomerId = 2,
                    OrderDateTime = SeedDate.AddMinutes(25),
                    Status = OrderStatus.Accepted,
                    Price = 7.49m
                },
                new Order
                {
                    OrderId = 3,
                    CustomerId = 1,
                    OrderDateTime = SeedDate.AddMinutes(32),
                    Status = OrderStatus.Accepted,
                    Price = 10.98m
                },
                new Order
                {
                    OrderId = 4,
                    CustomerId = 2,
                    OrderDateTime = SeedDate.AddMinutes(45),
                    Status = OrderStatus.Pending,
                    Price = 10.99m
                },
                new Order
                {
                    OrderId = 5,
                    CustomerId = 2,
                    OrderDateTime = SeedDate.AddMinutes(45),
                    Status = OrderStatus.Rejected,
                    Price = 10.98m
                },
                new Order
                {
                    OrderId = 6,
                    CustomerId = 1,
                    OrderDateTime = SeedDate.AddMinutes(50),
                    Status = OrderStatus.Rejected,
                    Price = 10.99m
                }
            );

            // Seed : lignes de commande correspondantes 
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    OrderItemId = 1,
                    OrderId = 1,
                    ItemId = 1,
                    Quantity = 1,
                    UnitPrice = 9.99m
                },
                new OrderItem
                {
                    OrderItemId = 2,
                    OrderId = 2,
                    ItemId = 2,
                    Quantity = 1,
                    UnitPrice = 7.49m
                },
                new OrderItem
                {
                    OrderItemId = 3,
                    OrderId = 3,
                    ItemId = 2,
                    Quantity = 1,
                    UnitPrice = 7.49m
                },
                new OrderItem
                {
                    OrderItemId = 4,
                    OrderId = 3,
                    ItemId = 5,
                    Quantity = 1,
                    UnitPrice = 3.49m
                },
                new OrderItem
                {
                    OrderItemId = 5,
                    OrderId = 4,
                    ItemId = 3,
                    Quantity = 1,
                    UnitPrice = 10.99m
                },
                new OrderItem
                {
                    OrderItemId = 6,
                    OrderId = 5,
                    ItemId = 2,
                    Quantity = 1,
                    UnitPrice = 7.49m
                },
                new OrderItem
                {
                    OrderItemId = 7,
                    OrderId = 5,
                    ItemId = 5,
                    Quantity = 1,
                    UnitPrice = 3.49m
                },
                new OrderItem
                {
                    OrderItemId = 8,
                    OrderId = 6,
                    ItemId = 3,
                    Quantity = 1,
                    UnitPrice = 10.99m
                }
            );
        }
    }
}