using System;
using System.ComponentModel.DataAnnotations;

namespace FoodReady.WebUI.Models
{
    public class LostPasswordModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Account email")]
        [EmailAddress(ErrorMessage = "Not a valid email--what are you trying to do here?")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        public string Email { get; set; }
    }
}