using System;
using System.ComponentModel.DataAnnotations;
namespace FoodReady.WebUI.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string Password { get; set; }

         [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "New password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ReturnToken { get; set; }

    }
}