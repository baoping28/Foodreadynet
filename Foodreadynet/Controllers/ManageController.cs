using FoodReady.WebUI.Controllers;
using FoodReady.WebUI.Models;
using FR.Domain.Model.Entities;
using FR.Infrastructure.Helpers;
using FR.Repository.Interfaces;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private IAddressRepository AddressRepository;
        private IContactInfoRepository ContactInfoRepository;
        private IUserDetailRepository UserDetailRepository;
        private IOrderRepository OrderRepository;
        private ICreditCardRepository CreditCardRepository;
        private IGiftCardRepository GiftCardRepository;
        private IFavorRestaurantRepository FavorRestaurantRepository;
        private IVoteRepository VoteRepository;
        public ManageController(IAddressRepository addressRepo, IContactInfoRepository contactInfoRepo,
                               IUserDetailRepository userDetailRepo,
                               IOrderRepository orderRepo, ICreditCardRepository creditCardRepo,
                               IGiftCardRepository giftCardRepo, IFavorRestaurantRepository favorRestaurantRepo,
                               IVoteRepository VoteRepo)
        {
            AddressRepository = addressRepo;
            ContactInfoRepository = contactInfoRepo;
            UserDetailRepository = userDetailRepo;
            OrderRepository = orderRepo;
            CreditCardRepository = creditCardRepo;
            GiftCardRepository = giftCardRepo;
            FavorRestaurantRepository = favorRestaurantRepo;
            VoteRepository = VoteRepo;
        }
        public ManageController()
        {
        }

        //
        // GET: /Account/Index
        public async Task<ActionResult> Index(ManageMessageId? message,ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two factor provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "The phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId()),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId()),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId())
            };
            return View(model);
        }

        //
        // GET: /Account/RemoveLogin
        public ActionResult RemoveLogin()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Account/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Account/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/RememberBrowser
        [HttpPost]
        public ActionResult RememberBrowser()
        {
            var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(User.Identity.GetUserId());
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, rememberBrowserIdentity);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/ForgetBrowser
        [HttpPost]
        public ActionResult ForgetBrowser()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/EnableTFA
        [HttpPost]
        public async Task<ActionResult> EnableTFA()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTFA
        [HttpPost]
        public async Task<ActionResult> DisableTFA()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Account/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            // This code allows you exercise the flow without actually sending codes
            // For production use please register a SMS provider in IdentityConfig and generate a code here.
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            ViewBag.Status = "For DEMO purposes only, the current code is " + code;
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Account/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Account/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model,ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model,ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Manage
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }
        public ActionResult MyProfile(ShoppingCart cart)
        {
            UserDetail ud = new UserDetail();
            ud = UserDetailRepository.GetUserDetailByUserId(CurrentUserID);
            ViewBag.myname = User.Identity.Name;
            ViewBag.myid = CurrentUserID;
            ViewBag.bagitems = GetCartItems(cart);
            return View(ud);
        }
        [HttpPost]
        public ActionResult AddMyDetail(MyDetailModel model)
        {
            ViewBag.ok = "no";
            if (ModelState.IsValid)
            {
                try
                {
                    Address ar = AddressRepository.AddAddress(0, model.AddressLine, "", model.City, model.State, model.ZipCode,
                                      model.CrossStreet, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ContactInfo ci = ContactInfoRepository.AddContactInfo(0, model.Phone, "", model.Email, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    UserDetail ud = UserDetailRepository.InsertUserDetail(0, model.UserID, model.FirstName, model.LastName, ar.AddressId,
                         ci.ContactInfoId, DateTime.Now, UserName, DateTime.Now, UserName, true, CryptionClass.Encrypt("no password"));
                    ViewBag.ok = "yes";
                    return PartialView(model);
                }
                catch
                {
                    ViewBag.ok = "no";
                }
            }
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult EditProfile(MyDetailModel model)
        {
            ViewBag.ok = "no";
            if (ModelState.IsValid)
            {
                try
                {

                    UserDetail ud = UserDetailRepository.GetUserDetailById(model.UserDetailID);
                    if (ud != null)
                    {
                        Address ar = AddressRepository.AddAddress(ud.AddressId, model.AddressLine, "", model.City, model.State, model.ZipCode,
                                          model.CrossStreet, ud.Address.AddedDate, ud.Address.AddedBy, DateTime.Now, UserName, true);
                        ContactInfo ci = ContactInfoRepository.AddContactInfo(ud.ContactInfoId, model.Phone, "", model.Email, ud.ContactInfo.AddedDate, ud.ContactInfo.AddedBy, DateTime.Now, UserName, true);
                        ud = UserDetailRepository.InsertUserDetail(ud.UserDetailId, ud.UserId, model.FirstName, model.LastName, ud.AddressId,
                             ud.ContactInfoId, ud.AddedDate, ud.AddedBy, DateTime.Now, UserName, true, CryptionClass.Encrypt("no password"));
                        ViewBag.ok = "yes";
                        return PartialView(model);
                    }
                }
                catch
                {
                    ViewBag.ok = "no";
                }
            }
            return PartialView(model);
        }
        public ActionResult OrderHistory(ShoppingCart cart)
        {
            DateTime df = DateTime.Now.AddDays(-14);
            DateTime dt = DateTime.Now;
            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            MyOrder mo = new MyOrder();
            mo.Orders = OrderRepository.GetOrdersByDateRangeWithLogonName(df, dt, UserName);
            mo.PeriodSubtotal = mo.Orders.Sum(e => e.SubTotal);
            mo.PeriodTotal = mo.Orders.Sum(e => e.OrderTotal);
            mo.PeriodOrderTax = mo.Orders.Sum(e => e.OrderTax);
            mo.PeriodServiceCharge = mo.Orders.Sum(e => e.ServiceCharge);
            mo.PeriodDriverTip = mo.Orders.Sum(e => e.DriverTip);
            mo.PeriodDiscountAmount = mo.Orders.Sum(e => e.DiscountAmount);
            mo.PeriodDeliveryCharge = mo.Orders.Sum(e => e.DeliveryCharge);
            ViewBag.note = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " (" + mo.Orders.Count.ToString() + ")";
            ViewBag.bagitems = GetCartItems(cart);
            return View(mo);
        }
        [HttpPost]
        public ActionResult OrderHistory(string vFromDate, string vToDate, string vEmail, string vInvoiceNumber, string vTransactionId,ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            MyOrder mo = new MyOrder();
            DateTime df;
            DateTime dt;
            bool bl = false;
            if (string.IsNullOrEmpty(vFromDate))
            {
                df = DateTime.Now.AddDays(-7);

            }
            else
            {
                bl = DateTime.TryParse(vFromDate, out df);
                if (!bl)
                {
                    df = DateTime.Now.AddDays(-7);
                }
            }
            if (string.IsNullOrEmpty(vToDate))
            {
                dt = DateTime.Now;

            }
            else
            {
                bl = DateTime.TryParse(vToDate, out dt);
                if (!bl)
                {
                    dt = DateTime.Now;
                }
            }
            if (df > dt)
            {
                df = DateTime.Now.AddDays(-7);
                dt = DateTime.Now;
            }

            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            if (!string.IsNullOrEmpty(vEmail))
            {
                mo.Orders = OrderRepository.GetUserOrdersByEmail(UserName, vEmail);
                ViewBag.note = "with " + vEmail + " (" + mo.Orders.Count + ")";
            }
            else if (!string.IsNullOrEmpty(vInvoiceNumber))
            {
                mo.Orders = OrderRepository.GetUserOrdersByInvoiceNumber(UserName, vInvoiceNumber);
                ViewBag.note = "with invoice number " + vInvoiceNumber + " (" + mo.Orders.Count + ")";
            }
            else if (!string.IsNullOrEmpty(vTransactionId))
            {
                mo.Orders = OrderRepository.GetUserOrdersByTransactionId(UserName, vTransactionId);
                ViewBag.note = "with PayPal Transaction ID " + vTransactionId + " (" + mo.Orders.Count + ")";
            }
            else
            {
                mo.Orders = OrderRepository.GetOrdersByDateRangeWithLogonName(df, dt, UserName);

                if (mo.Orders != null)
                {
                    mo.PeriodSubtotal = mo.Orders.Sum(e => e.SubTotal);
                    mo.PeriodTotal = mo.Orders.Sum(e => e.OrderTotal);
                    mo.PeriodOrderTax = mo.Orders.Sum(e => e.OrderTax);
                    mo.PeriodServiceCharge = mo.Orders.Sum(e => e.ServiceCharge);
                    mo.PeriodDriverTip = mo.Orders.Sum(e => e.DriverTip);
                    mo.PeriodDiscountAmount = mo.Orders.Sum(e => e.DiscountAmount);
                    mo.PeriodDeliveryCharge = mo.Orders.Sum(e => e.DeliveryCharge);
                    ViewBag.note = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " (" + mo.Orders.Count.ToString() + ")";
                }
                else
                {
                    mo.PeriodSubtotal = 0.0m;
                    mo.PeriodTotal = 0.0m;
                    mo.PeriodOrderTax = 0.0m;
                    mo.PeriodServiceCharge = 0.0m;
                    mo.PeriodDriverTip = 0.0m;
                    mo.PeriodDiscountAmount = 0.0m;
                    mo.PeriodDeliveryCharge = 0.0m;
                    ViewBag.note = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " (0)";
                }
            }
            return View(mo);
        }
        public ActionResult OrderDetails(int id) // id=OrderId
        {
            Order o = new Order();
            o = OrderRepository.GetOrderById(id);
            CreditCard cc = CreditCardRepository.GetCreditCardById(o.CreditCardId);
            ViewBag.creditcard = cc == null ? "" : cc.last4Digits;
            ViewBag.paymenttype = o.IsPayPalPayment ? "PayPal Payment" : o.IsGiftCardPayment == false ? "Credit Card Payment" : o.GiftCardAmountPay == o.OrderTotal ? "Gift Card Payment" : "Credit Card and Gift Card Payment";
            return PartialView(o);
        }
        public ActionResult OrderItems(int id) // id=OrderId
        {
            Order o = new Order();
            o = OrderRepository.GetOrderById(id);
            return PartialView(o);
        }
        public ActionResult PrintReceipt(int id) // id=OrderId
        {
            Order o = new Order();
            o = OrderRepository.GetOrderById(id);
            ViewBag.servicephone = ServicePhone;
            ViewBag.servicefax = ServiceFax;
            ViewBag.paymenttype = o.IsPayPalPayment ? "PayPal Payment" : o.IsGiftCardPayment == false ? "Credit Card Payment" : o.GiftCardAmountPay == o.OrderTotal ? "Gift Card Payment" : "Credit Card and Gift Card Payment";
            return View(o);
        }
        public ActionResult MyGiftCards(ShoppingCart cart)
        {
            List<GiftCard> lgc = new List<GiftCard>();
            lgc = GiftCardRepository.GetAllGiftCardsByUserId(CurrentUserID, true);
            ViewBag.bagitems = GetCartItems(cart);
            return View(lgc);
        }
        public ActionResult GiftCardDetails(int id, string code) // id=GiftCardId code=GiftCardCode
        {
            List<Order> lo = new List<Order>();
            lo = OrderRepository.GetOrdersByGiftCardId(id);
            ViewBag.gcode = "xxxx-xxxx-xxxx-" + code.Substring(code.Length - 4);
            return PartialView(lo);
        }
        public ActionResult MyFavorRestaurants(ShoppingCart cart)
        {
            List<FavorRestaurant> lf = new List<FavorRestaurant>();
            lf = FavorRestaurantRepository.GetFavorRestaurantsByUserId(CurrentUserID);
            ViewBag.bagitems = GetCartItems(cart);
            return View(lf);
        }
        public ActionResult MyReviews(ShoppingCart cart)
        {
            List<Vote> lv = new List<Vote>();
            lv = VoteRepository.GetAllVotesByUserId(CurrentUserID);
            ViewBag.bagitems = GetCartItems(cart);
            return View(lv);
        }
        public ActionResult DoReview(int id) // id=VoteId
        {
            Vote v = new Vote();
            v = VoteRepository.GetVoteById(id);
            if (v.Active)
            {
                VoteRepository.LockVote(v);
            }
            else
            {
                VoteRepository.UnlockVote(v);
            }
            v = VoteRepository.GetVoteById(id);
            return PartialView(v);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}