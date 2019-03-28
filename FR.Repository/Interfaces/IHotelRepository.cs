using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;


namespace FR.Repository.Interfaces
{
    public interface IHotelRepository
    {
        List<Hotel> GetAllHotels();
        List<Hotel> GetAllHotels(bool vActive);
        Hotel GetHotelById(int vHotelID);
        Dictionary<string, List<Hotel>> GetCityHotelsByTypeId(int vHotelTypeId, bool vActive);
        int GetAllHotelCount(bool vActive);
        Hotel AddHotel(int vHotelID, int vHotelTypeID, string vName, string vAddressLine, string vCity, string vState,
                                 string vZipCode, string vImageUrl, string vLatitude, string vLongitude, string vDescription, DateTime vAddedDate, string vAddedBy,
                            DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        Hotel AddHotel(Hotel vHotel);
        bool DeleteHotel(Hotel vHotel);
        bool DeleteHotel(int vHotelID);
        bool UnDeleteHotel(Hotel vHotel);
        Hotel UpdateHotel(Hotel vHotel);
    }
}