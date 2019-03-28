using System;
using System.Linq;
using System.Web.Mvc;
using FoodReady.WebUI.Models;
using FR.Domain.Model.Entities;
using FR.Repository.ShoppingRepository;
using FR.Services.ViewServices;
using FR.Repository.Interfaces;
using FR.Infrastructure.Helpers;
using FR.Services.FilterServces;
using Recaptcha;
using com.paypal.sdk.util;
using FR.Infrastructure.PaypalAPI;
using FoodReady.WebUI.EmailServices;
using System.Collections.Generic;
using FoodReady.WebUI.Controllers;
namespace IdentitySample.Controllers
{
    public class HomeController : BaseController
    {
        /*
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = CurrentUserID;

            return View();
        }

        public ActionResult Contact()
        {
            string nm = "";
            ViewBag.Message = "Your contact page.";
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                nm = frenty.AspNetUsers.FirstOrDefault(e => e.Id == "9e1ff96e-09f5-4ed1-a776-501833f05ff6").UserName;

            }
            ViewBag.Message = nm;
            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProcessChat()
        {
            return Json(new
            {
                test1 = "test1",
                test2 = "test2"
            });
        }
        public ActionResult HotelType()
        {
            return PartialView();
        }
         */

        public ActionResult ChatHelp(string chatUsername, string chatEmail, string chatQuestion)
        {
            ViewBag.chatname = chatUsername;
            ViewBag.chatemail = chatEmail;
            ViewBag.chatquestion = chatQuestion;
            ViewBag.chatadmin = CryptionClass.Encrypt("baoping.peng@hotmail.com");

            return PartialView();
        }
        public ActionResult Chat()
        {
            ViewBag.username = CryptionClass.Encrypt("tester1@tester1.com");
            return View();
        }
        private IBizInfoRepository BizInfoRepository;
        private IBizCuisineRepository BizCuisineRepository;
        private IGiftCardRepository GiftCardRepository;
        private IUserRepository UserRepository;
        private ICreditCardTypeRepository CreditCardTypeRepository;
        private ICreditCardRepository CreditCardRepository;
        private IDiscountCouponRepository DiscountCouponRepository;
        private IFreeItemCouponRepository FreeItemCouponRepository;
        private IHotelRepository HotelRepository;
        private IHotelTypeRepository HotelTypeRepository;
        public HomeController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo,
                              IGiftCardRepository giftCardRepo, IUserRepository userRepo,
                              ICreditCardTypeRepository creditCardTypeRepo, ICreditCardRepository creditCardRepo,
                              IDiscountCouponRepository discountCouponRepo, IFreeItemCouponRepository freeItemCouponRepo,
                              IHotelRepository hotelRepo, IHotelTypeRepository hotelTypeRepo)
        {
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
            GiftCardRepository = giftCardRepo;
            UserRepository = userRepo;
            CreditCardTypeRepository = creditCardTypeRepo;
            CreditCardRepository = creditCardRepo;
            DiscountCouponRepository = discountCouponRepo;
            FreeItemCouponRepository = freeItemCouponRepo;
            HotelRepository = hotelRepo;
            HotelTypeRepository = hotelTypeRepo;
        }

        [NonAction]
        public bool IsValidCombinedCart(ShoppingCart cart)
        {
            bool valid = false;
            if (base.CombinedCart != null)
            {
                if (cart.BossName == base.CombinedCart.BossName && string.IsNullOrEmpty(cart.BossName) == false && cart.BizId == base.CombinedCart.BizId &&
                    cart.BizId > 0 && cart.CartKey == base.CombinedCart.CartKey && string.IsNullOrEmpty(cart.CartKey) == false)
                {
                    valid = true;
                }
            }
            return valid;
        }
        public ActionResult Index(BrowseHistory bh, ShoppingCart cart)
        {
            if (bh == null)
            {
                ControllerContext cc = new ControllerContext();
                bh = new BrowseHistory();
                bh.IsDelivery = true;
                cc.HttpContext.Session["BorseHistory"] = bh;
                return Redirect("~/Home");
            }
            ViewBag.bagitems = GetCartItems(cart);
            ViewBag.delivery = bh.IsDelivery;
            return View();
        }
        //[OutputCache(Duration = 1000)]
        public ActionResult MainContent()
        {
            BizInfoModel bim = new BizInfoModel();
            bim.ShowMostPopularCities = AllCitiesView.ShowMostPopularCities_bootatrap(BizInfoRepository.GetBizInfoInMostPopularLocalCities(true));
            bim.ShowMostPopularCuisines = AllCuisinesView.ShowMostPopularCuisines_Bootstarp(BizCuisineRepository.GetBizCuisinesInMostPopularCuisines(true));
            List<BizInfo> lzips = BizInfoRepository.GetAllBizInfos(true);
            bim.ShowZipCodes = AllZopCodesView.ShowZipCodesView_Bootstarp(lzips.Take(12).ToList(), null, null);
            bim.HotelTypes = HotelTypeRepository.GetAllHotelTypes(true).Take(4).ToList();
            return PartialView(bim);
        }

