using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
namespace FR.Repository.Interfaces
{
    public interface IBizFoodTypeRepository
    {
        List<BizFoodType> GetAllBizFoodTypes(bool vActive);
        BizFoodType GetBizFoodTypeById(int vBizFoodTypeID);
        int GetAllBizFoodTypeCount(bool vActive);
        BizFoodType AddBizFoodType(int vBizFoodTypeID, int vBizInfoID, int vFoodTypeID, DateTime vAddedDate, string vAddedBy,
                     DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        BizFoodType AddBizFoodType(BizFoodType vBizFoodType);
        bool DeleteBizFoodType(BizFoodType vBizFoodType);
        bool DeleteBizFoodType(int vBizFoodTypeID);
        bool UnDeleteBizFoodType(BizFoodType vBizFoodType);
        BizFoodType UpdateBizFoodType(BizFoodType vBizFoodType);
    }
}