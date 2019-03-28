using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace FoodReady.WebUI.Models
{
    public class CheeseAmountModel
    {

        public CheeseAmountModel()
        {
            CheeseProducts = new List<SelectListItem>();
        }
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CheeseAmountID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 1000.00)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 100.00)]
        [Display(Name = "Restaurant Price")]
        public decimal BizPrice { get; set; }

        public Product Product { get; set; }
        public BizInfo Bizinfo { get; set; }
        public List<CheeseAmount> LCheeseAmount { get; set; }
        public List<SelectListItem> CheeseProducts { get; set; }
    }
}