using System.Collections.Generic;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;
namespace FoodReady.WebUI.Models
{
    public class FreeItemModel
    {
        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int FreeItemID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Item Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Item Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Order Minimum")]
        public int OrderMinimum { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        public List<FreeItemCoupon> FreeItems { get; set; }

        public BizInfo Bizinfo { get; set; }
    }
}