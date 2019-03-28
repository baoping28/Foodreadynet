using System.Collections.Generic;
using FR.Domain.Model.Entities;
using System.Web.Mvc;

namespace FoodReady.WebUI.Models
{
    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }
        public List<string> ListAllcities { get; set; }
        public string selectedcity { get; set; }
        public BizInfo Bizinfo { get; set; }
        public decimal PeriodSubtotal { get; set; }
        public decimal PeriodBizSubtotal { get; set; }
        public decimal PeriodTotal { get; set; }
        public decimal PeriodBizTotal { get; set; }
        public decimal PeriodOrderTax { get; set; }
        public decimal PeriodServiceCharge { get; set; }
        public decimal PeriodDriverTip { get; set; }
        public decimal PeriodDiscountAmount { get; set; }
        public decimal PeriodDeliveryCharge { get; set; }
        public List<SelectListItem> AllCities(List<string> allcities)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            foreach (var c in allcities)
            {
                l.Add(new SelectListItem { Text = c, Value = c, Selected = selectedcity.ToLower() == c.ToLower() });
            }
            return l;
        }
    }
}