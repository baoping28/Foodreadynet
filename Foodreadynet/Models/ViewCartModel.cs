using FR.Domain.Model.Entities;
namespace FoodReady.WebUI.Models
{
    public class ViewCartModel
    {
        public ShoppingCart Cart { get; set; }
        public BizInfo BizInfo { get; set; }
    }
}