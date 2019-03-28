using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class FamilyMealRepository : BaseShoppingCartRepository, IFamilyMealRepository
    {
        #region IFamilyMealRepository Members
        public List<FamilyMeal> GetAllFamilyMeals()
        {
            List<FamilyMeal> lFamilyMeals = default(List<FamilyMeal>);
            string lFamilyMealsKey = CacheKey + "_AllFamilyMeals";

            if (base.EnableCaching && (Cache[lFamilyMealsKey] != null))
            {
                lFamilyMeals = (List<FamilyMeal>)Cache[lFamilyMealsKey];
            }
            else
            {
                lFamilyMeals = (from lFamilyMeal in Shoppingctx.FamilyMeals.Include("Product")
                                select lFamilyMeal).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lFamilyMealsKey, lFamilyMeals, CacheDuration);
                }
            }
            return lFamilyMeals;
        }
        public List<FamilyMeal> GetAllFamilyMeals(bool vActive)
        {
            return GetAllFamilyMeals().Where(e => e.Active == vActive).ToList();
        }

        public FamilyMeal GetFamilyMealById(int vFamilyMealID)
        {
            return GetAllFamilyMeals().Where(e => e.FamilyMealId == vFamilyMealID).FirstOrDefault();
        }

        public int GetAllFamilyMealCount()
        {
            return Shoppingctx.FamilyMeals.Count();
        }
        public int GetAllFamilyMealCount(bool vActive)
        {
            return Shoppingctx.FamilyMeals.Where(e => e.Active == vActive).Count();
        }

        public List<FamilyMeal> GetFamilyMealsByProductId(bool vActive, int vProductId)
        {
            return GetAllFamilyMeals().Where(e => e.ProductId == vProductId && e.Active == vActive).ToList();
        }
        public FamilyMeal AddFamilyMeal(int vFamilyMealID, int vProductID, int vNumberOfPeople, string vMealToAdd, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            FamilyMeal lFamilyMeal = new FamilyMeal();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vFamilyMealID > 0)
                {
                    lFamilyMeal = frctx.FamilyMeals.FirstOrDefault(u => u.FamilyMealId == vFamilyMealID);
                    lFamilyMeal.NumberOfPeople = vNumberOfPeople;
                    lFamilyMeal.MealToAdd = vMealToAdd;

                    lFamilyMeal.UpdatedDate = vUpdatedDate;
                    lFamilyMeal.UpdatedBy = vUpdatedBy;
                    lFamilyMeal.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lFamilyMeal : null;
                }
                else
                {
                    lFamilyMeal.NumberOfPeople = vNumberOfPeople;
                    lFamilyMeal.MealToAdd = vMealToAdd;

                    lFamilyMeal.ProductId = vProductID;
                    lFamilyMeal.AddedDate = vAddedDate;
                    lFamilyMeal.AddedBy = vAddedBy;
                    lFamilyMeal.UpdatedDate = vUpdatedDate;
                    lFamilyMeal.UpdatedBy = vUpdatedBy;
                    lFamilyMeal.Active = vActive;
                    return AddFamilyMeal(lFamilyMeal);
                }
            }
        }

        public FamilyMeal AddFamilyMeal(FamilyMeal vFamilyMeal)
        {
            try
            {
                Shoppingctx.FamilyMeals.Add(vFamilyMeal);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vFamilyMeal : null;
            }
            catch
            {
                return null;
            }
        }
        public bool LockFamilyMeal(FamilyMeal vFamilyMeal)
        {
            return ChangeLockState(vFamilyMeal, false);
        }
        public bool UnlockFamilyMeal(FamilyMeal vFamilyMeal)
        {
            return ChangeLockState(vFamilyMeal, true);
        }
        private bool ChangeLockState(FamilyMeal vFamilyMeal, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                FamilyMeal up = frenty.FamilyMeals.FirstOrDefault(e => e.FamilyMealId == vFamilyMeal.FamilyMealId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }

        public bool DeleteFamilyMeal(FamilyMeal vFamilyMeal)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFamilyMeal(int vFamilyMealID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteFamilyMeal(FamilyMeal vFamilyMeal)
        {
            throw new NotImplementedException();
        }

        public FamilyMeal UpdateFamilyMeal(FamilyMeal vFamilyMeal)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
