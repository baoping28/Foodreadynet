using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IAddSideRepository
    {
        List<AddSide> GetAllAddSides();
        List<AddSide> GetAllAddSides(bool vActive);
        AddSide GetAddSideById(int vAddSideID);
        int GetAllAddSideCount();
        int GetAllAddSideCount(bool vActive);
        List<AddSide> GetAddSidesByProductId(bool vActive, int vProductId);
        AddSide AddAddSide(int vAddSideID, int vProductID, string vTitle, string vDescription, decimal vPrice, decimal vBizPrice,
                DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        AddSide AddAddSide(AddSide vAddSide);
        bool LockAddSide(AddSide vAddSide);
        bool UnlockAddSide(AddSide vAddSide);
        bool DeleteAddSide(AddSide vAddSide);
        bool DeleteAddSide(int vAddSideID);
        bool UnDeleteAddSide(AddSide vAddSide);
        AddSide UpdateAddSide(AddSide vAddSide);
    }
}
