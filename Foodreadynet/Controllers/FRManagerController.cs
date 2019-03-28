using FoodReady.WebUI.EmailServices;
using FoodReady.WebUI.Models;
using FR.Domain.Model.Entities;
using FR.Infrastructure.Helpers;
using FR.Repository.Interfaces;
using FR.Repository.ShoppingRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodReady.WebUI.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class FRManagerController : BaseController
    {
        private IDriverRepository DriverRepository;
        private IOrderRepository OrderRepository;
        private IOrderItemRepository OrderItemRepository;
        private IBizInfoRepository BizInfoRepository;
        private ICreditCardRepository CreditCardRepository;
        public FRManagerController(
                               IUserDetailRepository userDetailRepo, IBizInfoRepository bizInfoRepo,
                               IOrderRepository orderRepo, 
                               IOrderItemRepository OrderItemRepo,IDriverRepository driverRepo,
                               ICreditCardRepository creditCardRepo  )
        {
            BizInfoRepository = bizInfoRepo;
            OrderRepository= orderRepo;
            OrderItemRepository =OrderItemRepo;
            DriverRepository = driverRepo;
            CreditCardRepository = creditCardRepo;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Orders()
        {
            DateTime df = DateTime.Now.AddDays(-1);
            DateTime dt = DateTime.Now;
            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            OrderViewModel ovm = new OrderViewModel();
            ovm.ListAllcities = BizInfoRepository.GetAllCities();
            ovm.selectedcity = "Concord";
            ovm.Orders = OrderRepository.GetOrdersByDateRange(df, dt).Where(e=>ovm.selectedcity.ToLower().StartsWith(e.BizInfo.Address.City.ToLower())).ToList();
            ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " ( " + ovm.Orders.Count.ToString() + ")";
            return View(ovm);
        }
        [HttpPost]
        public ActionResult Orders(string vCity,string vFromDate, string vToDate, string vEmail, string vInvoiceNumber, string vTransactionId)
        {
            OrderViewModel ovm = new OrderViewModel();
            ovm.ListAllcities = BizInfoRepository.GetAllCities();
            ovm.selectedcity =string.IsNullOrEmpty(vCity)?"Concord":vCity;
            DateTime df;
            DateTime dt;
            df = string.IsNullOrEmpty(vFromDate) ? DateTime.Now.AddDays(-1) : DateTime.Parse(vFromDate);
            dt = string.IsNullOrEmpty(vToDate) ? DateTime.Now : DateTime.Parse(vToDate);
            if (df > dt)
            {
                df = DateTime.Now.AddDays(-1);
                dt = DateTime.Now;
            }

            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            if (!string.IsNullOrEmpty(vEmail))
            {
                ovm.Orders = OrderRepository.GetOrdersByEmail(vEmail);
            }
            else if (!string.IsNullOrEmpty(vInvoiceNumber))
            {
                ovm.Orders = OrderRepository.GetOrdersByInvoiceNumber(vInvoiceNumber);
            }
            else if (!string.IsNullOrEmpty(vTransactionId))
            {
                ovm.Orders = OrderRepository.GetOrdersByTransactionId(vTransactionId);
            }
            else
            {
                ovm.Orders = OrderRepository.GetOrdersByDateRange(df, dt).Where(e => ovm.selectedcity.ToLower().StartsWith(e.BizInfo.Address.City.ToLower())).ToList();
            }
            if (ovm.Orders != null)
            {
                ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " ( " + ovm.Orders.Count.ToString() + ")";
            }
            else
            {
                ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " (0)";
            }
            return View(ovm);
        }
        public ActionResult OrderDetails(int id) // id=OrderId
        {
            Order o = new Order();
            o = OrderRepository.GetOrderById(id);
            CreditCard cc = CreditCardRepository.GetCreditCardById(o.CreditCardId);
            ViewBag.creditcard = cc == null ? "" : cc.CreditCardType.Title + "</br>" + cc.CreditCardNumber + "</br>" + cc.ExpirationMonth + "/" + cc.ExpirationYear + "</br>" + cc.SecurityCode;
            return PartialView(o);
        }
        public ActionResult OrderItems(int id) // id=OrderId
        {
            Order o = new Order();
            o = OrderRepository.GetOrderById(id);
            return View(o);
        }
        public ActionResult ItemDetails(int id) // id=OrderItemId
        {
            OrderItem oi = new OrderItem();
            oi = OrderItemRepository.GetOrderItemById(id);
            return PartialView(oi);
        }
        public ActionResult FaxOrder(int id, int orderid) // id=BizInfoId
        {
            string result = "Fax failed!";
            if (id > 0 && orderid > 0)
            {
                Order od = OrderRepository.GetOrderById(orderid);
                EmailManager em = new EmailManager();
                em.FaxBody = EmailManager.BuildOrderFaxBody(od);
                em.SendOrderFax(od.BizInfo.Fax);
                if (em.IsFaxSent)
                {
                    BizInfo bi = BizInfoRepository.GetBizInfoById(id);
                    result = "Order(id=<b>" + orderid + "</b>) " + "has been successfully faxed to <b>" + bi.BizTitle + "</b>";
                }
            }
            ViewBag.faxnote = result;
            return PartialView();

        }
        public ActionResult OrderForDrivers(int id) // id=OrderId
        {
            OrderDriversModel odm = new OrderDriversModel();
            odm.Order = OrderRepository.GetOrderById(id);
            odm.LDrivers =(odm.Order==null)?null: DriverRepository.GetDriversByCity(odm.Order.BizInfo.Address.City, true, true);
            return PartialView(odm);
        }
        public ActionResult EmailOrder(int id, int orderid) // id=DriverId
        {
            ViewBag.msg = "";
            OrderDriversModel odm = new OrderDriversModel();
            odm.Order = OrderRepository.GetOrderById(orderid);
            odm.Driver = DriverRepository.GetDriverById(id);
            EmailManager em = new EmailManager();
            EmailContents ec = new EmailContents();
            ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
            ec.FromName = "foodready.net";
            ec.Subject = "Delivery order";
            ec.To = odm.Driver.Email;
            ec.Body = EmailManager.BuildEmailToDriverHtmlBody(odm.Order, odm.Driver.FirstName + " " + odm.Driver.LastName);
            em.Send(ec);
            if (em.IsSent == false)
            {
                ViewBag.msg = "email failed!";
            }
            else
            {
                OrderRepository.UpdateOrderByEmail(orderid, odm.Driver.FirstName + " " + odm.Driver.LastName, DateTime.Now, UserName);
            }
            return PartialView(odm);
        }

    }
}
