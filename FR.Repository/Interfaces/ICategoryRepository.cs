using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetAllCategories();
        List<Category> GetAllCategories(bool vActive);
        Category GetCategoryById(int vCategoryID);
        List<Category> GetCategoriesByBizCuisineId(int vBizCuisineID);
        List<Category> GetCategoriesByBizCuisineId(int vBizCuisineID, bool vActive);
        int GetAllCategoryCount();
        int GetAllCategoryCount(bool vActive);
        Category AddCategory(int vCategoryID, int vBizCuisineID, string vTitle, string vDescription, string vImageUrl,
                      DateTime vAddedDate,string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        Category AddCategory(Category vCategory);
        bool LockCategory(Category vCategory);
        bool UnlockCategory(Category vCategory);
        bool DeleteCategory(Category vCategory);
        bool DeleteCategory(int vCategoryID);
        bool UnDeleteCategory(Category vCategory);
        Category UpdateCategory(Category vCategory);
    }
}
