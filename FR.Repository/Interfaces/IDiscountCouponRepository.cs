using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IDiscountCouponRepository
    {
        List<DiscountCoupon> GetAllDiscountCoupons();
        List<DiscountCoupon> GetAllDiscountCoupons(bool vActive);
        List<DiscountCoupon> GetDiscountCouponsByBizId(int vBizInfoId);
        List<DiscountCoupon> GetDiscountCouponsByBizId(int vBizInfoId, bool vActive);
        DiscountCoupon GetDiscountCouponsByBizIdPercent(int vBizInfoId, int vPercentage, bool vActive);
        List<DiscountCoupon> GetBizDiscountCouponsByMinimum(int vBizInfoId, decimal vMinimum, bool vActive);
        DiscountCoupon GetDiscountCouponById(int vDiscountCouponID);
        int GetAllDiscountCouponCount(bool vActive);
        DiscountCoupon AddDiscountCoupon(int vDiscountCouponID, int vBizInfoID, string vTitle, string vDescription, 
            int vDiscountPercentage, decimal vOrderMinimum, DateTime vStartDate, DateTime vExpirationDate, 
            DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        DiscountCoupon AddDiscountCoupon(DiscountCoupon vDiscountCoupon);
        bool LockDiscountCoupon(DiscountCoupon vDiscountCoupon);
        bool UnlockDiscountCoupon(DiscountCoupon vDiscountCoupon);
        bool DeleteDiscountCoupon(DiscountCoupon vDiscountCoupon);
        bool DeleteDiscountCoupon(int vDiscountCouponID);
        bool UnDeleteDiscountCoupon(DiscountCoupon vDiscountCoupon);
        DiscountCoupon UpdateDiscountCoupon(DiscountCoupon vDiscountCoupon);
    }
}