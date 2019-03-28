using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class ToppingRepository : BaseShoppingCartRepository, IToppingRepository
    {
        #region IToppingRepository Members

        public List<Topping> GetAllToppings()
        {
            List<Topping> lToppings = default(List<Topping>);
            string lToppingsKey = CacheKey + "_AllToppings";

            if (base.EnableCaching && (Cache[lToppingsKey] != null))
            {
                lToppings = (List<Topping>)Cache[lToppingsKey];
            }
            else
            {

                lToppings = (from lTopping in Shoppingctx.Toppings
                             orderby lTopping.Title
                             select lTopping).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lToppingsKey, lToppings, CacheDuration);
                }
            }
            return lToppings; 
        }
        public List<Topping> GetAllToppings(bool vActive)
        {
            return GetAllToppings().Where(e => e.Active == vActive).ToList();
        }

        public Topping GetToppingById(int vToppingID)
        {
            return GetAllToppings().Where(e => e.ToppingId == vToppingID).FirstOrDefault();
        }

        public int GetAllToppingCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public Topping AddTopping(int vToppingID, int vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            throw new NotImplementedException();
        }

        public Topping AddTopping(Topping vTopping)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTopping(Topping vTopping)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTopping(int vToppingID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteTopping(Topping vTopping)
        {
            throw new NotImplementedException();
        }

        public Topping UpdateTopping(Topping vTopping)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
