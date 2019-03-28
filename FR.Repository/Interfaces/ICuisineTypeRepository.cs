using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface ICuisineTypeRepository
    {
        List<CuisineType> GetAllCuisineTypes();
        List<CuisineType> GetAllCuisineTypes(bool vActive);
        CuisineType GetCuisineTypeById(int vCuisineTypeID);
        int GetAllCuisineTypeCount(bool vActive);
        CuisineType AddCuisineType(int vCuisineTypeID, string vTitle,
                              DateTime vAddedDate, string vAddedBy,
                              DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        CuisineType AddCuisineType(CuisineType vCuisineType);
        bool DeleteCuisineType(CuisineType vCuisineType);
        bool DeleteCuisineType(int vCuisineTypeID);
        bool UnDeleteCuisineType(CuisineType vCuisineType);
        CuisineType UpdateCuisineType(CuisineType vCuisineType);
    }
}

