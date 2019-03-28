using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class CuisineTypeRepository : BaseShoppingCartRepository, ICuisineTypeRepository
    {
        #region ICuisineTypeRepository Members

        public List<CuisineType> GetAllCuisineTypes()
        {
            List<CuisineType> lCuisineTypes = default(List<CuisineType>);
            string lCuisineTypesKey = CacheKey + "_AllCuisineTypes";

            if (base.EnableCaching && (Cache[lCuisineTypesKey] != null))
            {
                lCuisineTypes = (List<CuisineType>)Cache[lCuisineTypesKey];
            }
            else
            {
                lCuisineTypes = (from lCuisineType in Shoppingctx.CuisineTypes
                                 orderby lCuisineType.Title
                                 select lCuisineType).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lCuisineTypesKey, lCuisineTypes, CacheDuration);
                }
            }
            return lCuisineTypes;
        }
        public List<CuisineType> GetAllCuisineTypes(bool vActive)
        {
            return GetAllCuisineTypes().Where(e => e.Active == vActive).ToList();
        }
        public CuisineType GetCuisineTypeById(int vCuisineTypeID)
        {
            throw new NotImplementedException();
        }

        public int GetAllCuisineTypeCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public CuisineType AddCuisineType(int vCuisineTypeID, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            throw new NotImplementedException();
        }

        public CuisineType AddCuisineType(CuisineType vCuisineType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCuisineType(CuisineType vCuisineType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCuisineType(int vCuisineTypeID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteCuisineType(CuisineType vCuisineType)
        {
            throw new NotImplementedException();
        }

        public CuisineType UpdateCuisineType(CuisineType vCuisineType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
