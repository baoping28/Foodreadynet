using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class CrustChoiceRepository : BaseShoppingCartRepository, ICrustChoiceRepository
    {
        #region ICrustChoiceRepository Members

        public List<CrustChoice> GetAllCrustChoices()
        {
            List<CrustChoice> lCrustChoices = default(List<CrustChoice>);
            string lCrustChoicesKey = CacheKey + "_AllCrustChoices";

            if (base.EnableCaching && (Cache[lCrustChoicesKey] != null))
            {
                lCrustChoices = (List<CrustChoice>)Cache[lCrustChoicesKey];
            }
            else
            {
                lCrustChoices = (from lCrustChoice in Shoppingctx.CrustChoices.Include("Product")
                                 select lCrustChoice).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lCrustChoicesKey, lCrustChoices, CacheDuration);
                }
            }
            return lCrustChoices;
        }

        public List<CrustChoice> GetAllCrustChoices(bool vActive)
        {
            return GetAllCrustChoices().Where(e => e.Active == vActive).ToList();
        }

        public CrustChoice GetCrustChoiceById(int vCrustChoiceID)
        {
            return GetAllCrustChoices().Where(e => e.CrustChoiceId == vCrustChoiceID).FirstOrDefault();
        }

        public int GetAllCrustChoiceCount()
        {
            return Shoppingctx.CrustChoices.Count();
        }

        public int GetAllCrustChoiceCount(bool vActive)
        {
            return Shoppingctx.CrustChoices.Where(e => e.Active == vActive).Count();
        }

        public List<CrustChoice> GetCrustChoicesByProductId(bool vActive, int vProductId)
        {
            return GetAllCrustChoices().Where(e => e.ProductId == vProductId && e.Active == vActive).ToList();
        }

        public CrustChoice AddCrustChoice(int vCrustChoiceID, int vProductID, string vTitle, string vDescription, decimal vPrice, decimal vBizPrice, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            CrustChoice lCrustChoice = new CrustChoice();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vCrustChoiceID > 0)
                {
                    lCrustChoice = frctx.CrustChoices.FirstOrDefault(u => u.CrustChoiceId == vCrustChoiceID);
                    lCrustChoice.Title = vTitle;
                    lCrustChoice.Description = vDescription;
                    lCrustChoice.Price = vPrice;
                    lCrustChoice.BizPrice = vBizPrice;
                    lCrustChoice.UpdatedDate = vUpdatedDate;
                    lCrustChoice.UpdatedBy = vUpdatedBy;
                    lCrustChoice.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lCrustChoice : null;
                }
                else
                {
                    lCrustChoice.Title = vTitle;
                    lCrustChoice.Description = vDescription;
                    lCrustChoice.Price = vPrice;
                    lCrustChoice.BizPrice = vBizPrice;
                    lCrustChoice.AddedDate = vAddedDate;
                    lCrustChoice.AddedBy = vAddedBy;
                    lCrustChoice.ProductId = vProductID;
                    lCrustChoice.UpdatedDate = vUpdatedDate;
                    lCrustChoice.UpdatedBy = vUpdatedBy;
                    lCrustChoice.Active = vActive;
                    return AddCrustChoice(lCrustChoice);
                }
            }
        }

        public CrustChoice AddCrustChoice(CrustChoice vCrustChoice)
        {
            try
            {
                Shoppingctx.CrustChoices.Add(vCrustChoice);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vCrustChoice : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockCrustChoice(CrustChoice vCrustChoice)
        {
            return ChangeLockState(vCrustChoice, false);
        }

        public bool UnlockCrustChoice(CrustChoice vCrustChoice)
        {
            return ChangeLockState(vCrustChoice, true);
        }

        private bool ChangeLockState(CrustChoice vCrustChoice, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                CrustChoice up = frenty.CrustChoices.FirstOrDefault(e => e.CrustChoiceId == vCrustChoice.CrustChoiceId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteCrustChoice(CrustChoice vCrustChoice)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCrustChoice(int vCrustChoiceID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteCrustChoice(CrustChoice vCrustChoice)
        {
            throw new NotImplementedException();
        }

        public CrustChoice UpdateCrustChoice(CrustChoice vCrustChoice)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
