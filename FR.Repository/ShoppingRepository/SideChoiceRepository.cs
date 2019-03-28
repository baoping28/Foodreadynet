using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class SideChoiceRepository : BaseShoppingCartRepository, ISideChoiceRepository
    {
        #region ISideChoiceRepository Members

        public List<SideChoice> GetAllSideChoices()
        {
            List<SideChoice> lSideChoices = default(List<SideChoice>);
            string lSideChoicesKey = CacheKey + "_AllSideChoices";

            if (base.EnableCaching && (Cache[lSideChoicesKey] != null))
            {
                lSideChoices = (List<SideChoice>)Cache[lSideChoicesKey];
            }
            else
            {
                lSideChoices = (from lSideChoice in Shoppingctx.SideChoices.Include("Product")
                                 select lSideChoice).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lSideChoicesKey, lSideChoices, CacheDuration);
                }
            }
            return lSideChoices;
        }
        public List<SideChoice> GetAllSideChoices(bool vActive)
        {
            return GetAllSideChoices().Where(e => e.Active == vActive).ToList();
        }
        public SideChoice GetSideChoiceById(int vSideChoiceID)
        {
            return GetAllSideChoices().Where(e => e.SideChoiceId == vSideChoiceID).FirstOrDefault();
        }

        public int GetAllSideChoiceCount()
        {
            return Shoppingctx.SideChoices.Count();
        }

        public int GetAllSideChoiceCount(bool vActive)
        {
            return Shoppingctx.SideChoices.Where(e => e.Active == vActive).Count();
        }
        public List<SideChoice> GetSideChoicesByProductId(bool vActive, int vProductId)
        {
            return GetAllSideChoices().Where(e => e.ProductId == vProductId && e.Active == vActive).ToList();
        }
        public SideChoice AddSideChoice(int vSideChoiceID, int vProductID, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            SideChoice lSideChoice = new SideChoice();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vSideChoiceID > 0)
                {
                    lSideChoice = frctx.SideChoices.FirstOrDefault(u => u.SideChoiceId == vSideChoiceID);
                    lSideChoice.Title = vTitle;

                    lSideChoice.UpdatedDate = vUpdatedDate;
                    lSideChoice.UpdatedBy = vUpdatedBy;
                    lSideChoice.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lSideChoice : null;
                }
                else
                {
                    lSideChoice.Title = vTitle;

                    lSideChoice.ProductId = vProductID;
                    lSideChoice.AddedDate = vAddedDate;
                    lSideChoice.AddedBy = vAddedBy;
                    lSideChoice.UpdatedDate = vUpdatedDate;
                    lSideChoice.UpdatedBy = vUpdatedBy;
                    lSideChoice.Active = vActive;
                    return AddSideChoice(lSideChoice);
                }
            }
        }

        public SideChoice AddSideChoice(SideChoice vSideChoice)
        {
            try
            {
                    Shoppingctx.SideChoices.Add(vSideChoice);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vSideChoice : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockSideChoice(SideChoice vSideChoice)
        {
            return ChangeLockState(vSideChoice, false);
        }
        public bool UnlockSideChoice(SideChoice vSideChoice)
        {
            return ChangeLockState(vSideChoice, true);
        }
        private bool ChangeLockState(SideChoice vSideChoice, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                SideChoice up = frenty.SideChoices.FirstOrDefault(e => e.SideChoiceId == vSideChoice.SideChoiceId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteSideChoice(SideChoice vSideChoice)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSideChoice(int vSideChoiceID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteSideChoice(SideChoice vSideChoice)
        {
            throw new NotImplementedException();
        }

        public SideChoice UpdateSideChoice(SideChoice vSideChoice)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
