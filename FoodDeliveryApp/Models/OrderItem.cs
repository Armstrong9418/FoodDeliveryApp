using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }  // Navigation → commande parente 

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }    // Navigation → plat commandé 

        public int Quantity { get; set; }  // Nombre d'unités commandées 

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }  // Prix au moment de la comma
    }
}
