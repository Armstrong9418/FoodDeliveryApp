using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }  // Navigation → Client 

        public DateTime OrderDateTime { get; set; }  // Heure de passage de commande

       [Required]
        public OrderStatus Status { get; set; }  // Pending / Accepted /  Rejected

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }  // Total calculé par le contrôleur

        public List<OrderItem> OrderItems { get; set; }  // Navigation → lignes
    }
}
