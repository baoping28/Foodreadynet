using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class ConvertedPointRepository : BaseShoppingCartRepository, IConvertedPointRepository
    {
        #region IConvertedPointRepository Members

        public List<ConvertedPoint> GetAllConvertedPoints()
        {
            List<ConvertedPoint> lConvertedPoints = default(List<ConvertedPoint>);
            string lConvertedPointsKey = CacheKey + "_AllConvertedPoints";

            if (base.EnableCaching && (Cache[lConvertedPointsKey] != null))
            {
                lConvertedPoints = (List<ConvertedPoint>)Cache[lConvertedPointsKey];
            }
            else
            {
                lConvertedPoints = (from lConvertedPoint in Shoppingctx.ConvertedPoints
                                    select lConvertedPoint).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lConvertedPointsKey, lConvertedPoints, CacheDuration);
                }
            }
            return lConvertedPoints;
        }

        public List<ConvertedPoint> GetAllConvertedPoints(bool vActive)
        {
            return GetAllConvertedPoints().Where(e => e.Active == vActive).ToList();
        }

        public ConvertedPoint GetConvertedPointById(int vConvertedPointID)
        {
            return GetAllConvertedPoints().Where(e => e.ConvertedPointId == vConvertedPointID).FirstOrDefault();
        }

        public int GetAllConvertedPointCount()
        {
            return Shoppingctx.ConvertedPoints.Count();
        }

        public int GetAllConvertedPointCount(bool vActive)
        {
            return Shoppingctx.ConvertedPoints.Where(e => e.Active == vActive).Count();
        }

        public List<ConvertedPoint> GetConvertedPointsByUserId(bool vActive, string vUserId)
        {
            return GetAllConvertedPoints().Where(e => e.UserId == vUserId && e.Active == vActive).ToList();
        }

        public List<ConvertedPoint> GetConvertedPointsByRewardVoucherId(bool vActive, int vRewardVoucherId)
        {
            return GetAllConvertedPoints().Where(e => e.RewardVoucherId == vRewardVoucherId && e.Active == vActive).ToList();
        }

        public ConvertedPoint AddConvertedPoint(int vConvertedPointID, string vUserID, int vRewardVoucherID, int vConvertedPoints, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            ConvertedPoint lConvertedPoint = new ConvertedPoint();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vConvertedPointID > 0)
                {
                    lConvertedPoint = frctx.ConvertedPoints.FirstOrDefault(u => u.ConvertedPointId == vConvertedPointID);

                    lConvertedPoint.ConvertedPoints = vConvertedPoints;

                    lConvertedPoint.UpdatedDate = vUpdatedDate;
                    lConvertedPoint.UpdatedBy = vUpdatedBy;
                    lConvertedPoint.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lConvertedPoint : null;
                }
                else
                {

                    lConvertedPoint.ConvertedPoints = vConvertedPoints;

                    lConvertedPoint.UserId = vUserID;

                    lConvertedPoint.RewardVoucherId = vRewardVoucherID;
                    lConvertedPoint.AddedDate = vAddedDate;
                    lConvertedPoint.AddedBy = vAddedBy;
                    lConvertedPoint.UpdatedDate = vUpdatedDate;
                    lConvertedPoint.UpdatedBy = vUpdatedBy;
                    lConvertedPoint.Active = vActive;
                    return AddConvertedPoint(lConvertedPoint);
                }
            }
        }

        public ConvertedPoint AddConvertedPoint(ConvertedPoint vConvertedPoint)
        {
            try
            {
                Shoppingctx.ConvertedPoints.Add(vConvertedPoint);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vConvertedPoint : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockConvertedPoint(ConvertedPoint vConvertedPoint)
        {
            return ChangeLockState(vConvertedPoint, false);
        }

        public bool UnlockConvertedPoint(ConvertedPoint vConvertedPoint)
        {
            return ChangeLockState(vConvertedPoint, true);
        }
        private bool ChangeLockState(ConvertedPoint vConvertedPoint, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                ConvertedPoint up = frenty.ConvertedPoints.FirstOrDefault(e => e.ConvertedPointId == vConvertedPoint.ConvertedPointId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteConvertedPoint(ConvertedPoint vConvertedPoint)
        {
            throw new NotImplementedException();
        }

        public bool DeleteConvertedPoint(int vConvertedPointID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteConvertedPoint(ConvertedPoint vConvertedPoint)
        {
            throw new NotImplementedException();
        }

        public ConvertedPoint UpdateConvertedPoint(ConvertedPoint vConvertedPoint)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
