using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace FoodReady.WebUI.Models
{
    public class ReservationModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ReservationId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizRVInfoId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "RV Date")]
        public string RVDate { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "RV Time")]
        public string RVTime { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Party Size")]
        public int PartySize { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Special Requests")]
        public string Message { get; set; }
    }
}