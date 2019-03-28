using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FoodReady.WebUI.Controllers;
using FR.Repository.Interfaces;
using System.Configuration;
using Recaptcha;
using FR.Domain.Model.Entities;
using FR.Infrastructure.Helpers;
using FoodReady.WebUI.EmailServices;

namespace IdentitySample.Controllers
{
    //[RequireHttps]
    [Authorize]
    public class AccountController : BaseController
    {
        private IAddressRepository AddressRepository;
        private IContactInfoRepository ContactInfoRepository;
        private IUserDetailRepository UserDetailRepository;
        public AccountController(IAddressRepository addressRepo, IContactInfoRepository contactInfoRepo,
                               IUserDetailRepository userDetailRepo)
        {
            AddressRepository = addressRepo;
            ContactInfoRepository = contactInfoRepo;
            UserDetailRepository = userDetailRepo;
        }
        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, FR.Domain.Model.Entities.ShoppingCart cart, bool backFromCheckout = false)
        {
            ViewBag.bagitems = GetCartItems(cart);

            if (returnUrl == Url.Content("~/RestaurantOwners"))
            {
                ViewBag.OwnerLogon = "Note: Restaurant Owners only use this local login form to log in.";
            }
            else
            {
                ViewBag.OwnerLogon = "";
            }
            if (backFromCheckout && string.IsNullOrEmpty(returnUrl) == false && User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }
            ViewBag.Groupcheckout = "n";
            ViewBag.Guest = "yes";
            if (cart == null)
            {
                ViewBag.Guest = "no";
            }
            else
            {
                if (string.IsNullOrEmpty(cart.CartKey) == false && string.IsNullOrEmpty(cart.PersonName) == false && cart.PersonName == cart.BossName)
                {
                    ViewBag.Groupcheckout = "y";
                }
                else
                {
                    bool b = cart.BizId == 0 ? true : (cart.IsBizDelivery == false && cart.IsDelivery);
                    if (cart.Lines.Sum(x => x.Quantity) == 0 || (cart.OrderMinimum > cart.SubTotal() && cart.IsDelivery) || b)
                    {
                        ViewBag.Guest = "no";
                    }
                }

            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, FR.Domain.Model.Entities.ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            if (!ModelState.IsValid)
            {
                ViewBag.Guest = "yes";
                if (cart == null)
                {
                    ViewBag.Guest = "no";
                }
                else
                {
                    bool b = cart.BizId == 0 ? true : (cart.IsBizDelivery == false && cart.IsDelivery);
                    if (cart.Lines.Sum(x => x.Quantity) == 0 || (cart.OrderMinimum > cart.SubTotal() && cart.IsDelivery) || b)
                        ViewBag.Guest = "no";
                }
                if (returnUrl == Url.Content("~/RestaurantOwners"))
                {
                    ViewBag.OwnerLogon = "Note: Restaurant Owners only use this local login form to log in.";
                }
                else
                {
                    ViewBag.OwnerLogon = "";
                }
                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            // find user by username first
            var user = await UserManager.FindByNameAsync(model.Email);

            var message = string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString());

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    await UserManager.ResetAccessFailedCountAsync(user.Id);
                    UserManager.SetLockoutEnabled(user.Id, !UserManager.IsInRole(user.Id, "Admin"));
                    if (UserManager.IsInRole(user.Id, "Admin"))
                    {
                        return RedirectToLocal("/Admin");
                    }
                    if (UserManager.IsInRole(user.Id, "Manager"))
                    {
                        return RedirectToLocal("/FRManager");
                    }
                    if (UserManager.IsInRole(user.Id, "Restaurant"))
                    {
                        return RedirectToLocal("/RestaurantOwners");
                    }
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", message);
                    return View(model);
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                default:
                    ViewBag.Guest = "yes";
                    if (cart == null)
                    {
                        ViewBag.Guest = "no";
                    }
                    else
                    {
                        bool b = cart.BizId == 0 ? true : (cart.IsBizDelivery == false && cart.IsDelivery);
                        if (cart.Lines.Sum(x => x.Quantity) == 0 || (cart.OrderMinimum > cart.SubTotal() && cart.IsDelivery) || b)
                            ViewBag.Guest = "no";
                    }
                    if (returnUrl == Url.Content("~/RestaurantOwners"))
                    {
                        ViewBag.OwnerLogon = "Note: Restaurant Owners only use this local login form to log in.";
                    }
                    else
                    {
                        ViewBag.OwnerLogon = "";
                    }
                    // If we got this far, something failed, redisplay form
                    ViewBag.ReturnUrl = returnUrl;
                    ModelState.AddModelError("", "Invalid login.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string returnUrl,ShoppingCart cart)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RecaptchaControlMvc.CaptchaValidator]
        public async Task<ActionResult> Register(string returnUrl, RegisterViewModel model, bool captchaValid, string mobile, string captchaErrorMessage,ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            if (ModelState.IsValid)
            {
                if (!captchaValid)
                {
                    ModelState.AddModelError("recaptcha", captchaErrorMessage);
                    ViewBag.ReturnUrl = returnUrl;
                    return View(model);
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, AddedDate = DateTime.Now, UpdatedDate = DateTime.Now, Active = true };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                     // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        try
                        {
                            Address ar = AddressRepository.AddAddress(0, model.AddressLine, "", model.City, model.State, model.ZipCode,
                                          "", DateTime.Now, model.Email, DateTime.Now, model.Email, true);
                            ContactInfo ci = ContactInfoRepository.AddContactInfo(0, model.Phone, "", model.Email, DateTime.Now, model.Email, DateTime.Now, model.Email, true);
                            UserDetail ud = UserDetailRepository.InsertUserDetail(0, user.Id, model.FirstName, model.LastName, ar.AddressId,
                                        ci.ContactInfoId, DateTime.Now, model.Email, DateTime.Now, model.Email, true, CryptionClass.Encrypt(model.Password));
                            EmailManager em = new EmailManager();
                            EmailContents ec = new EmailContents();
                            ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
                            ec.FromName = "FoodReady.Net";
                            ec.Subject = "confirm your email address for your foodready.net account";
                            ec.To = model.Email;
                            ec.Body = "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>";
                            em.Send(ec);
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", "Something failed: " + e.Message);
                            return View(model);
                        }

                        //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                        ViewBag.Link = callbackUrl;
                        return View("DisplayEmail");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model,ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                EmailManager em = new EmailManager();
                EmailContents ec = new EmailContents();
                ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
                ec.FromName = "FoodReady.Net";
                ec.Subject = "Reset Password for your foodready.net account";
                ec.To = model.Email;
                ec.Body = "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>";
                try
                {
                    em.Send(ec);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Issue sending email: " + e.Message);
                    return View(model);
                }



               // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Link = callbackUrl;
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code,ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model,ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, AddedDate = DateTime.Now, UpdatedDate = DateTime.Now, Active = true };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}