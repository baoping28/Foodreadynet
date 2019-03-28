using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
namespace FR.Repository.Interfaces
{
    public interface IZoneNameRepository
    {
        List<ZoneName> GetAllZoneNames();
        List<ZoneName> GetAllZoneNames(bool vActive);
        ZoneName GetZoneNameById(int vZoneNameID);
        int GetAllZoneNameCount();
        int GetAllZoneNameCount(bool vActive);
        ZoneName AddZoneName(int vZoneNameID,string vTitle, DateTime vAddedDate, string vAddedBy,
                     DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        ZoneName AddZoneName(ZoneName vZoneName);
        bool DeleteZoneName(ZoneName vZoneName);
        bool DeleteZoneName(int vZoneNameID);
        bool UnDeleteZoneName(ZoneName vZoneName);
        ZoneName UpdateZoneName(ZoneName vZoneName);
    }
}