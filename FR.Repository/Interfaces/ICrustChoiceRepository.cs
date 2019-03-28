using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;


namespace FR.Repository.Interfaces
{
    public interface ICrustChoiceRepository
    {
        List<CrustChoice> GetAllCrustChoices();
        List<CrustChoice> GetAllCrustChoices(bool vActive);
        CrustChoice GetCrustChoiceById(int vCrustChoiceID);
        int GetAllCrustChoiceCount();
        int GetAllCrustChoiceCount(bool vActive);
        List<CrustChoice> GetCrustChoicesByProductId(bool vActive, int vProductId);
        CrustChoice AddCrustChoice(int vCrustChoiceID, int vProductID, string vTitle, string vDescription, decimal vPrice, decimal vBizPrice,
                DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        CrustChoice AddCrustChoice(CrustChoice vCrustChoice);
        bool LockCrustChoice(CrustChoice vCrustChoice);
        bool UnlockCrustChoice(CrustChoice vCrustChoice);
        bool DeleteCrustChoice(CrustChoice vCrustChoice);
        bool DeleteCrustChoice(int vCrustChoiceID);
        bool UnDeleteCrustChoice(CrustChoice vCrustChoice);
        CrustChoice UpdateCrustChoice(CrustChoice vCrustChoice);
    }
}
