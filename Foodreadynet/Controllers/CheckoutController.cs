using System.Web.Mvc;
using System.Linq;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using FR.Domain.Model.Entities;
using FR.Services.FilterServces;
using System;
using com.paypal.sdk.util;
using FR.Infrastructure.PaypalAPI;
using FR.Infrastructure.Helpers;
using FoodReady.WebUI.EmailServices;
using com.paypal.sdk.services;
using System.IO;
using System.Text;
namespace FoodReady.WebUI.Controllers
{
    public class CheckoutController : BaseController
    {
       private IProductRepository ProductRepository;
        private IBizInfoRepository BizInfoRepository;
        private IBizCuisineRepository BizCuisineRepository;
        private ICuisineTypeRepository CuisineTypeRepository;
        private ICategoryRepository CategoryRepository;
        private IProductToppingRepository ProductToppingRepository;
        private IProductDressingRepository ProductDressingRepository;
        private IDiscountCouponRepository DiscountCouponRepository;
        private IFreeItemCouponRepository FreeItemCouponRepository;
        private IUserDetailRepository UserDetailRepository;
        private ICreditCardTypeRepository CreditCardTypeRepository;
        private IOrderRepository OrderRepository;
        private ICreditCardRepository CreditCardRepository;
        private IGiftCardRepository GiftCardRepository;
        public CheckoutController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo,
                                     ICuisineTypeRepository cuisineTypeRepo, ICategoryRepository categoryRepo,
                                     IProductRepository productRepo, IProductToppingRepository productToppingRepo,
                                     IProductDressingRepository productDressingRepo,IDiscountCouponRepository discountCouponRepo,
                                     IFreeItemCouponRepository FreeItemCouponRepo, IUserDetailRepository userDetailRepo,
                                     ICreditCardTypeRepository creditCardTypeRepo, IOrderRepository orderRepo, 
                                     ICreditCardRepository creditCardRepo,IGiftCardRepository giftCardRepo)
        {
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
            CuisineTypeRepository = cuisineTypeRepo;
            CategoryRepository = categoryRepo;
            ProductRepository = productRepo;
            ProductToppingRepository = productToppingRepo;
            ProductDressingRepository = productDressingRepo;
            DiscountCouponRepository = discountCouponRepo;
            FreeItemCouponRepository = FreeItemCouponRepo;
            UserDetailRepository = userDetailRepo;
            CreditCardTypeRepository = creditCardTypeRepo;
            OrderRepository =orderRepo;
            CreditCardRepository = creditCardRepo;
            GiftCardRepository =giftCardRepo;
            TempData["valid"] = "";
        }
        public ActionResult Index(ShoppingCart cart,BrowseHistory bh)
        {
            HandleCart(cart);
            cart.ScheduleDate = "Today";
            cart.ScheduleTime = "ASAP";
            if (cart.SubTotal()<=0.0m || string.IsNullOrEmpty(cart.CartKey)==false)
            {
                return Redirect("~/Home");
            }
            PayModel pm = new PayModel();
            CheckoutModel cm = new CheckoutModel();
            CheckoutPayPalModel cppm = new CheckoutPayPalModel();
            cm.AddressLine = bh.Address;
            cm.City = bh.City;
            cm.ZipCode = bh.Zip;
            cm.State = string.IsNullOrEmpty(bh.State) ? "CA" : bh.State;
            cppm.PPAddressLine = bh.Address;
            cppm.PPCity = bh.City;
            cppm.PPZipCode = bh.Zip;
            cppm.PPState = string.IsNullOrEmpty(bh.State) ? "CA" : bh.State;

            if (User.Identity.IsAuthenticated)
            {
                UserDetail ud = UserDetailRepository.GetUserDetailByUserId(CurrentUserID);
                if (ud != null)
                {
                    cm.FirstName = ud.FirstName;
                    cm.LastName = ud.LastName;
                    cm.AddressLine = ud.Address.AddressLine;
                    cm.City = ud.Address.City;
                    cm.State = ud.Address.State;
                    cm.ZipCode = ud.Address.ZipCode;
                    cm.Phone = ud.ContactInfo.Phone;
                    cm.Email = ud.ContactInfo.Email;

                    cppm.PPFirstName = ud.FirstName;
                    cppm.PPLastName = ud.LastName;
                    cppm.PPAddressLine = ud.Address.AddressLine;
                    cppm.PPCity = ud.Address.City;
                    cppm.PPState = ud.Address.State;
                    cppm.PPZipCode = ud.Address.ZipCode;
                    cppm.PPPhone = ud.ContactInfo.Phone;
                    cppm.PPEmail = ud.ContactInfo.Email;
                }
            }

            cm.Cart = cart;
            cppm.PPCart = cart;
            ViewBag.id = cart.BizId == 0 ? "/Home" : "/Restaurants/Menu/" + cart.BizId;
            cm.BizInfo = BizInfoRepository.GetBizInfoById(cart.BizId);
            cppm.PPBizInfo = cm.BizInfo;
            pm.CreditCardModel = cm;
            pm.PaypalModel = cppm;
            ViewBag.servicephone = ServicePhone;
            ViewBag.bagitems = GetCartItems(cart);

            return View(pm);
        }
        [HttpPost]
        public ActionResult CheckoutConfirmation(CheckoutModel cm,ShoppingCart cart)
        {

            HandleCart(cart);
            TempData["valid"] = "";
            if (ModelState.IsValid)
            {
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            ViewBag.ScheduleOK = (SearchFilter.IsOpenAt(bi, cart.ScheduleDate=="Tomorrow"?2:1, cart.ScheduleTime))?"OK":"NO";
            cm.Cart = cart;
            cm.BizInfo = bi;
            base.CurrentCreditCard = new CreditCard();
            base.CurrentCreditCard.CreditCardId = 0; // base.CurrentCreditCard.CreditCardID=any
            base.CurrentCreditCard.UserName = string.IsNullOrEmpty(UserName)?cm.BillFirstName + " " +  cm.BillLastName:UserName;
            base.CurrentCreditCard.CreditCardTypeId = int.Parse(cm.CardType);
            base.CurrentCreditCard.FirstName = cm.BillFirstName;
            base.CurrentCreditCard.LastName = cm.BillLastName;
            /*
            base.CurrentCreditCard.AddressLine = cm.BillAddressLine;
            base.CurrentCreditCard.AddressLine2 = "";
            base.CurrentCreditCard.City = cm.BillCity;

            base.CurrentCreditCard.State = cm.BillState;
            base.CurrentCreditCard.ZipCode = cm.BillZipCode;
            base.CurrentCreditCard.Country = "US";
            */
            base.CurrentCreditCard.Phone = cm.Phone;
            base.CurrentCreditCard.Email = cm.Email;

            base.CurrentCreditCard.CreditCardNumber = cm.CardNumber;
            base.CurrentCreditCard.ExpirationMonth = int.Parse(cm.ExpirationMonth);
            base.CurrentCreditCard.ExpirationYear = int.Parse(cm.ExpirationYear);
            base.CurrentCreditCard.SecurityCode = int.Parse(cm.SecurityCode);
            base.CurrentCreditCard.AddedDate = DateTime.Now;
            base.CurrentCreditCard.AddedBy = string.IsNullOrEmpty(UserName) ? cm.BillFirstName + " " + cm.BillLastName : UserName; ;
            base.CurrentCreditCard.UpdatedDate = DateTime.Now;
            base.CurrentCreditCard.UpdatedBy = string.IsNullOrEmpty(UserName) ? cm.BillFirstName + " " + cm.BillLastName : UserName; ;
            base.CurrentCreditCard.Active = true;


            base.CurrentOrder = new Order();
            base.CurrentOrder.FirstName = cm.FirstName;
            base.CurrentOrder.LastName = cm.LastName;
            base.CurrentOrder.RoomNumber = cm.RoomNumber;
            base.CurrentOrder.Street = cm.AddressLine;
            base.CurrentOrder.City = cm.City;
            base.CurrentOrder.State = cm.State;
            base.CurrentOrder.ZipCode = cm.ZipCode;

            string approxtime = string.Empty;
            string mysAddress = cm.AddressLine + ", " + cm.City + ", " + cm.State + " " + cm.ZipCode;
            ViewBag.ValidateAddress = string.Empty;
            if (string.IsNullOrEmpty(cm.AddressLine) || string.IsNullOrEmpty(cm.City) || string.IsNullOrEmpty(cm.State) || string.IsNullOrEmpty(cm.ZipCode))
            {
                ViewBag.ValidateAddress = "Oops, feel like something wrong with you address. Please check it and try again.";
            }
            else
            {
                decimal st = SearchFilter.GetDistance(mysAddress, bi.BizAddressString, out approxtime);
                string xx = approxtime;
                if (st < 0.0m)
                {
                    ViewBag.ValidateAddress = "Oops, feel like something wrong with you address. Please check it and try again.";
                }
                else if (st > bi.DeliveryRadius && bi.Delivery)
                {
                    ViewBag.ValidateAddress = "Sorry, your address is beyond the area this restaurant deliveries. Please try other restaurants.";
                }
            }

            base.CurrentOrder.LogonName = UserName;
            base.CurrentOrder.IsLoggedUser = User.Identity.IsAuthenticated;
            base.CurrentOrder.CustomerMessage = cm.Instructions;
            base.CurrentOrder.IsDelivery = cart.IsDelivery;
            base.CurrentOrder.ServiceCharge = cart.serviceCharge;
            base.CurrentOrder.DeliveryCharge = cart.DeliveryFee;
            base.CurrentOrder.DriverTip = cart.DriverTip;
            base.CurrentOrder.IpAddress = base.CurrentUserIP;
            base.CurrentOrder.Phone = cm.Phone;
            base.CurrentOrder.Email = cm.Email;
            base.CurrentOrder.ScheduleDate = cm.ScheduleDate;
            base.CurrentOrder.ScheduleTime = cm.ScheduleTime;
            base.CurrentOrder.CouponChoice = cart.CouponChoice;
            base.CurrentOrder.OrderTax = cart.Tax();
            base.CurrentOrder.SubTotal = cart.SubTotal();
            base.CurrentOrder.OrderTotal = cart.Total();
            base.CurrentOrder.BizInfoId = cart.BizId;

            ViewBag.servicephone = ServicePhone;
            ViewBag.maplink = GoogleMapLink(cm.BizInfo); 
            return PartialView(cm);
            }
            TempData["valid"] = "check input and try again.";
            return RedirectToAction("Index", "Checkout");

        }

