using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;


namespace FR.Repository.Interfaces
{
    public interface ICreditCardRepository
    {
        List<CreditCard> GetAllCreditCards();
        List<CreditCard> GetAllCreditCards(bool vActive);
        CreditCard GetCreditCardById(int vCreditCardID);
        int GetAllCreditCardCount();
        int GetAllCreditCardCount(bool vActive);
        int GetCreditCardId(string vUserName, int vCreditCardTypeID, string vCreditCardNumber, int vExpirationMonth, int vExpirationYear, int vSecurityCode);
        CreditCard InsertCreditCard(int vCreditCardID, string vUserName, int vCreditCardTypeID, string vFirstName,
                string vLastName, string vCreditCardNumber, int vExpirationMonth, int vExpirationYear, int vSecurityCode,
                string vPhone, string vEmail, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate,
                string vUpdatedBy, bool vActive);
        CreditCard AddCreditCard(CreditCard vCreditCard);
        bool DeleteCreditCard(CreditCard vCreditCard);
        bool DeleteCreditCard(int vCreditCardID);
        bool UnDeleteCreditCard(CreditCard vCreditCard);
        CreditCard UpdateCreditCard(CreditCard vCreditCard);
    }
}
