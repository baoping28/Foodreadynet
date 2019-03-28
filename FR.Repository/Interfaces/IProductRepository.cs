using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        List<Product> GetAllProducts(bool vActive);
        List<Product> GetAllProductsByBizInfoId(int vBizInfoID, bool vActive);
        Product GetProductById(int vProductID);
        List<Product> GetProductsByCategoryId(int vCategoryID);
        List<Product> GetProductsByCategoryId(int vCategoryID, bool vActive);
        List<Product> GetProductsByCookMethodId(int vCookMethodID);
        List<Product> GetProductsByCookMethodId(int vCookMethodID, bool vActive);
        List<Product> GetProductsByFoodTypeId(int vFoodTypeID);
        List<Product> GetProductsByFoodTypeId(int vFoodTypeID, bool vActive);
        List<Product> GetProductsByMealSectionId(int vMealSectionID);
        List<Product> GetProductsByMealSectionId(int vMealSectionID, bool vActive);
        List<Product> GetDiscountProducts();
        List<Product> GetDiscountProducts(bool vActive);
        List<Product> GetSpicyProducts(bool vIsSpicy);
        List<Product> GetSpicyProducts(bool vIsSpicy, bool vActive);
        List<Product> GetPopularProducts(bool vIsMostPopular);
        List<Product> GetPopularProducts(bool vIsMostPopular, bool vActive);
        List<Product> GetProductsWithToppingByBizInfoId(int vBizInfoId, bool vActive);
        List<Product> GetProductsWithDressingByBizInfoId(int vBizInfoId, bool vActive);
        List<Product> GetProductsWithSizeByBizInfoId(int vBizInfoId, bool vActive);
        List<Product> GetProductsWithCrustByBizInfoId(int vBizInfoId, bool vActive);
        List<Product> GetProductsWithSauceByBizInfoId(int vBizInfoId, bool vActive);
        List<Product> GetProductsWithCheeseAmountByBizInfoId(int vBizInfoId, bool vActive);
        List<Product> GetProductsWithSideChoiceByBizInfoId(int vBizInfoId, bool vActive);
        int GetAllProductCount();
        int GetAllProductCount(bool vActive);
        List<string> GetProductKeywoods(bool vActive);
        Product AddProduct(int vProductID, int vCategoryID, int vCookMethodID, int vFoodTypeID, int vMealSectionID,
                       string vTitle, string vDescription, decimal vUnitPrice, decimal vBizPrice, int vDiscountPercentage,
                       string vSmallImage, string vBigImage, int vMaxNumOfFreeTopping, bool vIsSpicy, bool vIsVegetarian, bool vIsMostPopular,
                       bool vIsFamilyDinner, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate,
                       string vUpdatedBy, bool vActive);
        Product AddProduct(Product vProduct);
        bool LockProduct(Product vProduct);
        bool UnlockProduct(Product vProduct);
        bool DeleteProduct(Product vProduct);
        bool DeleteProduct(int vProductID);
        bool UnDeleteProduct(Product vProduct);
        Product UpdateProduct(Product vProduct);
    }
}