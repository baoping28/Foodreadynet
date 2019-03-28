using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IDayOfCloseRepository
    {
        List<DayOfClose> GetAllDayOfCloses();
        List<DayOfClose> GetAllDayOfCloses(bool vActive);
        DayOfClose GetDayOfCloseById(int vDayOfCloseID);
        DayOfClose GetDayOfCloseById(int vDayOfCloseID, bool vActive);
        List<DayOfClose> GetDayOfClosesByBizId(int vBizInfoId);
        int GetAllDayOfCloseCount(bool vActive);
        DayOfClose AddDayOfClose(int vDayOfCloseID, int vBizInfoID, string vTitle, DateTime vCloseDay, int vZoneNameID, 
                   DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        DayOfClose AddDayOfClose(DayOfClose vDayOfClose);
        bool LockDayOfClose(DayOfClose vDayOfClose);
        bool UnlockDayOfClose(DayOfClose vDayOfClose);
        bool DeleteDayOfClose(DayOfClose vDayOfClose);
        bool DeleteDayOfClose(int vDayOfCloseID);
        bool UnDeleteDayOfClose(DayOfClose vDayOfClose);
        DayOfClose UpdateDayOfClose(DayOfClose vDayOfClose);
    }
}

