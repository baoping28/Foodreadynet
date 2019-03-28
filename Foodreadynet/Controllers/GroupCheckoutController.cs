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
using System.Collections.Generic;

namespace FoodReady.WebUI.Controllers
{
    public class GroupCheckoutController : BaseController
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
        public GroupCheckoutController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo,
                                     ICuisineTypeRepository cuisineTypeRepo, ICategoryRepository categoryRepo,
                                     IProductRepository productRepo, IProductToppingRepository productToppingRepo,
                                     IProductDressingRepository productDressingRepo,IDiscountCouponRepository discountCouponRepo,
                                     IFreeItemCouponRepository FreeItemCouponRepo, IUserDetailRepository userDetailRepo,
                                     ICreditCardTypeRepository creditCardTypeRepo, IOrderRepository orderRepo, 
                                     ICreditCardRepository creditCardRepo)
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
            TempData["valid"] = "";
        }
        public ActionResult Index(ShoppingCart cart, BrowseHistory bh)
        {
            HandleCart(cart);
            GroupCheckoutModel gcm = new GroupCheckoutModel();
            SharedShoppingCart ssc = new SharedShoppingCart();
            if (string.IsNullOrEmpty(cart.CartKey) == false)
            {
                ssc = GetGroupShoppingCart(cart.CartKey);
            }
            bool isBoss = (string.IsNullOrEmpty(cart.PersonName) == false && cart.PersonName == GroupCart.GroupCarts[cart.CartKey].PartyBossName);
            if (!isBoss || GroupCart.GroupCarts[cart.CartKey].PartyTotalItems == 0 || (GroupCart.GroupCarts[cart.CartKey].PartyOrderMinimum() > GroupCart.GroupCarts[cart.CartKey].PartySubTotal() && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery) || (GroupCart.GroupCarts[cart.CartKey].PartyCart[cart.BossName].IsBizDelivery == false && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery))
            {
                return Redirect("~/Group"); ;
            }
            gcm.AddressLine = bh.Address;
            gcm.City = bh.City;
            gcm.State = string.IsNullOrEmpty(bh.State) ? "CA" : bh.State;
            gcm.ZipCode = bh.Zip;

            if (User.Identity.IsAuthenticated)
            {
                UserDetail ud = UserDetailRepository.GetUserDetailByUserId(CurrentUserID);
                if (ud != null)
                {
                    gcm.FirstName = ud.FirstName;
                    gcm.LastName = ud.LastName;
                    gcm.AddressLine =string.IsNullOrEmpty(bh.Address)? ud.Address.AddressLine:bh.Address;
                    gcm.City = string.IsNullOrEmpty(bh.City) ? ud.Address.City : bh.City;
                    gcm.State = string.IsNullOrEmpty(bh.State) ? ud.Address.State : bh.State;
                    gcm.ZipCode = string.IsNullOrEmpty(bh.Zip) ? ud.Address.ZipCode : bh.Zip;
                    gcm.Phone = ud.ContactInfo.Phone;
                    gcm.Email = ud.ContactInfo.Email;
                }
            }
            ssc.PartyScheduleDate = GroupCart.GroupCarts[cart.CartKey].PartyScheduleDate;
            ssc.PartyScheduleTime = GroupCart.GroupCarts[cart.CartKey].PartyScheduleTime;
            gcm.GroupCheckoutCart =ssc;
            return View(gcm);
        }
        [HttpPost]
        public ActionResult ProcessGroupSchedule(string schedule, string time, ShoppingCart cart)
        {
            HandleCart(cart);
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            bool isopen = SearchFilter.IsOpenAt(bi, int.Parse(schedule), time);
            cart.ScheduleDate = schedule == "2" ? "Tomorrow" : "Today";
            cart.ScheduleTime = time;
            GroupCart.GroupCarts[cart.CartKey].PartyScheduleDate = cart.ScheduleDate;
            GroupCart.GroupCarts[cart.CartKey].PartyScheduleTime = cart.ScheduleTime;
            return Json(new
            {
                schDate = cart.ScheduleDate,
                schTime = cart.ScheduleTime,
                result = isopen ? "success" : "failed"
            });
        }
        [HttpPost]
        public ActionResult GroupCheckoutConfirmation(GroupCheckoutModel gcm, ShoppingCart cart)
        {
            HandleCart(cart);
            TempData["valid"] = "";
            ViewBag.ValidateAddress = string.Empty;
            ViewBag.ScheduleOK = "OK";
            ViewBag.Restaurant = string.Empty;
            ViewBag.LabelName = string.Empty;
            gcm.GroupCheckoutCart = GroupCart.GroupCarts[cart.CartKey];

            List<BizInfo> lb = new List<BizInfo>();
            if (ModelState.IsValid)
            {
                string mysAddress = gcm.AddressLine + ", " + gcm.City + ", " + gcm.State + " " + gcm.ZipCode;
                Dictionary<int, string> l = new Dictionary<int, string>();
                foreach (var g in GroupCart.GroupCarts[cart.CartKey].PartyCart.Keys)
                {
                    int i = GroupCart.GroupCarts[cart.CartKey].PartyCart[g].BizId;
                    if (l.ContainsKey(i) == false)
                    {
                        l.Add(i, g);
                        BizInfo biz = BizInfoRepository.GetBizInfoById(i);
                        lb.Add(biz);
                    }
                }
                foreach (var b in l.Keys)
                {
                    BizInfo bi = BizInfoRepository.GetBizInfoById(b);
                    if (SearchFilter.IsOpenAt(bi, cart.ScheduleDate == "Tomorrow" ? 2 : 1, cart.ScheduleTime) == false)
                    {
                        ViewBag.ScheduleOK = "NO";
                        ViewBag.Restaurant = bi.BizTitle;
                        ViewBag.LabelName = l[b];
                        break;
                    }
                    string approxtime = string.Empty;
                    if (string.IsNullOrEmpty(gcm.AddressLine) || string.IsNullOrEmpty(gcm.City) || string.IsNullOrEmpty(gcm.State) || string.IsNullOrEmpty(gcm.ZipCode))
                    {
                        ViewBag.ValidateAddress = "Oops, feel like something wrong with you address. Please check it and try again.";
                        break;
                    }
                    else
                    {
                        decimal st = SearchFilter.GetDistance(mysAddress, bi.BizAddressString, out approxtime);
                        string nouse = approxtime;
                        if (st < 0.0m)
                        {
                            ViewBag.ValidateAddress = "Oops, feel like something wrong with you address. Please check it and try again.";
                            break;
                        }
                        else if (st > bi.DeliveryRadius && bi.Delivery)
                        {
                            ViewBag.ValidateAddress = "Sorry, your address is beyond the area the restaurant deliveries. Please try other restaurants. ( Order person: " + l[b] + " | Order restaurant: " + bi.BizTitle + " )";
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(ViewBag.ValidateAddress) && ViewBag.ScheduleOK == "OK")
                {
                    base.CurrentCreditCard = new CreditCard();
                    base.CurrentCreditCard.CreditCardId = 0; // base.CurrentCreditCard.CreditCardID=any
                    base.CurrentCreditCard.UserName = string.IsNullOrEmpty(UserName) ? gcm.BillFirstName + " " + gcm.BillLastName : UserName;
                    base.CurrentCreditCard.CreditCardTypeId = int.Parse(gcm.CardType);
                    base.CurrentCreditCard.FirstName = gcm.BillFirstName;
                    base.CurrentCreditCard.LastName = gcm.BillLastName;
                    /*
                    base.CurrentCreditCard.AddressLine = cm.BillAddressLine;
                    base.CurrentCreditCard.AddressLine2 = "";
                    base.CurrentCreditCard.City = cm.BillCity;

                    base.CurrentCreditCard.State = cm.BillState;
                    base.CurrentCreditCard.ZipCode = cm.BillZipCode;
                    base.CurrentCreditCard.Country = "US";
                    */
                    base.CurrentCreditCard.Phone = gcm.Phone;
                    base.CurrentCreditCard.Email = gcm.Email;

                    base.CurrentCreditCard.CreditCardNumber = gcm.CardNumber;
                    base.CurrentCreditCard.ExpirationMonth = int.Parse(gcm.ExpirationMonth);
                    base.CurrentCreditCard.ExpirationYear = int.Parse(gcm.ExpirationYear);
                    base.CurrentCreditCard.SecurityCode = int.Parse(gcm.SecurityCode);
                    base.CurrentCreditCard.AddedDate = DateTime.Now;
                    base.CurrentCreditCard.AddedBy = string.IsNullOrEmpty(UserName) ? gcm.BillFirstName + " " + gcm.BillLastName : UserName; ;
                    base.CurrentCreditCard.UpdatedDate = DateTime.Now;
                    base.CurrentCreditCard.UpdatedBy = string.IsNullOrEmpty(UserName) ? gcm.BillFirstName + " " + gcm.BillLastName : UserName; ;
                    base.CurrentCreditCard.Active = true;

                    base.CurrentOrder = new Order();
                    base.CurrentOrder.FirstName = gcm.FirstName;
                    base.CurrentOrder.LastName = gcm.LastName;
                    base.CurrentOrder.RoomNumber = gcm.RoomNumber;
                    base.CurrentOrder.Street = gcm.AddressLine;
                    base.CurrentOrder.City = gcm.City;
                    base.CurrentOrder.State = gcm.State;
                    base.CurrentOrder.ZipCode = gcm.ZipCode;

                    base.CurrentOrder.LogonName = UserName;
                    base.CurrentOrder.IsLoggedUser = User.Identity.IsAuthenticated;
                    base.CurrentOrder.CustomerMessage = gcm.Instructions;
                    base.CurrentOrder.IsDelivery = gcm.GroupCheckoutCart.IsPartyDelivery;
                    base.CurrentOrder.ServiceCharge = gcm.GroupCheckoutCart.PartyServiceCharge;
                    base.CurrentOrder.DeliveryCharge = gcm.GroupCheckoutCart.PartyDeliveryFee();
                    base.CurrentOrder.DriverTip = gcm.GroupCheckoutCart.PartyDriverTip();
                    base.CurrentOrder.IpAddress = base.CurrentUserIP;
                    base.CurrentOrder.Phone = gcm.Phone;
                    base.CurrentOrder.Email = gcm.Email;
                    base.CurrentOrder.ScheduleDate = gcm.GroupCheckoutCart.PartyScheduleDate;
                    base.CurrentOrder.ScheduleTime = gcm.GroupCheckoutCart.PartyScheduleTime;
                    base.CurrentOrder.CouponChoice = gcm.GroupCheckoutCart.PartyCouponChoice;
                    base.CurrentOrder.OrderTax = gcm.GroupCheckoutCart.PartyTax();
                    base.CurrentOrder.SubTotal = gcm.GroupCheckoutCart.PartySubTotal();
                    base.CurrentOrder.OrderTotal = gcm.GroupCheckoutCart.PartyTotal();
                    base.CurrentOrder.BizInfoId = cart.BizId;
                }
            }
            else
            {
                ViewBag.ValidateAddress = "Oops, feel like something wrong with you address. Please check it and try again.";
                TempData["valid"] = "check input and try again.";
            }
            gcm.LBizForGoogleMap = lb;
            ViewBag.servicephone = ServicePhone;
            //ViewBag.maplink = GoogleMapLink(cm.BizInfo);
            return PartialView(gcm);
        }
        [HttpPost]
        public ActionResult ProcessGroupCheckout(ShoppingCart cart)
        {
            string success = "failed";
            string addDatabase = "failed";
            string orderlist = string.Empty;
            List<RealTimeOrder> lr = new List<RealTimeOrder>();
            if (DoGroupPaymentSuccess(GroupCart.GroupCarts[cart.CartKey].PartyTotal()))
            {
                success = "success";

                NVPCodec decoderResults = new NVPCodec();
                decoderResults = (NVPCodec)Session["result"];
                TempData["OrderSummaryTitle"] = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
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

                Dictionary<int, ShoppingCart> dCarts = GetCheckoutGroupCart(GroupCart.GroupCarts[cart.CartKey], cart);
                List<BizInfo> lbs = new List<BizInfo>();
                foreach (var i in dCarts.Keys)
                {
                    BizInfo bi = BizInfoRepository.GetBizInfoById(i);
                    lbs.Add(bi);
                    string restaurantUserName = bi.AspNetUser.UserName; 
                    Order od = AddGroupOrderToDB(dCarts[i], decoderResults["TRANSACTIONID"].ToString());
                    if (od != null)
                    {
                        base.CurrentOrder.OrderId = od.OrderId;
                        base.CurrentOrder.BizInfoId = i;
                    }
                    orderlist = BuildGroupOrderList(dCarts[i]);
                   SendGroupEmail(dCarts[i], decoderResults["AMT"].ToString());
                   lr.Add(new RealTimeOrder() { UserName = restaurantUserName, BizTitle = bi.BizTitle, OrderId = base.CurrentOrder.OrderId.ToString(), OrderList = orderlist });
                }
                SendEmailToBoss(lbs, decoderResults["AMT"].ToString());
                addDatabase = "success";
                GroupCleanup(cart);
            }

            return Json(new
            {
                result = success,
                databaseresult = addDatabase,
                orderResult = lr.ToArray(),
                test = TempData["EmailSentMsg"] //test only
            });
        }

        [NonAction]
        public bool DoGroupPaymentSuccess(decimal vCreditTotal)
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
        public void SendGroupEmail(ShoppingCart cart, string paypalTotal)
        {
            EmailManager em = new EmailManager();
            EmailContents ec = new EmailContents();
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            em.EMBizInfo = bi;
            em.EMShoppingCart = cart;

            em.OrderTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
            TempData["DeliveryAddress"] = "";
            em.OrderType = cart.IsDelivery ? "Delivery" : "Pickup";
            em.PaymentType = "Credit Card";
            em.ScheduleDateTime = base.CurrentOrder.ScheduleTime + ", " + base.CurrentOrder.ScheduleDate;
            em.OrderNumber = (800000000 + base.CurrentOrder.OrderId).ToString();
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
            em.ServiceCharge = ToUSD(cart.serviceCharge.ToString("N2"));
            em.DeliveryCharge = ToUSD(cart.DeliveryFee.ToString("N2"));
            em.Tip = ToUSD(cart.DriverTip.ToString("N2"));
            em.DriveName = base.CurrentOrder.DriverName;
            em.CouponChoice = base.CurrentOrder.CouponChoice;
            em.Instruction = base.CurrentOrder.CustomerMessage;
            em.Total = Helper.FormatPriceWithDollar(cart.Total()); // ToUSD(cart.Total().ToString("N2"));
            ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
            ec.FromName = "FoodReady.Net";
            ec.Subject = "New online order";
            ec.To = Globals.Settings.ContactForm.MailTo; // send to FoodReady.Net
            ec.Body = em.BuildEmailHtmlBodyForOrder();
            em.FaxBody = em.BuildFaxHtmlBodyForOrder();
            em.SendFax(bi.Fax);
            em.Send(ec);
           // ec.To = base.CurrentOrder.Email; // send to user
            //em.Send(ec);
            ec.To = bi.ContactInfo.Email; // send to restaurant
            em.Send(ec);
        }

        [NonAction]
        public void SendEmailToBoss(List<BizInfo> lcarts, string vTotal)
        {
            EmailManager em = new EmailManager();
            EmailContents ec = new EmailContents();
            em.OrderTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
            em.OrderType = base.CurrentOrder.IsDelivery ? "Delivery" : "Pickup";
            em.Total = vTotal;
            ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
            ec.FromName = "FoodReady.Net";
            ec.Subject = "Your online order";
            ec.To = base.CurrentOrder.Email; // send to boss
            ec.Body = em.BuildEmailHtmlOrderForBoss(lcarts);
            em.Send(ec);
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
        public Order AddGroupOrderToDB(ShoppingCart cart, string transactionid)
        {
            string st = (string.IsNullOrEmpty(base.CurrentOrder.RoomNumber)) ? base.CurrentOrder.Street : "#" + base.CurrentOrder.RoomNumber + " " + base.CurrentOrder.Street;
            string username = User.Identity.IsAuthenticated ? UserName : "$g$";
            DateTime dt = DateTime.Now;
            return OrderRepository.InsertOrder(cart, username, cart.BizId, 2, base.CurrentCreditCard.CreditCardId, User.Identity.IsAuthenticated,
                                            cart.IsDelivery, cart.DeliveryFee, cart.SubTotal(), cart.Tax(), cart.Total(), cart.serviceCharge, cart.DriverTip, base.CurrentOrder.FirstName,
                                            base.CurrentOrder.LastName, st, base.CurrentOrder.City,
                                            base.CurrentOrder.State, base.CurrentOrder.ZipCode, base.CurrentOrder.Phone,
                                            base.CurrentOrder.Email, transactionid, dt, username, dt, username, true,
                                            cart.ScheduleDate, cart.ScheduleTime, cart.CouponChoice, false, cart.BizDiscountAmount,
                                            cart.DiscountAmount, cart.BizSubTotal(), cart.BizTotal(), base.CurrentOrder.CustomerMessage, "N/A", CurrentUserIP, false, 0, 0.0m);


        }

        [NonAction]
        public string BuildGroupOrderList(ShoppingCart cart)
        {
            string pagepath = "~/Content/HTMLPages/OrderList.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+orderNumber+", (800000000 + base.CurrentOrder.OrderId).ToString());
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
        protected void GroupCleanup(ShoppingCart cart)
        {
            if (string.IsNullOrEmpty(cart.CartKey)==false)
            {
                string ck = cart.CartKey;
                GroupCart.GroupCarts[cart.CartKey].Clear();

                SharedShoppingCart ssc = new SharedShoppingCart();
                if (GroupCart.GroupCarts.TryRemove(ck, out ssc))
                {
                }
                MyInfo = null;
            }
        }
    }
}