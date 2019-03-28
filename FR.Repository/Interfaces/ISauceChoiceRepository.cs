using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface ISauceChoiceRepository
    {
        List<SauceChoice> GetAllSauceChoices();
        List<SauceChoice> GetAllSauceChoices(bool vActive);
        SauceChoice GetSauceChoiceById(int vSauceChoiceID);
        int GetAllSauceChoiceCount();
        int GetAllSauceChoiceCount(bool vActive);
        List<SauceChoice> GetSauceChoicesByProductId(bool vActive, int vProductId);
        SauceChoice AddSauceChoice(int vSauceChoiceID, int vProductID, string vTitle, DateTime vAddedDate, string vAddedBy,
                                 DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        SauceChoice AddSauceChoice(SauceChoice vSauceChoice);
        bool LockSauceChoice(SauceChoice vSauceChoice);
        bool UnlockSauceChoice(SauceChoice vSauceChoice);
        bool DeleteSauceChoice(SauceChoice vSauceChoice);
        bool DeleteSauceChoice(int vSauceChoiceID);
        bool UnDeleteSauceChoice(SauceChoice vSauceChoice);
        SauceChoice UpdateSauceChoice(SauceChoice vSauceChoice);
    }
}