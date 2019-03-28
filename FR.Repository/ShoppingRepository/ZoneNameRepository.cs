using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class ZoneNameRepository : BaseShoppingCartRepository, IZoneNameRepository
    {
        #region IZoneNameRepository Members

        public List<ZoneName> GetAllZoneNames()
        {
            List<ZoneName> lZoneNames = default(List<ZoneName>);
            string lZoneNamesKey = CacheKey + "_AllZoneNames";

            if (base.EnableCaching && (Cache[lZoneNamesKey] != null))
            {
                lZoneNames = (List<ZoneName>)Cache[lZoneNamesKey];
            }
            else
            {
                lZoneNames = (from lZoneName in Shoppingctx.ZoneNames
                          orderby lZoneName.Title
                          select lZoneName).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lZoneNamesKey, lZoneNames, CacheDuration);
                }
            }
            return lZoneNames;
        }

        public List<ZoneName> GetAllZoneNames(bool vActive)
        {
            return GetAllZoneNames().Where(e => e.Active == vActive).ToList();
        }

        public ZoneName GetZoneNameById(int vZoneNameID)
        {
            return GetAllZoneNames().Where(e => e.ZoneNameId == vZoneNameID).FirstOrDefault();
        }

        public int GetAllZoneNameCount()
        {
            throw new NotImplementedException();
        }

        public int GetAllZoneNameCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public ZoneName AddZoneName(int vZoneNameID, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            ZoneName lZoneName = new ZoneName();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vZoneNameID > 0)
                {
                    lZoneName = frctx.ZoneNames.FirstOrDefault(u => u.ZoneNameId == vZoneNameID);
                    lZoneName.Title = vTitle;

                    lZoneName.UpdatedDate = vUpdatedDate;
                    lZoneName.UpdatedBy = vUpdatedBy;
                    lZoneName.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lZoneName : null;
                }
                else
                {
                    lZoneName.Title = vTitle;

                    lZoneName.AddedDate = vAddedDate;
                    lZoneName.AddedBy = vAddedBy;
                    lZoneName.UpdatedDate = vUpdatedDate;
                    lZoneName.UpdatedBy = vUpdatedBy;
                    lZoneName.Active = vActive;
                    return AddZoneName(lZoneName);
                }
            }
        }

        public ZoneName AddZoneName(ZoneName vZoneName)
        {
            try
            {
                    Shoppingctx.ZoneNames.Add(vZoneName);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vZoneName : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteZoneName(ZoneName vZoneName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteZoneName(int vZoneNameID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteZoneName(ZoneName vZoneName)
        {
            throw new NotImplementedException();
        }

        public ZoneName UpdateZoneName(ZoneName vZoneName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
