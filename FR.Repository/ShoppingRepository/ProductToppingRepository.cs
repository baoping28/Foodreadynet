using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class ProductToppingRepository : BaseShoppingCartRepository, IProductToppingRepository
    {
        #region IProductToppingRepository Members

        public List<ProductTopping> GetAllProductToppings()
        {
            List<ProductTopping> lProductToppings = default(List<ProductTopping>);
            string lProductToppingsKey = CacheKey + "_AllProductToppings";

            if (base.EnableCaching && (Cache[lProductToppingsKey] != null))
            {
                lProductToppings = (List<ProductTopping>)Cache[lProductToppingsKey];
            }
            else
            {
                lProductToppings = (from lProductTopping in Shoppingctx.ProductToppings.Include("Product").Include("Topping")
                                select lProductTopping).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lProductToppingsKey, lProductToppings, CacheDuration);
                }
            }
            return lProductToppings;
        }
        public List<ProductTopping> GetAllProductToppings(bool vActive)
        {
            return GetAllProductToppings().Where(e => e.Active == vActive).ToList();
        }
        public ProductTopping GetProductToppingById(int vProductToppingID)
        {
            return GetAllProductToppings().Where(e => e.ProductToppingId == vProductToppingID).FirstOrDefault();
        }

        public int GetAllProductToppingCount()
        {
            return Shoppingctx.ProductToppings.Count();
        }
        public int GetAllProductToppingCount(bool vActive)
        {
            return Shoppingctx.ProductToppings.Where(e => e.Active == vActive).Count();
        }

        public List<ProductTopping> GetProductToppingsByProductId(bool vActive, int vProductId)
        {
            return GetAllProductToppings().Where(e => e.ProductId == vProductId && e.Active == vActive).ToList();
        }
        public List<string> GetToppingKeywoods(bool vActive)
        {
            List<string> lkeys = new List<string>();
            string lKey = CacheKey + "_AllToppingKeywoods";

            if (base.EnableCaching && (Cache[lKey] != null))
            {
                lkeys = (List<string>)Cache[lKey];
            }
            else
            {
                var lToppingKeys = (from l in Shoppingctx.ProductToppings
                                    where l.Active == vActive
                                    select new
                                    {
                                        ToppingKey = l.Topping.Title
                                    }).Distinct().ToList();
                foreach (var z in lToppingKeys)
                {
                    lkeys.Add(z.ToppingKey);
                }
                if (base.EnableCaching)
                {
                    CacheData(lKey, lkeys, CacheDuration);
                }
            }
            return lkeys;
        }
        public ProductTopping AddProductTopping(int vProductToppingID, int vProductID, int vToppingID, decimal vExtraPrice, decimal vIncrement, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            ProductTopping lProductTopping = new ProductTopping();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vProductToppingID > 0)
                {
                    lProductTopping = frctx.ProductToppings.FirstOrDefault(u => u.ProductToppingId == vProductToppingID);
                    lProductTopping.ExtraToppingPrice = vExtraPrice;
                    lProductTopping.Increment = vIncrement;
                    lProductTopping.UpdatedDate = vUpdatedDate;
                    lProductTopping.UpdatedBy = vUpdatedBy;
                    lProductTopping.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lProductTopping : null;
                }
                else
                {
                    lProductTopping.ExtraToppingPrice = vExtraPrice;
                    lProductTopping.Increment = vIncrement;
                    lProductTopping.ToppingTitle = vTitle;
                    lProductTopping.ProductId = vProductID;
                    lProductTopping.ToppingId = vToppingID;
                    lProductTopping.AddedDate = vAddedDate;
                    lProductTopping.AddedBy = vAddedBy;
                    lProductTopping.UpdatedDate = vUpdatedDate;
                    lProductTopping.UpdatedBy = vUpdatedBy;
                    lProductTopping.Active = vActive;
                    return AddProductTopping(lProductTopping);
                }
            }
        }

        public ProductTopping AddProductTopping(ProductTopping vProductTopping)
        {
            try
            {
                    Shoppingctx.ProductToppings.Add(vProductTopping);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vProductTopping : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public bool LockProductTopping(ProductTopping vProductTopping)
        {
            return ChangeLockState(vProductTopping, false);
        }
        public bool UnlockProductTopping(ProductTopping vProductTopping)
        {
            return ChangeLockState(vProductTopping, true);
        }
        private bool ChangeLockState(ProductTopping vProductTopping, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                ProductTopping up = frenty.ProductToppings.FirstOrDefault(e => e.ProductToppingId == vProductTopping.ProductToppingId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteProductTopping(ProductTopping vProductTopping)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProductTopping(int vProductToppingID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteProductTopping(ProductTopping vProductTopping)
        {
            throw new NotImplementedException();
        }

        public ProductTopping UpdateProductTopping(ProductTopping vProductTopping)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
