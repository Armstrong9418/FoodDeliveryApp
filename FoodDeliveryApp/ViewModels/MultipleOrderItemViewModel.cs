namespace FoodDeliveryApp.ViewModels
{
    public class MultipleOrderItemViewModel
    {
        public List<OrderItemViewModel> Items { get; set; }
    }

    // Représente UN plat dans le formulaire de commande 
    public class OrderItemViewModel
    {
        public int ItemId { get; set; }      // Transmis en champ caché (hidden)
        public string ItemName { get; set; } // Affiché dans le tableau 
        public decimal Price { get; set; }   // Affiché dans le tableau 
        public bool IsAvailable { get; set; }
        public int Quantity { get; set; }    // Saisi par l'utilisateur (default: 0) 
    }
}
