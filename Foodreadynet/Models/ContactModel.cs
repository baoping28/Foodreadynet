using System.ComponentModel.DataAnnotations;

namespace FoodReady.WebUI.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Name")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }

        [Display(Name = "Order Date")]
        public string OrderDate { get; set; }

        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}