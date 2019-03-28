using System.Web.Mvc;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
using FR.Services.FilterServces;
using System;
using FR.Services.ViewServices;
using FoodReady.WebUI.EmailServices;
using FR.Infrastructure.Helpers;

namespace FoodReady.WebUI.Controllers
{
    public class ReservationController : BaseController
    {
        private IBizInfoRepository BizInfoRepository;
        private IBizCuisineRepository BizCuisineRepository;
        private IBizRVInfoRepository BizRVInfoRepository;
        private IReservationRepository ReservationRepository;
        public ReservationController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo,
                                     IBizRVInfoRepository bizRVInfoRepo, IReservationRepository reservationRepo)
        {
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
            BizRVInfoRepository = bizRVInfoRepo;
            ReservationRepository = reservationRepo;
        }
        public ActionResult Index(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }
        //[OutputCache(Duration = 1000)]
        public ActionResult MainContent()
        {
            BizInfoModel bim = new BizInfoModel();
            bim.ShowMostPopularCities = AllCitiesView.ShowMostPopularCities_bootatrap(BizInfoRepository.GetBizInfoInMostPopularLocalCities(true));
            bim.ShowMostPopularCuisines = AllCuisinesView.ShowMostPopularCuisines_Bootstarp(BizCuisineRepository.GetBizCuisinesInMostPopularCuisines(true));
            return PartialView(bim);
        }
        [HttpPost]
        public ActionResult MakeReservation(int id) // id=BizInfoId
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(id);
            ViewBag.biztitle = bi.BizTitle;
            ViewBag.imgUrl = bi.ImageUrl;
            BizRVInfo brv = bi.GetBizRVInfo;
            ViewBag.maplink = GoogleMapLink(bi); 
            return PartialView(brv);
        }
        [HttpPost]
        public ActionResult FindTable(int id, string rvDate, string rvTime, int rvNum) // id=BizInfoId
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(id);
            ViewBag.biztitle = bi.BizTitle;
            BizRVInfo brv = bi.GetBizRVInfo;
            int totalRV = ReservationRepository.GetAllReservationByBizInfoRVDate(id, rvDate).Count;
            bool seatsOK = (brv.SeatedCapacity - totalRV) > rvNum;
            ViewBag.tableAvailable = (seatsOK && SearchFilter.IsTheDayOpen(bi,DateTime.Parse(rvDate)))? "y" : "n";
            RVConfirmationModel rvc = new RVConfirmationModel();
            rvc.BizInfoId = id;
            rvc.RVDate = rvDate;
            rvc.RVTime = rvTime;
            rvc.RVNum = rvNum;
            ViewBag.datetime = DateTime.Parse(rvDate).ToLongDateString() + " " + rvTime;
            return PartialView(rvc);
        }
        [HttpPost]
        public ActionResult Completed(RVConfirmationModel model)
        {
            bool sendSuccess = false;
            if (ModelState.IsValid)
            {
                if (model.Message == "Please note that not all requests can be accommodated.")
                {
                    model.Message = "";
                }
                BizInfo bi = BizInfoRepository.GetBizInfoById(model.BizInfoId);
                BizRVInfo brv = bi.GetBizRVInfo;
                model.Bizinfo = bi;
                ViewBag.biztitle = bi.BizTitle;
                ViewBag.datetime = DateTime.Parse(model.RVDate).ToLongDateString() + " " + model.RVTime;
                Reservation r = ReservationRepository.AddReservation(0, brv.BizRVInfoId, model.FirstName, model.LastName, model.Phone,
                              model.Email, model.RVDate, model.RVTime, model.RVNum, model.Message, DateTime.Now, model.FirstName + model.LastName, DateTime.Now, model.FirstName + model.LastName, true);
               
                if (r != null)
                {
                    EmailManager em = new EmailManager();
                    EmailContents ec = new EmailContents("FoodReady.Net", model.Email, Globals.Settings.ContactForm.MailFrom,
                                "Restaurant Reservation", EmailManager.BuildRVtoCustomerHtmlBody(model));

                    em.FaxBody = EmailManager.BuildRVtoRestaurantHtmlBody(model);
                   em.SendFax(bi.Fax);
                    em.Send(ec); // send to customer

                    if (em.IsSent == false)
                    {
                        sendSuccess = false;
                        TempData["sentCustomerMsg"] = "Your message has not been sent out for some reasons.";
                    }
                    else
                    {
                        sendSuccess = true;
                        TempData["sentCustomerMsg"] = "Your message has  been sent out successfully.";
                    }
                    ec.FromName = "FoodReady.Net";
                    ec.To = bi.ContactInfo.Email; // to restaurant;
                    ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
                    ec.Subject = "Restaurant Reservation";
                    ec.Body = EmailManager.BuildRVtoRestaurantHtmlBody(model);
                    em.Send(ec);
                    if (em.IsSent == false)
                    {
                        sendSuccess = false;
                        TempData["sentRestaurantMsg"] = "Your message has not been sent out for some reasons.";
                    }
                    else
                    {
                        sendSuccess = true;
                        TempData["sentRestaurantMsg"] = "Your message has  been sent out successfully.";
                    }
                }
                else
                {
                    sendSuccess = false;
                    TempData["addToDBMsg"] = "Adding reservation to database failed for some reasons.";
                }
            }
            ViewBag.SendingSuccess = sendSuccess;
            return View(model);
        }
        [Authorize]
        public ActionResult ViewReservations(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult ViewReservations(string vEmail,ShoppingCart cart)
        {
            List<Reservation> lr = ReservationRepository.GetAllUpcomingReservationByEmail(vEmail, true);
            ViewBag.bagitems = GetCartItems(cart);
            return View(lr);
        }
        [Authorize]
        public ActionResult DoRVCancel(int id) // id=ReservationId
        {
            bool sendSuccess = false;
            Reservation r = new Reservation();
            r = ReservationRepository.GetReservationById(id);
            BizInfo bi = BizInfoRepository.GetBizInfoById(r.BizRVInfo.BizInfoId);
            if (r.Active)
            {
                ReservationRepository.LockReservation (r);
                EmailManager em = new EmailManager();
                EmailContents ec = new EmailContents("FoodReady.Net", r.Email, Globals.Settings.ContactForm.MailFrom,
                            "Reservation cancelled", EmailManager.BuildRVCancelToCustomer(r));

                em.FaxBody = EmailManager.BuildRVCancelToRestaurant(r);
                em.SendFax(bi.Fax);
                em.Send(ec); // send to customer

                if (em.IsSent == false)
                {
                    sendSuccess = false;
                    TempData["sentCustomerMsg"] = "Your message has not been sent out for some reasons.";
                }
                else
                {
                    sendSuccess = true;
                    TempData["sentCustomerMsg"] = "Your message has  been sent out successfully.";
                }
                ec.FromName = "FoodReady.Net";
                ec.To = bi.ContactInfo.Email; // to restaurant;
                ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
                ec.Subject = "Reservation cancelled";
                ec.Body = EmailManager.BuildRVCancelToRestaurant(r);
                em.Send(ec);
                if (em.IsSent == false)
                {
                    sendSuccess = false;
                    TempData["sentRestaurantMsg"] = "Your message has not been sent out for some reasons.";
                }
                else
                {
                    sendSuccess = true;
                    TempData["sentRestaurantMsg"] = "Your message has  been sent out successfully.";
                }
            }
            ViewBag.SendingSuccess = sendSuccess;
            return PartialView();
        }
        [Authorize]
        public ActionResult FindTableForChange(int id) // id=ReservationId
        {
            Reservation rv = ReservationRepository.GetReservationById(id);
            return PartialView(rv);
        }
        [Authorize]
        [HttpPost]
        public ActionResult MakeReservationChange(int id, string rvDate, string rvTime, int rvNum) // id=ReservationId
        {
            Reservation rv = ReservationRepository.GetReservationById(id);
            int totalRV = ReservationRepository.GetAllReservationByBizInfoRVDate(rv.BizRVInfo.BizInfoId, rvDate).Count;
            bool seatsOK = (rv.BizRVInfo.SeatedCapacity - totalRV) > rvNum;
            ViewBag.tableAvailable = (seatsOK && SearchFilter.IsTheDayOpen(rv.BizRVInfo.BizInfo, DateTime.Parse(rvDate))) ? "y" : "n";
            ViewBag.rvdate = rvDate;
            ViewBag.rvtime = rvTime;
            ViewBag.rvnum = rvNum;
            ViewBag.datetime = DateTime.Parse(rvDate).ToLongDateString() + " " + rvTime;
            return PartialView(rv);
        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangeCompleted(int id, string rvDate, string rvTime, int rvNum, string Message) // id=ReservationId
        {
            bool sendSuccess = false; 
            ViewBag.rvdate = rvDate;
            ViewBag.rvtime = rvTime;
            ViewBag.rvnum = rvNum;
            ViewBag.datetime = DateTime.Parse(rvDate).ToLongDateString() + " " + rvTime;
            if (Message=="Please note that not all requests can be accommodated.")
            {
                Message = "";
            }
            Reservation oldRv = ReservationRepository.GetReservationById(id);

            ViewBag.datetime = DateTime.Parse(rvDate).ToLongDateString() + " " + rvTime;
            ReservationRepository.AddReservation(id, oldRv.BizRVInfoId, oldRv.FirstName, oldRv.LastName, oldRv.Phone,
                          oldRv.Email, rvDate, rvTime, rvNum, Message, oldRv.AddedDate, oldRv.AddedBy, DateTime.Now, oldRv.UpdatedBy, true);
            Reservation r = ReservationRepository.GetReservationById(id);
             if (r != null)
             {
                 EmailManager em = new EmailManager();
                 EmailContents ec = new EmailContents("FoodReady.Net", r.Email, Globals.Settings.ContactForm.MailFrom,
                             "Reservation changed", EmailManager.BuildRVChangetoCustomer(r, oldRv));

                 em.FaxBody = EmailManager.BuildRVChangetoRestaurant(r, oldRv);
                em.SendFax(r.BizRVInfo.BizInfo.Fax);
                 em.Send(ec); // send to customer

                 if (em.IsSent == false)
                 {
                     sendSuccess = false;
                     TempData["sentCustomerMsg"] = "Your message has not been sent out for some reasons.";
                 }
                 else
                 {
                     sendSuccess = true;
                     TempData["sentCustomerMsg"] = "Your message has  been sent out successfully.";
                 }
                 ec.FromName = "FoodReady.Net";
                 ec.To = r.BizRVInfo.BizInfo.ContactInfo.Email; // to restaurant;
                 ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
                 ec.Subject = "Reservation changed";
                 ec.Body = EmailManager.BuildRVChangetoRestaurant(r, oldRv);
                 em.Send(ec);
                 if (em.IsSent == false)
                 {
                     sendSuccess = false;
                     TempData["sentRestaurantMsg"] = "Your message has not been sent out for some reasons.";
                 }
                 else
                 {
                     sendSuccess = true;
                     TempData["sentRestaurantMsg"] = "Your message has  been sent out successfully.";
                 }
             }
             else
             {
                 sendSuccess = false;
                 TempData["addToDBMsg"] = "Adding reservation to database failed for some reasons.";
             }
            ViewBag.SendingSuccess = sendSuccess;
            return View(oldRv);
        }
    }
}
