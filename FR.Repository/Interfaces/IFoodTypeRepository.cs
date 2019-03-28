using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;


namespace FR.Repository.Interfaces
{
    public interface IFoodTypeRepository
    {
        List<FoodType> GetAllFoodTypes();
        List<FoodType> GetAllFoodTypes(bool vActive);
        FoodType GetFoodTypeById(int vFoodTypeID);
        int GetAllFoodTypeCount(bool vActive);
        FoodType AddFoodType(int vFoodTypeID, string vTitle, DateTime vAddedDate, string vAddedBy,
                            DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        FoodType AddFoodType(FoodType vFoodType);
        bool DeleteFoodType(FoodType vFoodType);
        bool DeleteFoodType(int vFoodTypeID);
        bool UnDeleteFoodType(FoodType vFoodType);
        FoodType UpdateFoodType(FoodType vFoodType);
    }
}

