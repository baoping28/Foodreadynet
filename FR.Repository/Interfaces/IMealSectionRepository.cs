using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IMealSectionRepository
    {
        List<MealSection> GetAllMealSections();
        List<MealSection> GetAllMealSections(bool vActive);
        MealSection GetMealSectionById(int vMealSectionID);
        int GetAllMealSectionCount(bool vActive);
        MealSection AddMealSection(int vMealSectionID, string vTitle, string vDescription, DateTime vAddedDate,
                                   string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        MealSection AddMealSection(MealSection vMealSection);
        bool DeleteMealSection(MealSection vMealSection);
        bool DeleteMealSection(int vMealSectionID);
        bool UnDeleteMealSection(MealSection vMealSection);
        MealSection UpdateMealSection(MealSection vMealSection);
    }
}