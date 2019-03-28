using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class ReservationRepository : BaseShoppingCartRepository,IReservationRepository
    {
        #region IReservationRepository Members

        public List<Reservation> GetAllReservations()
        {
            List<Reservation> lReservation = default(List<Reservation>);
            string lReservationKey = CacheKey + "_AllReservations";

            if (base.EnableCaching && (Cache[lReservationKey] != null))
            {
                lReservation = (List<Reservation>)Cache[lReservationKey];
            }
            else
            {
                lReservation = Shoppingctx.Reservations.Include("BizRVInfo").ToList();
                if (base.EnableCaching)
                {
                    CacheData(lReservationKey, lReservation, CacheDuration);
                }
            }
            return lReservation; 
        }

        public List<Reservation> GetAllReservations(bool vActive)
        {
            return GetAllReservations().Where(e => e.Active == vActive).ToList();
        }

        public Reservation GetReservationById(int vReservationID)
        {
            return GetAllReservations().FirstOrDefault(e => e.ReservationId == vReservationID);
        }

        public List<Reservation> GetAllReservationByBizInfoId(int vBizInfoId, bool vActive)
        {
            return GetAllReservations().Where(e =>(e.BizRVInfo.BizInfoId==vBizInfoId && e.Active == vActive)).ToList();
        }
        public List<Reservation> GetAllReservationByRVDate(string vRVDate)
        {
            return GetAllReservations().Where(e =>e.RVDate == vRVDate).ToList();
        }
        public List<Reservation> GetAllReservationByBizInfoRVDate(int vBizInfoId,string vRVDate)
        {
            return GetAllReservations().Where(e =>(e.BizRVInfo.BizInfoId==vBizInfoId &&  e.RVDate == vRVDate)).ToList();
        }
        public int GetNumberOfPeopleOnRVDate(int vBizInfoId, string vRVDate)
        {
            return GetAllReservationByBizInfoRVDate(vBizInfoId,vRVDate).Sum(e =>e.PartySize);
        }
        public List<Reservation> GetAllReservationByPhone(string vPhone, bool vActive)
        {
            throw new NotImplementedException();
        }

        public List<Reservation> GetAllReservationByEmail(string vEmail, bool vActive)
        {
            return GetAllReservations().Where(e => e.Email == vEmail && e.Active==vActive).ToList();
        }

        public List<Reservation> GetAllUpcomingReservationByEmail(string vEmail, bool vActive)
        {
            string s = DateTime.Now.ToShortDateString();
            DateTime today = DateTime.Parse(s);

            return GetAllReservationByEmail(vEmail, vActive).Where(e => DateTime.Compare(DateTime.Parse(e.RVDate), today) >= 0).ToList();
        }
        public int GetReservationsCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public List<Reservation> GetReservationsByDateRange(DateTime vFromDate, DateTime vToDate)
        {
            List<Reservation> lReservations = new List<Reservation>();

            if (vFromDate == null)
            {
                vFromDate = DateTime.MinValue;
            }
            if (vToDate == null)
            {
                vToDate = DateTime.MaxValue;
            }
            if (vFromDate > DateTime.MinValue && vToDate > DateTime.MinValue && vToDate >= vFromDate)
            {
                foreach (var r in Shoppingctx.Reservations.ToList())
                {
                    if (DateTime.Parse(r.RVDate) >= vFromDate && DateTime.Parse(r.RVDate) < vToDate)
                    {
                        lReservations.Add(r);
                    }
                }
                return lReservations.OrderByDescending(e=>DateTime.Parse(e.RVDate)).ToList();
            }

            return lReservations;
        }

        public List<Reservation> GetReservationsByDateRange(DateTime vFromDate, DateTime vToDate, bool vActive)
        {
            return GetReservationsByDateRange(vFromDate, vToDate).Where(e => e.Active == vActive).ToList();
        }
        public Reservation AddReservation(int vReservationID,int vBizRVInfoId,string vFirstName,string vLastName, string vPhone, 
                           string vEmail, string vRVDate,string vRVTime,int vPartySize,string vMessage,
                           DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            Reservation lReservation = new Reservation();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vReservationID > 0)
                {
                    lReservation = frctx.Reservations.FirstOrDefault(u => u.ReservationId == vReservationID);
                    lReservation.FirstName = vFirstName;
                    lReservation.LastName = vLastName;
                    lReservation.Phone = vPhone;
                    lReservation.Email = vEmail;
                    lReservation.RVDate = vRVDate;
                    lReservation.RVTime = vRVTime;
                    lReservation.PartySize = vPartySize;
                    lReservation.Message = vMessage;

                    lReservation.UpdatedDate = vUpdatedDate;
                    lReservation.UpdatedBy = vUpdatedBy;
                    lReservation.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lReservation : null;
                }
                else
                {
                    lReservation.FirstName = vFirstName;
                    lReservation.LastName = vLastName;
                    lReservation.Phone = vPhone;
                    lReservation.Email = vEmail;
                    lReservation.RVDate = vRVDate;
                    lReservation.RVTime = vRVTime;
                    lReservation.PartySize = vPartySize;
                    lReservation.Message = vMessage;

                    lReservation.Message = vMessage;
                    lReservation.BizRVInfoId = vBizRVInfoId;
                    lReservation.AddedDate = vAddedDate;
                    lReservation.AddedBy = vAddedBy;
                    lReservation.UpdatedDate = vUpdatedDate;
                    lReservation.UpdatedBy = vUpdatedBy;
                    lReservation.Active = vActive;
                    return AddReservation(lReservation);
                }
            }
        }

        public Reservation AddReservation(Reservation vReservation)
        {
            try
            {
                    Shoppingctx.Reservations.Add(vReservation);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vReservation : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockReservation(Reservation vReservation)
        {
            return ChangeLockState(vReservation, false);
        }

        public bool UnlockReservation(Reservation vReservation)
        {
            return ChangeLockState(vReservation, true);
        }

        private bool ChangeLockState(Reservation vReservation, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                Reservation up = frenty.Reservations.FirstOrDefault(e => e.ReservationId == vReservation.ReservationId);
                up.UpdatedBy = CurrentUser.Identity.Name;
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteReservation(Reservation vReservation)
        {
            throw new NotImplementedException();
        }

        public bool DeleteReservation(int vReservationID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteReservation(Reservation vReservation)
        {
            throw new NotImplementedException();
        }

        public Reservation UpdateReservation(Reservation vReservation)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
