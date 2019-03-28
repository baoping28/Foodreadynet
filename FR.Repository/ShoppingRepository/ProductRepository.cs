using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class ProductRepository : BaseShoppingCartRepository, IProductRepository
    {
        #region IProductRepository Members

        public List<Product> GetAllProducts()
        {
            List<Product> lProducts = default(List<Product>);
            string lProductsKey = CacheKey + "_AllProducts";

            if (base.EnableCaching && (Cache[lProductsKey] != null))
            {
                lProducts = (List<Product>)Cache[lProductsKey];
            }
            else
            {
                lProducts = (from lProduct in Shoppingctx.Products.Include("Category")
                             orderby lProduct.Title
                             select lProduct).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductsKey, lProducts, CacheDuration);
                }
            }
            return lProducts; 
        }

        public List<Product> GetAllProducts(bool vActive)
        {
            return GetAllProducts().Where(e=>e.Active==vActive).ToList();
        }

        public List<Product> GetAllProductsByBizInfoId(int vBizInfoID,bool vActive)
        {
            return GetAllProducts().Where(e => e.BizId == vBizInfoID && e.Active == vActive).ToList();
        }

        public Product GetProductById(int vProductID)
        {
            return GetAllProducts().Where(e => e.ProductId == vProductID).FirstOrDefault();
        }
        public List<Product> GetProductsByCategoryId(int vCategoryID)
        {
            List<Product> lProducts = default(List<Product>);
            string lProductsKey = CacheKey + "_Cat" +vCategoryID +"_Products";

            if (base.EnableCaching && (Cache[lProductsKey] != null))
            {
                lProducts = (List<Product>)Cache[lProductsKey];
            }
            else
            {
                lProducts = (from lProduct in Shoppingctx.Products.Include("Category")
                             where lProduct.CategoryId == vCategoryID
                             orderby lProduct.Title
                             select lProduct).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductsKey, lProducts, CacheDuration);
                }
            }
            return lProducts; 
        }

        public List<Product> GetProductsByCategoryId(int vCategoryID, bool vActive)
        {
            return GetProductsByCategoryId(vCategoryID).Where(e => e.Active == vActive).ToList();
        }

        public List<Product> GetProductsByCookMethodId(int vCookMethodID)
        {
            List<Product> lProducts = default(List<Product>);
            string lProductsKey = CacheKey + "_CM" + vCookMethodID + "_Products";

            if (base.EnableCaching && (Cache[lProductsKey] != null))
            {
                lProducts = (List<Product>)Cache[lProductsKey];
            }
            else
            {
                lProducts = (from lProduct in Shoppingctx.Products.Include("CookMethod")
                             where lProduct.CategoryId == vCookMethodID
                             orderby lProduct.Title
                             select lProduct).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductsKey, lProducts, CacheDuration);
                }
            }
            return lProducts; 
        }

        public List<Product> GetProductsByCookMethodId(int vCookMethodID, bool vActive)
        {
            return GetProductsByCookMethodId(vCookMethodID).Where(e => e.Active == vActive).ToList();
        }

        public List<Product> GetProductsByFoodTypeId(int vFoodTypeID)
        {
            List<Product> lProducts = default(List<Product>);
            string lProductsKey = CacheKey + "_FT" + vFoodTypeID + "_Products";

            if (base.EnableCaching && (Cache[lProductsKey] != null))
            {
                lProducts = (List<Product>)Cache[lProductsKey];
            }
            else
            {
                lProducts = (from lProduct in Shoppingctx.Products.Include("CookMethod")
                             where lProduct.CategoryId == vFoodTypeID
                             orderby lProduct.Title
                             select lProduct).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductsKey, lProducts, CacheDuration);
                }
            }
            return lProducts; 
        }

        public List<Product> GetProductsByFoodTypeId(int vFoodTypeID, bool vActive)
        {
            return GetProductsByFoodTypeId(vFoodTypeID).Where(e => e.Active == vActive).ToList();
        }

        public List<Product> GetProductsByMealSectionId(int vMealSectionID)
        {
            List<Product> lProducts = default(List<Product>);
            string lProductsKey = CacheKey + "_MS" + vMealSectionID + "_Products";

            if (base.EnableCaching && (Cache[lProductsKey] != null))
            {
                lProducts = (List<Product>)Cache[lProductsKey];
            }
            else
            {
                lProducts = (from lProduct in Shoppingctx.Products.Include("MealSection")
                             where lProduct.CategoryId == vMealSectionID
                             orderby lProduct.Title
                             select lProduct).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductsKey, lProducts, CacheDuration);
                }
            }
            return lProducts;
        }

        public List<Product> GetProductsByMealSectionId(int vMealSectionID, bool vActive)
        {
            return GetProductsByMealSectionId(vMealSectionID).Where(e => e.Active == vActive).ToList();
        }

        public List<Product> GetDiscountProducts()
        {
            List<Product> lProducts = default(List<Product>);
            string lProductsKey = CacheKey + "_Discount_Products";

            if (base.EnableCaching && (Cache[lProductsKey] != null))
            {
                lProducts = (List<Product>)Cache[lProductsKey];
            }
            else
            {
                lProducts = (from lProduct in Shoppingctx.Products.Include("Category")
                             where lProduct.DiscountPercentage>0
                             orderby lProduct.Title
                             select lProduct).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductsKey, lProducts, CacheDuration);
                }
            }
            return lProducts;
        }

        public List<Product> GetDiscountProducts(bool vActive)
        {
            return GetDiscountProducts().Where(e => e.Active == vActive).ToList();
        }

        public List<Product> GetSpicyProducts(bool vIsSpicy)
        {
            List<Product> lProducts = default(List<Product>);
            string lProductsKey = CacheKey + "_Spicy_Products";

            if (base.EnableCaching && (Cache[lProductsKey] != null))
            {
                lProducts = (List<Product>)Cache[lProductsKey];
            }
            else
            {
                lProducts = (from lProduct in Shoppingctx.Products.Include("Category")
                             where lProduct.IsSpicy == vIsSpicy
                             orderby lProduct.Title
                             select lProduct).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductsKey, lProducts, CacheDuration);
                }
            }
            return lProducts;
        }

        public List<Product> GetSpicyProducts(bool vIsSpicy, bool vActive)
        {
            return GetSpicyProducts(vIsSpicy).Where(e => e.Active == vActive).ToList();
        }

        public List<Product> GetPopularProducts(bool vIsMostPopular)
        {
            List<Product> lProducts = default(List<Product>);
            string lProductsKey = CacheKey + "_Popular_Products";

            if (base.EnableCaching && (Cache[lProductsKey] != null))
            {
                lProducts = (List<Product>)Cache[lProductsKey];
            }
            else
            {
                lProducts = (from lProduct in Shoppingctx.Products.Include("Category")
                             where lProduct.IsSpicy == vIsMostPopular
                             orderby lProduct.Title
                             select lProduct).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductsKey, lProducts, CacheDuration);
                }
            }
            return lProducts;
        }

        public List<Product> GetPopularProducts(bool vIsMostPopular, bool vActive)
        {
            return GetPopularProducts(vIsMostPopular).Where(e => e.Active == vActive).ToList();
        }
        public List<Product> GetProductsWithToppingByBizInfoId( int vBizInfoId,bool vActive)
        {
            return GetAllProducts(true).Where(e => e.ProductToppings.Count > 0 && e.BizId==vBizInfoId).ToList();
        }
        public List<Product> GetProductsWithDressingByBizInfoId(int vBizInfoId, bool vActive)
        {
            return GetAllProducts(true).Where(e => e.ProductDressings.Count > 0 && e.BizId == vBizInfoId).ToList();
        }
        public List<Product> GetProductsWithSizeByBizInfoId(int vBizInfoId, bool vActive)
        {
            return GetAllProducts(true).Where(e => e.ProductSizes.Count > 0 && e.BizId == vBizInfoId).ToList();
        }
        public List<Product> GetProductsWithCrustByBizInfoId(int vBizInfoId, bool vActive)
        {
            return GetAllProducts(true).Where(e => e.CrustChoices.Count > 0 && e.BizId == vBizInfoId).ToList();
        }
        public List<Product> GetProductsWithSauceByBizInfoId(int vBizInfoId, bool vActive)
        {
            return GetAllProducts(true).Where(e => e.SauceChoices.Count > 0 && e.BizId == vBizInfoId).ToList();
        }
        public List<Product> GetProductsWithCheeseAmountByBizInfoId(int vBizInfoId, bool vActive)
        {
            return GetAllProducts(true).Where(e => e.CheeseAmounts.Count > 0 && e.BizId == vBizInfoId).ToList();
        }
        public List<Product> GetProductsWithSideChoiceByBizInfoId(int vBizInfoId, bool vActive)
        {
            return GetAllProducts(true).Where(e => e.SideChoices.Count > 0 && e.BizId == vBizInfoId).ToList();
        }
        public int GetAllProductCount()
        {
            return Shoppingctx.Products.Count();
        }

        public int GetAllProductCount(bool vActive)
        {
            return Shoppingctx.Products.Where(e=>e.Active==vActive).Count();
        }
        public List<string> GetProductKeywoods(bool vActive)
        {
            List<string> lkeys = new List<string>();
            string lKey = CacheKey + "_AllProductKeywoods";

            if (base.EnableCaching && (Cache[lKey] != null))
            {
                lkeys = (List<string>)Cache[lKey];
            }
            else
            {
                var lProductKeys = (from l in Shoppingctx.Products
                                    where l.Active == vActive
                                    select new
                                    {
                                        ProductKey = l.Title
                                    }).Distinct().ToList();
                foreach (var z in lProductKeys)
                {
                    lkeys.Add(z.ProductKey);
                }
                if (base.EnableCaching)
                {
                    CacheData(lKey, lkeys, CacheDuration);
                }
            }
            return lkeys;
        }

        public Product AddProduct(int vProductID, int vCategoryID, int vCookMethodID, int vFoodTypeID, int vMealSectionID,
                       string vTitle, string vDescription, decimal vUnitPrice, decimal vBizPrice, int vDiscountPercentage,
                       string vSmallImage, string vBigImage,int vMaxNumOfFreeTopping, bool vIsSpicy, bool vIsVegetarian, bool vIsMostPopular,
                       bool vIsFamilyDinner, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate,
                       string vUpdatedBy,bool vActive)
        {
            Product lProduct = new Product();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vProductID > 0)
                {
                    lProduct = frctx.Products.FirstOrDefault(u => u.ProductId == vProductID);
                    lProduct.CategoryId = vCategoryID;
                    lProduct.CookMethodId = vCookMethodID;
                    lProduct.FoodTypeId = vFoodTypeID;
                    lProduct.Description = vDescription;
                    lProduct.MealSectionId = vMealSectionID;
                    lProduct.Title = vTitle;
                    lProduct.Description = vDescription;
                    lProduct.UnitPrice = vUnitPrice;
                    lProduct.BizPrice = vBizPrice;
                    lProduct.DiscountPercentage = vDiscountPercentage;
                    lProduct.SmallImage = vSmallImage;
                    lProduct.BigImage = vBigImage;
                    lProduct.MaxNumOfFreeTopping = vMaxNumOfFreeTopping;
                    lProduct.IsSpicy = vIsSpicy;
                    lProduct.IsVegetarian = vIsVegetarian;
                    lProduct.IsMostPopular = vIsMostPopular;
                    lProduct.IsFamilyDinner = vIsFamilyDinner;

                    lProduct.UpdatedDate = vUpdatedDate;
                    lProduct.UpdatedBy = vUpdatedBy;
                    lProduct.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lProduct : null;
                }
                else
                {
                    lProduct.CategoryId = vCategoryID;
                    lProduct.CookMethodId = vCookMethodID;
                    lProduct.FoodTypeId = vFoodTypeID;
                    lProduct.Description = vDescription;
                    lProduct.MealSectionId = vMealSectionID;
                    lProduct.Title = vTitle;
                    lProduct.Description = vDescription;
                    lProduct.UnitPrice = vUnitPrice;
                    lProduct.BizPrice = vBizPrice;
                    lProduct.DiscountPercentage = vDiscountPercentage;
                    lProduct.SmallImage = vSmallImage;
                    lProduct.BigImage = vBigImage;
                    lProduct.MaxNumOfFreeTopping = vMaxNumOfFreeTopping;
                    lProduct.IsSpicy = vIsSpicy;
                    lProduct.IsVegetarian = vIsVegetarian;
                    lProduct.IsMostPopular = vIsMostPopular;
                    lProduct.IsFamilyDinner = vIsFamilyDinner;

                    lProduct.AddedDate = vAddedDate;
                    lProduct.AddedBy = vAddedBy;
                    lProduct.UpdatedDate = vUpdatedDate;
                    lProduct.UpdatedBy = vUpdatedBy;
                    lProduct.Active = vActive;
                    return AddProduct(lProduct);
                }
            }
        }

        public Product AddProduct(Product vProduct)
        {
            try
            {
                    Shoppingctx.Products.Add(vProduct);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vProduct : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool LockProduct(Product vProduct)
        {
            return ChangeLockState(vProduct, false);
        }
        public bool UnlockProduct(Product vProduct)
        {
            return ChangeLockState(vProduct, true);
        }
        private bool ChangeLockState(Product vProduct, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                Product up = frenty.Products.FirstOrDefault(e => e.ProductId == vProduct.ProductId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteProduct(Product vProduct)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProduct(int vProductID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteProduct(Product vProduct)
        {
            throw new NotImplementedException();
        }

        public Product UpdateProduct(Product vProduct)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
