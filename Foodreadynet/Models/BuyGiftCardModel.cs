using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;

namespace FoodReady.WebUI.Models
{
    public class BuyGiftCardModel
    {
        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Buyer Email")]
        public string BuyerEmail { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Amount")]
        [Range(10.00, 500.00)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Recipient Email")]
        public string RecipientEmail { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Confirm Recipient Email")]
        [System.Web.Mvc.Compare("RecipientEmail", ErrorMessage = "The recipient email and confirmation recipient email do not match.")]
        public string Re_RecipientEmail { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Message to Recipient")]
        public string Message { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string BillFirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string BillLastName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Card Type")]
        public string CardType { get; set; }

        [RegularExpression("^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[4,7][0-9]{13}|6011[0-9]{12})$", ErrorMessage = "*")]
        [Required(ErrorMessage = "*")]
        //[StringLength(20, MinimumLength = 16)]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^([0-9]{3}|[0-9]{4})$", ErrorMessage = "*")]
        [StringLength(4, MinimumLength = 3)]
        [Display(Name = "Security Code")]
        public string SecurityCode { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Expiration Month")]
        public string ExpirationMonth { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Expiration Year")]
        public string ExpirationYear { get; set; }

    }
}