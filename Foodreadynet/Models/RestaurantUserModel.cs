using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FoodReady.WebUI.Models
{
    public class RestaurantUserModel
    {
        [HiddenInput(DisplayValue = false)]
        public int UserDetailID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int AddressID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ContactInfoID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Street")]
        public string AddressLine { get; set; }

        [Display(Name = "Street2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Cross Street")]
        public string CrossStreet { get; set; }

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

        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Phone2")]
        public string Phone2 { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public FR.Domain.Model.Entities.AspNetUser User { get; set; }
    }
}