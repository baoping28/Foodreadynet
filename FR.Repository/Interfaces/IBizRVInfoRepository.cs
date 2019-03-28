using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
using System.Linq;
using FR.Infrastructure.Helpers;

namespace FR.Repository.Interfaces
{
   public interface IBizRVInfoRepository
   {
       IQueryable<BizRVInfo> BizRVInfos { get; }
       List<BizRVInfo> GetAllBizRVInfos();
       List<BizRVInfo> GetAllBizRVInfos(bool vActive);
       BizRVInfo GetBizRVInfoById(int vBizRVInfoID);
       int GetAllBizRVInfoCount();
       int GetAllBizRVInfoCount(bool vActive);
       BizRVInfo AddBizRVInfo(int vBizRVInfoID, int vBizInfoID, string vBizPrice, int vStandingCapacity,
                         int vSeatedCapacity, string vStartTime, string vEndTime, string vDiningStyle, string vPaymentOptions,
                         bool vAcceptWalkIn, string vExcutiveChef, string vPrivatePartyFacilities, string vSpecialEvents, string vParking,
                         string vAdditionalDetails, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
       BizRVInfo AddBizRVInfo(BizRVInfo vBizRVInfo);
       bool DeleteBizRVInfo(BizRVInfo vBizRVInfo);
       bool DeleteBizRVInfo(int vBizRVInfoID);
       bool UnDeleteBizRVInfo(BizRVInfo vBizRVInfo);
       BizRVInfo UpdateBizRVInfo(BizRVInfo vBizRVInfo);
    }
}
