using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IProductDressingRepository
    {
        List<ProductDressing> GetAllProductDressings();
        List<ProductDressing> GetAllProductDressings(bool vActive);
        ProductDressing GetProductDressingById(int vProductDressingID);
        int GetAllProductDressingCount();
        int GetAllProductDressingCount(bool vActive);
        List<ProductDressing> GetProductDressingsByProductId(bool vActive, int vProductId);
        List<string> GetDressingKeywoods(bool vActive);
        ProductDressing AddProductDressing(int vProductDressingID, int vProductID, int vDressingID, decimal vExtraDressingPrice, string vTitle, 
                            DateTime vAddedDate, string vAddedBy,DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        ProductDressing AddProductDressing(ProductDressing vProductDressing);
        bool LockProductDressing(ProductDressing vProductDressing);
        bool UnlockProductDressing(ProductDressing vProductDressing);
        bool DeleteProductDressing(ProductDressing vProductDressing);
        bool DeleteProductDressing(int vProductDressingID);
        bool UnDeleteProductDressing(ProductDressing vProductDressing);
        ProductDressing UpdateProductDressing(ProductDressing vProductDressing);
    }
}