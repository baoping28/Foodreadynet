using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class DressingRepository : BaseShoppingCartRepository, IDressingRepository
    {
        #region IDressingRepository Members
        public List<Dressing> GetAllDressings()
        {
            List<Dressing> lDressings = default(List<Dressing>);
            string lDressingsKey = CacheKey + "_AllDressings";

            if (base.EnableCaching && (Cache[lDressingsKey] != null))
            {
                lDressings = (List<Dressing>)Cache[lDressingsKey];
            }
            else
            {
                lDressings = (from lDressing in Shoppingctx.Dressings
                              orderby lDressing.Title
                              select lDressing).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lDressingsKey, lDressings, CacheDuration);
                }
            }
            return lDressings;
        }
        public List<Dressing> GetAllDressings(bool vActive)
        {
            return GetAllDressings().Where(e => e.Active == vActive).ToList();
        }

        public Dressing GetDressingById(int vDressingID)
        {
            return GetAllDressings().Where(e => e.DressingId == vDressingID).FirstOrDefault();
        }

        public int GetAllDressingCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public Dressing AddDressing(int vDressingID, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            throw new NotImplementedException();
        }

        public Dressing AddDressing(Dressing vDressing)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDressing(Dressing vDressing)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDressing(int vDressingID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteDressing(Dressing vDressing)
        {
            throw new NotImplementedException();
        }

        public Dressing UpdateDressing(Dressing vDressing)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
