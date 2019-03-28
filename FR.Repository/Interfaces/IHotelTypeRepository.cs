using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;


namespace FR.Repository.Interfaces
{
    public interface IHotelTypeRepository
    {
        List<HotelType> GetAllHotelTypes();
        List<HotelType> GetAllHotelTypes(bool vActive);
        HotelType GetHotelTypeById(int vHotelTypeID);
        int GetAllHotelTypeCount(bool vActive);
        HotelType AddHotelType(int vHotelTypeID, string vName, string vImageUrl, DateTime vAddedDate, string vAddedBy,
                            DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        HotelType AddHotelType(HotelType vHotelType);
        bool LockHotelType(HotelType vHotelType);
        bool UnlockHotelType(HotelType vHotelType);
        bool DeleteHotelType(HotelType vHotelType);
        bool DeleteHotelType(int vHotelTypeID);
        bool UnDeleteHotelType(HotelType vHotelType);
        HotelType UpdateHotelType(HotelType vHotelType);
    }
}