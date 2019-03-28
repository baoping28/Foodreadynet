using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class FreeItemCouponRepository : BaseShoppingCartRepository, IFreeItemCouponRepository
    {
        #region IFreeItemCouponRepository Members

        public List<FreeItemCoupon> GetAllFreeItemCoupons()
        {
            List<FreeItemCoupon> lFreeItemCoupons = default(List<FreeItemCoupon>);
            string lFreeItemCouponsKey = CacheKey + "_AllFreeItemCoupons";

            if (base.EnableCaching && (Cache[lFreeItemCouponsKey] != null))
            {
                lFreeItemCoupons = (List<FreeItemCoupon>)Cache[lFreeItemCouponsKey];
            }
            else
            {
                lFreeItemCoupons = (from lFreeItemCoupon in Shoppingctx.FreeItemCoupons
                                    select lFreeItemCoupon).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lFreeItemCouponsKey, lFreeItemCoupons, CacheDuration);
                }
            }
            return lFreeItemCoupons;
        }
        public List<FreeItemCoupon> GetAllFreeItemCoupons(bool vActive)
        {
            return GetAllFreeItemCoupons().Where(e => e.Active == vActive).ToList();
        }

        public List<FreeItemCoupon> GetFreeItemCouponsByBizId(int vBizInfoId)
        {
            return GetAllFreeItemCoupons().Where(e => e.BizInfoId == vBizInfoId).ToList();
        }
        public List<FreeItemCoupon> GetFreeItemCouponsByBizId(int vBizInfoId, bool vActive)
        {
            return GetAllFreeItemCoupons(vActive).Where(e => e.BizInfoId == vBizInfoId).ToList();
        }
        public List<FreeItemCoupon> GetBizFreeItemCouponsByMinimum(int vBizInfoId, decimal vMinimum, bool vActive)
        {
            return GetFreeItemCouponsByBizId(vBizInfoId, vActive).Where(e => e.OrderMinimum < vMinimum && e.IsFreeItemCouponDateOK).ToList();
        }
        public FreeItemCoupon GetFreeItemCouponById(int vFreeItemCouponID)
        {
            return Shoppingctx.FreeItemCoupons.FirstOrDefault(e => e.FreeItemId == vFreeItemCouponID);
        }

        public int GetAllFreeItemCouponCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public FreeItemCoupon AddFreeItemCoupon(int vFreeItemCouponID, int vBizInfoID, string vTitle, string vDescription, decimal vUnitPrice, int vOrderMinimum, DateTime vStartDate, DateTime vExpirationDate, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            FreeItemCoupon lFreeItemCoupon = new FreeItemCoupon();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vFreeItemCouponID > 0)
                {
                    lFreeItemCoupon = frctx.FreeItemCoupons.FirstOrDefault(u => u.FreeItemId == vFreeItemCouponID);
                    lFreeItemCoupon.Title = vTitle;
                    lFreeItemCoupon.Description = vDescription;
                    lFreeItemCoupon.UnitPrice = vUnitPrice;
                    lFreeItemCoupon.OrderMinimum = vOrderMinimum;
                    lFreeItemCoupon.StartDate = vStartDate;
                    lFreeItemCoupon.ExpirationDate = vExpirationDate;

                    lFreeItemCoupon.UpdatedDate = vUpdatedDate;
                    lFreeItemCoupon.UpdatedBy = vUpdatedBy;
                    lFreeItemCoupon.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lFreeItemCoupon : null;
                }
                else
                {
                    lFreeItemCoupon.Title = vTitle;
                    lFreeItemCoupon.Description = vDescription;
                    lFreeItemCoupon.UnitPrice = vUnitPrice;
                    lFreeItemCoupon.OrderMinimum = vOrderMinimum;
                    lFreeItemCoupon.StartDate = vStartDate;
                    lFreeItemCoupon.ExpirationDate = vExpirationDate;

                    lFreeItemCoupon.BizInfoId = vBizInfoID;
                    lFreeItemCoupon.AddedDate = vAddedDate;
                    lFreeItemCoupon.AddedBy = vAddedBy;
                    lFreeItemCoupon.UpdatedDate = vUpdatedDate;
                    lFreeItemCoupon.UpdatedBy = vUpdatedBy;
                    lFreeItemCoupon.Active = vActive;
                    return AddFreeItemCoupon(lFreeItemCoupon);
                }
            }
        }

        public FreeItemCoupon AddFreeItemCoupon(FreeItemCoupon vFreeItemCoupon)
        {
            try
            {
                Shoppingctx.FreeItemCoupons.Add(vFreeItemCoupon);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vFreeItemCoupon : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockFreeItemCoupon(FreeItemCoupon vFreeItemCoupon)
        {
            return ChangeLockState(vFreeItemCoupon, false);
        }
        public bool UnlockFreeItemCoupon(FreeItemCoupon vFreeItemCoupon)
        {
            return ChangeLockState(vFreeItemCoupon, true);
        }
        private bool ChangeLockState(FreeItemCoupon vFreeItemCoupon, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                FreeItemCoupon up = frenty.FreeItemCoupons.FirstOrDefault(e => e.FreeItemId == vFreeItemCoupon.FreeItemId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }

        public bool DeleteFreeItemCoupon(FreeItemCoupon vFreeItemCoupon)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFreeItemCoupon(int vFreeItemCouponID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteFreeItemCoupon(FreeItemCoupon vFreeItemCoupon)
        {
            throw new NotImplementedException();
        }

        public FreeItemCoupon UpdateFreeItemCoupon(FreeItemCoupon vFreeItemCoupon)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
