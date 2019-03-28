using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class BizRVInfoRepository : BaseShoppingCartRepository, IBizRVInfoRepository
    {
        #region IBizRVInfoRepository Members

        public IQueryable<BizRVInfo> BizRVInfos
        {
            get
            {
                List<BizRVInfo> lBizRVInfos = default(List<BizRVInfo>);
                string lActiveBizRVInfosKey = CacheKey + "_IQueryBizRVInfos";

                if (base.EnableCaching && (Cache[lActiveBizRVInfosKey] != null))
                {
                    lBizRVInfos = (List<BizRVInfo>)Cache[lActiveBizRVInfosKey];
                }
                else
                {
                    lBizRVInfos = (from lBizRVInfo in Shoppingctx.BizRVInfos
                                   orderby lBizRVInfo.BizInfo.BizTitle
                                   select lBizRVInfo).ToList();
                    if (base.EnableCaching)
                    {
                        CacheData(lActiveBizRVInfosKey, lBizRVInfos, CacheDuration);
                    }
                }
                return lBizRVInfos.AsQueryable();
            }
        }

        public List<BizRVInfo> GetAllBizRVInfos()
        {
            List<BizRVInfo> lBizRVInfos = default(List<BizRVInfo>);
            string lBizRVInfosKey = CacheKey + "_AllBizRVInfos";

            if (base.EnableCaching && (Cache[lBizRVInfosKey] != null))
            {
                lBizRVInfos = (List<BizRVInfo>)Cache[lBizRVInfosKey];
            }
            else
            {

                lBizRVInfos = (from lBizRVInfo in Shoppingctx.BizRVInfos
                               orderby lBizRVInfo.BizInfo.BizTitle
                               select lBizRVInfo).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lBizRVInfosKey, lBizRVInfos, CacheDuration);
                }
            }
            return lBizRVInfos;
        }

        public List<BizRVInfo> GetAllBizRVInfos(bool vActive)
        {
            return GetAllBizRVInfos().Where(e => e.Active == vActive).ToList();
        }

        public BizRVInfo GetBizRVInfoById(int vBizRVInfoID)
        {
            return GetAllBizRVInfos().Where(e => e.BizRVInfoId == vBizRVInfoID).FirstOrDefault();
        }

        public int GetAllBizRVInfoCount()
        {
            return Shoppingctx.BizRVInfos.Count();
        }

        public int GetAllBizRVInfoCount(bool vActive)
        {
            return Shoppingctx.BizRVInfos.Where(e => e.Active == vActive).Count();
        }

        public BizRVInfo AddBizRVInfo(int vBizRVInfoID, int vBizInfoID, string vBizPrice, int vStandingCapacity,
                         int vSeatedCapacity, string vStartTime, string vEndTime, string vDiningStyle, string vPaymentOptions,
                         bool vAcceptWalkIn, string vExcutiveChef, string vPrivatePartyFacilities, string vSpecialEvents, string vParking,
                         string vAdditionalDetails, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            BizRVInfo lBizRVInfo = new BizRVInfo();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vBizRVInfoID > 0)
                {
                    lBizRVInfo.BizPrice = vBizPrice;
                    lBizRVInfo.StandingCapacity = vStandingCapacity;
                    lBizRVInfo.SeatedCapacity = vSeatedCapacity;
                    lBizRVInfo.StartTime = vStartTime;
                    lBizRVInfo.EndTime = vEndTime;
                    lBizRVInfo.DiningStyle = vDiningStyle;
                    lBizRVInfo.PaymentOptions = vPaymentOptions;
                    lBizRVInfo.AcceptWalkIn = vAcceptWalkIn;
                    lBizRVInfo.ExecutiveChef = vExcutiveChef;
                    lBizRVInfo.PrivatePartyFacilities = vPrivatePartyFacilities;
                    lBizRVInfo.SpecialEvents = vSpecialEvents;
                    lBizRVInfo.Parking = vParking;
                    lBizRVInfo.AdditionalDetails = vAdditionalDetails;

                    lBizRVInfo.UpdatedDate = vUpdatedDate;
                    lBizRVInfo.UpdatedBy = vUpdatedBy;
                    lBizRVInfo.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lBizRVInfo : null;
                }
                else
                {
                    lBizRVInfo.BizPrice = vBizPrice;
                    lBizRVInfo.StandingCapacity = vStandingCapacity;
                    lBizRVInfo.SeatedCapacity = vSeatedCapacity;
                    lBizRVInfo.StartTime = vStartTime;
                    lBizRVInfo.EndTime = vEndTime;
                    lBizRVInfo.DiningStyle = vDiningStyle;
                    lBizRVInfo.PaymentOptions = vPaymentOptions;
                    lBizRVInfo.AcceptWalkIn = vAcceptWalkIn;
                    lBizRVInfo.ExecutiveChef = vExcutiveChef;
                    lBizRVInfo.PrivatePartyFacilities = vPrivatePartyFacilities;
                    lBizRVInfo.SpecialEvents = vSpecialEvents;
                    lBizRVInfo.Parking = vParking;
                    lBizRVInfo.AdditionalDetails = vAdditionalDetails;

                    lBizRVInfo.BizInfoId = vBizInfoID;
                    lBizRVInfo.AddedDate = vAddedDate;
                    lBizRVInfo.AddedBy = vAddedBy;
                    lBizRVInfo.UpdatedDate = vUpdatedDate;
                    lBizRVInfo.UpdatedBy = vUpdatedBy;
                    lBizRVInfo.Active = vActive;
                    return AddBizRVInfo(lBizRVInfo);
                }
            }
        }

        public BizRVInfo AddBizRVInfo(BizRVInfo vBizRVInfo)
        {
            try
            {
                Shoppingctx.BizRVInfos.Add(vBizRVInfo);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vBizRVInfo : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteBizRVInfo(BizRVInfo vBizRVInfo)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBizRVInfo(int vBizRVInfoID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteBizRVInfo(BizRVInfo vBizRVInfo)
        {
            throw new NotImplementedException();
        }

        public BizRVInfo UpdateBizRVInfo(BizRVInfo vBizRVInfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
