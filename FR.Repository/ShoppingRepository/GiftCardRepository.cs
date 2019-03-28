using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class GiftCardRepository : BaseShoppingCartRepository, IGiftCardRepository
    {
        #region IGiftCardRepository Members

        public List<GiftCard> GetAllGiftCards()
        {
            List<GiftCard> lGiftCard = default(List<GiftCard>);
            string lGiftCardKey = CacheKey + "_AllGiftCards";

            if (base.EnableCaching && (Cache[lGiftCardKey] != null))
            {
                lGiftCard = (List<GiftCard>)Cache[lGiftCardKey];
            }
            else
            {
                lGiftCard = Shoppingctx.GiftCards.ToList();
                if (base.EnableCaching)
                {
                    CacheData(lGiftCardKey, lGiftCard, CacheDuration);
                }
            }
            return lGiftCard;
        }

        public List<GiftCard> GetAllGiftCards(bool vActive)
        {
            return GetAllGiftCards().Where(e => e.Active == vActive).ToList();
        }

        public GiftCard GetGiftCardById(int vGiftCardID)
        {
            return GetAllGiftCards().FirstOrDefault(e => e.GiftCardId == vGiftCardID);
        }

        public List<GiftCard> GetAllGiftCardsByUserId(string vUserId)
        {
            return GetAllGiftCards().Where(e => e.UserId == vUserId).ToList();
        }
        public List<GiftCard> GetAllGiftCardsByUserId(string vUserId, bool vActive)
        {
            return GetAllGiftCards().Where(e => e.UserId == vUserId && e.Active == vActive).ToList();
        }
        public List<GiftCard> GetAllGiftCardByBizInfoId(int vBizInfoId, bool vActive)
        {
            throw new NotImplementedException();
        }

        public List<GiftCard> GetGiftCardsByCode(string vGiftCardCode)
        {
            return GetAllGiftCards().Where(e => e.GiftCardCode == vGiftCardCode).ToList();
        }
        public List<GiftCard> GetAllGiftCardByUserEmail(string vUserEmail)
        {
            return GetAllGiftCards().Where(e => e.UserEmail == vUserEmail).ToList();
        }
        public List<GiftCard> GetAllGiftCardByUserEmail(string vUserEmail, bool vActive)
        {
            return GetAllGiftCards(vActive).Where(e => e.UserEmail == vUserEmail).ToList();
        }

        public List<GiftCard> GetAllGiftCardByRecipientEmail(string vRecipientEmail)
        {
            return GetAllGiftCards().Where(e => e.RecipientEmail == vRecipientEmail).ToList();
        }
        public List<GiftCard> GetAllGiftCardByRecipientEmail(string vRecipientEmail, bool vActive)
        {
            return GetAllGiftCards(vActive).Where(e => e.UserEmail == vRecipientEmail).ToList();
        }
        public GiftCard GetGiftCardByCode(string vGiftCardCode)
        {
            return GetAllGiftCards().FirstOrDefault(e => e.GiftCardCode == vGiftCardCode);
        }
        public GiftCard GetGiftCardByCode(string vGiftCardCode, bool vActive)
        {
            return GetAllGiftCards(vActive).FirstOrDefault(e => e.GiftCardCode == vGiftCardCode);
        }
        public int GetGiftCardsCount(bool vActive)
        {
            throw new NotImplementedException();
        }


        public List<GiftCard> GetGiftCardsByAddedDateRange(DateTime vFromDate, DateTime vToDate)
        {
            List<GiftCard> lGiftCards = default(List<GiftCard>);

            if (vFromDate == null)
            {
                vFromDate = DateTime.MinValue;
            }
            if (vToDate == null)
            {
                vToDate = DateTime.MaxValue;
            }
            if (vFromDate > DateTime.MinValue && vToDate > DateTime.MinValue && vToDate >= vFromDate)
            {
                lGiftCards = (from lGiftCard in Shoppingctx.GiftCards
                              where lGiftCard.AddedDate >= vFromDate &&
                                    lGiftCard.AddedDate < vToDate
                              select lGiftCard).ToList();
                return lGiftCards;
            }

            return GetAllGiftCards();
        }

        public List<GiftCard> GetGiftCardsByAddedDateRange(DateTime vFromDate, DateTime vToDate, bool vActive)
        {
            return GetGiftCardsByAddedDateRange(vFromDate, vToDate).Where(e => e.Active == vActive).ToList();
        }

        public List<GiftCard> GetGiftCardsByUpdatedDateRange(DateTime vFromDate, DateTime vToDate)
        {
            List<GiftCard> lGiftCards = default(List<GiftCard>);

            if (vFromDate == null)
            {
                vFromDate = DateTime.MinValue;
            }
            if (vToDate == null)
            {
                vToDate = DateTime.MaxValue;
            }
            if (vFromDate > DateTime.MinValue && vToDate > DateTime.MinValue && vToDate >= vFromDate)
            {
                lGiftCards = (from lGiftCard in Shoppingctx.GiftCards
                              where lGiftCard.UpdatedDate >= vFromDate &&
                                    lGiftCard.UpdatedDate < vToDate
                              select lGiftCard).ToList();
                return lGiftCards;
            }

            return GetAllGiftCards();
        }

        public List<GiftCard> GetGiftCardsByUpdatedDateRange(DateTime vFromDate, DateTime vToDate, bool vActive)
        {
            return GetGiftCardsByUpdatedDateRange(vFromDate, vToDate).Where(e => e.Active == vActive).ToList();
        }

        public GiftCard AddGiftCard(int vGiftCardID, string vGiftCardCode, string vUserId, int vCreditCardId, int vLastPayBizInfoId,
            string vUserEmail, string vRecipientEmail, decimal vAmount, DateTime vLastTimeUse, decimal vBalance,
            decimal vLastPayAmount, decimal vLastOrderTotal, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            GiftCard lGiftCard = new GiftCard();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vGiftCardID > 0)
                {
                    lGiftCard = frctx.GiftCards.FirstOrDefault(u => u.GiftCardId == vGiftCardID);
                    lGiftCard.LastPayBizInfoId = vLastPayBizInfoId;
                    lGiftCard.LastTimeUse = vLastTimeUse;
                    lGiftCard.Balance = vBalance;
                    lGiftCard.LastPayAmount = vLastPayAmount;
                    lGiftCard.LastOrderTotal = vLastOrderTotal;

                    lGiftCard.UpdatedDate = vUpdatedDate;
                    lGiftCard.UpdatedBy = vUpdatedBy;
                    lGiftCard.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lGiftCard : null;
                }
                else
                {
                    lGiftCard.LastPayBizInfoId = vLastPayBizInfoId;
                    lGiftCard.LastTimeUse = vLastTimeUse;
                    lGiftCard.Balance = vBalance;
                    lGiftCard.LastPayAmount = vLastPayAmount;
                    lGiftCard.LastOrderTotal = vLastOrderTotal;

                    lGiftCard.GiftCardCode = vGiftCardCode;
                    lGiftCard.UserId = vUserId;
                    lGiftCard.UserEmail = vUserEmail;
                    lGiftCard.RecipientEmail = vRecipientEmail;
                    lGiftCard.Amount = vAmount;

                    lGiftCard.CreditCardId = vCreditCardId;
                    lGiftCard.AddedDate = vAddedDate;
                    lGiftCard.AddedBy = vAddedBy;
                    lGiftCard.UpdatedDate = vUpdatedDate;
                    lGiftCard.UpdatedBy = vUpdatedBy;
                    lGiftCard.Active = vActive;
                    return AddGiftCard(lGiftCard);
                }
            }
        }

        public GiftCard AddGiftCard(GiftCard vGiftCard)
        {
            try
            {
                Shoppingctx.GiftCards.Add(vGiftCard);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vGiftCard : null;
            }
            catch
            {
                return null;
            }
        }
        public bool LockGiftCard(GiftCard vGiftCard)
        {
            return ChangeLockState(vGiftCard, false);
        }
        public bool UnlockGiftCard(GiftCard vGiftCard)
        {
            return ChangeLockState(vGiftCard, true);
        }
        private bool ChangeLockState(GiftCard vGiftCard, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                GiftCard up = frenty.GiftCards.FirstOrDefault(e => e.GiftCardId == vGiftCard.GiftCardId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteGiftCard(GiftCard vGiftCard)
        {
            throw new NotImplementedException();
        }

        public bool DeleteGiftCard(int vGiftCardID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteGiftCard(GiftCard vGiftCard)
        {
            throw new NotImplementedException();
        }

        public GiftCard UpdateGiftCard(GiftCard vGiftCard)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
