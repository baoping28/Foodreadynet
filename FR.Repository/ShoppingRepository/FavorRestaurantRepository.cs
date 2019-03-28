using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class FavorRestaurantRepository : BaseShoppingCartRepository, IFavorRestaurantRepository
    {
        #region IFavorRestaurantRepository Members

        public List<FavorRestaurant> GetAllFavorRestaurants()
        {
            List<FavorRestaurant> lFavorRestaurants = default(List<FavorRestaurant>);
            string lFavorRestaurantsKey = CacheKey + "_AllFavorRestaurants";

            if (base.EnableCaching && (Cache[lFavorRestaurantsKey] != null))
            {
                lFavorRestaurants = (List<FavorRestaurant>)Cache[lFavorRestaurantsKey];
            }
            else
            {
                lFavorRestaurants = (from lFavorRestaurant in Shoppingctx.FavorRestaurants
                                     orderby lFavorRestaurant.BizInfo.BizTitle
                                     select lFavorRestaurant).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lFavorRestaurantsKey, lFavorRestaurants, CacheDuration);
                }
            }
            return lFavorRestaurants;
        }

        public List<FavorRestaurant> GetAllFavorRestaurants(bool vActive)
        {
            return GetAllFavorRestaurants().Where(e => e.Active == vActive).ToList();
        }

        public List<FavorRestaurant> GetFavorRestaurantsByUserId(string vUserId)
        {
            return GetAllFavorRestaurants().Where(e => e.UserId == vUserId).ToList();
        }

        public List<FavorRestaurant> GetFavorRestaurantsByUserId(string vUserId, bool vActive)
        {
            return GetFavorRestaurantsByUserId(vUserId).Where(e => e.Active == vActive).ToList();
        }
        public List<FavorRestaurant> GetFavorRestaurantsByUserBiz(string vUserId, int vBizInfoId)
        {
            return GetAllFavorRestaurants().Where(e => e.UserId == vUserId && e.BizInfoId == vBizInfoId).ToList();
        }

        public List<FavorRestaurant> GetFavorRestaurantsByUserBiz(string vUserId, int vBizInfoId, bool vActive)
        {
            return GetFavorRestaurantsByUserBiz(vUserId, vBizInfoId).Where(e => e.Active == vActive).ToList();
        }

        public FavorRestaurant GetFavorRestaurantById(int vFavorRestaurantID)
        {
            return GetAllFavorRestaurants().Where(e => e.FavorRestaurantId == vFavorRestaurantID).FirstOrDefault();
        }

        public int GetAllFavorRestaurantCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public FavorRestaurant AddFavorRestaurant(int vFavorRestaurantID, string vUserId, int vBizInfoId, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            FavorRestaurant lFavorRestaurant = new FavorRestaurant();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vFavorRestaurantID > 0)
                {
                    lFavorRestaurant = frctx.FavorRestaurants.FirstOrDefault(u => u.FavorRestaurantId == vFavorRestaurantID);
                    lFavorRestaurant.UpdatedDate = vUpdatedDate;
                    lFavorRestaurant.UpdatedBy = vUpdatedBy;
                    lFavorRestaurant.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lFavorRestaurant : null;
                }
                else
                {
                    lFavorRestaurant.BizInfoId = vBizInfoId;
                    lFavorRestaurant.UserId = vUserId;
                    lFavorRestaurant.AddedDate = vAddedDate;
                    lFavorRestaurant.AddedBy = vAddedBy;
                    lFavorRestaurant.UpdatedDate = vUpdatedDate;
                    lFavorRestaurant.UpdatedBy = vUpdatedBy;
                    lFavorRestaurant.Active = vActive;
                    return AddFavorRestaurant(lFavorRestaurant);
                }
            }
        }

        public FavorRestaurant AddFavorRestaurant(FavorRestaurant vFavorRestaurant)
        {
            try
            {
                Shoppingctx.FavorRestaurants.Add(vFavorRestaurant);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vFavorRestaurant : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteFavorRestaurant(FavorRestaurant vFavorRestaurant)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFavorRestaurant(int vFavorRestaurantID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteFavorRestaurant(FavorRestaurant vFavorRestaurant)
        {
            throw new NotImplementedException();
        }

        public FavorRestaurant UpdateFavorRestaurant(FavorRestaurant vFavorRestaurant)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
