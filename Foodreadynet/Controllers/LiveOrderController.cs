using System;
using FR.Repository.Interfaces;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace FoodReady.WebUI.Controllers
{
    [Authorize]
    public class LiveOrderController : BaseController
    {
        private IBizInfoRepository BizInfoRepository;
        private IOrderRepository OrderRepository;
        public LiveOrderController(IBizInfoRepository bizInfoRepo,IOrderRepository orderRepo)
        {
            BizInfoRepository = bizInfoRepo;
            OrderRepository =orderRepo;
        }
        public ActionResult Index()
        {
            BizInfo bi = BizInfoRepository.GetBizInfoByUserId(CurrentUserID);
            ViewBag.bizname = bi.BizTitle;
            ViewBag.bizid = bi.BizInfoId.ToString();
            ViewBag.contime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            return View();
        }
        [HttpPost]
        public ActionResult MarkOrderRemoved(string orderid)
        {
            string success = "failed";
            int i=0;
            if (int.TryParse(orderid,out i))
            {
                success = OrderRepository.UpdateOrderStatus(i)?"success":"failed";
            }
            return Json(new
            {
                result = success
            });
        }
        public ActionResult GetMissedOrders(string id, string fromtime)
        {
            List<Order> lo = new List<Order>();
            int i=0;
            DateTime dt;
            if (int.TryParse(id,out i))
            {

                lo = OrderRepository.GetMissedOrdersByBizId(i, DateTime.TryParse(fromtime, out dt)?dt:DateTime.Now);
            }
            return PartialView(lo);
        }
        public ActionResult MissedOrderCount(string bizid)
        {
            List<Order> lo = new List<Order>();
            int i = 0;
            if (int.TryParse(bizid, out i))
            {
                lo = OrderRepository.GetMissedOrdersByBizId(i);
            }
            return Json(new
            {
                result = lo.Count.ToString()
            });
        }
        public ActionResult GetOrderByNumber(string ordernum)
        {
            Order o = new Order();
            ViewBag.ok = "n";
            int i = 0;
           if (int.TryParse(ordernum,out i))
           {
               if (i>0)
               {
                   o = OrderRepository.GetOrderById(i);
                   ViewBag.ok =o==null?"n":"y";
               }
           }
            return PartialView(o);
        }
    }
}