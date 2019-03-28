using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class CreditCardRepository : BaseShoppingCartRepository, ICreditCardRepository
    {
        #region ICreditCardRepository Members

        public List<CreditCard> GetAllCreditCards()
        {

            string key = CacheKey + "_AllCreditCardList";

            if (EnableCaching && (Cache[key] != null))
            {
                return (List<CreditCard>)Cache[key];
            }

            List<CreditCard> lCreditCards = (from lCreditCard in Shoppingctx.CreditCards
                                             select lCreditCard).ToList();

            if (EnableCaching)
            {
                CacheData(key, lCreditCards, CacheDuration);
            }

            return lCreditCards;
        }

        public List<CreditCard> GetAllCreditCards(bool vActive)
        {
            return GetAllCreditCards().Where(e => e.Active == vActive).ToList();
        }
        public CreditCard GetCreditCardById(int vCreditCardID)
        {
            return GetAllCreditCards().Where(e => e.CreditCardId == vCreditCardID).FirstOrDefault();
        }

        public int GetAllCreditCardCount()
        {
            return Shoppingctx.CreditCards.Count();
        }

        public int GetAllCreditCardCount(bool vActive)
        {
            return Shoppingctx.CreditCards.Where(e => e.Active == vActive).Count();
        }
        public int GetCreditCardId(string vUserName, int vCreditCardTypeID, string vCreditCardNumber, int vExpirationMonth, int vExpirationYear, int vSecurityCode)
        {
            CreditCard cc = GetAllCreditCards().Where(e => e.UserName.ToString() == vUserName.ToLower() && e.CreditCardTypeId == vCreditCardTypeID &&
                          e.CreditCardNumber == vCreditCardNumber && e.ExpirationMonth == vExpirationMonth && e.ExpirationYear == vExpirationYear &&
                          e.SecurityCode == vSecurityCode).FirstOrDefault();
            return cc == null ? 0 : cc.CreditCardId; // 0---new card 
        }
        public CreditCard InsertCreditCard(int vCreditCardID, string vUserName, int vCreditCardTypeID, string vFirstName,
               string vLastName, string vCreditCardNumber, int vExpirationMonth, int vExpirationYear, int vSecurityCode,
               string vPhone, string vEmail, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate,
               string vUpdatedBy, bool vActive)
        {
            CreditCard lCreditCard = new CreditCard();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vCreditCardID > 0)
                {
                    lCreditCard = frctx.CreditCards.FirstOrDefault(u => u.CreditCardId == vCreditCardID);
                    lCreditCard.UserName = vUserName;
                    lCreditCard.FirstName = vFirstName;
                    lCreditCard.LastName = vLastName;
                    lCreditCard.CreditCardTypeId = vCreditCardTypeID;
                    lCreditCard.CreditCardNumber = vCreditCardNumber;
                    lCreditCard.ExpirationMonth = vExpirationMonth; ;
                    lCreditCard.ExpirationYear = vExpirationYear;
                    lCreditCard.SecurityCode = vSecurityCode;
                    lCreditCard.Phone = vPhone;
                    lCreditCard.Email = vEmail;

                    lCreditCard.UpdatedDate = vUpdatedDate;
                    lCreditCard.UpdatedBy = vUpdatedBy;
                    lCreditCard.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lCreditCard : null;
                }
                else
                {
                    lCreditCard.UserName = vUserName;
                    lCreditCard.FirstName = vFirstName;
                    lCreditCard.LastName = vLastName;
                    lCreditCard.CreditCardTypeId = vCreditCardTypeID;
                    lCreditCard.CreditCardNumber = vCreditCardNumber;
                    lCreditCard.ExpirationMonth = vExpirationMonth; ;
                    lCreditCard.ExpirationYear = vExpirationYear;
                    lCreditCard.SecurityCode = vSecurityCode;
                    lCreditCard.Phone = vPhone;
                    lCreditCard.Email = vEmail;

                    lCreditCard.AddedDate = vAddedDate;
                    lCreditCard.AddedBy = vAddedBy;
                    lCreditCard.UpdatedDate = vUpdatedDate;
                    lCreditCard.UpdatedBy = vUpdatedBy;
                    lCreditCard.Active = vActive;
                    return AddCreditCard(lCreditCard);
                }
            }
        }

        public CreditCard AddCreditCard(CreditCard vCreditCard)
        {
            try
            {
                Shoppingctx.CreditCards.Add(vCreditCard);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vCreditCard : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteCreditCard(CreditCard vCreditCard)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCreditCard(int vCreditCardID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteCreditCard(CreditCard vCreditCard)
        {
            throw new NotImplementedException();
        }

        public CreditCard UpdateCreditCard(CreditCard vCreditCard)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
