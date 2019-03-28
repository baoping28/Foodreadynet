using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllReservations();
        List<Reservation> GetAllReservations(bool vActive);
        Reservation GetReservationById(int vReservationID);
        List<Reservation> GetAllReservationByBizInfoId(int vBizInfoId, bool vActive);
        List<Reservation> GetAllReservationByRVDate(string vRVDate);
        List<Reservation> GetAllReservationByBizInfoRVDate(int vBizInfoId, string vRVDate);
        int GetNumberOfPeopleOnRVDate(int vBizInfoId, string vRVDate);
        List<Reservation> GetAllReservationByPhone(string vPhone, bool vActive);
        List<Reservation> GetAllReservationByEmail(string vEmail, bool vActive);
        List<Reservation> GetAllUpcomingReservationByEmail(string vEmail, bool vActive);
        int GetReservationsCount(bool vActive);
        List<Reservation> GetReservationsByDateRange(DateTime vFromDate, DateTime vToDate);
        List<Reservation> GetReservationsByDateRange(DateTime vFromDate, DateTime vToDate, bool vActive);
        Reservation AddReservation(int vReservationID, int vBizRVInfoId, string vFirstName, string vLastName, string vPhone,
                           string vEmail, string vRVDate, string vRVTime, int vPartySize, string vMessage,
                           DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        Reservation AddReservation(Reservation vReservation);
        bool LockReservation(Reservation vReservation);
        bool UnlockReservation(Reservation vReservation);
        bool DeleteReservation(Reservation vReservation);
        bool DeleteReservation(int vReservationID);
        bool UnDeleteReservation(Reservation vReservation);
        Reservation UpdateReservation(Reservation vReservation);
    }
}
