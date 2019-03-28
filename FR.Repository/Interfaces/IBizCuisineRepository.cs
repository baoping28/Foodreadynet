using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IBizCuisineRepository
    {
        List<BizCuisine> GetAllBizCuisines();
        List<BizCuisine> GetAllBizCuisines(bool vActive);
        List<BizCuisine> GetBizCuisinesInMostPopularCuisines(bool vActive);
        List<string> GetMostPopularCuisines(bool vActive);
        List<string> GetPopularCuisines(bool vActive);
        BizCuisine GetBizCuisineById(int vBizCuisineID);
        int GetNumberOfCuisineInBizCuisines(List<BizCuisine> vList, bool vActive);
        List<string> GetAllCuisines(bool vActive);
        int GetAllBizCuisineCount();
        int GetAllBizCuisineCount(bool vActive);
        List<BizCuisine> GetBizCuisinesByBizInfoId(int vBizInfoId);
        List<BizCuisine> GetBizCuisinesByBizInfoId(bool vActive, int vBizInfoId);
        BizCuisine GetBizCuisineByBizID_CuisineTypeID(int vBizInfoID, int vCuisineTypeID);
        List<BizCuisine> GetBizCuisinesByCity(bool vActive, string vCity);
        List<BizCuisine> GetBizCuisinesByZip(bool vActive, string vZip);
        List<string> GetCuisineKeywoods(bool vActive);
        BizCuisine AddBizCuisine(int vBizCuisineID, int vBizInfoID, int vCuisineTypeID,
                            DateTime vAddedDate, string vAddedBy,
                            DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        BizCuisine AddBizCuisine(BizCuisine vBizCuisine);
        bool LockBizCuisine(BizCuisine vBizCuisine);
        bool UnlockBizCuisine(BizCuisine vBizCuisine);
        bool DeleteBizCuisine(BizCuisine vBizCuisine);
        bool DeleteBizCuisine(int vBizCuisineID);
        bool UnDeleteBizCuisine(BizCuisine vBizCuisine);
        BizCuisine UpdateBizCuisine(BizCuisine vBizCuisine);
    }
}