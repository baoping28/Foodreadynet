using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Models
{
    public class FreshShareCartOthersModel
    {
        public SharedShoppingCart SharedCart { get; set; }
        public ShoppingCart Cart { get; set; }
    }
}