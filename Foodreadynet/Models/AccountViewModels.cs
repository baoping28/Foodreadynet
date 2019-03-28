using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IdentitySample.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

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
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class MyDetailModel
    {
        [HiddenInput(DisplayValue = false)]
        public int UserDetailID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserID { get; set; }


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


        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}