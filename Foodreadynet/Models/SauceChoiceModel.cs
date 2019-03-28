using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;


namespace FoodReady.WebUI.Models
{
    public class SauceChoiceModel
    {
        public SauceChoiceModel()
        {
            SauceProducts = new List<SelectListItem>();
        }
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SauceChoiceID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public Product Product { get; set; }
        public BizInfo Bizinfo { get; set; }
        public List<SauceChoice> LSauceChoice { get; set; }
        public List<SelectListItem> SauceProducts { get; set; }
    }
}