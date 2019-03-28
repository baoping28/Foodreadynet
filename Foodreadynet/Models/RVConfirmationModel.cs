using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Models
{
    public class RVConfirmationModel
    {
       [HiddenInput(DisplayValue = false)]
        public int BizInfoId { get; set; }

       [HiddenInput(DisplayValue = false)]
       public string RVDate { get; set; }

       [HiddenInput(DisplayValue = false)]
       public string RVTime { get; set; }

       [HiddenInput(DisplayValue = false)]
       public int RVNum { get; set; }

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

        [Display(Name = "Message")]
        public string Message { get; set; }

        public BizInfo Bizinfo { get; set; }
    }
}