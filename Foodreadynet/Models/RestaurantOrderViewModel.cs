using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Models
{
    public class RestaurantOrderViewModel
    {
        public List<Order> Orders { get; set; }
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
    }
}