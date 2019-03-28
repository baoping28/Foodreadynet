using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface ISideChoiceRepository
    {
        List<SideChoice> GetAllSideChoices();
        List<SideChoice> GetAllSideChoices(bool vActive);
        SideChoice GetSideChoiceById(int vSideChoiceID);
        int GetAllSideChoiceCount();
        int GetAllSideChoiceCount(bool vActive);
        List<SideChoice> GetSideChoicesByProductId(bool vActive, int vProductId);
        SideChoice AddSideChoice(int vSideChoiceID, int vProductID, string vTitle,DateTime vAddedDate, string vAddedBy,
                                 DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        SideChoice AddSideChoice(SideChoice vSideChoice);
        bool LockSideChoice(SideChoice vSideChoice);
        bool UnlockSideChoice(SideChoice vSideChoice);
        bool DeleteSideChoice(SideChoice vSideChoice);
        bool DeleteSideChoice(int vSideChoiceID);
        bool UnDeleteSideChoice(SideChoice vSideChoice);
        SideChoice UpdateSideChoice(SideChoice vSideChoice);
    }
}