using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class ProductDressingRepository : BaseShoppingCartRepository, IProductDressingRepository
    {
        #region IProductDressingRepository Members

        public List<ProductDressing> GetAllProductDressings()
        {
            List<ProductDressing> lProductDressings = default(List<ProductDressing>);
            string lProductDressingsKey = CacheKey + "_AllProductDressings";

            if (base.EnableCaching && (Cache[lProductDressingsKey] != null))
            {
                lProductDressings = (List<ProductDressing>)Cache[lProductDressingsKey];
            }
            else
            {
                lProductDressings = (from lProductDressing in Shoppingctx.ProductDressings.Include("Product")
                                    select lProductDressing).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductDressingsKey, lProductDressings, CacheDuration);
                }
            }
            return lProductDressings;
        }

        public List<ProductDressing> GetAllProductDressings(bool vActive)
        {
            return GetAllProductDressings().Where(e => e.Active == vActive).ToList();
        }
        public ProductDressing GetProductDressingById(int vProductDressingID)
        {
            return GetAllProductDressings().Where(e => e.ProductDressingId == vProductDressingID).FirstOrDefault();
        }

        public int GetAllProductDressingCount()
        {
            return Shoppingctx.ProductDressings.Count();
        }
        public int GetAllProductDressingCount(bool vActive)
        {
            return Shoppingctx.ProductDressings.Where(e => e.Active == vActive).Count();
        }

        public List<ProductDressing> GetProductDressingsByProductId(bool vActive, int vProductId)
        {
            return GetAllProductDressings().Where(e => e.ProductId == vProductId && e.Active == vActive).ToList();
        }

        public List<string> GetDressingKeywoods(bool vActive)
        {
            List<string> lkeys = new List<string>();
            string lKey = CacheKey + "_AllDressingKeywoods";

            if (base.EnableCaching && (Cache[lKey] != null))
            {
                lkeys = (List<string>)Cache[lKey];
            }
            else
            {
                var lDressingKeys = (from l in Shoppingctx.ProductDressings
                                    where l.Active == vActive
                                    select new
                                    {
                                        DressingKey = l.Dressing.Title
                                    }).Distinct().ToList();
                foreach (var z in lDressingKeys)
                {
                    lkeys.Add(z.DressingKey);
                }
                if (base.EnableCaching)
                {
                    CacheData(lKey, lkeys, CacheDuration);
                }
            }
            return lkeys;
        }

        public ProductDressing AddProductDressing(int vProductDressingID, int vProductID, int vDressingID, decimal vExtraPrice, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            ProductDressing lProductDressing = new ProductDressing();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vProductDressingID > 0)
                {
                    lProductDressing = frctx.ProductDressings.FirstOrDefault(u => u.ProductDressingId == vProductDressingID);
                    lProductDressing.ExtraDressingPrice = vExtraPrice;
                    lProductDressing.UpdatedDate = vUpdatedDate;
                    lProductDressing.UpdatedBy = vUpdatedBy;
                    lProductDressing.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lProductDressing : null;
                }
                else
                {
                    lProductDressing.ExtraDressingPrice = vExtraPrice;
                    lProductDressing.DressingTitle = vTitle;
                    lProductDressing.ProductId = vProductID;
                    lProductDressing.DressingId = vDressingID;
                    lProductDressing.AddedDate = vAddedDate;
                    lProductDressing.AddedBy = vAddedBy;
                    lProductDressing.UpdatedDate = vUpdatedDate;
                    lProductDressing.UpdatedBy = vUpdatedBy;
                    lProductDressing.Active = vActive;
                    return AddProductDressing(lProductDressing);
                }
            }
        }

        public ProductDressing AddProductDressing(ProductDressing vProductDressing)
        {
            try
            {
                    Shoppingctx.ProductDressings.Add(vProductDressing);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vProductDressing : null;
            }
            catch
            {
                return null;
            }
        }
        public bool LockProductDressing(ProductDressing vProductDressing)
        {
            return ChangeLockState(vProductDressing, false);
        }
        public bool UnlockProductDressing(ProductDressing vProductDressing)
        {
            return ChangeLockState(vProductDressing, true);
        }
        private bool ChangeLockState(ProductDressing vProductDressing, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                ProductDressing up = frenty.ProductDressings.FirstOrDefault(e => e.ProductDressingId == vProductDressing.ProductDressingId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteProductDressing(ProductDressing vProductDressing)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProductDressing(int vProductDressingID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteProductDressing(ProductDressing vProductDressing)
        {
            throw new NotImplementedException();
        }

        public ProductDressing UpdateProductDressing(ProductDressing vProductDressing)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
