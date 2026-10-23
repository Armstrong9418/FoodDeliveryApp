namespace FoodDeliveryApp.ViewModels
{
    public class CustomerOrderViewModel
    {
        public int OrderId { get; set; }
        public decimal OrderTotalPrice { get; set; }
        public string Status { get; set; }        // "Pending", "Accepted", "Rejected" 
        public string OrderDateTime { get; set; } // Formaté en string 
        public List<CustomerOrderItemViewModel> Items { get; set; }
    }
    // Détail d'un article dans une commande 
    public class CustomerOrderItemViewModel
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
