using System.ComponentModel.DataAnnotations;

namespace FoodReady.WebUI.Models
{
    public class ReferRestaurantModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Your Name")]
        public string YourName { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Your Email")]
        public string YourEmail { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Restaurant Phone")]
        public string RestaurantPhone { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Restaurant City")]
        public string RestaurantCity { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Restaurant State")]
        public string RestaurantState { get; set; }

    }
}