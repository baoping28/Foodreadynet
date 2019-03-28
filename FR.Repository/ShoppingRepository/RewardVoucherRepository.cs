using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class RewardVoucherRepository : BaseShoppingCartRepository, IRewardVoucherRepository
    {
        #region IRewardVoucherRepository Members

        public List<RewardVoucher> GetAllRewardVouchers()
        {
            List<RewardVoucher> lRewardVouchers = default(List<RewardVoucher>);
            string lRewardVouchersKey = CacheKey + "_AllRewardVouchers";

            if (base.EnableCaching && (Cache[lRewardVouchersKey] != null))
            {
                lRewardVouchers = (List<RewardVoucher>)Cache[lRewardVouchersKey];
            }
            else
            {
                lRewardVouchers = (from lRewardVoucher in Shoppingctx.RewardVouchers
                                 select lRewardVoucher).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lRewardVouchersKey, lRewardVouchers, CacheDuration);
                }
            }
            return lRewardVouchers;
        }

        public List<RewardVoucher> GetAllRewardVouchers(bool vActive)
        {
            return GetAllRewardVouchers().Where(e => e.Active == vActive).ToList();
        }

        public RewardVoucher GetRewardVoucherById(int vRewardVoucherID)
        {
            return GetAllRewardVouchers().Where(e => e.RewardVoucherId == vRewardVoucherID).FirstOrDefault();
        }

        public int GetAllRewardVoucherCount()
        {
            return Shoppingctx.RewardVouchers.Count();
        }

        public int GetAllRewardVoucherCount(bool vActive)
        {
            return Shoppingctx.RewardVouchers.Where(e => e.Active == vActive).Count();
        }

        public RewardVoucher AddRewardVoucher(int vRewardVoucherID, string vTitle, string vDescription,string vImageUrl, decimal vValue, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            RewardVoucher lRewardVoucher = new RewardVoucher();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vRewardVoucherID > 0)
                {
                    lRewardVoucher = frctx.RewardVouchers.FirstOrDefault(u => u.RewardVoucherId == vRewardVoucherID);
                    lRewardVoucher.Title = vTitle;
                    lRewardVoucher.Description = vDescription;
                    lRewardVoucher.Value = vValue;
                    lRewardVoucher.ImageUrl = vImageUrl;

                    lRewardVoucher.UpdatedDate = vUpdatedDate;
                    lRewardVoucher.UpdatedBy = vUpdatedBy;
                    lRewardVoucher.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lRewardVoucher : null;
                }
                else
                {
                    lRewardVoucher.Title = vTitle;
                    lRewardVoucher.Description = vDescription;
                    lRewardVoucher.Value = vValue;
                    lRewardVoucher.ImageUrl = vImageUrl;

                    lRewardVoucher.AddedDate = vAddedDate;
                    lRewardVoucher.AddedBy = vAddedBy;
                    lRewardVoucher.UpdatedDate = vUpdatedDate;
                    lRewardVoucher.UpdatedBy = vUpdatedBy;
                    lRewardVoucher.Active = vActive;
                    return AddRewardVoucher(lRewardVoucher);
                }
            }
        }

        public RewardVoucher AddRewardVoucher(RewardVoucher vRewardVoucher)
        {
            try
            {
                    Shoppingctx.RewardVouchers.Add(vRewardVoucher);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vRewardVoucher : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool LockRewardVoucher(RewardVoucher vRewardVoucher)
        {
            return ChangeLockState(vRewardVoucher, false);
        }

        public bool UnlockRewardVoucher(RewardVoucher vRewardVoucher)
        {
            return ChangeLockState(vRewardVoucher, true);
        }
        private bool ChangeLockState(RewardVoucher vRewardVoucher, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                RewardVoucher up = frenty.RewardVouchers.FirstOrDefault(e => e.RewardVoucherId == vRewardVoucher.RewardVoucherId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }

        public bool DeleteRewardVoucher(RewardVoucher vRewardVoucher)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRewardVoucher(int vRewardVoucherID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteRewardVoucher(RewardVoucher vRewardVoucher)
        {
            throw new NotImplementedException();
        }

        public RewardVoucher UpdateRewardVoucher(RewardVoucher vRewardVoucher)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
