using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IProductToppingRepository
    {
        List<ProductTopping> GetAllProductToppings();
        List<ProductTopping> GetAllProductToppings(bool vActive);
        ProductTopping GetProductToppingById(int vProductToppingID);
        int GetAllProductToppingCount();
        int GetAllProductToppingCount(bool vActive);
        List<ProductTopping> GetProductToppingsByProductId(bool vActive, int vProductId);
        List<string> GetToppingKeywoods(bool vActive);
        ProductTopping AddProductTopping(int vProductToppingID, int vProductID, int vToppingID, decimal vExtraPrice, decimal vIncrement, 
            string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        ProductTopping AddProductTopping(ProductTopping vProductTopping);
        bool LockProductTopping(ProductTopping vProductTopping);
        bool UnlockProductTopping(ProductTopping vProductTopping);
        bool DeleteProductTopping(ProductTopping vProductTopping);
        bool DeleteProductTopping(int vProductToppingID);
        bool UnDeleteProductTopping(ProductTopping vProductTopping);
        ProductTopping UpdateProductTopping(ProductTopping vProductTopping);
    }
}