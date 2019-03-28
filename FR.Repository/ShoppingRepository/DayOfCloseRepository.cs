using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class DayOfCloseRepository : BaseShoppingCartRepository, IDayOfCloseRepository
    {
        #region IDayOfCloseRepository Members
        public List<DayOfClose> GetAllDayOfCloses()
        {
            List<DayOfClose> lDayOfCloses = default(List<DayOfClose>);
            string lDayOfClosesKey = CacheKey + "_AllDayOfCloses";

            if (base.EnableCaching && (Cache[lDayOfClosesKey] != null))
            {
                lDayOfCloses = (List<DayOfClose>)Cache[lDayOfClosesKey];
            }
            else
            {

                lDayOfCloses = (from lDayOfClose in Shoppingctx.DayOfCloses
                                select lDayOfClose).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lDayOfClosesKey, lDayOfCloses, CacheDuration);
                }
            }
            return lDayOfCloses;
        }
        public List<DayOfClose> GetAllDayOfCloses(bool vActive)
        {
            return GetAllDayOfCloses().Where(e => e.Active == vActive).ToList();
        }
        public DayOfClose GetDayOfCloseById(int vDayOfCloseID)
        {
            return Shoppingctx.DayOfCloses.FirstOrDefault(e => e.DayOfCloseId == vDayOfCloseID);
        }
        public DayOfClose GetDayOfCloseById(int vDayOfCloseID, bool vActive)
        {
            return Shoppingctx.DayOfCloses.FirstOrDefault(e => e.DayOfCloseId == vDayOfCloseID && e.Active == vActive);
        }

        public List<DayOfClose> GetDayOfClosesByBizId(int vBizInfoId)
        {
            return GetAllDayOfCloses().Where(e => e.BizInfoId == vBizInfoId).ToList();
        }
        public List<DayOfClose> GetDayOfClosesByBizId(int vBizInfoId, bool vActive)
        {
            return GetAllDayOfCloses(vActive).Where(e => e.BizInfoId == vBizInfoId).ToList();
        }
        public int GetAllDayOfCloseCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public DayOfClose AddDayOfClose(int vDayOfCloseID, int vBizInfoID, string vTitle, DateTime vCloseDay, int vZoneNameID, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            DayOfClose lDayOfClose = new DayOfClose();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vDayOfCloseID > 0)
                {
                    lDayOfClose = frctx.DayOfCloses.FirstOrDefault(u => u.DayOfCloseId == vDayOfCloseID);
                    lDayOfClose.Title = vTitle;
                    lDayOfClose.CloseDay = vCloseDay;
                    lDayOfClose.ZoneNameId = vZoneNameID;

                    lDayOfClose.UpdatedDate = vUpdatedDate;
                    lDayOfClose.UpdatedBy = vUpdatedBy;
                    lDayOfClose.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lDayOfClose : null;
                }
                else
                {
                    lDayOfClose.Title = vTitle;
                    lDayOfClose.CloseDay = vCloseDay;
                    lDayOfClose.ZoneNameId = vZoneNameID;

                    lDayOfClose.AddedDate = vAddedDate;
                    lDayOfClose.AddedBy = vAddedBy;
                    lDayOfClose.BizInfoId = vBizInfoID;
                    lDayOfClose.UpdatedDate = vUpdatedDate;
                    lDayOfClose.UpdatedBy = vUpdatedBy;
                    lDayOfClose.Active = vActive;
                    return AddDayOfClose(lDayOfClose);
                }
            }
        }

        public DayOfClose AddDayOfClose(DayOfClose vDayOfClose)
        {
            try
            {

                Shoppingctx.DayOfCloses.Add(vDayOfClose);

                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vDayOfClose : null;
            }
            catch
            {
                return null;
            }
        }
        public bool LockDayOfClose(DayOfClose vDayOfClose)
        {
            return ChangeLockState(vDayOfClose, false);
        }
        public bool UnlockDayOfClose(DayOfClose vDayOfClose)
        {
            return ChangeLockState(vDayOfClose, true);
        }
        private bool ChangeLockState(DayOfClose vDayOfClose, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                DayOfClose up = frenty.DayOfCloses.FirstOrDefault(e => e.DayOfCloseId == vDayOfClose.DayOfCloseId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteDayOfClose(DayOfClose vDayOfClose)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDayOfClose(int vDayOfCloseID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteDayOfClose(DayOfClose vDayOfClose)
        {
            throw new NotImplementedException();
        }

        public DayOfClose UpdateDayOfClose(DayOfClose vDayOfClose)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
