using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace FoodReady.WebUI.Models
{
    [Serializable()]
    public class CheckBoxModel
    {
        public string BoxName { get; set; }
        public string BoxID { get; set; }
        public bool Checked { get; set; }
    }
    public class ProductToppingModel
    {
        public ProductToppingModel()
        {
            ToppingAssistances = new List<CheckBoxModel>();
            ToppingProducts = new List<SelectListItem>();
        }

        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ProductToppingID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ToppingID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ToppingTitle { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 100.00)]
        [Display(Name = "Extra Price")]
        public decimal ExtraPrice { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 100.00)]
        [Display(Name = "Increment Value")]
        public decimal Increment { get; set; }

        public Product Product { get; set; }
        public BizInfo Bizinfo { get; set; }
        public List<ProductTopping> LProductToppings { get; set; }
        public List<CheckBoxModel> ToppingAssistances { get; set; }
        public List<SelectListItem> ToppingProducts { get; set; }
    }
     public class ProductDressingModel
    {
        public ProductDressingModel()
        {
            DressingAssistances = new List<CheckBoxModel>();
            DressingProducts = new List<SelectListItem>();
        }

        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ProductDressingID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int DressingID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string DressingTitle { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 100.00)]
        [Display(Name = "Extra Price")]
        public decimal ExtraPrice { get; set; }

        public Product Product { get; set; }
        public BizInfo Bizinfo { get; set; }
        public List<ProductDressing> LProductDressings { get; set; }
        public List<CheckBoxModel> DressingAssistances { get; set; }
        public List<SelectListItem> DressingProducts { get; set; }
    }
}