using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class FoodTypeRepository : BaseShoppingCartRepository, IFoodTypeRepository
    {
        #region IFoodTypeRepository Members
        public List<FoodType> GetAllFoodTypes()
        {
            List<FoodType> lFoodType = default(List<FoodType>);
            string lFoodTypeKey = CacheKey + "_AllFoodTypes";

            if (base.EnableCaching && (Cache[lFoodTypeKey] != null))
            {
                lFoodType = (List<FoodType>)Cache[lFoodTypeKey];
            }
            else
            {
                lFoodType = Shoppingctx.FoodTypes.ToList();
                if (base.EnableCaching)
                {
                    CacheData(lFoodTypeKey, lFoodType, CacheDuration);
                }
            }
            return lFoodType;
        }
        public List<FoodType> GetAllFoodTypes(bool vActive)
        {
            return GetAllFoodTypes().Where(e => e.Active == vActive).ToList();
        }

        public FoodType GetFoodTypeById(int vFoodTypeID)
        {
            throw new NotImplementedException();
        }

        public int GetAllFoodTypeCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public FoodType AddFoodType(int vFoodTypeID, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            FoodType lFoodType = new FoodType();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vFoodTypeID > 0)
                {
                    lFoodType = frctx.FoodTypes.FirstOrDefault(u => u.FoodTypeId == vFoodTypeID);
                    lFoodType.Title = vTitle;
                    lFoodType.UpdatedDate = vUpdatedDate;
                    lFoodType.UpdatedBy = vUpdatedBy;
                    lFoodType.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lFoodType : null;
                }
                else
                {
                    lFoodType.Title = vTitle;
                    lFoodType.AddedDate = vAddedDate;
                    lFoodType.AddedBy = vAddedBy;
                    lFoodType.UpdatedDate = vUpdatedDate;
                    lFoodType.UpdatedBy = vUpdatedBy;
                    lFoodType.Active = vActive;
                    return AddFoodType(lFoodType);
                }
            }
        }

        public FoodType AddFoodType(FoodType vFoodType)
        {
            try
            {
                Shoppingctx.FoodTypes.Add(vFoodType);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vFoodType : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteFoodType(FoodType vFoodType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFoodType(int vFoodTypeID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteFoodType(FoodType vFoodType)
        {
            throw new NotImplementedException();
        }

        public FoodType UpdateFoodType(FoodType vFoodType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
