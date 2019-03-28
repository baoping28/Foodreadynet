using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IFavorRestaurantRepository
    {
        List<FavorRestaurant> GetAllFavorRestaurants();
        List<FavorRestaurant> GetAllFavorRestaurants(bool vActive);
        List<FavorRestaurant> GetFavorRestaurantsByUserId(string vUserId);
        List<FavorRestaurant> GetFavorRestaurantsByUserId(string vUserId, bool vActive);
        List<FavorRestaurant> GetFavorRestaurantsByUserBiz(string vUserId, int vBizInfoId);
        List<FavorRestaurant> GetFavorRestaurantsByUserBiz(string vUserId, int vBizInfoId, bool vActive);
        FavorRestaurant GetFavorRestaurantById(int vFavorRestaurantID);
        int GetAllFavorRestaurantCount(bool vActive);
        FavorRestaurant AddFavorRestaurant(int vFavorRestaurantID, string vUserId, int vBizInfoId,
                              DateTime vAddedDate, string vAddedBy,
                              DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        FavorRestaurant AddFavorRestaurant(FavorRestaurant vFavorRestaurant);
        bool DeleteFavorRestaurant(FavorRestaurant vFavorRestaurant);
        bool DeleteFavorRestaurant(int vFavorRestaurantID);
        bool UnDeleteFavorRestaurant(FavorRestaurant vFavorRestaurant);
        FavorRestaurant UpdateFavorRestaurant(FavorRestaurant vFavorRestaurant);
    }
}