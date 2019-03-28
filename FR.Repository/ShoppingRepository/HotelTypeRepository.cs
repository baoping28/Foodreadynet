using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace FR.Repository.ShoppingRepository
{
    public class HotelTypeRepository : BaseShoppingCartRepository, IHotelTypeRepository
    {
        public List<HotelType> GetAllHotelTypes()
        {
            {
                List<HotelType> lHotelType = default(List<HotelType>);
                string lHotelTypeKey = CacheKey + "_AllHotelTypes";

                if (base.EnableCaching && (Cache[lHotelTypeKey] != null))
                {
                    lHotelType = (List<HotelType>)Cache[lHotelTypeKey];
                }
                else
                {
                    lHotelType = Shoppingctx.HotelTypes.ToList();
                    if (base.EnableCaching)
                    {
                        CacheData(lHotelTypeKey, lHotelType, CacheDuration);
                    }
                }
                return lHotelType;
            }
        }

        public List<HotelType> GetAllHotelTypes(bool vActive)
        {
            return GetAllHotelTypes().Where(e => e.Active == vActive).ToList();
        }

        public HotelType GetHotelTypeById(int vHotelTypeID)
        {
            return GetAllHotelTypes().Where(e => e.HotelTypeId == vHotelTypeID).FirstOrDefault();
        }

        public int GetAllHotelTypeCount(bool vActive)
        {
            return Shoppingctx.HotelTypes.Where(e => e.Active == vActive).Count();
        }

        public HotelType AddHotelType(int vHotelTypeID, string vName,string vImageUrl, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            HotelType lHotelType = new HotelType();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vHotelTypeID > 0)
                {
                    lHotelType = frctx.HotelTypes.FirstOrDefault(u => u.HotelTypeId == vHotelTypeID);
                    lHotelType.Name = vName;
                    lHotelType.ImageUrl = vImageUrl;
                    lHotelType.UpdatedDate = vUpdatedDate;
                    lHotelType.UpdatedBy = vUpdatedBy;
                    lHotelType.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lHotelType : null;
                }
                else
                {
                    lHotelType.Name = vName;
                    lHotelType.ImageUrl = vImageUrl;
                    lHotelType.AddedDate = vAddedDate;
                    lHotelType.AddedBy = vAddedBy;
                    lHotelType.UpdatedDate = vUpdatedDate;
                    lHotelType.UpdatedBy = vUpdatedBy;
                    lHotelType.Active = vActive;
                    return AddHotelType(lHotelType);
                }
            }
        }

        public HotelType AddHotelType(HotelType vHotelType)
        {
            try
            {
                Shoppingctx.HotelTypes.Add(vHotelType);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vHotelType : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockHotelType(HotelType vHotelType)
        {
            return ChangeLockState(vHotelType, false);
        }
        public bool UnlockHotelType(HotelType vHotelType)
        {
            return ChangeLockState(vHotelType, true);
        }
        private bool ChangeLockState(HotelType vHotelType, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                HotelType up = frenty.HotelTypes.FirstOrDefault(e => e.HotelTypeId == vHotelType.HotelTypeId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteHotelType(HotelType vHotelType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteHotelType(int vHotelTypeID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteHotelType(HotelType vHotelType)
        {
            throw new NotImplementedException();
        }

        public HotelType UpdateHotelType(HotelType vHotelType)
        {
            throw new NotImplementedException();
        }
    }
}
