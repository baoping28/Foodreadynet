using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IProductSizeRepository
    {
        List<ProductSize> GetAllProductSizes();
        List<ProductSize> GetAllProductSizes(bool vActive);
        ProductSize GetProductSizeById(int vProductSizeID);
        int GetAllProductSizeCount();
        int GetAllProductSizeCount(bool vActive);
        List<ProductSize> GetProductSizesByProductId(bool vActive, int vProductId);
        ProductSize GetProductSizeByProductID_Title(int vProductID, string vTitle);
        ProductSize AddProductSize(int vProductSizeID, int vProductID, string vTitle, int vSize, decimal vPrice, 
                                  decimal vBizPrice, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, 
                                  string vUpdatedBy, bool vActive);
        ProductSize AddProductSize(ProductSize vProductSize);
        bool LockProductSize(ProductSize vProductSize);
        bool UnlockProductSize(ProductSize vProductSize);
        bool DeleteProductSize(ProductSize vProductSize);
        bool DeleteProductSize(int vProductSizeID);
        bool UnDeleteProductSize(ProductSize vProductSize);
        ProductSize UpdateProductSize(ProductSize vProductSize);
    }
}