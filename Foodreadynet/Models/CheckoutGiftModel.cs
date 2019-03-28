using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace FoodReady.WebUI.Models
{
    public class CheckoutGiftModel
    {
        public CheckoutGiftModel()
        {

            GCScheduleAssistances = new[]
        {
            new SelectListItem{Text="Today",Value="1"},
            new SelectListItem {Text="Tomorrow",Value="2"}
        };
            GCTimeAssistances = new[]
        {
            new SelectListItem{Text="ASAP",Value="ASAP"},
            new SelectListItem{Text="12:00 AM",Value="12:00 AM"},
            new SelectListItem {Text="12:10 AM",Value="12:10 AM"},
            new SelectListItem {Text="12:20 AM",Value="12:20 AM"},
            new SelectListItem{Text="12:30 AM",Value="12:30 AM"},
            new SelectListItem {Text="12:40 AM",Value="12:40 AM"},
            new SelectListItem{Text="12:50 AM",Value="12:50 AM"},
            new SelectListItem {Text="1:00 AM",Value="1:00 AM"},
            new SelectListItem {Text="1:10 AM",Value="1:10 AM"},
            new SelectListItem {Text="1:20 AM",Value="1:20 AM"},
            new SelectListItem{Text="1:30 AM",Value="1:30 AM"},
            new SelectListItem {Text="1:40 AM",Value="1:40 AM"},
            new SelectListItem{Text="1:50 AM",Value="1:50 AM"},
            new SelectListItem {Text="2:00 AM",Value="2:00 AM"},
            new SelectListItem {Text="2:10 AM",Value="2:10 AM"},
            new SelectListItem {Text="2:20 AM",Value="2:20 AM"},
            new SelectListItem{Text="2:30 AM",Value="2:30 AM"},
            new SelectListItem {Text="2:40 AM",Value="2:40 AM"},
            new SelectListItem{Text="2:50 AM",Value="2:50 AM"},
            new SelectListItem {Text="3:00 AM",Value="3:00 AM"},
            new SelectListItem {Text="3:10 AM",Value="3:10 AM"},
            new SelectListItem {Text="3:20 AM",Value="3:20 AM"},
            new SelectListItem{Text="3:30 AM",Value="3:30 AM"},
            new SelectListItem {Text="3:40 AM",Value="3:40 AM"},
            new SelectListItem{Text="3:50 AM",Value="3:50 AM"},
            new SelectListItem {Text="4:00 AM",Value="4:00 AM"},
            new SelectListItem {Text="4:10 AM",Value="4:10 AM"},
            new SelectListItem {Text="4:20 AM",Value="4:20 AM"},
            new SelectListItem{Text="4:30 AM",Value="4:30 AM"},
            new SelectListItem {Text="4:40 AM",Value="4:40 AM"},
            new SelectListItem{Text="4:50 AM",Value="4:50 AM"},
            new SelectListItem {Text="5:00 AM",Value="5:00 AM"},
            new SelectListItem {Text="5:10 AM",Value="5:10 AM"},
            new SelectListItem {Text="5:20 AM",Value="5:20 AM"},
            new SelectListItem{Text="5:30 AM",Value="5:30 AM"},
            new SelectListItem {Text="5:40 AM",Value="5:40 AM"},
            new SelectListItem{Text="5:50 AM",Value="5:50 AM"},
            new SelectListItem {Text="6:00 AM",Value="6:00 AM"},
            new SelectListItem {Text="6:10 AM",Value="6:10 AM"},
            new SelectListItem {Text="6:20 AM",Value="6:20 AM"},
            new SelectListItem{Text="6:30 AM",Value="6:30 AM"},
            new SelectListItem {Text="6:40 AM",Value="6:40 AM"},
            new SelectListItem{Text="6:50 AM",Value="6:50 AM"},
            new SelectListItem {Text="7:00 AM",Value="7:00 AM"},
            new SelectListItem {Text="7:10 AM",Value="7:10 AM"},
            new SelectListItem {Text="7:20 AM",Value="7:20 AM"},
            new SelectListItem{Text="7:30 AM",Value="7:30 AM"},
            new SelectListItem {Text="7:40 AM",Value="7:40 AM"},
            new SelectListItem{Text="7:50 AM",Value="7:50 AM"},
            new SelectListItem {Text="8:00 AM",Value="8:00 AM"},
            new SelectListItem {Text="8:10 AM",Value="8:10 AM"},
            new SelectListItem {Text="8:20 AM",Value="8:20 AM"},
            new SelectListItem{Text="8:30 AM",Value="8:30 AM"},
            new SelectListItem {Text="8:40 AM",Value="8:40 AM"},
            new SelectListItem{Text="8:50 AM",Value="8:50 AM"},
            new SelectListItem {Text="9:00 AM",Value="9:00 AM"},
            new SelectListItem {Text="9:10 AM",Value="9:10 AM"},
            new SelectListItem {Text="9:20 AM",Value="9:20 AM"},
            new SelectListItem{Text="9:30 AM",Value="9:30 AM"},
            new SelectListItem {Text="9:40 AM",Value="9:40 AM"},
            new SelectListItem{Text="9:50 AM",Value="9:50 AM"},
            new SelectListItem {Text="10:00 AM",Value="10:00 AM"},
            new SelectListItem {Text="10:10 AM",Value="10:10 AM"},
            new SelectListItem {Text="10:20 AM",Value="10:20 AM"},
            new SelectListItem{Text="10:30 AM",Value="10:30 AM"},
            new SelectListItem {Text="10:40 AM",Value="10:40 AM"},
            new SelectListItem{Text="10:50 AM",Value="10:50 AM"},
            new SelectListItem {Text="11:00 AM",Value="11:00 AM"},
            new SelectListItem {Text="11:10 AM",Value="11:10 AM"},
            new SelectListItem {Text="11:20 AM",Value="11:20 AM"},
            new SelectListItem{Text="11:30 AM",Value="11:30 AM"},
            new SelectListItem {Text="11:40 AM",Value="11:40 AM"},
            new SelectListItem{Text="11:50 AM",Value="11:50 AM"},
            new SelectListItem {Text="12:00 PM",Value="12:00 PM"},
            new SelectListItem {Text="12:10 PM",Value="12:10 PM"},
            new SelectListItem {Text="12:20 PM",Value="12:20 PM"},
            new SelectListItem{Text="12:30 PM",Value="12:30 PM"},
            new SelectListItem {Text="12:40 PM",Value="12:40 PM"},
            new SelectListItem{Text="12:50 PM",Value="12:50 PM"},
            new SelectListItem {Text="1:00 PM",Value="1:00 PM"},
            new SelectListItem {Text="1:10 PM",Value="1:10 PM"},
            new SelectListItem {Text="1:20 PM",Value="1:20 PM"},
            new SelectListItem{Text="1:30 PM",Value="1:30 PM"},
            new SelectListItem {Text="1:40 PM",Value="1:40 PM"},
            new SelectListItem{Text="1:50 PM",Value="1:50 PM"},
            new SelectListItem {Text="2:00 PM",Value="2:00 PM"},
            new SelectListItem {Text="2:10 PM",Value="2:10 PM"},
            new SelectListItem {Text="2:20 PM",Value="2:20 PM"},
            new SelectListItem{Text="2:30 PM",Value="2:30 PM"},
            new SelectListItem {Text="2:40 PM",Value="2:40 PM"},
            new SelectListItem{Text="2:50 PM",Value="2:50 PM"},
            new SelectListItem {Text="3:00 PM",Value="3:00 PM"},
            new SelectListItem {Text="3:10 PM",Value="3:10 PM"},
            new SelectListItem {Text="3:20 PM",Value="3:20 PM"},
            new SelectListItem{Text="3:30 PM",Value="3:30 PM"},
            new SelectListItem {Text="3:40 PM",Value="3:40 PM"},
            new SelectListItem{Text="3:50 PM",Value="3:50 PM"},
            new SelectListItem {Text="4:00 PM",Value="4:00 PM"},
            new SelectListItem {Text="4:10 PM",Value="4:10 PM"},
            new SelectListItem {Text="4:20 PM",Value="4:20 PM"},
            new SelectListItem{Text="4:30 PM",Value="4:30 PM"},
            new SelectListItem {Text="4:40 PM",Value="4:40 PM"},
            new SelectListItem{Text="4:50 PM",Value="4:50 PM"},
            new SelectListItem {Text="5:00 PM",Value="5:00 PM"},
            new SelectListItem {Text="5:10 PM",Value="5:10 PM"},
            new SelectListItem {Text="5:20 PM",Value="5:20 PM"},
            new SelectListItem{Text="5:30 PM",Value="5:30 PM"},
            new SelectListItem {Text="5:40 PM",Value="5:40 PM"},
            new SelectListItem{Text="5:50 PM",Value="5:50 PM"},
            new SelectListItem {Text="6:00 PM",Value="6:00 PM"},
            new SelectListItem {Text="6:10 PM",Value="6:10 PM"},
            new SelectListItem {Text="6:20 PM",Value="6:20 PM"},
            new SelectListItem{Text="6:30 PM",Value="6:30 PM"},
            new SelectListItem {Text="6:40 PM",Value="6:40 PM"},
            new SelectListItem{Text="6:50 PM",Value="6:50 PM"},
            new SelectListItem {Text="7:00 PM",Value="7:00 PM"},
            new SelectListItem {Text="7:10 PM",Value="7:10 PM"},
            new SelectListItem {Text="7:20 PM",Value="7:20 PM"},
            new SelectListItem{Text="7:30 PM",Value="7:30 PM"},
            new SelectListItem {Text="7:40 PM",Value="7:40 PM"},
            new SelectListItem{Text="7:50 PM",Value="7:50 PM"},
            new SelectListItem {Text="8:00 PM",Value="8:00 PM"},
            new SelectListItem {Text="8:10 PM",Value="8:10 PM"},
            new SelectListItem {Text="8:20 PM",Value="8:20 PM"},
            new SelectListItem{Text="8:30 PM",Value="8:30 PM"},
            new SelectListItem {Text="8:40 PM",Value="8:40 PM"},
            new SelectListItem{Text="8:50 PM",Value="8:50 PM"},
            new SelectListItem {Text="9:00 PM",Value="9:00 PM"},
            new SelectListItem {Text="9:10 PM",Value="9:10 PM"},
            new SelectListItem {Text="9:20 PM",Value="9:20 PM"},
            new SelectListItem{Text="9:30 PM",Value="9:30 PM"},
            new SelectListItem {Text="9:40 PM",Value="9:40 PM"},
            new SelectListItem{Text="9:50 PM",Value="9:50 PM"},
            new SelectListItem {Text="10:00 PM",Value="10:00 PM"},
            new SelectListItem {Text="10:10 PM",Value="10:10 PM"},
            new SelectListItem {Text="10:20 PM",Value="10:20 PM"},
            new SelectListItem{Text="10:30 PM",Value="10:30 PM"},
            new SelectListItem {Text="10:40 PM",Value="10:40 PM"},
            new SelectListItem{Text="10:50 PM",Value="10:50 PM"},
            new SelectListItem {Text="11:00 PM",Value="11:00 PM"},
            new SelectListItem {Text="11:10 PM",Value="11:10 PM"},
            new SelectListItem {Text="11:20 PM",Value="11:20 PM"},
            new SelectListItem{Text="11:30 PM",Value="11:30 PM"},
            new SelectListItem {Text="11:40 PM",Value="11:40 PM"},
            new SelectListItem{Text="11:50 PM",Value="11:50 PM"},
        };
        }

        [HiddenInput(DisplayValue = false)]
        public int GCGiftcardID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public decimal GCBalance { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string GCUserID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string GCFirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string GCLastName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Street")]
        public string GCAddressLine { get; set; }

        [Display(Name = "Street2")]
        public string GCAddressLine2 { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "City")]
        public string GCCity { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "State")]
        public string GCState { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[1-9][0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Postal Code")]
        public string GCZipCode { get; set; }

        [Display(Name = "Room Number")]
        public string GCRoomNumber { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Phone")]
        public string GCPhone { get; set; }


        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string GCEmail { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string GCBillFirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string GCBillLastName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Card Type")]
        public string GCCardType { get; set; }

        [RegularExpression("^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[4,7][0-9]{13}|6011[0-9]{12})$", ErrorMessage = "*")]
        [Required(ErrorMessage = "*")]
        //[StringLength(20, MinimumLength = 16)]
        [Display(Name = "Card Number")]
        public string GCCardNumber { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^([0-9]{3}|[0-9]{4})$", ErrorMessage = "*")]
        [StringLength(4, MinimumLength = 3)]
        [Display(Name = "Security Code")]
        public string GCSecurityCode { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Expiration Month")]
        public string GCExpirationMonth { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Expiration Year")]
        public string GCExpirationYear { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Instructions")]
        public string GCInstructions { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Schedule Date")]
        public string GCScheduleDate { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Schedule Time")]
        public string GCScheduleTime { get; set; }

        public ShoppingCart GCCart { get; set; }


        public int GiftCardStatus { get; set; }
        public IEnumerable<SelectListItem> GCScheduleAssistances { get; set; }
        public IEnumerable<SelectListItem> GCTimeAssistances { get; set; }
        public BizInfo GCBizInfo { get; set; }
    }
}