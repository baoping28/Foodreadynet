using System.Linq;
using System;
using System.Data;
using System.Collections.Generic;
using FR.Repository.Interfaces;
using FR.Domain.Model.Entities;


namespace FR.Repository.ShoppingRepository
{
    public class BizFoodTypeRepository : BaseShoppingCartRepository, IBizFoodTypeRepository
    {
        #region IBizFoodTypeRepository Members

        public List<BizFoodType> GetAllBizFoodTypes(bool vActive)
        {
            throw new NotImplementedException();
        }

        public BizFoodType GetBizFoodTypeById(int vBizFoodTypeID)
        {
            throw new NotImplementedException();
        }

        public int GetAllBizFoodTypeCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public BizFoodType AddBizFoodType(int vBizFoodTypeID, int vBizInfoID, int vFoodTypeID, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            throw new NotImplementedException();
        }

        public BizFoodType AddBizFoodType(BizFoodType vBizFoodType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBizFoodType(BizFoodType vBizFoodType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBizFoodType(int vBizFoodTypeID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteBizFoodType(BizFoodType vBizFoodType)
        {
            throw new NotImplementedException();
        }

        public BizFoodType UpdateBizFoodType(BizFoodType vBizFoodType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
