using System.Collections.Generic;
using System.Linq;
using FR.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FoodReady.WebUI.Models
{
    public class MenuViewModel
    {
        public ShoppingCart Cart { get; set; }
        public BizInfo BizInfo { get; set; }
        public BrowseHistory History { get; set; }
        public string ReturnUrl { get; set; }
        public List<BizCuisine> MenuList { get; set; }
        public BizCuisine FirstSubmenu { get; set; }
        public List<DiscountCoupon> DiscountCouponList { get; set; }
        public List<FreeItemCoupon> FreeItemCouponList { get; set; }
        public List<Product> ProductsWithImage { get; set; }
        public List<BizImage> BizImages { get; set; }
        [Display(Name = "Side Choice")]
        public string SideChoice { get; set; }
        [Display(Name = "Coupon Choice")]
        public string CouponChoice { get; set; }
        [Display(Name = "Driver Tip")]
        public string DriverTip { get; set; }
        public string MapMarkers { get; set; }
        public string Maplink { get; set; }
        public List<SelectListItem> CouponChoices(string selectedV, decimal subtotal)
        {
            decimal t = 0.0m;
            List<SelectListItem> l = new List<SelectListItem>();
            l.Add(new SelectListItem { Text = "Apply a coupon? No, Thanks", Value = "" });
            foreach (var d in DiscountCouponList)
            {
                t = d.DiscountPercentage ==0 ? 0.0m : (subtotal * d.DiscountPercentage / 100);
                l.Add(new SelectListItem { Text = d.Title + " ------ ( -$" + t.ToString("N2") + " )", Value = d.DiscountPercentage.ToString() + ";" + d.OrderMinimum.ToString("N2"), Selected = selectedV == d.DiscountPercentage.ToString() });
            }
            foreach (var f in FreeItemCouponList)
            {
                l.Add(new SelectListItem { Text = f.Title + " ( save: $" + f.UnitPrice.ToString("N2") + " )", Value = f.Title, Selected = selectedV == f.Title });
            }
            return l;
        }
        public bool CheckoutBtnShow()
        {
            bool result = true;
            if (this.Cart.Lines.Sum(x => x.Quantity) == 0 || (this.Cart.OrderMinimum > this.Cart.SubTotal() && this.Cart.IsDelivery) || (this.BizInfo.Delivery == false && this.Cart.IsDelivery))
            {
                result = false;
            }
            return result;
        }
    }
}