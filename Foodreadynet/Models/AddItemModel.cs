using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;
namespace FoodReady.WebUI.Models
{

    public class AddItemModel
    {
        public AddItemModel()
        {

            SpicyChoices = new[] {
                new SelectListItem{Text="How spicy? (Regular)",Value="Regular"},
                new SelectListItem{Text="Mild",Value="Mild"},
                new SelectListItem {Text="Medium",Value="Medium"},
                new SelectListItem {Text="Hot",Value="Hot"},
                new SelectListItem {Text="Very Hot",Value="Very Hot"}
        };
            QtyList = new SelectList(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" });
            FamilyQtyList = new SelectList(new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" });
            FreeToppingAssistances = new List<CheckBoxViewModel>();
            ToppingAssistances = new List<CheckBoxViewModel>();
            DressingAssistances = new List<CheckBoxViewModel>();
            AddSideAssistances = new List<AddSideCheckBoxModel>();
        }

        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Dinner Quantity")]
        public int FamilyQty { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }


        [Display(Name = "Topping")]
        public string Topping { get; set; }

        [Display(Name = "Dressing")]
        public string Dressing { get; set; }

        [Display(Name = "Dressing Choice ")]
        public string DressingChoice { get; set; }

        [Display(Name = "Side Choice")]
        public string SideChoice { get; set; }

        [Display(Name = "Sauce Choice")]
        public string SauceChoice { get; set; }

        [Display(Name = "Crust Choice")]
        public string CrustChoiceID { get; set; }

        [Display(Name = "Cheese Choice")]
        public string CheeseAmountID { get; set; }

        [Display(Name = "Coupon Choice")]
        public string CouponChoice { get; set; }

        [Display(Name = "How Spicy")]
        public string HowSpicy { get; set; }

        [Display(Name = "Driver Tip")]
        public string DriverTip { get; set; } 

        public string ProductSize { get; set; }

        public SharedShoppingCart SharedCart { get; set; }
        public ShoppingCart Cart { get; set; }
        public BizInfo BizInfo { get; set; }
        public Product Product { get; set; }
        public SelectList QtyList { get; set; }
        public SelectList FamilyQtyList { get; set; }
        public IEnumerable<SelectListItem> SpicyChoices { get; set; }
        public SelectList DefaultToppings { get; set; }
        public List<CheckBoxViewModel> FreeToppingAssistances { get; set; }
        public List<CheckBoxViewModel> ToppingAssistances { get; set; }
        public List<AddSideCheckBoxModel> AddSideAssistances { get; set; }
        public List<CheckBoxViewModel> DressingAssistances { get; set; }
        public List<SelectListItem> SizeAssistances { get; set; }
        public List<DiscountCoupon> DiscountCouponList { get; set; }
        public List<FreeItemCoupon> FreeItemCouponList { get; set; }
        public List<SelectListItem> FamilyQtyDropdown(List<FamilyMeal> lfm )
        {
            string t = "";
            int n = 2;
            List<SelectListItem> l = new List<SelectListItem>();
            foreach (var f in lfm)
            {
                if (n == 2)
                {
                    t = n.ToString() + " people ($" + (n * this.Product.FinalUnitPrice).ToString("N2") + ")";
                }
                else
                {
                    t = n.ToString() + " people ($" + (n * this.Product.FinalUnitPrice).ToString("N2") + ") -- then add " + f.MealToAdd;
                }
               
                l.Add(new SelectListItem { Text = t, Value = n.ToString() });
                n++;
            }
            return l;
        }
        public List<SelectListItem> SideChoices(List<SideChoice> lsc)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            foreach (var s in lsc)
            {
                l.Add(new SelectListItem { Text = s.Title, Value = s.Title });
            }
            return l;
        }
        public List<SelectListItem> SauceChoices(List<SauceChoice> lpd)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            l.Add(new SelectListItem { Text = "No Thanks", Value = "No Thanks" });
            foreach (var d in lpd)
            {
                l.Add(new SelectListItem { Text = d.Title, Value = d.Title });
            }
            return l;
        }
        public List<SelectListItem> CrustChoices(List<CrustChoice> lcc)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            foreach (var s in lcc)
            {
                string des = s.Price == 0.0m ? string.Empty : " ($" + s.Price.ToString("N2") + " extra)";
                l.Add(new SelectListItem { Text = s.Title + des, Value = s.CrustChoiceId.ToString() });
            }
            return l;
        }
        public List<SelectListItem> CheeseChoices(List<CheeseAmount> lcc)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            foreach (var s in lcc)
            {
                string des = s.Price == 0.0m ? string.Empty : " ($" + s.Price.ToString("N2") + " extra)";
                l.Add(new SelectListItem { Text = s.Title + des, Value = s.CheeseAmountId.ToString() });
            }
            return l;
        }
        public List<SelectListItem> DressingChoices(List<ProductDressing> lpd)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            l.Add(new SelectListItem { Text = "No Thanks", Value = "No Thanks" });
            foreach (var d in lpd)
            {
                l.Add(new SelectListItem { Text = d.DressingTitle, Value = d.DressingTitle });
            }
            return l;
        }
        public List<SelectListItem> CouponChoices(string selectedV, decimal subtotal)
        {
            decimal t = 0.0m;
            List<SelectListItem> l = new List<SelectListItem>();
            l.Add(new SelectListItem { Text = "Apply a coupon? No, Thanks", Value = "" });
            foreach (var d in DiscountCouponList)
            {
                t = d.DiscountPercentage == 0 ? 0.0m : (subtotal * d.DiscountPercentage / 100);
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
            if (this.SharedCart == null)
            {
                if (this.Cart.Lines.Sum(x => x.Quantity) == 0 || (this.Cart.OrderMinimum > this.Cart.SubTotal() && this.Cart.IsDelivery) || (this.BizInfo.Delivery == false && this.Cart.IsDelivery))
                {
                    result = false;
                }
            }
            else
            {
                bool isBoss = (this.Cart.PersonName == this.SharedCart.PartyBossName && string.IsNullOrEmpty(this.Cart.PersonName) == false);
                if (!isBoss || this.SharedCart.PartyTotalItems == 0 || (this.SharedCart.PartyOrderMinimum() > this.SharedCart.PartySubTotal() && this.SharedCart.IsPartyDelivery) || (this.BizInfo.Delivery == false && this.SharedCart.IsPartyDelivery))
                {
                    result = false;
                }
            }
            return result;
        }
    }
}