using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace FR.Repository.ShoppingRepository
{
    public class CreditCardTypeRepository : BaseShoppingCartRepository, ICreditCardTypeRepository
    {
        #region ICreditCardTypeRepository Members

        public List<CreditCardType> GetAllCreditCardTypes()
        {
            List<CreditCardType> lCreditCardTypes = default(List<CreditCardType>);
            string lCreditCardTypesKey = CacheKey + "_AllCreditCardTypes";

            if (base.EnableCaching && (Cache[lCreditCardTypesKey] != null))
            {
                lCreditCardTypes = (List<CreditCardType>)Cache[lCreditCardTypesKey];
            }
            else
            {

                lCreditCardTypes = (from lCreditCardType in Shoppingctx.CreditCardTypes
                                    orderby lCreditCardType.Title
                                    select lCreditCardType).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lCreditCardTypesKey, lCreditCardTypes, CacheDuration);
                }
            }
            return lCreditCardTypes;
        }

        public List<CreditCardType> GetAllCreditCardTypes(bool vActive)
        {
            return GetAllCreditCardTypes().Where(e => e.Active == vActive).ToList();
        }
        public CreditCardType GetCreditCardTypeById(int vCreditCardTypeID)
        {
            return GetAllCreditCardTypes().Where(e => e.CreditCardTypeId == vCreditCardTypeID).FirstOrDefault();
        }

        public int GetCreditCardTypesCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public CreditCardType AddCreditCardType(int vCreditCardTypeID, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            throw new NotImplementedException();
        }

        public CreditCardType AddCreditCardType(CreditCardType vCreditCardType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCreditCardType(CreditCardType vCreditCardType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCreditCardType(int vCreditCardTypeID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteCreditCardType(CreditCardType vCreditCardType)
        {
            throw new NotImplementedException();
        }

        public CreditCardType UpdateCreditCardType(CreditCardType vCreditCardType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
