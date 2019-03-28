using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class CheeseAmountRepository : BaseShoppingCartRepository, ICheeseAmountRepository
    {
        #region ICheeseAmountRepository Members

        public List<CheeseAmount> GetAllCheeseAmounts()
        {
            List<CheeseAmount> lCheeseAmounts = default(List<CheeseAmount>);
            string lCheeseAmountsKey = CacheKey + "_AllCheeseAmounts";

            if (base.EnableCaching && (Cache[lCheeseAmountsKey] != null))
            {
                lCheeseAmounts = (List<CheeseAmount>)Cache[lCheeseAmountsKey];
            }
            else
            {
                lCheeseAmounts = (from lCheeseAmount in Shoppingctx.CheeseAmounts.Include("Product")
                                  select lCheeseAmount).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lCheeseAmountsKey, lCheeseAmounts, CacheDuration);
                }
            }
            return lCheeseAmounts;
        }

        public List<CheeseAmount> GetAllCheeseAmounts(bool vActive)
        {
            return GetAllCheeseAmounts().Where(e => e.Active == vActive).ToList();
        }

        public CheeseAmount GetCheeseAmountById(int vCheeseAmountID)
        {
            return GetAllCheeseAmounts().Where(e => e.CheeseAmountId == vCheeseAmountID).FirstOrDefault();
        }

        public int GetAllCheeseAmountCount()
        {
            return Shoppingctx.CheeseAmounts.Count();
        }

        public int GetAllCheeseAmountCount(bool vActive)
        {
            return Shoppingctx.CheeseAmounts.Where(e => e.Active == vActive).Count();
        }

        public List<CheeseAmount> GetCheeseAmountsByProductId(bool vActive, int vProductId)
        {
            return GetAllCheeseAmounts().Where(e => e.ProductId == vProductId && e.Active == vActive).ToList();
        }

        public CheeseAmount AddCheeseAmount(int vCheeseAmountID, int vProductID, string vTitle, string vDescription, decimal vPrice, decimal vBizPrice, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            CheeseAmount lCheeseAmount = new CheeseAmount();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vCheeseAmountID > 0)
                {
                    lCheeseAmount = frctx.CheeseAmounts.FirstOrDefault(u => u.CheeseAmountId == vCheeseAmountID);
                    lCheeseAmount.Title = vTitle;
                    lCheeseAmount.Description = vDescription;
                    lCheeseAmount.Price = vPrice;
                    lCheeseAmount.BizPrice = vBizPrice;
                    lCheeseAmount.UpdatedDate = vUpdatedDate;
                    lCheeseAmount.UpdatedBy = vUpdatedBy;
                    lCheeseAmount.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lCheeseAmount : null;
                }
                else
                {
                    lCheeseAmount.Title = vTitle;
                    lCheeseAmount.Description = vDescription;
                    lCheeseAmount.Price = vPrice;
                    lCheeseAmount.BizPrice = vBizPrice;
                    lCheeseAmount.ProductId = vProductID;
                    lCheeseAmount.AddedDate = vAddedDate;
                    lCheeseAmount.AddedBy = vAddedBy;
                    lCheeseAmount.UpdatedDate = vUpdatedDate;
                    lCheeseAmount.UpdatedBy = vUpdatedBy;
                    lCheeseAmount.Active = vActive;
                    return AddCheeseAmount(lCheeseAmount);
                }
            }
        }

        public CheeseAmount AddCheeseAmount(CheeseAmount vCheeseAmount)
        {
            try
            {
                Shoppingctx.CheeseAmounts.Add(vCheeseAmount);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vCheeseAmount : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockCheeseAmount(CheeseAmount vCheeseAmount)
        {
            return ChangeLockState(vCheeseAmount, false);
        }

        public bool UnlockCheeseAmount(CheeseAmount vCheeseAmount)
        {
            return ChangeLockState(vCheeseAmount, true);
        }

        private bool ChangeLockState(CheeseAmount vCheeseAmount, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                CheeseAmount up = frenty.CheeseAmounts.FirstOrDefault(e => e.CheeseAmountId == vCheeseAmount.CheeseAmountId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteCheeseAmount(CheeseAmount vCheeseAmount)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCheeseAmount(int vCheeseAmountID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteCheeseAmount(CheeseAmount vCheeseAmount)
        {
            throw new NotImplementedException();
        }

        public CheeseAmount UpdateCheeseAmount(CheeseAmount vCheeseAmount)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
