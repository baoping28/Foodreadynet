using FR.Domain.Model.Entities;
using System.Collections.Generic;

namespace FoodReady.WebUI.Models
{
    public class CouponViewModel
    {
        public List<DiscountCoupon> Discounts { get; set; }
        public List<FreeItemCoupon> FreeItems { get; set; }
        public BizInfo Bizinfo { get; set; }
    }
}