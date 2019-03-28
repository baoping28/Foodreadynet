using System;
using System.Linq;
using System.Web.Mvc;
using FoodReady.WebUI.Models;
using FR.Domain.Model.Entities;
using FR.Repository.ShoppingRepository;
using FR.Repository.Interfaces;
using FR.Infrastructure.Helpers;
using FoodReady.WebUI.EmailServices;
using System.Collections.Generic;

namespace FoodReady.WebUI.Controllers
{
    public class RewardsController : BaseController
    {
        private IConvertedPointRepository ConvertedPointRepository;
        private IRewardVoucherRepository RewardVoucherRepository;
        private IOrderRepository OrderRepository;
        public RewardsController(IConvertedPointRepository convertedPointRepo, IRewardVoucherRepository rewardVoucherRepo,
                                 IOrderRepository orderRepo)
        {
            ConvertedPointRepository = convertedPointRepo;
            RewardVoucherRepository = rewardVoucherRepo;
            OrderRepository = orderRepo;
        }

        public ActionResult Index(ShoppingCart cart)
        {
            List<RewardVoucher> lrv = new List<RewardVoucher>();
            lrv = RewardVoucherRepository.GetAllRewardVouchers(true);
            ViewBag.bagitems = GetCartItems(cart);
            return View(lrv);
        }
        [Authorize]
        public ActionResult MyPoints(ShoppingCart cart)
        {
            PointSumaryModel psm = new PointSumaryModel();
            psm.UsedPoints=ConvertedPointRepository.GetConvertedPointsByUserId(true,CurrentUserID).Sum(e=>e.ConvertedPoints);
            psm.TotalPoints=(int)(OrderRepository.GetOrdersByLogonName(UserName,true).Sum(e=>e.OrderTotal) * DollarToPoints);
            psm.AvailablePoints = psm.TotalPoints - psm.UsedPoints;
            psm.AvailableGiftList = RewardVoucherRepository.GetAllRewardVouchers(true).Where(e => e.Points < psm.AvailablePoints).ToList();
            psm.QualifiedPoints = RewardVoucherRepository.GetAllRewardVouchers(true).Min(e => e.Points);
            ViewBag.bagitems = GetCartItems(cart);
            return View(psm);
        }
        [Authorize]
        [HttpPost]
        public ActionResult MyPoints(PointSumaryModel model,ShoppingCart cart)
        {
            TempData["sendmsg"] = "";
            model.UsedPoints = ConvertedPointRepository.GetConvertedPointsByUserId(true, CurrentUserID).Sum(e => e.ConvertedPoints);
            model.TotalPoints = (int)(OrderRepository.GetOrdersByLogonName(UserName, true).Sum(e => e.OrderTotal) * DollarToPoints);
            model.AvailablePoints = model.TotalPoints - model.UsedPoints;
            model.AvailableGiftList = RewardVoucherRepository.GetAllRewardVouchers(true).Where(e => e.Points < model.AvailablePoints).ToList();
            model.QualifiedPoints = RewardVoucherRepository.GetAllRewardVouchers(true).Min(e => e.Points);
            ViewBag.bagitems = GetCartItems(cart);
            if (ModelState.IsValid)
            {
                string giftname = "";
                try
                {
                    DateTime dt = DateTime.Now;
                    RewardVoucher rv= RewardVoucherRepository.GetRewardVoucherById(int.Parse(model.GiftChoice));
                    giftname=rv.Title;
                    int pts = rv.Points;
                    ConvertedPoint cp = ConvertedPointRepository.AddConvertedPoint(0, CurrentUserID, int.Parse(model.GiftChoice), pts, dt, UserName, dt, UserName, true);
                }
                catch
                {
                    return View(model);
                }
                TempData["sendmsg"] = "Your request of sending reward gift from foodready.net has been sent to us. Please check your email for datails.";
                SendEmail(model, giftname);
                return RedirectToAction("Appreciate", "Rewards");
             }
            return View(model);
        }
        [NonAction]
        public void SendEmail(PointSumaryModel model, string gift)
        {
            bool sendSuccess = false;
                //ViewBag.qqq = EmailManager.BuildPartnerInfoHtmlBody(model);
                EmailManager em = new EmailManager();
                EmailContents ec = new EmailContents(model.Name, Globals.Settings.ContactForm.MailTo, model.Email,
                                    "Send a reward gift", EmailManager.BuildSendGiftBody(model,gift));

                em.Send(ec); // send to foodready.net
                if (em.IsSent == false)
                {
                    sendSuccess = false;
                }
                else
                {
                    sendSuccess = true;
                }
                ec = new EmailContents("Foodready.net", model.Email, Globals.Settings.ContactForm.MailTo, "You get a gift!", EmailManager.BuildGiftToCustomerBody(model, gift));
                em.Send(ec); // send to Customer
                if (em.IsSent == false)
                {
                    sendSuccess = false;
                }
                else
                {
                    sendSuccess = true;
                }
            ViewBag.SendingSuccess = sendSuccess;
        }

        public ActionResult Appreciate()
        {
            return View();
        }
    }
}
