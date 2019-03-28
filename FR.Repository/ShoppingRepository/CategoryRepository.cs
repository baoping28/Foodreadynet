using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class CategoryRepository : BaseShoppingCartRepository, ICategoryRepository
    {
        #region ICategoryRepository Members

        public List<Category> GetAllCategories()
        {
            List<Category> lCategories = default(List<Category>);
            string lCategoryKey = CacheKey + "_AllCategories";

            if (base.EnableCaching && (Cache[lCategoryKey] != null))
            {
                lCategories = (List<Category>)Cache[lCategoryKey];
            }
            else
            {
                lCategories = (from lCategory in Shoppingctx.Categories.Include("Products")
                               orderby lCategory.Title
                               select lCategory).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lCategoryKey, lCategories, CacheDuration);
                }
            }
            return lCategories;
        }

        public List<Category> GetAllCategories(bool vActive)
        {
            return GetAllCategories().Where(e => e.Active == vActive).ToList();
        }

        public Category GetCategoryById(int vCategoryID)
        {
            return GetAllCategories().Where(e => e.CategoryId == vCategoryID).FirstOrDefault();
        }

        public List<Category> GetCategoriesByBizCuisineId(int vBizCuisineID)
        {
            List<Category> lCategories = default(List<Category>);
            string lCategoryKey = CacheKey + "_BC" + vBizCuisineID + "_Categories";

            if (base.EnableCaching && (Cache[lCategoryKey] != null))
            {
                lCategories = (List<Category>)Cache[lCategoryKey];
            }
            else
            {
                lCategories = (from lCategory in Shoppingctx.Categories.Include("Products")
                               where lCategory.BizCuisineId == vBizCuisineID
                               orderby lCategory.Title
                               select lCategory).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lCategoryKey, lCategories, CacheDuration);
                }
            }
            return lCategories;
        }

        public List<Category> GetCategoriesByBizCuisineId(int vBizCuisineID, bool vActive)
        {
            return GetCategoriesByBizCuisineId(vBizCuisineID).Where(e => e.Active == vActive).ToList();
        }

        public int GetAllCategoryCount()
        {
            return Shoppingctx.Categories.Count();
        }

        public int GetAllCategoryCount(bool vActive)
        {
            return Shoppingctx.Categories.Where(e => e.Active == vActive).Count();
        }

        public Category AddCategory(int vCategoryID, int vBizCuisineID, string vTitle, string vDescription, string vImageUrl, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            Category lCategory = new Category();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vCategoryID > 0)
                {
                    lCategory = frctx.Categories.FirstOrDefault(u => u.CategoryId == vCategoryID);
                    lCategory.Title = vTitle;
                    lCategory.Description = vDescription;
                    lCategory.ImageUrl = vImageUrl;

                    lCategory.UpdatedDate = vUpdatedDate;
                    lCategory.UpdatedBy = vUpdatedBy;
                    lCategory.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lCategory : null;
                }
                else
                {
                    lCategory.BizCuisineId = vBizCuisineID;
                    lCategory.Title = vTitle;
                    lCategory.Description = vDescription;
                    lCategory.ImageUrl = vImageUrl;

                    lCategory.AddedDate = vAddedDate;
                    lCategory.AddedBy = vAddedBy;
                    lCategory.UpdatedDate = vUpdatedDate;
                    lCategory.UpdatedBy = vUpdatedBy;
                    lCategory.Active = vActive;
                    return AddCategory(lCategory);
                }
            }

        }

        public Category AddCategory(Category vCategory)
        {
            try
            {
                Shoppingctx.Categories.Add(vCategory);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vCategory : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockCategory(Category vCategory)
        {
            return ChangeLockState(vCategory, false);
        }
        public bool UnlockCategory(Category vCategory)
        {
            return ChangeLockState(vCategory, true);
        }
        private bool ChangeLockState(Category vCategory, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                Category up = frenty.Categories.FirstOrDefault(e => e.CategoryId == vCategory.CategoryId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }

        public bool DeleteCategory(Category vCategory)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCategory(int vCategoryID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteCategory(Category vCategory)
        {
            throw new NotImplementedException();
        }

        public Category UpdateCategory(Category vCategory)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
