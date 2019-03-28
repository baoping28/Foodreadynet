using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;
namespace FoodReady.WebUI.Models
{
    public class RestaurantJoinModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Restaurant Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[1-9][0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Postal Code")]
        public string ZipCode { get; set; }


        [Required(ErrorMessage = "*")]
        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Restaurant Fax")]
        public string Fax { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Do you deliver?")]
        public string Delivery { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 100.00)]
        [Display(Name = "Delivery Radius")]
        public decimal Radius { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, double.MaxValue)]
        [Display(Name = "Order Minimum")]
        public decimal OrderMinimum { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 100.00)]
        [Display(Name = "Tax Percentage")]
        public decimal TaxPercentage { get; set; }
        
        [Required(ErrorMessage = "*")]
        [Display(Name = "Primary Cuisine Type")]
        public string CuisineType { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        public List<SelectListItem> CuisineAssistances { get; set; }
    }
}