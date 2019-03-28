using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IFreeItemCouponRepository
    {
        List<FreeItemCoupon> GetAllFreeItemCoupons();
        List<FreeItemCoupon> GetAllFreeItemCoupons(bool vActive);
        List<FreeItemCoupon> GetFreeItemCouponsByBizId(int vBizInfoId);
        List<FreeItemCoupon> GetFreeItemCouponsByBizId(int vBizInfoId, bool vActive);
        List<FreeItemCoupon> GetBizFreeItemCouponsByMinimum(int vBizInfoId, decimal vMinimum, bool vActive);
        FreeItemCoupon GetFreeItemCouponById(int vFreeItemCouponID);
        int GetAllFreeItemCouponCount(bool vActive);
        FreeItemCoupon AddFreeItemCoupon(int vFreeItemCouponID, int vBizInfoID, string vTitle, string vDescription,
                      decimal vUnitPrice, int vOrderMinimum, DateTime vStartDate, DateTime vExpirationDate,
                      DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        FreeItemCoupon AddFreeItemCoupon(FreeItemCoupon vFreeItemCoupon);
        bool LockFreeItemCoupon(FreeItemCoupon vFreeItemCoupon);
        bool UnlockFreeItemCoupon(FreeItemCoupon vFreeItemCoupon);
        bool DeleteFreeItemCoupon(FreeItemCoupon vFreeItemCoupon);
        bool DeleteFreeItemCoupon(int vFreeItemCouponID);
        bool UnDeleteFreeItemCoupon(FreeItemCoupon vFreeItemCoupon);
        FreeItemCoupon UpdateFreeItemCoupon(FreeItemCoupon vFreeItemCoupon);
    }
}