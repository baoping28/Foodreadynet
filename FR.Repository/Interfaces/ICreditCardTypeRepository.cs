using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface ICreditCardTypeRepository
    {
        List<CreditCardType> GetAllCreditCardTypes();
        List<CreditCardType> GetAllCreditCardTypes(bool vActive);
        CreditCardType GetCreditCardTypeById(int vCreditCardTypeID);
        int GetCreditCardTypesCount(bool vActive);
        CreditCardType AddCreditCardType(int vCreditCardTypeID, string vTitle, DateTime vAddedDate,
                                string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        CreditCardType AddCreditCardType(CreditCardType vCreditCardType);
        bool DeleteCreditCardType(CreditCardType vCreditCardType);
        bool DeleteCreditCardType(int vCreditCardTypeID);
        bool UnDeleteCreditCardType(CreditCardType vCreditCardType);
        CreditCardType UpdateCreditCardType(CreditCardType vCreditCardType);
    }
}
