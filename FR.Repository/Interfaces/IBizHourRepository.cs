using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IBizHourRepository
    {
        List<BizHour> GetAllBizHours();
        List<BizHour> GetAllBizHours(bool vActive);
        BizHour GetBizHourById(int vBizHourID);
        int GetAllBizHourCount(bool vActive);
        BizHour AddBizHour(int vBizHourID, string vLMonStart, string vLMonClose, string vLTueStart, string vLTueClose,
                      string vLWedStart, string vLWedClose, string vLThuStart, string vLThuClose, string vLFriStart,
                      string vLFriClose, string vLSatStart, string vLSatClose, string vLSunStart, string vLSunClose,
                      string vMonStart, string vMonClose, string vTueStart, string vTueClose,
                      string vWedStart, string vWedClose, string vThuStart, string vThuClose, string vFriStart,
                      string vFriClose, string vSatStart, string vSatClose, string vSunStart, string vSunClose,
                      int vZoneNameId, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        BizHour AddBizHour(BizHour vBizHour);
        bool DeleteBizHour(BizHour vBizHour);
        bool DeleteBizHour(int vBizHourID);
        bool UnDeleteBizHour(BizHour vBizHour);
        BizHour UpdateBizHour(BizHour vBizHour);
    }
}