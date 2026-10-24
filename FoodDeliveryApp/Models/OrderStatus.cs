namespace FoodDeliveryApp.Models
{
    public enum OrderStatus
    {
        Pending = 1,   // En attente de traitement admin 
        Accepted = 2,   // Accepté par l'admin 
        Rejected
    }
}
