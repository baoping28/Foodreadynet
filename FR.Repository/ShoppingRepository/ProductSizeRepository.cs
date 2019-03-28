using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class ProductSizeRepository : BaseShoppingCartRepository,IProductSizeRepository
    {
        #region IProductSizeRepository Members

        public List<ProductSize> GetAllProductSizes()
        {
            List<ProductSize> lProductSizes = default(List<ProductSize>);
            string lProductSizesKey = CacheKey + "_AllProductSizes";

            if (base.EnableCaching && (Cache[lProductSizesKey] != null))
            {
                lProductSizes = (List<ProductSize>)Cache[lProductSizesKey];
            }
            else
            {
                lProductSizes = (from lProductSize in Shoppingctx.ProductSizes.Include("Product")
                                    select lProductSize).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductSizesKey, lProductSizes, CacheDuration);
                }
            }
            return lProductSizes;
        }

        public List<ProductSize> GetAllProductSizes(bool vActive)
        {
            return GetAllProductSizes().Where(e => e.Active == vActive).ToList();
        }
        public ProductSize GetProductSizeById(int vProductSizeID)
        {
            return GetAllProductSizes().Where(e => e.ProductSizeId == vProductSizeID).FirstOrDefault();
        }

        public int GetAllProductSizeCount()
        {
            return Shoppingctx.ProductSizes.Count();
        }
        public int GetAllProductSizeCount(bool vActive)
        {
            return Shoppingctx.ProductSizes.Where(e => e.Active == vActive).Count();
        }

        public List<ProductSize> GetProductSizesByProductId(bool vActive, int vProductId)
        {
            return GetAllProductSizes().Where(e => e.ProductId == vProductId && e.Active == vActive).ToList();
        }
        public ProductSize GetProductSizeByProductID_Title(int vProductID, string vTitle)
        {
            return GetAllProductSizes().Where(e => e.ProductId== vProductID && e.Title == vTitle).FirstOrDefault();
        }
        public ProductSize AddProductSize(int vProductSizeID, int vProductID, string vTitle, int vSize, decimal vPrice, decimal vBizPrice, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            ProductSize lProductSize = new ProductSize();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vProductSizeID > 0)
                {
                    lProductSize = frctx.ProductSizes.FirstOrDefault(u => u.ProductSizeId == vProductSizeID);
                    lProductSize.Title = vTitle;
                    lProductSize.Size = vSize;
                    lProductSize.Price = vPrice;
                    lProductSize.BizPrice = vBizPrice;

                    lProductSize.UpdatedDate = vUpdatedDate;
                    lProductSize.UpdatedBy = vUpdatedBy;
                    lProductSize.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lProductSize : null;
                }
                else
                {
                    lProductSize.Title = vTitle;
                    lProductSize.Size = vSize;
                    lProductSize.Price = vPrice;
                    lProductSize.BizPrice = vBizPrice;

                    lProductSize.ProductId = vProductID;
                    lProductSize.AddedDate = vAddedDate;
                    lProductSize.AddedBy = vAddedBy;
                    lProductSize.UpdatedDate = vUpdatedDate;
                    lProductSize.UpdatedBy = vUpdatedBy;
                    lProductSize.Active = vActive;
                    return AddProductSize(lProductSize);
                }
            }
        }

        public ProductSize AddProductSize(ProductSize vProductSize)
        {
            try
            {
                    Shoppingctx.ProductSizes.Add(vProductSize);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vProductSize : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool LockProductSize(ProductSize vProductSize)
        {
            return ChangeLockState(vProductSize, false);
        }
        public bool UnlockProductSize(ProductSize vProductSize)
        {
            return ChangeLockState(vProductSize, true);
        }
        private bool ChangeLockState(ProductSize vProductSize, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                ProductSize up = frenty.ProductSizes.FirstOrDefault(e => e.ProductSizeId == vProductSize.ProductSizeId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteProductSize(ProductSize vProductSize)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProductSize(int vProductSizeID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteProductSize(ProductSize vProductSize)
        {
            throw new NotImplementedException();
        }

        public ProductSize UpdateProductSize(ProductSize vProductSize)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
