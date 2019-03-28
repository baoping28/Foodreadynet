using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FoodReady.WebUI.Models
{
    public class DeliveryCheckModel
    {
        [HiddenInput(DisplayValue = false)]
        public int BizInfoId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string BizName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[1-9][0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
    }
}