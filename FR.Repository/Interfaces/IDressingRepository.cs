using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IDressingRepository
    {
        List<Dressing> GetAllDressings();
        List<Dressing> GetAllDressings(bool vActive);
        Dressing GetDressingById(int vDressingID);
        int GetAllDressingCount(bool vActive);
        Dressing AddDressing(int vDressingID, string vTitle,
                              DateTime vAddedDate, string vAddedBy,
                              DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        Dressing AddDressing(Dressing vDressing);
        bool DeleteDressing(Dressing vDressing);
        bool DeleteDressing(int vDressingID);
        bool UnDeleteDressing(Dressing vDressing);
        Dressing UpdateDressing(Dressing vDressing);
    }
}