        [HttpPost]
        public ActionResult ProcessSchedule(string schedule, string time, ShoppingCart cart)
        {
            HandleCart(cart);
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            bool isopen = SearchFilter.IsOpenAt(bi, int.Parse(schedule), time); 
            cart.ScheduleDate = schedule == "2" ? "Tomorrow" : "Today";
            cart.ScheduleTime = time;
            return Json(new
            {
                schDate = cart.ScheduleDate,
                schTime = cart.ScheduleTime,
                result = isopen?"success":"failed"
            }); 
        }

        [HttpPost]
        public ActionResult ProcessCheckout(ShoppingCart cart)
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            string restaurantUserName = bi.AspNetUser.UserName; // GetUserNameByUserId(bi.UserId)
            string success = "failed";
            string addDatabase = "failed";
            string orderlist = string.Empty;
            string bizTitle = bi.BizTitle;
            if (DoDirectPaymentSuccess(cart.Total()))
            {
                success = "success";
                if (DoCheckoutCompleted(cart))
                {
                    // base.CurrentOrder.OrderId = 99999;
                    addDatabase = "success";
                    orderlist = BuildOrderList(cart);
                    Cleanup(cart);
                }
            }
            return Json(new
            {
                result = success,
                databaseresult = addDatabase,
                dataResult = orderlist,
                username= restaurantUserName,
                biztitle = bizTitle,
                orderId=base.CurrentOrder.OrderId
            });
        }
        [NonAction]
        public bool DoDirectPaymentSuccess(decimal vCreditTotal)
        {
            bool success = false;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            com.paypal.sdk.services.NVPCallerServices caller = PayPalAPI.PayPalAPIInitialize();
            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "DoDirectPayment";
            encoder["PAYMENTACTION"] = "Sale";
            encoder["IPADDRESS"] = Helper.CurrentUserIP;
            encoder["AMT"] = Helper.FormatPriceToPayPalStringFormat(vCreditTotal);
            encoder["CREDITCARDTYPE"] = CreditCardTypeRepository.GetCreditCardTypeById(base.CurrentCreditCard.CreditCardTypeId).Title;
            encoder["ACCT"] = base.CurrentCreditCard.CreditCardNumber;
            encoder["EXPDATE"] = base.CurrentCreditCard.ExpirationMonth.ToString() + base.CurrentCreditCard.ExpirationYear.ToString();
            encoder["CVV2"] = base.CurrentCreditCard.SecurityCode.ToString();
            encoder["FIRSTNAME"] = base.CurrentCreditCard.FirstName;
            encoder["LASTNAME"] = base.CurrentCreditCard.LastName;
           /*
            encoder["STREET"] = base.CurrentCreditCard.AddressLine;
            encoder["CITY"] = base.CurrentCreditCard.City;
            encoder["STATE"] = base.CurrentCreditCard.State;
            encoder["ZIP"] = base.CurrentCreditCard.ZipCode;
            encoder["COUNTRYCODE"] = "US";
            */
            encoder["CURRENCYCODE"] = "USD";

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = caller.Call(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"];
            if (strAck != null && (strAck == "Success" || strAck == "SuccessWithWarning"))
            {
                Session["result"] = decoder;
                // string pStrResQue = "API=" + "DoDirect Payment ";
                success = true;
            }
            else
            {
                Session["errorresult"] = decoder;
            }
            return success;
        }
        [NonAction]
        public bool DoCheckoutCompleted(ShoppingCart cart)
        {
            bool completed = false;
            NVPCodec decoderResults = new NVPCodec();
            decoderResults = (NVPCodec)Session["result"];
            TempData["OrderSummaryTitle"] = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // string res = Util.BuildResponse(Session["result"], Request.QueryString.Get("API"), "Thank you for your payment!");

            if (User.Identity.IsAuthenticated)
            {
                base.CurrentCreditCard.CreditCardId = CreditCardRepository.GetCreditCardId(User.Identity.Name, base.CurrentCreditCard.CreditCardTypeId, base.CurrentCreditCard.CreditCardNumber, base.CurrentCreditCard.ExpirationMonth, base.CurrentCreditCard.ExpirationYear, base.CurrentCreditCard.SecurityCode);
                if (base.CurrentCreditCard.CreditCardId == 0)
                {
                        CreditCard cc = AddCreditCardToDB();
                        base.CurrentCreditCard.CreditCardId = cc == null ? 0 : cc.CreditCardId;
                }
            }
            else
            {
                base.CurrentCreditCard.CreditCardId = 0;//guest
            }
            Order od = AddOrderToDB(cart, decoderResults["TRANSACTIONID"].ToString(), false, 0, 0.0m);
            base.CurrentOrder.OrderId = od.OrderId;
            EmailManager em = new EmailManager();
            EmailContents ec = new EmailContents();
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            em.EMBizInfo =bi;
            em.EMShoppingCart = cart;

            em.OrderTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
            TempData["DeliveryAddress"] = "";
            em.OrderType = cart.IsDelivery ? "Delivery" : "Pickup";
            em.PaymentType = "Credit Card";
            em.ScheduleDateTime = base.CurrentOrder.ScheduleTime + ", " + base.CurrentOrder.ScheduleDate;
            em.OrderNumber = (800000000 + od.OrderId).ToString();
             em.Name = base.CurrentOrder.FirstName + " " + base.CurrentOrder.LastName;

            em.Address = base.CurrentOrder.Street;
            em.City = base.CurrentOrder.City;
            em.State = base.CurrentOrder.State;
            em.Zip = base.CurrentOrder.ZipCode;
            em.Phone = base.CurrentCreditCard.Phone;
            em.IpAddress = base.CurrentUserIP;
            em.CreditCardType = CreditCardTypeRepository.GetCreditCardTypeById(base.CurrentCreditCard.CreditCardTypeId).Title;
            em.CreditCardNumber = "xxxx-xxxx-xxxx-" + base.CurrentCreditCard.CreditCardNumber.Substring(base.CurrentCreditCard.CreditCardNumber.Length - 4);
            em.ExpirationDate = base.CurrentCreditCard.ExpirationMonth.ToString() + "/" + base.CurrentCreditCard.ExpirationYear.ToString();
            em.SecurityCode = "xx" + base.CurrentCreditCard.SecurityCode.ToString().Substring(base.CurrentCreditCard.SecurityCode.ToString().Length - 1); ;
            
            em.Subtotal = Helper.FormatPriceWithDollar(cart.SubTotal()); ;
            em.Tax = ToUSD(cart.Tax().ToString("N2"));
            em.CouponChoice = base.CurrentOrder.CouponChoice;
            em.ServiceCharge = ToUSD(base.CurrentOrder.ServiceCharge.ToString("N2"));
            em.DeliveryCharge = ToUSD(base.CurrentOrder.DeliveryCharge.ToString("N2"));
            em.Tip = ToUSD(base.CurrentOrder.DriverTip.ToString("N2"));
            em.DriveName = base.CurrentOrder.DriverName;
            em.CouponChoice = base.CurrentOrder.CouponChoice;
            em.Instruction = base.CurrentOrder.CustomerMessage;
            em.Total = Helper.FormatPriceWithDollar(decoderResults["AMT"].ToString()); // ToUSD(cart.Total().ToString("N2"));
            ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
            ec.FromName = "FoodReady.Net";
            ec.Subject = "New online order";
            ec.To = Globals.Settings.ContactForm.MailTo; // send to FoodReady.Net
            ec.Body = em.BuildEmailHtmlBodyForOrder();
            em.FaxBody = em.BuildFaxHtmlBodyForOrder();
            em.SendFax(bi.Fax);
            em.Send(ec);
            ec.To = base.CurrentOrder.Email; // send to user
            em.Send(ec);
            ec.To = bi.ContactInfo.Email; // send to restaurant
            em.Send(ec);

            if (em.IsSent == false)
            {
                completed = false;
                TempData["EmailSentMsg"] = ec.Body;
                // TempData["paymentResult"] = "Send Email failed";
            }
            else
            {
                completed = true;
                TempData["EmailSentMsg"] = ec.Body;
                //TempData["paymentResult"] = string.Empty;
            }
            return completed;
        }

        [NonAction]
        public CreditCard AddCreditCardToDB()
        {
            CreditCard cc = new CreditCard();
            cc = CreditCardRepository.InsertCreditCard(0, UserName, base.CurrentCreditCard.CreditCardTypeId,
               base.CurrentCreditCard.FirstName, base.CurrentCreditCard.LastName, base.CurrentCreditCard.CreditCardNumber, base.CurrentCreditCard.ExpirationMonth,
               base.CurrentCreditCard.ExpirationYear, base.CurrentCreditCard.SecurityCode, base.CurrentCreditCard.Phone,
               base.CurrentCreditCard.Email, DateTime.Now, UserName, DateTime.Now, UserName, true);
            base.CurrentCreditCard.CreditCardId = cc.CreditCardId;
            return cc;
        }
        [NonAction]
        public Order AddOrderToDB(ShoppingCart cart, string transactionid,bool vIsGiftCardPayment,int vGiftCardId,decimal vGiftCardAmountPay)
        {
            string st = (string.IsNullOrEmpty(base.CurrentOrder.RoomNumber)) ? base.CurrentOrder.Street : "#" + base.CurrentOrder.RoomNumber + " " + base.CurrentOrder.Street;
            string username = User.Identity.IsAuthenticated? UserName:"$g$";
            DateTime dt = DateTime.Now;
              return OrderRepository.InsertOrder(cart, username, cart.BizId, 2, base.CurrentCreditCard.CreditCardId,User.Identity.IsAuthenticated,
                                              cart.IsDelivery,cart.DeliveryFee, cart.SubTotal(), cart.Tax(),cart.Total(),cart.serviceCharge,cart.DriverTip,base.CurrentOrder.FirstName,
                                              base.CurrentOrder.LastName, st, base.CurrentOrder.City,
                                              base.CurrentOrder.State, base.CurrentOrder.ZipCode,base.CurrentOrder.Phone,
                                              base.CurrentOrder.Email, transactionid, dt, username, dt, username, true,
                                              cart.ScheduleDate,cart.ScheduleTime,cart.CouponChoice,false,cart.BizDiscountAmount,
                                              cart.DiscountAmount, cart.BizSubTotal(), cart.BizTotal(), base.CurrentOrder.CustomerMessage, "N/A", CurrentUserIP, vIsGiftCardPayment, vGiftCardId, vGiftCardAmountPay);
            
       
        }

        [NonAction]
        public string BuildOrderList(ShoppingCart cart)
        {
            string pagepath = "";
            if (cart.IsFinalSharedCart)
            {
                pagepath = "~/Content/HTMLPages/SharedOrderList.htm";
            }
            else
            {
                pagepath = "~/Content/HTMLPages/OrderList.htm";
            }
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+orderNumber+", (800000000 + base.CurrentOrder.OrderId).ToString());
            msgbody = msgbody.Replace("+orderTime+", DateTime.Now.ToShortTimeString());
            msgbody = msgbody.Replace("+orderType+", cart.IsDelivery ? "Delivery" : "Pickup");
            msgbody = msgbody.Replace("+scheduledTime+", cart.ScheduleTime + ", " + cart.ScheduleDate);
            msgbody = msgbody.Replace("+orderItems+", GetOrderItems(cart));
            return msgbody;
        }
        [NonAction]
        public string GetOrderItems(ShoppingCart cart)
        {
            StringBuilder sb = new StringBuilder();
            if (cart != null)
            {
                foreach (ShoppingCartItem sci in cart.Items)
                {
                    sb.Append("<div class='row'>");
                    sb.Append("<div class='col-sm-2 col-md-2'>");
                    sb.Append("<b>" + sci.Quantity.ToString() + "</b>");
                    sb.Append(" </div><div class='col-sm-10 col-md-10'>");
                    sb.Append("<b>" + sci.Title + "</b>");
                    if (string.IsNullOrEmpty(sci.ProductSizeTitle) == false)
                    {
                        sb.Append("(" + (sci.ProductSizeTitle) + ")");
                    }
                    if (string.IsNullOrEmpty(GetItemNote(sci)) == false)
                    {
                        sb.Append("<br/>" + GetItemNote(sci));
                    }
                    sb.Append(" </div></div><hr />");
                }
                if (string.IsNullOrEmpty(cart.FreeItem) == false)
                {
                    sb.Append("<div class='row'>");
                    sb.Append("<div class='col-sm-12 col-md-12'>");
                    sb.Append(cart.FreeItem + "</div></div><hr />");
                }
                if (string.IsNullOrEmpty(base.CurrentOrder.CustomerMessage) == false)
                {
                    sb.Append("<div class='row'>");
                    sb.Append("<div class='col-sm-12 col-md-12'>");
                    sb.Append("Instructions: " + base.CurrentOrder.CustomerMessage + "</div></div><hr />");
                }
                return sb.ToString();
            }
            return string.Empty;
        }
        [NonAction]
        public string GetItemNote(ShoppingCartItem line)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(line.DressingChoice) == false)
            {

                str = "Dressing:" + line.DressingChoice;
            }
            if (string.IsNullOrEmpty(line.SelectedFreeToppings) == false)
            {
                str += "Free toppings:<br />" + line.SelectedFreeToppings + "<br/>";
            }
            if (line.ToppingList.Count > 0)
            {
                str += "Extra toppings for each:<br />" + line.ToppingListNote;
            }
            if (line.IsSpicy && string.IsNullOrEmpty(line.HowSpicy) == false)
            {
                str += "Spicy: " + line.HowSpicy + "<br />";
            }
            if (string.IsNullOrEmpty(line.SideChoice) == false)
            {
                str += "Side choice: " + line.SideChoice + "<br />";
            }
            if (string.IsNullOrEmpty(line.SauceChoice) == false)
            {
                str += "Sauce choice: " + line.SauceChoice + "<br />";
            }
            if (string.IsNullOrEmpty(line.CrustChoice) == false)
            {
                str += "Crust choice: " + line.CrustChoice + "<br />";
            }
            if (string.IsNullOrEmpty(line.CheeseChoice) == false)
            {
                str += "Cheese Amount: " + line.CheeseChoice + "<br />";
            }
            if (line.AddSideList.Count > 0)
            {
                str += line.AddSideListNote;
            }
            if (string.IsNullOrEmpty(line.Instruction) == false)
            {
                str += "Instructions: " + line.Instruction;
            }
            return str;

        }
        [HttpPost]
        public ActionResult PayWithPayPal(CheckoutPayPalModel cppm, ShoppingCart cart)
        {
            HandleCart(cart);

            TempData["OrderSummaryTitle"] ="";
            TempData["DeliveryAddress"] = "";
            TempData["BizAddress"] = "";
            //return View("APIError");
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
                ViewBag.emptyCart = "Sorry, your cart is empty!";
            }
            else
            {
                ViewBag.emptyCart = "";
            }
            if (ModelState.IsValid)
            {
                cppm.PPBizInfo = BizInfoRepository.GetBizInfoById(cart.BizId);
                TempData["OrderSummaryTitle"] = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                TempData["DeliveryAddress"] = cppm.PPFirstName + " " + cppm.PPLastName +
                        "<br/>" + cppm.PPAddressLine + "<br/>" + cppm.PPCity + ", " + cppm.PPState + " " + cppm.PPZipCode;
                TempData["BizAddress"] = cppm.PPBizInfo.BizAddressString;


                base.CurrentOrder = new Order();
                base.CurrentOrder.FirstName = cppm.PPFirstName;
                base.CurrentOrder.LastName = cppm.PPLastName;
                base.CurrentOrder.RoomNumber = cppm.PPRoomNumber;
                base.CurrentOrder.Street = cppm.PPAddressLine;
                base.CurrentOrder.City = cppm.PPCity;
                base.CurrentOrder.State = cppm.PPState;
                base.CurrentOrder.ZipCode = cppm.PPZipCode;

                string approxtime = string.Empty;
                string mysAddress = cppm.PPAddressLine + ", " + cppm.PPCity + ", " + cppm.PPState + " " + cppm.PPZipCode;
                decimal st = SearchFilter.GetDistance(mysAddress, cppm.PPBizInfo.BizAddressString, out approxtime);
                string xx = approxtime;
                ViewBag.ValidateAddress = string.Empty;
                if (st < 0.0m)
                {
                    ViewBag.ValidateAddress = "Oops, feel like something wrong with you address. Please check it and try again.";
                    return View("APIError");
                }
                else if (st > cppm.PPBizInfo.DeliveryRadius && cppm.PPBizInfo.Delivery)
                {
                    ViewBag.ValidateAddress = "Sorry,your address is beyond the area that this restaurant deliveries. Please try other restaurants.";
                    return View("APIError");
                } 

                base.CurrentOrder.LogonName = UserName;
                base.CurrentOrder.IsLoggedUser = User.Identity.IsAuthenticated;
                base.CurrentOrder.CustomerMessage = cppm.PPInstructions;
                base.CurrentOrder.IsDelivery = cart.IsDelivery;
                base.CurrentOrder.ServiceCharge = cart.serviceCharge;
                base.CurrentOrder.DeliveryCharge = cart.DeliveryFee;
                base.CurrentOrder.DriverTip = cart.DriverTip;
                base.CurrentOrder.IpAddress = base.CurrentUserIP;
                base.CurrentOrder.Phone = cppm.PPPhone;
                base.CurrentOrder.Email = cppm.PPEmail;
                base.CurrentOrder.ScheduleDate = cppm.PPScheduleDate;
                base.CurrentOrder.ScheduleTime = cppm.PPScheduleTime;
                base.CurrentOrder.CouponChoice = cart.CouponChoice;
                base.CurrentOrder.OrderTax = cart.Tax();
                base.CurrentOrder.SubTotal = cart.SubTotal();
                base.CurrentOrder.OrderTotal = cart.Total();
                base.CurrentOrder.BizInfoId = cart.BizId;

                string amount = Helper.FormatPriceToPayPalStringFormat(cart.Total());
                string url = Request.Url.Scheme + "://" + Globals.Settings.SiteDomainName + "/";
                string cancelURL = url + "Checkout";
                //string cancelURL = Request.Url.AbsoluteUri + "?cancel=true"; //url + ResolveUrl("SetExpressCheckout.aspx");
                string returnURL = url + "Checkout/DoExpressPayment?&amount=" + amount;

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                NVPCallerServices caller = new NVPCallerServices();
                caller = PayPalAPI.PayPalAPIInitialize();
                NVPCodec encoder = new NVPCodec();
                encoder["VERSION"] = "63.0";
                encoder["METHOD"] = "SetExpressCheckout";
                encoder["CUSTOMERSERVICENUMBER"] = "510-512-6789";

                // Add request-specific fields to the request.
                encoder["RETURNURL"] = returnURL;
                encoder["CANCELURL"] = cancelURL;
                encoder["PAYMENTREQUEST_0_AMT"] = amount;
                encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
                encoder["PAYMENTREQUEST_0_ALLOWEDPAYMENTMETHOD"] = "InstantPaymentOnly";
                encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "USD";
                encoder["NOSHIPPING"] = "1";
                // Execute the API operation and obtain the response.
                string pStrrequestforNvp = encoder.Encode();
                string pStresponsenvp = caller.Call(pStrrequestforNvp);

                NVPCodec decoder = new NVPCodec();
                decoder.Decode(pStresponsenvp);
                string strAck = decoder["ACK"];
                if (strAck != null && (strAck == "Success" || strAck == "SuccessWithWarning"))
                {
                    Session["TOKEN"] = decoder["TOKEN"];
                    string ECURL = Helper.GetPayPalServerUrl() + "?cmd=_express-checkout" + "&token=" + decoder["TOKEN"] + "&useraction=commit";
                    //Response.Redirect(ECURL);
                    return Redirect(ECURL);
                }
                else
                {
                    TempData["EmailSentStatus"] = "API Failed";
                    return View("APIError");
                }
            }
            else
            {
                TempData["EmailSentStatus"] = "Model State Error";
                return View("APIError");
            }
        }
        public ActionResult DoExpressPayment(ShoppingCart cart)
        {
            TempData["paymentResult"] = string.Empty;
            TempData["EmailSentMsg"] = string.Empty;
            string orderlist = string.Empty;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            com.paypal.sdk.services.NVPCallerServices caller = PayPalAPI.PayPalAPIInitialize();
            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "GetExpressCheckoutDetails";
            encoder["TOKEN"] = Session["TOKEN"].ToString();
            encoder["VERSION"] = "63.0";
            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = caller.Call(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            int doPaymentResult = 1;
            string strAck = decoder["ACK"];
            if (strAck != null && (strAck == "Success" || strAck == "SuccessWithWarning"))
            {
                Session["PAYERID"] = decoder["PAYERID"];
                Session["AMT"] = decoder["AMT"];
                Session["GetExpressCheckoutDetails"] = decoder;
                doPaymentResult = MakeExpressCheckoutPayment(cart);
                if (doPaymentResult == 1)
                {
                    TempData["EmailSentStatus"] = "Transaction failed";
                    return View("APIError");
                }
                else if (doPaymentResult == 2)
                {
                    TempData["EmailSentMsg"] = "SendEmail failed";
                    return View("APIError");
                }
                orderlist = BuildOrderList(cart);
            }
            else
            {
                TempData["EmailSentStatus"] = "Transaction failed";
                return View("APIError");
            }
            ViewBag.servicephone = ServicePhone;
            BizInfo bi=BizInfoRepository.GetBizInfoById(cart.BizId);
            ViewBag.maplink = GoogleMapLink(bi);
            ViewBag.total=cart.Total().ToString("N2");
            ViewBag.isDelivery = cart.IsBizDelivery;
            ViewBag.bagitems = "";
            ViewBag.username = bi.AspNetUser.UserName;
            ViewBag.biztitle = bi.BizTitle;
            ViewBag.orderid = base.CurrentOrder.OrderId;
            ViewBag.orderlist = orderlist;
            Cleanup(cart);
            return View();
        }
        [NonAction]
        protected int MakeExpressCheckoutPayment(ShoppingCart cart)
        {
            int checkoutSuccess = 1;// Payment failed

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            com.paypal.sdk.services.NVPCallerServices caller = PayPalAPI.PayPalAPIInitialize();
            NVPCodec encoder = new NVPCodec();
            encoder["VERSION"] = "63.0";
            encoder["METHOD"] = "DoExpressCheckoutPayment";
            encoder["TOKEN"] = Session["TOKEN"].ToString();
            encoder["PAYERID"] = Session["PAYERID"].ToString();
            encoder["PAYMENTREQUEST_0_AMT"] = Session["AMT"].ToString();
            encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
            encoder["PAYMENTREQUEST_0_ALLOWEDPAYMENTMETHOD"] = "InstantPaymentOnly";
            encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "USD";

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = caller.Call(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);


            string strAck = decoder["ACK"];
            if (strAck != null && (strAck == "Success" || strAck == "SuccessWithWarning"))
            {
                Session["TRANSACTIONID"] = decoder["PAYMENTINFO_0_TRANSACTIONID"];
                checkoutSuccess = 0;
                //string res = Util.BuildResponse(decoder, "Thank you for your payment!", "");
                // lblPaymentResult.Text = res;
                TempData["TransIDandTime"] = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "<br/>TRANSACTION ID: " + decoder["PAYMENTINFO_0_TRANSACTIONID"].ToString();
                Order od = AddOrderToDatabase(cart, Session["TRANSACTIONID"].ToString());
                base.CurrentOrder.OrderId = od.OrderId;
                //payment successed
                if (SendEmail(Session["GetExpressCheckoutDetails"],od, decoder, cart) == false)
                {
                    checkoutSuccess = 2;// SendEmail failed
                }

            }
            else
            {
                Session["errorresult"] = decoder;
                checkoutSuccess = 1;
                //string pStrResQue = "API=" + "Error Detail ";
                // Response.Redirect("APIError.aspx?" + pStrResQue);
            }
            return checkoutSuccess;
        }
        [NonAction]
        public Order AddOrderToDatabase(ShoppingCart cart, string transactionid)
        {
            string st = (string.IsNullOrEmpty(base.CurrentOrder.RoomNumber)) ? base.CurrentOrder.Street : "#" + base.CurrentOrder.RoomNumber + " " + base.CurrentOrder.Street;
            string username = User.Identity.IsAuthenticated? UserName:"$p$";
            DateTime dt = DateTime.Now;
            return OrderRepository.InsertOrder(cart, username, cart.BizId, 2, 0, User.Identity.IsAuthenticated,
                                            cart.IsDelivery, cart.DeliveryFee, cart.SubTotal(), cart.Tax(), cart.Total(), cart.serviceCharge, cart.DriverTip, base.CurrentOrder.FirstName,
                                            base.CurrentOrder.LastName, st, base.CurrentOrder.City,
                                            base.CurrentOrder.State, base.CurrentOrder.ZipCode, base.CurrentOrder.Phone,
                                            base.CurrentOrder.Email, transactionid, dt, username, dt, username, true,
                                            cart.ScheduleDate, cart.ScheduleTime, cart.CouponChoice, true, cart.BizDiscountAmount,
                                            cart.DiscountAmount, cart.BizSubTotal(), cart.BizTotal(), base.CurrentOrder.CustomerMessage, "N/A", CurrentUserIP,false,0,0.0m);
            
       

        }
        protected bool SendEmail(object getDetailsResponse,Order od, object doPaymentResponse, ShoppingCart cart)
        {
            bool sendSuccess = false;
            if (getDetailsResponse != null && doPaymentResponse != null)
            {
                NVPCodec decoderDetails = new NVPCodec();
                decoderDetails = (NVPCodec)getDetailsResponse;
                NVPCodec decoderPayment = new NVPCodec();
                decoderPayment = (NVPCodec)doPaymentResponse;

                EmailManager em = new EmailManager();
                EmailContents ec = new EmailContents();
                BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
                em.EMBizInfo = bi;
                em.EMShoppingCart = cart;
                em.RoomNumber = base.CurrentOrder.RoomNumber;
                em.Address = base.CurrentOrder.Street;
                em.City = base.CurrentOrder.City;
                em.State = base.CurrentOrder.State;
                em.Zip = base.CurrentOrder.ZipCode;
                em.Phone = base.CurrentCreditCard.Phone;
                em.IpAddress = base.CurrentUserIP;
                em.Instruction = base.CurrentOrder.CustomerMessage;
                em.OrderTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
                em.PaymentType = "PayPal";
                em.OrderType = cart.IsDelivery ? "Delivery" : "Pickup";
                em.Name = base.CurrentOrder.FirstName + " " + base.CurrentOrder.LastName;
                em.OrderNumber =(800000000 + od.OrderId).ToString();
                em.Phone = base.CurrentOrder.Phone;
                em.Subtotal = Helper.FormatPriceWithDollar(cart.SubTotal());
                em.Tax = Helper.FormatPriceWithDollar(cart.Tax());
                em.Total = decoderPayment["PAYMENTINFO_0_AMT"].ToString();
                ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
                ec.FromName = "www.foodready.net";
                ec.Subject = "New online order";
                ec.To = Globals.Settings.ContactForm.MailTo; // send to FoodReady.Net
                ec.Body = em.BuildEmailHtmlBodyForOrder();
                em.FaxBody = em.BuildFaxHtmlBodyForOrder();
                SetCompletedInfo(cart, em);
                em.SendFax(bi.Fax);
                em.Send(ec);
                ec.To = base.CurrentOrder.Email; // send to user
                em.Send(ec);
                ec.To = bi.ContactInfo.Email; // send to restaurant
                em.Send(ec);
                if (em.IsSent == false)
                {
                    sendSuccess = false;
                    TempData["EmailSentMsg"] = ec.Body;
                    //TempData["EmailSentMsg"]  = "The email has not been sent out for some reasons.";
                }
                else
                {
                    sendSuccess = true;
                    TempData["EmailSentMsg"] = ec.Body;
                    //TempData["EmailSentMsg"] = "";
                }
                sendSuccess = true;//test for now    em.IsSent;
                Cleanup(cart);
            }
            return sendSuccess;
        }
        [NonAction]
        public void SetCompletedInfo(ShoppingCart cart, EmailManager vem)
        {
            TempData["EmailSentStatus"] = vem.BuildEmailHtmlBodyForOrder(); // test
            TempData["OrderTime"] = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            TempData["OrderNumber"] = vem.OrderNumber;

        }
        protected void Cleanup(ShoppingCart cart)
        {
            if (cart.IsFinalSharedCart)
            {
                if (HttpContext.Application[cart.CartKey] != null)
                {
                    HttpContext.Application[cart.CartKey] = null;
                }
                if (base.CheckoutSharedCart != null)
                {
                    base.CheckoutSharedCart = null;
                }
                if (base.CombinedCart != null)
                {
                    base.CombinedCart.Clear();
                }
                if (HttpContext.Application["Share" + cart.CartKey] != null)
                {
                    HttpContext.Application["Share" + cart.CartKey] = null;
                }
                MyInfo = null;
            }
            cart.Clear();
        }
        [HttpPost]
        public ActionResult ValidateGiftCard(string txtGC, ShoppingCart cart, BrowseHistory bh)
        {
            HandleCart(cart);
            CheckoutGiftModel cgm = new CheckoutGiftModel();
            cart.ScheduleDate = "Today";
            cart.ScheduleTime = "ASAP";
            cgm.GiftCardStatus = 0; // 0= No such gift card; 1=Expired;2=No money;3=Cover;4=Not cover;
            if (!CanCheckCode(bh))
            {
                cgm.GiftCardStatus = 5;
            }
            GiftCard gcd = GiftCardRepository.GetGiftCardByCode(txtGC,true);
            if (gcd != null)
            {
                bh.TryGiftCodeTimes = 0;
                cgm.GiftCardStatus = gcd.Expired ? 1 : gcd.Balance <= 0.0m ? 2 : gcd.Cover(cart.Total()) ? 3 : 4;
                cgm.GCGiftcardID = gcd.GiftCardId;
                cgm.GCBalance = gcd.Balance;
                cgm.GCUserID = gcd.UserId;
                cgm.GCZipCode = bh.Zip;
                cgm.GCState = string.IsNullOrEmpty(bh.State) ? "CA" : bh.State;

            }
            if (User.Identity.IsAuthenticated)
            {
                UserDetail ud = UserDetailRepository.GetUserDetailByUserId(CurrentUserID);
                if (ud != null)
                {
                    cgm.GCFirstName = ud.FirstName;
                    cgm.GCLastName = ud.LastName;
                    cgm.GCAddressLine = ud.Address.AddressLine;
                    cgm.GCCity = ud.Address.City;
                    cgm.GCState = ud.Address.State;
                    cgm.GCZipCode = ud.Address.ZipCode;
                    cgm.GCPhone = ud.ContactInfo.Phone;
                    cgm.GCEmail = ud.ContactInfo.Email;
                }
            }
            cgm.GCCart = cart;
            cgm.GCBizInfo = BizInfoRepository.GetBizInfoById(cart.BizId);
            ViewBag.servicephone = ServicePhone;
            return PartialView(cgm);
        }
        [NonAction]
        public bool CanCheckCode(BrowseHistory bh)
        {
            bh.TryGiftCodeTimes ++;
            if (bh.LastTryGiftCodeTime.AddMinutes(10) < DateTime.Now) // over 10 minites
            {
                bh.LastTryGiftCodeTime = DateTime.Now;
                bh.TryGiftCodeTimes = 0;
                return true;
            }
            else if (bh.TryGiftCodeTimes > 4)
            {
                bh.LastTryGiftCodeTime = DateTime.Now;
                return false;
            }
            else
            {
                bh.LastTryGiftCodeTime = DateTime.Now;
                return true;
            }
        }
          
        [HttpPost]
        public ActionResult GiftPayConfirmation(CheckoutGiftModel cgm, ShoppingCart cart)
        {
            HandleCart(cart);

            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            ViewBag.ScheduleOK = (SearchFilter.IsOpenAt(bi, cart.ScheduleDate=="Tomorrow"?2:1, cart.ScheduleTime))?"OK":"NO";
            cgm.GCCart = cart;
            cgm.GCBizInfo = BizInfoRepository.GetBizInfoById(cart.BizId);
            base.CurrentCreditCard = new CreditCard();
            base.CurrentCreditCard.CreditCardId = 0; // base.CurrentCreditCard.CreditCardID=any
            base.CurrentCreditCard.UserName = string.IsNullOrEmpty(UserName)?cgm.GCBillFirstName + " " +  cgm.GCBillLastName:UserName;
            base.CurrentCreditCard.CreditCardTypeId = int.Parse(cgm.GCCardType);
            base.CurrentCreditCard.FirstName = cgm.GCBillFirstName;
            base.CurrentCreditCard.LastName = cgm.GCBillLastName;
            /*
            base.CurrentCreditCard.AddressLine = cgm.GCBillAddressLine;
            base.CurrentCreditCard.AddressLine2 = "";
            base.CurrentCreditCard.City = cgm.GCBillCity;

            base.CurrentCreditCard.State = cgm.GCBillState;
            base.CurrentCreditCard.ZipCode = cgm.GCBillZipCode;
            base.CurrentCreditCard.Country = "US";
            */
            base.CurrentCreditCard.Phone = cgm.GCPhone;
            base.CurrentCreditCard.Email = cgm.GCEmail;

            base.CurrentCreditCard.CreditCardNumber = cgm.GCCardNumber;
            base.CurrentCreditCard.ExpirationMonth = int.Parse(cgm.GCExpirationMonth);
            base.CurrentCreditCard.ExpirationYear = int.Parse(cgm.GCExpirationYear);
            base.CurrentCreditCard.SecurityCode = int.Parse(cgm.GCSecurityCode);
            base.CurrentCreditCard.AddedDate = DateTime.Now;
            base.CurrentCreditCard.AddedBy = string.IsNullOrEmpty(UserName) ? cgm.GCBillFirstName + " " + cgm.GCBillLastName : UserName; ;
            base.CurrentCreditCard.UpdatedDate = DateTime.Now;
            base.CurrentCreditCard.UpdatedBy = string.IsNullOrEmpty(UserName) ? cgm.GCBillFirstName + " " + cgm.GCBillLastName : UserName; ;
            base.CurrentCreditCard.Active = true;


            base.CurrentOrder = new Order();
            base.CurrentOrder.FirstName = cgm.GCFirstName;
            base.CurrentOrder.LastName = cgm.GCLastName;
            base.CurrentOrder.Street = cgm.GCAddressLine;
            base.CurrentOrder.City = cgm.GCCity;
            base.CurrentOrder.State = cgm.GCState;
            base.CurrentOrder.ZipCode = cgm.GCZipCode;

            string approxtime = string.Empty;
            string mysAddress = cgm.GCAddressLine + ", " + cgm.GCCity + ", " + cgm.GCState + " " + cgm.GCZipCode;
            decimal st = SearchFilter.GetDistance(mysAddress, bi.BizAddressString, out approxtime);
            string xx = approxtime;
            ViewBag.ValidateAddress = string.Empty;
            if (st < 0.0m)
            {
                ViewBag.ValidateAddress = "Oops, feel like something wrong with you address. Please check it and try again.";
            }
            else if (st > bi.DeliveryRadius && bi.Delivery)
            {
                ViewBag.ValidateAddress = "Sorry,your address is beyond the area that this restaurant deliveries. Please try other restaurants.";
            } 

            base.CurrentOrder.LogonName = UserName;
            base.CurrentOrder.IsLoggedUser = User.Identity.IsAuthenticated;
            base.CurrentOrder.CustomerMessage = cgm.GCInstructions;
            base.CurrentOrder.IsDelivery = cart.IsDelivery;
            base.CurrentOrder.ServiceCharge = cart.serviceCharge;
            base.CurrentOrder.DeliveryCharge = cart.DeliveryFee;
            base.CurrentOrder.DriverTip = cart.DriverTip;
            base.CurrentOrder.IpAddress = base.CurrentUserIP;
            base.CurrentOrder.Phone = cgm.GCPhone;
            base.CurrentOrder.Email = cgm.GCEmail;
            base.CurrentOrder.ScheduleDate = cgm.GCScheduleDate;
            base.CurrentOrder.ScheduleTime = cgm.GCScheduleTime;
            base.CurrentOrder.CouponChoice = cart.CouponChoice;
            base.CurrentOrder.OrderTax = cart.Tax();
            base.CurrentOrder.OrderTax = cart.Tax();
            base.CurrentOrder.SubTotal = cart.SubTotal();
            base.CurrentOrder.OrderTotal = cart.Total();
            base.CurrentOrder.BizInfoId = cart.BizId;

            ViewBag.servicephone = ServicePhone;
            ViewBag.maplink = GoogleMapLink(bi);
            return PartialView(cgm);
        }
        [HttpPost]
        public ActionResult ProcessGiftCheckout(int gcid, decimal balance,int userid, ShoppingCart cart)
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            string restaurantUserName = bi.AspNetUser.UserName; // GetUserNameByUserId(bi.UserId)
            string orderlist = string.Empty;
            string bizTitle = bi.BizTitle;
            string success = "failed";
            string addDatabase = "failed";
            if (DoDirectPaymentSuccess(cart.Total()-balance))
            {
                success = UpdateGiftCard(gcid, balance, cart)?"success":success;
                if (DoGiftCheckoutCompleted(gcid,balance,userid, cart))
                {
                    orderlist = BuildOrderList(cart);
                    addDatabase = "success";
                }
            }
            return Json(new
            {
                result = success,
                databaseresult = addDatabase,
                dataResult = orderlist,
                username= restaurantUserName,
                biztitle = bizTitle,
                orderId=base.CurrentOrder.OrderId,
                test = TempData["EmailSentMsg"] //test only
            });
        }
        [NonAction]
        public bool UpdateGiftCard(int vGiftCardId, decimal vBalance, ShoppingCart cart)
        {
            string username = (base.CurrentCreditCard.CreditCardId) == 0 ? "$g$" : UserName;
            GiftCard gc = GiftCardRepository.GetGiftCardById(vGiftCardId);
            decimal b = gc.Balance > cart.Total() ? gc.Balance - cart.Total() : 0.0m;
            decimal lp = gc.Balance > cart.Total() ? cart.Total() : gc.Balance;
            gc = GiftCardRepository.AddGiftCard(gc.GiftCardId, gc.GiftCardCode, gc.UserId,gc.CreditCardId, cart.BizId, gc.UserEmail,
                                  gc.RecipientEmail, gc.Amount, DateTime.Now, b, lp, cart.Total(), gc.AddedDate,
                                  gc.AddedBy, DateTime.Now, username, true);
            return gc == null ? false : true;
        }
        [NonAction]
        public bool DoGiftCheckoutCompleted(int vGCid, decimal vGCbalance, int vUserid, ShoppingCart cart)
        {
            bool completed = false;
            NVPCodec decoderResults = new NVPCodec();
            decoderResults = (NVPCodec)Session["result"];
            TempData["OrderSummaryTitle"] = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // string res = Util.BuildResponse(Session["result"], Request.QueryString.Get("API"), "Thank you for your payment!");

            if (User.Identity.IsAuthenticated)
            {
                base.CurrentCreditCard.CreditCardId = CreditCardRepository.GetCreditCardId(User.Identity.Name, base.CurrentCreditCard.CreditCardTypeId, base.CurrentCreditCard.CreditCardNumber, base.CurrentCreditCard.ExpirationMonth, base.CurrentCreditCard.ExpirationYear, base.CurrentCreditCard.SecurityCode);
                if (base.CurrentCreditCard.CreditCardId == 0)
                {
                    CreditCard cc=AddCreditCardToDB();
                    base.CurrentCreditCard.CreditCardId=cc==null?0:cc.CreditCardId;
                }
            }
            else
            {
                base.CurrentCreditCard.CreditCardId = 0;//guest
            }
            Order od = AddOrderToDB(cart, decoderResults["TRANSACTIONID"].ToString(), true, vGCid, vGCbalance);
            base.CurrentOrder.OrderId = od.OrderId;
            EmailManager em = new EmailManager();
            EmailContents ec = new EmailContents();
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            em.EMBizInfo = bi;
            em.EMShoppingCart = cart;

            em.OrderTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
            TempData["DeliveryAddress"] = "";
            em.OrderType = cart.IsDelivery ? "Delivery" : "Pickup";
            em.PaymentType = "Gift Card, Credit Card";
            em.ScheduleDateTime = base.CurrentOrder.ScheduleTime + ", " + base.CurrentOrder.ScheduleDate;
            em.OrderNumber = (800000000 + od.OrderId).ToString();
            em.Name = base.CurrentOrder.FirstName + " " + base.CurrentOrder.LastName;

            em.Address = base.CurrentOrder.Street;
            em.City = base.CurrentOrder.City;
            em.State = base.CurrentOrder.State;
            em.Zip = base.CurrentOrder.ZipCode;
            em.Phone = base.CurrentCreditCard.Phone;
            em.IpAddress = base.CurrentUserIP;
            em.CreditCardType = CreditCardTypeRepository.GetCreditCardTypeById(base.CurrentCreditCard.CreditCardTypeId).Title;
            em.CreditCardNumber = "xxxx-xxxx-xxxx-" + base.CurrentCreditCard.CreditCardNumber.Substring(base.CurrentCreditCard.CreditCardNumber.Length - 4);
            em.ExpirationDate = base.CurrentCreditCard.ExpirationMonth.ToString() + "/" + base.CurrentCreditCard.ExpirationYear.ToString();
            em.SecurityCode = "xx" + base.CurrentCreditCard.SecurityCode.ToString().Substring(base.CurrentCreditCard.SecurityCode.ToString().Length - 1); ;

            em.Subtotal = Helper.FormatPriceWithDollar(cart.SubTotal()); ;
            em.Tax = ToUSD(cart.Tax().ToString("N2"));
            em.CouponChoice = base.CurrentOrder.CouponChoice;
            em.ServiceCharge = ToUSD(base.CurrentOrder.ServiceCharge.ToString("N2"));
            em.DeliveryCharge = ToUSD(base.CurrentOrder.DeliveryCharge.ToString("N2"));
            em.Tip = ToUSD(base.CurrentOrder.DriverTip.ToString("N2"));
            em.DriveName = base.CurrentOrder.DriverName;
            em.CouponChoice = base.CurrentOrder.CouponChoice;
            em.Instruction = base.CurrentOrder.CustomerMessage;
            em.Total = ToUSD(cart.Total().ToString("N2")); //Helper.FormatPriceWithDollar(decoderResults["AMT"].ToString());
            ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
            ec.FromName = "foodready.net";
            ec.Subject = "New online order";
            ec.To = Globals.Settings.ContactForm.MailTo; // send to FoodReady.Net
            ec.Body = em.BuildEmailHtmlBodyForOrder();
            em.FaxBody = em.BuildFaxHtmlBodyForOrder();
            em.SendFax(bi.Fax);
            em.Send(ec);
            ec.To = base.CurrentOrder.Email; // send to user
            em.Send(ec);
            ec.To = bi.ContactInfo.Email; // send to restaurant
            em.Send(ec);

            if (em.IsSent == false)
            {
                completed = false;
                TempData["EmailSentMsg"] = ec.Body;
                // TempData["paymentResult"] = "SendEmail failed";
            }
            else
            {
                completed = true;
                TempData["EmailSentMsg"] = ec.Body;
                //TempData["paymentResult"] = string.Empty;
            }
            Cleanup(cart);
            return completed;
        }
        [HttpPost]
        public ActionResult GiftOnlyConfirmation(CheckoutGiftModel cgm, ShoppingCart cart)
        {
            TempData["valid"] = "";
            HandleCart(cart);
            bool statevalid = string.IsNullOrEmpty(cgm.GCFirstName) == false && string.IsNullOrEmpty(cgm.GCLastName) == false
                 && string.IsNullOrEmpty(cgm.GCAddressLine) == false && string.IsNullOrEmpty(cgm.GCCity) == false && string.IsNullOrEmpty(cgm.GCState) == false
                 && string.IsNullOrEmpty(cgm.GCZipCode) == false && string.IsNullOrEmpty(cgm.GCPhone) == false && string.IsNullOrEmpty(cgm.GCEmail) == false
                 && cgm.GCGiftcardID > 0 && cgm.GCBalance > 0 && string.IsNullOrEmpty(cgm.GCUserID) == false;
            if (statevalid)
            {
                TempData["valid"] = "";
                BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
                ViewBag.ScheduleOK = (SearchFilter.IsOpenAt(bi, cart.ScheduleDate == "Tomorrow" ? 2 : 1, cart.ScheduleTime)) ? "OK" : "NO";
                cgm.GCCart = cart;
                cgm.GCBizInfo = BizInfoRepository.GetBizInfoById(cart.BizId);
                base.CurrentCreditCard = new CreditCard();
                base.CurrentCreditCard.CreditCardId = 0; // base.CurrentCreditCard.CreditCardID=any


                base.CurrentOrder = new Order();
                base.CurrentOrder.FirstName = cgm.GCFirstName;
                base.CurrentOrder.LastName = cgm.GCLastName;
                base.CurrentOrder.Street = cgm.GCAddressLine;
                base.CurrentOrder.City = cgm.GCCity;
                base.CurrentOrder.State = cgm.GCState;
                base.CurrentOrder.ZipCode = cgm.GCZipCode;

                string approxtime = string.Empty;
                string mysAddress = cgm.GCAddressLine + ", " + cgm.GCCity + ", " + cgm.GCState + " " + cgm.GCZipCode;
                decimal st = SearchFilter.GetDistance(mysAddress, bi.BizAddressString, out approxtime);
                string xx = approxtime;
                ViewBag.ValidateAddress = string.Empty;
                if (st < 0.0m)
                {
                    ViewBag.ValidateAddress = "Oops, feel like something wrong with you address. Please check it and try again.";
                }
                else if (st > bi.DeliveryRadius && bi.Delivery)
                {
                    ViewBag.ValidateAddress = "Sorry, your address is beyond the area that this restaurant deliveries. Please try other restaurants.";
                }

                base.CurrentOrder.LogonName = UserName;
                base.CurrentOrder.IsLoggedUser = User.Identity.IsAuthenticated;
                base.CurrentOrder.CustomerMessage = cgm.GCInstructions;
                base.CurrentOrder.IsDelivery = cart.IsDelivery;
                base.CurrentOrder.ServiceCharge = cart.serviceCharge;
                base.CurrentOrder.DeliveryCharge = cart.DeliveryFee;
                base.CurrentOrder.DriverTip = cart.DriverTip;
                base.CurrentOrder.IpAddress = base.CurrentUserIP;
                base.CurrentOrder.Phone = cgm.GCPhone;
                base.CurrentOrder.Email = cgm.GCEmail;
                base.CurrentOrder.ScheduleDate = cgm.GCScheduleDate;
                base.CurrentOrder.ScheduleTime = cgm.GCScheduleTime;
                base.CurrentOrder.CouponChoice = cart.CouponChoice;
                base.CurrentOrder.OrderTax = cart.Tax();
                base.CurrentOrder.OrderTax = cart.Tax();
                base.CurrentOrder.SubTotal = cart.SubTotal();
                base.CurrentOrder.OrderTotal = cart.Total();
                base.CurrentOrder.BizInfoId = cart.BizId;

                ViewBag.servicephone = ServicePhone;
                ViewBag.maplink = GoogleMapLink(bi);
                return PartialView(cgm);
            }
            TempData["valid"] = "Please check input and try again.";
            return RedirectToAction("Index", "Checkout");
        }
        [HttpPost]
        public ActionResult ProcessGiftOnlyCheckout(int gcid, decimal balance, int userid, ShoppingCart cart)
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            string restaurantUserName = bi.AspNetUser.UserName; // GetUserNameByUserId(bi.UserId)
            string orderlist = string.Empty;
            string bizTitle = bi.BizTitle;
            string success = "failed";
            string addDatabase = "failed";
            success = UpdateGiftCard(gcid, balance, cart) ? "success" : success;
            if (DoGiftOnlyCheckoutCompleted(gcid, balance, userid, cart))
            {
                orderlist = BuildOrderList(cart);
                addDatabase = "success";
            }
            return Json(new
            {
                result = success,
                databaseresult = addDatabase,
                dataResult = orderlist,
                username = restaurantUserName,
                biztitle = bizTitle,
                orderId = base.CurrentOrder.OrderId,
                test = TempData["EmailSentMsg"] //test only
            });
        }
        [NonAction]
        public bool DoGiftOnlyCheckoutCompleted(int vGCid, decimal vGCbalance, int vUserid, ShoppingCart cart)
        {
            bool completed = false;
            TempData["OrderSummaryTitle"] = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // string res = Util.BuildResponse(Session["result"], Request.QueryString.Get("API"), "Thank you for your payment!");
            base.CurrentCreditCard.CreditCardId = 0;//guest
            decimal paymount = vGCbalance > cart.Total() ? cart.Total() : vGCbalance;
            Order od = AddOrderToDB(cart, "", true, vGCid, paymount);
            base.CurrentOrder.OrderId = od.OrderId;
            EmailManager em = new EmailManager();
            EmailContents ec = new EmailContents();
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            em.EMBizInfo =bi;
            em.EMShoppingCart = cart;

            em.OrderTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
            TempData["DeliveryAddress"] = "";
            em.OrderType = cart.IsDelivery ? "Delivery" : "Pickup";
            em.PaymentType = "Gift Card";
            em.ScheduleDateTime = base.CurrentOrder.ScheduleTime + ", " + base.CurrentOrder.ScheduleDate;
            em.OrderNumber = (800000000 + od.OrderId).ToString();
            em.Name = base.CurrentOrder.FirstName + " " + base.CurrentOrder.LastName;

            em.Address = base.CurrentOrder.Street;
            em.City = base.CurrentOrder.City;
            em.State = base.CurrentOrder.State;
            em.Zip = base.CurrentOrder.ZipCode;
            em.Phone = base.CurrentOrder.Phone;
            em.IpAddress = base.CurrentUserIP;
          
            em.Subtotal = Helper.FormatPriceWithDollar(cart.SubTotal()); ;
            em.Tax = ToUSD(cart.Tax().ToString("N2"));
            em.CouponChoice = base.CurrentOrder.CouponChoice;
            em.ServiceCharge = ToUSD(base.CurrentOrder.ServiceCharge.ToString("N2"));
            em.DeliveryCharge = ToUSD(base.CurrentOrder.DeliveryCharge.ToString("N2"));
            em.Tip = ToUSD(base.CurrentOrder.DriverTip.ToString("N2"));
            em.DriveName = base.CurrentOrder.DriverName;
            em.CouponChoice = base.CurrentOrder.CouponChoice;
            em.Instruction = base.CurrentOrder.CustomerMessage;
            em.Total = ToUSD(cart.Total().ToString("N2")); //Helper.FormatPriceWithDollar(decoderResults["AMT"].ToString());
            ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
            ec.FromName = "foodready.net";
            ec.Subject = "New online order";
            ec.To = Globals.Settings.ContactForm.MailTo; // send to FoodReady.Net
            ec.Body = em.BuildEmailHtmlBodyForOrder();
            em.FaxBody = em.BuildFaxHtmlBodyForOrder();
            em.SendFax(bi.Fax);
            em.Send(ec);
            ec.To = base.CurrentOrder.Email; // send to user
            em.Send(ec);
            ec.To = bi.ContactInfo.Email; // send to restaurant
            em.Send(ec);
            if (em.IsSent == false)
            {
                completed = false;
                TempData["EmailSentMsg"] = ec.Body;
                // TempData["paymentResult"] = "SendEmail failed";
            }
            else
            {
                completed = true;
                TempData["EmailSentMsg"] = ec.Body;
                //TempData["paymentResult"] = string.Empty;
            }
            Cleanup(cart);
            return completed;
        }
        public ActionResult GiftCard(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }
    }
}