        //[OutputCache(Duration = 1000)]
        public ActionResult Footer()
        {
            FooterModel fm = new FooterModel();
            fm.PopularCities = BizInfoRepository.GetMostPopularCities(true);
            fm.PopularCuisines = BizCuisineRepository.GetMostPopularCuisines(true);
            return PartialView(fm);
        }
        [HttpPost]
        public ActionResult ValidateAddress(string address, string zipcode, BrowseHistory bh)
        {
            string approxtime = string.Empty;
            bh.AddressCityState = address;
            bh.Zip = zipcode;
            decimal st = SearchFilter.GetDistance(address + " " + zipcode, "1291 Parkside Dr.,Walnut Creek, CA 94596", out approxtime);
            string xx = approxtime;
            string result = "OK";
            if (st < 0.0m)
            {
                result = "failed";
            }
            return Json(new { resp = result, xxx = approxtime });
        }


        [HttpPost]
        public ActionResult UpdateHistory(string del, BrowseHistory bh, ShoppingCart cart)
        {
            bh.IsDelivery = del == "true" ? true : false;
            if (cart != null)
            {
                cart.IsDelivery = bh.IsDelivery;
            }
            return Json(new { resp = bh.IsDelivery });

        }
        public ActionResult About(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            ViewBag.Message = "About Us";

            return View();
        }

        public ActionResult ZipCodes(string term) // have to use parameter named 'term'
        {
            string[] zip = BizInfoRepository.GetAllZipCodes(true).ToArray();
            return this.Json(zip.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);

        }
        //[OutputCache(Duration = 1000)]
        public ActionResult Contact(ShoppingCart cart)
        {
            ViewBag.Message = "Contact Page.";
            ContactModel cm = new ContactModel();
            ViewBag.servicephone = ServicePhone;
            ViewBag.bagitems = GetCartItems(cart);

            return View(cm);
        }
        [OutputCache(Duration = 1000)]
        public ActionResult Help(ShoppingCart cart)
        {
            ViewBag.servicephone = ServicePhone;
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }

        [HttpPost]
        [RecaptchaControlMvc.CaptchaValidator]
        public ActionResult Contact(ContactModel model, bool captchaValid, string mobile, string captchaErrorMessage)
        {
            ViewBag.servicephone = ServicePhone;
            TempData["SentIMEALMsg"] = "";
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(mobile) || mobile != "y")
                {
                    if (!captchaValid)
                    {
                        ModelState.AddModelError("recaptcha", captchaErrorMessage);
                        return View(model);
                    }
                }
                bool sendSuccess = false;
                //ViewBag.qqq = EmailManager.BuildCustomerEmailBody(model);
                EmailManager em = new EmailManager();
                EmailContents ec = new EmailContents(model.ContactName, Globals.Settings.ContactForm.MailTo, model.Email,
                                    "Customer Help", EmailManager.BuildCustomerEmailBody(model));

