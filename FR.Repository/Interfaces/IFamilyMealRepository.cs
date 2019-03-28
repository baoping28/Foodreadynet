using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IFamilyMealRepository
    {
        List<FamilyMeal> GetAllFamilyMeals();
        List<FamilyMeal> GetAllFamilyMeals(bool vActive);
        FamilyMeal GetFamilyMealById(int vFamilyMealID);
        int GetAllFamilyMealCount();
        int GetAllFamilyMealCount(bool vActive);
        List<FamilyMeal> GetFamilyMealsByProductId(bool vActive, int vProductId);
        FamilyMeal AddFamilyMeal(int vFamilyMealID, int vProductID, int vNumberOfPeople, string vMealToAdd, 
                   DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        FamilyMeal AddFamilyMeal(FamilyMeal vFamilyMeal);
        bool LockFamilyMeal(FamilyMeal vFamilyMeal);
        bool UnlockFamilyMeal(FamilyMeal vFamilyMeal);
        bool DeleteFamilyMeal(FamilyMeal vFamilyMeal);
        bool DeleteFamilyMeal(int vFamilyMealID);
        bool UnDeleteFamilyMeal(FamilyMeal vFamilyMeal);
        FamilyMeal UpdateFamilyMeal(FamilyMeal vFamilyMeal);
    }
}

