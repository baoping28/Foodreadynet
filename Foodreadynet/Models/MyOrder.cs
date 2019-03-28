using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Models
{
    public class MyOrder
    {
        public List<Order> Orders { get; set; }
        public decimal PeriodSubtotal { get; set; }
        public decimal PeriodTotal { get; set; }
        public decimal PeriodOrderTax { get; set; }
        public decimal PeriodServiceCharge { get; set; }
        public decimal PeriodDriverTip { get; set; }
        public decimal PeriodDiscountAmount { get; set; }
        public decimal PeriodDeliveryCharge { get; set; }
    }
}