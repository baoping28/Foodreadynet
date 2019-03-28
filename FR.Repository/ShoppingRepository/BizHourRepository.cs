using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class BizHourRepository : BaseShoppingCartRepository, IBizHourRepository
    {
        #region IBizHourRepository Members

        public List<BizHour> GetAllBizHours()
        {
            List<BizHour> lBizHours = default(List<BizHour>);
            string lBizHoursKey = CacheKey + "_AllBizHours";

            if (base.EnableCaching && (Cache[lBizHoursKey] != null))
            {
                lBizHours = (List<BizHour>)Cache[lBizHoursKey];
            }
            else
            {
                lBizHours = (from lBizHour in Shoppingctx.BizHours
                             select lBizHour).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lBizHoursKey, lBizHours, CacheDuration);
                }
            }
            return lBizHours;
        }

        public List<BizHour> GetAllBizHours(bool vActive)
        {
            return GetAllBizHours().Where(e => e.Active == vActive).ToList();
        }
        public BizHour GetBizHourById(int vBizHourID)
        {
            return GetAllBizHours().FirstOrDefault(e => e.BizHourId == vBizHourID);
        }

        public int GetAllBizHourCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public BizHour AddBizHour(int vBizHourID, string vLMonStart, string vLMonClose, string vLTueStart, string vLTueClose,
                       string vLWedStart, string vLWedClose, string vLThuStart, string vLThuClose, string vLFriStart,
                       string vLFriClose, string vLSatStart, string vLSatClose, string vLSunStart, string vLSunClose,
                       string vMonStart, string vMonClose, string vTueStart, string vTueClose,
                       string vWedStart, string vWedClose, string vThuStart, string vThuClose, string vFriStart,
                       string vFriClose, string vSatStart, string vSatClose, string vSunStart, string vSunClose,
                       int vZoneNameId, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            BizHour lBizHour = new BizHour();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vBizHourID > 0)
                {
                    lBizHour = frctx.BizHours.FirstOrDefault(u => u.BizHourId == vBizHourID);

                    lBizHour.LMonStart = vLMonStart;
                    lBizHour.LMonClose = vLMonClose;
                    lBizHour.LTueStart = vLTueStart;
                    lBizHour.LTueClose = vLTueClose;
                    lBizHour.LWedStart = vLWedStart;
                    lBizHour.LWedClose = vLWedClose;
                    lBizHour.LThuStart = vLThuStart;
                    lBizHour.LThuClose = vLThuClose;
                    lBizHour.LFriStart = vLFriStart;
                    lBizHour.LFriClose = vLFriClose;
                    lBizHour.LSatStart = vLSatStart;
                    lBizHour.LSatClose = vLSatClose;
                    lBizHour.LSunStart = vLSunStart;
                    lBizHour.LSunClose = vLSunClose;


                    lBizHour.MonStart = vMonStart;
                    lBizHour.MonClose = vMonClose;
                    lBizHour.TueStart = vTueStart;
                    lBizHour.TueClose = vTueClose;
                    lBizHour.WedStart = vWedStart;
                    lBizHour.WedClose = vWedClose;
                    lBizHour.ThuStart = vThuStart;
                    lBizHour.ThuClose = vThuClose;
                    lBizHour.FriStart = vFriStart;
                    lBizHour.FriClose = vFriClose;
                    lBizHour.SatStart = vSatStart;
                    lBizHour.SatClose = vSatClose;
                    lBizHour.SunStart = vSunStart;
                    lBizHour.SunColse = vSunClose;

                    lBizHour.ZoneNameId = vZoneNameId;
                    lBizHour.UpdatedDate = vUpdatedDate;

                    lBizHour.UpdatedBy = vUpdatedBy;
                    lBizHour.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lBizHour : null;
                }
                else
                {

                    lBizHour.LMonStart = vLMonStart;
                    lBizHour.LMonClose = vLMonClose;
                    lBizHour.LTueStart = vLTueStart;
                    lBizHour.LTueClose = vLTueClose;
                    lBizHour.LWedStart = vLWedStart;
                    lBizHour.LWedClose = vLWedClose;
                    lBizHour.LThuStart = vLThuStart;
                    lBizHour.LThuClose = vLThuClose;
                    lBizHour.LFriStart = vLFriStart;
                    lBizHour.LFriClose = vLFriClose;
                    lBizHour.LSatStart = vLSatStart;
                    lBizHour.LSatClose = vLSatClose;
                    lBizHour.LSunStart = vLSunStart;
                    lBizHour.LSunClose = vLSunClose;


                    lBizHour.MonStart = vMonStart;
                    lBizHour.MonClose = vMonClose;
                    lBizHour.TueStart = vTueStart;
                    lBizHour.TueClose = vTueClose;
                    lBizHour.WedStart = vWedStart;
                    lBizHour.WedClose = vWedClose;
                    lBizHour.ThuStart = vThuStart;
                    lBizHour.ThuClose = vThuClose;
                    lBizHour.FriStart = vFriStart;
                    lBizHour.FriClose = vFriClose;
                    lBizHour.SatStart = vSatStart;
                    lBizHour.SatClose = vSatClose;
                    lBizHour.SunStart = vSunStart;
                    lBizHour.SunColse = vSunClose;

                    lBizHour.ZoneNameId = vZoneNameId;
                    lBizHour.AddedDate = vAddedDate;
                    lBizHour.AddedBy = vAddedBy;
                    lBizHour.UpdatedDate = vUpdatedDate;
                    lBizHour.UpdatedBy = vUpdatedBy;
                    lBizHour.Active = vActive;
                    return AddBizHour(lBizHour);
                }
            }
        }

        public BizHour AddBizHour(BizHour vBizHour)
        {
            try
            {
                Shoppingctx.BizHours.Add(vBizHour);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vBizHour : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteBizHour(BizHour vBizHour)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBizHour(int vBizHourID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteBizHour(BizHour vBizHour)
        {
            throw new NotImplementedException();
        }

        public BizHour UpdateBizHour(BizHour vBizHour)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