                // em.FaxBody = EmailManager.BuildCustomerEmailBody(model);
                //em.SendFax(WebConfigurationManager.AppSettings["OnlineFaxNumber"].ToString());
                em.Send(ec); // send to foodready.net
                if (em.IsSent == false)
                {
                    sendSuccess = false;
                    TempData["SentIMEALMsg"] = "Your message has not been sent out for some reasons.";
                }
                else
                {
                    sendSuccess = true;
                    TempData["SentIMEALMsg"] = "Your message has  been sent out successfully.";
                }
                ViewBag.SendingSuccess = sendSuccess;
                return View(model);

            }
            return View(model);
        }

        [NonAction]
        public bool CanDoCheckCode(BrowseHistory bh)
        {
            bh.TryGiftCodeTimes++;
            if (bh.LastTryGiftCodeTime.AddMinutes(10) < DateTime.Now) // over 10 minites
            {
                bh.LastTryGiftCodeTime = DateTime.Now;
                bh.TryGiftCodeTimes = 0;
                return true;
            }
            else if (bh.TryGiftCodeTimes > 5)
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
        public ActionResult GiftCardCheck(string txtGC, BrowseHistory bh)
        {
            if (bh == null)
            {
                ControllerContext cc = new ControllerContext();
                bh = new BrowseHistory();
                bh.IsDelivery = true;

                cc.HttpContext.Session["BorseHistory"] = bh;
            }
            int gcs = 0; // 0= No such gift card; 1=Expired; 2=OK; 3=Too many times;
            string v = "Invalid gift card code. Please try again.";
            if (!CanDoCheckCode(bh))
            {
                gcs = 3;
                v = "Please come back in 10 minites.";
            }
            GiftCard gcd = GiftCardRepository.GetGiftCardByCode(txtGC, true);
            if (gcd != null)
            {
                bh.TryGiftCodeTimes = 0;
                if (gcd.Expired)
                {
                    gcs = 1;
                    v = "your gift card code has been expired.";
                }
                else
                {
                    gcs = 2;
                    v = "Gift card balance: $" + gcd.Balance.ToString("N2");
                }

            }
            ViewBag.CardStatus = gcs;
            ViewBag.StatusNote = v;
            ViewBag.servicephone = ServicePhone;
            return PartialView();
        }
        public ActionResult GiftCard(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }
        [Authorize]
        public ActionResult CardPurchase(ShoppingCart cart)
        {
            ViewBag.result = 0;
            BuyGiftCardModel gcb = new BuyGiftCardModel();
            if (User.Identity.IsAuthenticated)
            {
                AspNetUser up = new AspNetUser();
                up = UserRepository.GetUserByUserName(User.Identity.Name);
                gcb.BuyerEmail = up.UserDetails.Count > 0 ? up.UserDetails.FirstOrDefault(e => e.UserId == up.Id).ContactInfo.Email : "";
            }
            ViewBag.bagitems = GetCartItems(cart);
            return View(gcb);
        }
        [Authorize]
        [HttpPost]
        public ActionResult CardPurchase(BuyGiftCardModel model, ShoppingCart cart)
        {
            TempData["EmailSentMsg"] = "";
            TempData["CardCode"] = "";
            ViewBag.result = 0;
            if (ModelState.IsValid)
            {
                try
                {
                     if (DoDirectPaymentSuccess(model))
                    {
                        ViewBag.result = 1;
                        if (DoCheckoutCompleted(model))
                        {
                            ViewBag.result = 2;
                        }
                    }

                }
                catch
                {
                    ViewBag.result = 3;
                }
                // test = TempData["EmailSentMsg"] //test only
            }
            ViewBag.bagitems = GetCartItems(cart);
            return View(model);
        }
        [NonAction]
        public bool DoDirectPaymentSuccess(BuyGiftCardModel model)
        {
            bool success = false;
            com.paypal.sdk.services.NVPCallerServices caller = PayPalAPI.PayPalAPIInitialize();
            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "DoDirectPayment";
            encoder["PAYMENTACTION"] = "Sale";
            encoder["IPADDRESS"] = Helper.CurrentUserIP;
            encoder["AMT"] = Helper.FormatPriceToPayPalStringFormat(model.Amount);
            encoder["CREDITCARDTYPE"] = CreditCardTypeRepository.GetCreditCardTypeById(int.Parse(model.CardType)).Title;
            encoder["ACCT"] = model.CardNumber;
            encoder["EXPDATE"] = int.Parse(model.ExpirationMonth).ToString() + int.Parse(model.ExpirationYear).ToString();
            encoder["CVV2"] = int.Parse(model.SecurityCode).ToString();
            encoder["FIRSTNAME"] = model.BillFirstName;
            encoder["LASTNAME"] = model.BillLastName;
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
        public bool DoCheckoutCompleted(BuyGiftCardModel model)
        {
            bool completed = false;
            NVPCodec decoderResults = new NVPCodec();
            decoderResults = (NVPCodec)Session["result"];
            TempData["OrderSummaryTitle"] = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // string res = Util.BuildResponse(Session["result"], Request.QueryString.Get("API"), "Thank you for your payment!");

            int cardid = CreditCardRepository.GetCreditCardId(UserName, int.Parse(model.CardType), model.CardNumber, int.Parse(model.ExpirationMonth), int.Parse(model.ExpirationYear), int.Parse(model.SecurityCode));
            if (cardid == 0)
            {
                cardid = AddCreditCardToDB(model).CreditCardId;
            }
            CreditCard cc = CreditCardRepository.GetCreditCardById(cardid);
            //Order od = AddOrderToDB(cart, decoderResults["TRANSACTIONID"].ToString(), false, 0, 0.0m);
            string code = Guid.NewGuid().ToString();
            TempData["CardCode"] = code;
            DateTime dt = DateTime.Now;
            GiftCard gc = GiftCardRepository.AddGiftCard(0, code, CurrentUserID, cardid, 0, model.BuyerEmail, model.RecipientEmail,
                        model.Amount, dt, model.Amount, 0.0m, 0.0m, dt, UserName, dt, UserName, true);
            EmailManager em = new EmailManager();
            EmailContents ec = new EmailContents();

            em.OrderTime = dt.ToShortTimeString() + " " + dt.ToShortDateString();
            TempData["DeliveryAddress"] = "";
            em.OrderType = "Buy Gift Card";
            em.PaymentType = "Credit Card";
            em.OrderNumber = code; //gift card code
            em.Name = UserName;
            em.Phone = model.Phone;
            em.IpAddress = base.CurrentUserIP;
            em.CreditCardType = cc.CreditCardType.Title;
            em.CreditCardNumber = "xxxx-xxxx-xxxx-" + cc.CreditCardNumber.Substring(cc.CreditCardNumber.Length - 4);
            em.ExpirationDate = cc.ExpirationMonth.ToString() + "/" + cc.ExpirationYear.ToString();
            em.SecurityCode = "xx" + cc.SecurityCode.ToString().Substring(cc.SecurityCode.ToString().Length - 1);
            em.Instruction = model.Message;
            em.Total = Helper.FormatPriceWithDollar(decoderResults["AMT"].ToString()); // ToUSD(cart.Total().ToString("N2"));
            ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
            ec.FromName = "foodready.net";
            ec.Subject = "New online order";
            ec.To = model.RecipientEmail;
            ec.Body = em.BuildGiftCardEmailHtmlBody(model);
            //em.FaxBody = em.BuildGiftCardFaxHtmlBody(model);
            //em.SendFax(WebConfigurationManager.AppSettings["OnlineFaxNumber"].ToString());
            em.Send(ec);
            ec.To = Globals.Settings.ContactForm.MailTo;
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
            return completed;
        }
        [NonAction]
        public CreditCard AddCreditCardToDB(BuyGiftCardModel model)
        {
            CreditCard cc = new CreditCard();
            cc = CreditCardRepository.InsertCreditCard(0, UserName, int.Parse(model.CardType),
               model.BillFirstName, model.BillLastName, model.CardNumber, int.Parse(model.ExpirationMonth),
              int.Parse(model.ExpirationYear), int.Parse(model.SecurityCode), model.Phone,
              model.BuyerEmail, DateTime.Now, UserName, DateTime.Now, UserName, true);
            return cc;
        }
        [HttpPost]
        public ActionResult GetCart(ShoppingCart cart)
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            string timeout = "timein";
            if (bi == null)
            {
                timeout = "timeout";
                return Json(new
                {
                    timeexp = timeout
                });
            }
            decimal cartTotal = cart == null ? 0.00m : cart.Total();
            decimal ordermin = cart == null ? 0.00m : cart.OrderMinimum;
            string btnShow = "show";
            if (cart.Lines.Sum(x => x.Quantity) == 0 || (cart.OrderMinimum > cart.SubTotal() && cart.IsDelivery) || (bi.Delivery == false && cart.IsDelivery))
            {
                btnShow = "off";
            }
            return Json(new
            {
                bizid = cart.BizId.ToString(),
                biztitle = bi.BizTitle,
                bizimg = Url.Content("~/Content/BizImages/" + bi.ImageUrl),
                bizaddress = bi.BizTwoLineAddressString,
                bizlink = "/Restaurants/Menu/" + cart.BizId,
                isbizdelivery = cart.IsBizDelivery ? "yes" : "no",
                isDelivery = cart.IsDelivery ? "Delivery" : "Pickup",
                deliverymin = ordermin > 0.0m ? ToUSD(ordermin.ToString("N2")) : "",
                total = ToUSD(cartTotal.ToString("N2")),
                cartitems = cart.TotalItems.ToString(),
                subtotal = ToUSD(cart.SubTotal().ToString("N2")),
                btnshow = btnShow,
                timeexp = timeout
            });
        }
        public ActionResult ViewCart(ShoppingCart cart, BrowseHistory bh)
        {
            if (cart == null)
            {
                ControllerContext cc = new ControllerContext();
                cart = new ShoppingCart();
                cc.HttpContext.Session["ShoppingCart"] = cart;
                return Redirect("/Home");
            }
            if (bh == null)
            {
                ControllerContext cc = new ControllerContext();
                bh = new BrowseHistory();
                bh.IsDelivery = true;
                cc.HttpContext.Session["BorseHistory"] = bh;
                return Redirect("~/Home");
            }
            MenuViewModel mvm = new MenuViewModel();
            mvm.BizInfo = BizInfoRepository.GetBizInfoById(cart.BizId);
            mvm.Cart = cart;
            if (cart.BizId > 0)
            {
                cart.IsBizDelivery = mvm.BizInfo.Delivery;
                cart.TaxRate = mvm.BizInfo.TaxPercentageRate;
                cart.OrderMinimum = mvm.BizInfo.DeliveryMinimum;
                cart.DeliveryFee = mvm.BizInfo.DeliveryFee;
                mvm.Cart = cart;
                mvm.Cart.IsDelivery = bh.IsDelivery;
                mvm.Cart.BizId = cart.BizId;
                mvm.Cart.DeliveryFee = mvm.BizInfo.DeliveryFee;
                mvm.Cart.OrderMinimum = mvm.BizInfo.DeliveryMinimum;
                mvm.History = bh;
                string rul = HttpContext.Request.UrlReferrer == null ? "~/Home" : HttpContext.Request.UrlReferrer.PathAndQuery;
                mvm.ReturnUrl = rul;
                List<DiscountCoupon> ldc = new List<DiscountCoupon>();
                List<FreeItemCoupon> lfc = new List<FreeItemCoupon>();
                if (mvm.BizInfo.HasDiscountCoupons)
                {
                    ldc = DiscountCouponRepository.GetBizDiscountCouponsByMinimum(cart.BizId, cart.SubTotal(), true);
                }
                mvm.DiscountCouponList = ldc;
                if (mvm.BizInfo.HasFreeItemCoupons)
                {
                    lfc = FreeItemCouponRepository.GetBizFreeItemCouponsByMinimum(cart.BizId, cart.SubTotal(), true);
                }
                mvm.FreeItemCouponList = lfc;
                ViewBag.rurl = HttpContext.Request.UrlReferrer == null ? "~/Home" : HttpContext.Request.Url.PathAndQuery;

            }
            return PartialView(mvm);
        }
        public ActionResult TermsOfUse(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }
        public ActionResult PrivacyPolicy(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }
        public ActionResult SiteMap(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }
        //[OutputCache(Duration = 1000)]
        public ActionResult HotelType()
        {
            HotelModel hm = new HotelModel();
            hm.LHotelType = HotelTypeRepository.GetAllHotelTypes(true);
            return PartialView(hm);
        }
        public ActionResult Hotel(int id, string name) // id=HotelTypeId
        {
            Dictionary<string, List<Hotel>> dh = HotelRepository.GetCityHotelsByTypeId(id, true);
            HotelModel hm = new HotelModel();
            hm.DCityHotel = dh;
            ViewBag.typenme = name;
            return PartialView(hm);
        }
        public ActionResult GetHotel(int id, string name) // id=HotelTypeId
        {
            Dictionary<string, List<Hotel>> dh = HotelRepository.GetCityHotelsByTypeId(id, true);
            HotelModel hm = new HotelModel();
            hm.DCityHotel = dh;
            ViewBag.typenme = name;
            return PartialView(hm);
        }
    }
}
