using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class SauceChoiceRepository : BaseShoppingCartRepository, ISauceChoiceRepository
    {
        #region ISauceChoiceRepository Members

        public List<SauceChoice> GetAllSauceChoices()
        {
            List<SauceChoice> lSauceChoices = default(List<SauceChoice>);
            string lSauceChoicesKey = CacheKey + "_AllSauceChoices";

            if (base.EnableCaching && (Cache[lSauceChoicesKey] != null))
            {
                lSauceChoices = (List<SauceChoice>)Cache[lSauceChoicesKey];
            }
            else
            {
                lSauceChoices = (from lSauceChoice in Shoppingctx.SauceChoices.Include("Product")
                                select lSauceChoice).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lSauceChoicesKey, lSauceChoices, CacheDuration);
                }
            }
            return lSauceChoices;
        }
        public List<SauceChoice> GetAllSauceChoices(bool vActive)
        {
            return GetAllSauceChoices().Where(e => e.Active == vActive).ToList();
        }
        public SauceChoice GetSauceChoiceById(int vSauceChoiceID)
        {
            return GetAllSauceChoices().Where(e => e.SauceChoiceId == vSauceChoiceID).FirstOrDefault();
        }

        public int GetAllSauceChoiceCount()
        {
            return Shoppingctx.SauceChoices.Count();
        }

        public int GetAllSauceChoiceCount(bool vActive)
        {
            return Shoppingctx.SauceChoices.Where(e => e.Active == vActive).Count();
        }
        public List<SauceChoice> GetSauceChoicesByProductId(bool vActive, int vProductId)
        {
            return GetAllSauceChoices().Where(e => e.ProductId == vProductId && e.Active == vActive).ToList();
        }
        public SauceChoice AddSauceChoice(int vSauceChoiceID, int vProductID, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            SauceChoice lSauceChoice = new SauceChoice();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vSauceChoiceID > 0)
                {
                    lSauceChoice = frctx.SauceChoices.FirstOrDefault(u => u.SauceChoiceId == vSauceChoiceID);
                    lSauceChoice.Title = vTitle;

                    lSauceChoice.UpdatedDate = vUpdatedDate;
                    lSauceChoice.UpdatedBy = vUpdatedBy;
                    lSauceChoice.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lSauceChoice : null;
                }
                else
                {
                    lSauceChoice.Title = vTitle;

                    lSauceChoice.ProductId = vProductID;
                    lSauceChoice.AddedDate = vAddedDate;
                    lSauceChoice.AddedBy = vAddedBy;
                    lSauceChoice.UpdatedDate = vUpdatedDate;
                    lSauceChoice.UpdatedBy = vUpdatedBy;
                    lSauceChoice.Active = vActive;
                    return AddSauceChoice(lSauceChoice);
                }
            }
        }

        public SauceChoice AddSauceChoice(SauceChoice vSauceChoice)
        {
            try
            {
                    Shoppingctx.SauceChoices.Add(vSauceChoice);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vSauceChoice : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockSauceChoice(SauceChoice vSauceChoice)
        {
            return ChangeLockState(vSauceChoice, false);
        }
        public bool UnlockSauceChoice(SauceChoice vSauceChoice)
        {
            return ChangeLockState(vSauceChoice, true);
        }
        private bool ChangeLockState(SauceChoice vSauceChoice, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                SauceChoice up = frenty.SauceChoices.FirstOrDefault(e => e.SauceChoiceId == vSauceChoice.SauceChoiceId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteSauceChoice(SauceChoice vSauceChoice)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSauceChoice(int vSauceChoiceID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteSauceChoice(SauceChoice vSauceChoice)
        {
            throw new NotImplementedException();
        }

        public SauceChoice UpdateSauceChoice(SauceChoice vSauceChoice)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
