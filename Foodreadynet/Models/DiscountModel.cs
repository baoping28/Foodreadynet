using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
namespace FoodReady.WebUI.Models
{
    public class DiscountModel
    {
        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int DiscountCouponID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Item Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Item Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Discount Percentage")]
        public int DiscountPercentage { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Order Minimum")]
        public decimal OrderMinimum { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        public List<DiscountCoupon> DiscountCoupons { get; set; }

        public BizInfo Bizinfo { get; set; }
    }
}