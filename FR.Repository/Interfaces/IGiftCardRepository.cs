using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IGiftCardRepository
    {
        List<GiftCard> GetAllGiftCards();
        List<GiftCard> GetAllGiftCards(bool vActive);
        GiftCard GetGiftCardById(int vGiftCardID);
        List<GiftCard> GetAllGiftCardsByUserId(string vUserId);
        List<GiftCard> GetAllGiftCardsByUserId(string vUserId, bool vActive);
        List<GiftCard> GetAllGiftCardByBizInfoId(int vBizInfoId, bool vActive);
        List<GiftCard> GetGiftCardsByCode(string vGiftCardCode);
        List<GiftCard> GetAllGiftCardByUserEmail(string vUserEmail);
        List<GiftCard> GetAllGiftCardByUserEmail(string vUserEmail, bool vActive);
        List<GiftCard> GetAllGiftCardByRecipientEmail(string vRecipientEmail);
        List<GiftCard> GetAllGiftCardByRecipientEmail(string vRecipientEmail, bool vActive);
        GiftCard GetGiftCardByCode(string vGiftCardCode);
        GiftCard GetGiftCardByCode(string vGiftCardCode, bool vActive);
        List<GiftCard> GetGiftCardsByAddedDateRange(DateTime vFromDate, DateTime vToDate);
        List<GiftCard> GetGiftCardsByAddedDateRange(DateTime vFromDate, DateTime vToDate, bool vActive);
        List<GiftCard> GetGiftCardsByUpdatedDateRange(DateTime vFromDate, DateTime vToDate);
        List<GiftCard> GetGiftCardsByUpdatedDateRange(DateTime vFromDate, DateTime vToDate, bool vActive);
        int GetGiftCardsCount(bool vActive);
        GiftCard AddGiftCard(int vGiftCardID, string vGiftCardCode, string vUserId, int vCreditCardId, int vLastPayBizInfoId,
             string vUserEmail, string vRecipientEmail, decimal vAmount, DateTime vLastTimeUse, decimal vBalance,
             decimal vLastPayAmount, decimal vLastOrderTotal, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        GiftCard AddGiftCard(GiftCard vGiftCard);
        bool LockGiftCard(GiftCard vGiftCard);
        bool UnlockGiftCard(GiftCard vGiftCard);
        bool DeleteGiftCard(GiftCard vGiftCard);
        bool DeleteGiftCard(int vGiftCardID);
        bool UnDeleteGiftCard(GiftCard vGiftCard);
        GiftCard UpdateGiftCard(GiftCard vGiftCard);
    }
}
