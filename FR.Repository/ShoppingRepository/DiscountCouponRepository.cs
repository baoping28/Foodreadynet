using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class DiscountCouponRepository : BaseShoppingCartRepository, IDiscountCouponRepository
    {
        #region IDiscountCouponRepository Members

        public List<DiscountCoupon> GetAllDiscountCoupons()
        {
            List<DiscountCoupon> lDiscountCoupons = default(List<DiscountCoupon>);
            string lDiscountCouponsKey = CacheKey + "_AllDiscountCoupons";

            if (base.EnableCaching && (Cache[lDiscountCouponsKey] != null))
            {
                lDiscountCoupons = (List<DiscountCoupon>)Cache[lDiscountCouponsKey];
            }
            else
            {

                lDiscountCoupons = (from lDiscountCoupon in Shoppingctx.DiscountCoupons
                                    select lDiscountCoupon).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lDiscountCouponsKey, lDiscountCoupons, CacheDuration);
                }
            }
            return lDiscountCoupons;
        }

        public List<DiscountCoupon> GetAllDiscountCoupons(bool vActive)
        {
            return GetAllDiscountCoupons().Where(e => e.Active == vActive).ToList();
        }

        public List<DiscountCoupon> GetDiscountCouponsByBizId(int vBizInfoId)
        {
            return GetAllDiscountCoupons().Where(e => e.BizInfoId == vBizInfoId).ToList();
        }

        public List<DiscountCoupon> GetDiscountCouponsByBizId(int vBizInfoId, bool vActive)
        {
            return GetAllDiscountCoupons(vActive).Where(e => e.BizInfoId == vBizInfoId).ToList();
        }
        public DiscountCoupon GetDiscountCouponsByBizIdPercent(int vBizInfoId, int vPercentage, bool vActive)
        {
            return GetAllDiscountCoupons(vActive).Where(e => e.BizInfoId == vBizInfoId && e.DiscountPercentage == vPercentage && e.IsDiscountCouponDateOK).FirstOrDefault();
        }
        public List<DiscountCoupon> GetBizDiscountCouponsByMinimum(int vBizInfoId, decimal vMinimum, bool vActive)
        {
            return GetDiscountCouponsByBizId(vBizInfoId, vActive).Where(e => e.OrderMinimum < vMinimum && e.IsDiscountCouponDateOK).ToList();
        }
        public DiscountCoupon GetDiscountCouponById(int vDiscountCouponID)
        {
            return Shoppingctx.DiscountCoupons.FirstOrDefault(e => e.DiscountCouponId == vDiscountCouponID);
        }

        public int GetAllDiscountCouponCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public DiscountCoupon AddDiscountCoupon(int vDiscountCouponID, int vBizInfoID, string vTitle, string vDescription, int vDiscountPercentage, decimal vOrderMinimum, DateTime vStartDate, DateTime vExpirationDate, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            DiscountCoupon lDiscountCoupon = new DiscountCoupon();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vDiscountCouponID > 0)
                {
                    lDiscountCoupon = frctx.DiscountCoupons.FirstOrDefault(u => u.DiscountCouponId == vDiscountCouponID);
                    lDiscountCoupon.Title = vTitle;
                    lDiscountCoupon.Description = vDescription;
                    lDiscountCoupon.DiscountPercentage = vDiscountPercentage;
                    lDiscountCoupon.OrderMinimum = vOrderMinimum;
                    lDiscountCoupon.StartDate = vStartDate;
                    lDiscountCoupon.ExpirationDate = vExpirationDate;

                    lDiscountCoupon.UpdatedDate = vUpdatedDate;
                    lDiscountCoupon.UpdatedBy = vUpdatedBy;
                    lDiscountCoupon.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lDiscountCoupon : null;
                }
                else
                {
                    lDiscountCoupon.Title = vTitle;
                    lDiscountCoupon.Description = vDescription;
                    lDiscountCoupon.DiscountPercentage = vDiscountPercentage;
                    lDiscountCoupon.OrderMinimum = vOrderMinimum;
                    lDiscountCoupon.StartDate = vStartDate;
                    lDiscountCoupon.ExpirationDate = vExpirationDate;

                    lDiscountCoupon.BizInfoId = vBizInfoID;
                    lDiscountCoupon.AddedDate = vAddedDate;
                    lDiscountCoupon.AddedBy = vAddedBy;
                    lDiscountCoupon.UpdatedDate = vUpdatedDate;
                    lDiscountCoupon.UpdatedBy = vUpdatedBy;
                    lDiscountCoupon.Active = vActive;
                    return AddDiscountCoupon(lDiscountCoupon);
                }
            }
        }

        public DiscountCoupon AddDiscountCoupon(DiscountCoupon vDiscountCoupon)
        {
            try
            {
                Shoppingctx.DiscountCoupons.Add(vDiscountCoupon);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vDiscountCoupon : null;
            }
            catch
            {
                return null;
            }
        }
        public bool LockDiscountCoupon(DiscountCoupon vDiscountCoupon)
        {
            return ChangeLockState(vDiscountCoupon, false);
        }
        public bool UnlockDiscountCoupon(DiscountCoupon vDiscountCoupon)
        {
            return ChangeLockState(vDiscountCoupon, true);
        }
        private bool ChangeLockState(DiscountCoupon vDiscountCoupon, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                DiscountCoupon up = frenty.DiscountCoupons.FirstOrDefault(e => e.DiscountCouponId == vDiscountCoupon.DiscountCouponId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteDiscountCoupon(DiscountCoupon vDiscountCoupon)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDiscountCoupon(int vDiscountCouponID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteDiscountCoupon(DiscountCoupon vDiscountCoupon)
        {
            throw new NotImplementedException();
        }

        public DiscountCoupon UpdateDiscountCoupon(DiscountCoupon vDiscountCoupon)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
