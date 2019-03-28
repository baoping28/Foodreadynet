using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IConvertedPointRepository
    {
        List<ConvertedPoint> GetAllConvertedPoints();
        List<ConvertedPoint> GetAllConvertedPoints(bool vActive);
        ConvertedPoint GetConvertedPointById(int vConvertedPointID);
        int GetAllConvertedPointCount();
        int GetAllConvertedPointCount(bool vActive);
        List<ConvertedPoint> GetConvertedPointsByUserId(bool vActive, string vUserId);
        List<ConvertedPoint> GetConvertedPointsByRewardVoucherId(bool vActive, int vRewardVoucherId);
        ConvertedPoint AddConvertedPoint(int vConvertedPointID, string vUserID, int vRewardVoucherID, int vConvertedPoints,
                      DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate,string vUpdatedBy, bool vActive);
        ConvertedPoint AddConvertedPoint(ConvertedPoint vConvertedPoint);
        bool LockConvertedPoint(ConvertedPoint vConvertedPoint);
        bool UnlockConvertedPoint(ConvertedPoint vConvertedPoint);
        bool DeleteConvertedPoint(ConvertedPoint vConvertedPoint);
        bool DeleteConvertedPoint(int vConvertedPointID);
        bool UnDeleteConvertedPoint(ConvertedPoint vConvertedPoint);
        ConvertedPoint UpdateConvertedPoint(ConvertedPoint vConvertedPoint);
    }
}