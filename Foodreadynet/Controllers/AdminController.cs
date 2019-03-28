using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using FoodReady.WebUI.Models;
using FR.Repository.Interfaces;
using FR.Domain.Model.Entities;
using FoodReady.WebUI.EmailServices;
using FR.Infrastructure.Helpers;
using FRContext = FoodReady.WebUI.Models;
using System.Web;
using System.IO;
using FR.Services.GoogleAPI;
using System.Configuration;
using PagedList;
using IdentitySample.Models;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Owin.Security;
namespace FoodReady.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        private ICategoryRepository CategoryRepository;
        private IDayOfCloseRepository DayOfCloseRepository;
        private IDiscountCouponRepository DiscountCouponRepository;
        private IFreeItemCouponRepository FreeItemCouponRepository;
        private ICuisineTypeRepository CuisineTypeRepository;
        private IBizCuisineRepository BizCuisineRepository;
        private IAddressRepository AddressRepository;
        private IContactInfoRepository ContactInfoRepository;
        private IUserDetailRepository UserDetailRepository;
        private IBizInfoRepository BizInfoRepository;
        private IZoneNameRepository ZoneNameRepository;
        private IBizHourRepository BizHourRepository;
        private IUserRepository UserRepository;
        private IUserLoginRepository UserLoginRepository;
        private IMealSectionRepository MealSectionRepository;
        private IFoodTypeRepository FoodTypeRepository;
        private ICookMethodRepository CookMethodRepository;
        private IProductRepository ProductRepository;
        private IToppingRepository ToppingRepository;
        private IProductToppingRepository ProductToppingRepository;
        private IProductSizeRepository ProductSizeRepository;
        private ISideChoiceRepository SideChoiceRepository;
        private IDressingRepository DressingRepository;
        private IProductDressingRepository ProductDressingRepository;
        private IOrderRepository OrderRepository;
        private ICreditCardRepository CreditCardRepository;
        private IOrderItemRepository OrderItemRepository;
        private IGiftCardRepository GiftCardRepository;
        private IFamilyMealRepository FamilyMealRepository;
        private IAddSideRepository AddSideRepository;
        private ICrustChoiceRepository CrustChoiceRepository;
        private ICheeseAmountRepository CheeseAmountRepository;
        private ISauceChoiceRepository SauceChoiceRepository;
        private IBizImageRepository BizImageRepository;
        private IBizRVInfoRepository BizRVInfoRepository;
        private IReservationRepository ReservationRepository;
        private IHotelRepository HotelRepository;
        private IHotelTypeRepository HotelTypeRepository;
        private IDriverRepository DriverRepository;
        public AdminController(IAddressRepository addressRepo, IContactInfoRepository contactInfoRepo,
                               IUserDetailRepository userDetailRepo, IBizInfoRepository bizInfoRepo,
                               IZoneNameRepository zoneNameRepo, IBizHourRepository bizHourRepo,
                               IUserRepository userRepo, IUserLoginRepository UserLoginRepo,
                               ICuisineTypeRepository cuisineTypeRepo, IBizCuisineRepository bizCuisineRepo,
                               IFreeItemCouponRepository freeItemCouponRepo, IDiscountCouponRepository discountCouponRepo,
                               IDayOfCloseRepository dayOfCloseRepo, ICategoryRepository categoryRepo,
                               IMealSectionRepository mealSectionRepo, IFoodTypeRepository foodTypeRepo,
                               ICookMethodRepository cookMethodRepo, IProductRepository productRepo,
                               IToppingRepository toppingRepo, IProductToppingRepository productToppingRepo,
                               IProductSizeRepository productSizeRepo, ISideChoiceRepository sideChoiceRepo,
                               IDressingRepository dressingRepo, IProductDressingRepository productDressingRepo,
                               IOrderRepository orderRepo, ICreditCardRepository creditCardRepo,
                               IOrderItemRepository OrderItemRepo, IGiftCardRepository giftCardRepo,
                               IFamilyMealRepository familyMealRepo, IAddSideRepository addSideRepo,
                               ICrustChoiceRepository crustChoiceRepo, ICheeseAmountRepository cheeseAmountRepo,
                               ISauceChoiceRepository sauceChoiceRepo, IBizImageRepository bizImageRepo,
                               IBizRVInfoRepository bizRVInfoRepo, IReservationRepository reservationRepo,
                               IHotelRepository hotelRepo, IHotelTypeRepository hotelTypeRepo,
                               IDriverRepository driverRepo)
        {
            AddressRepository = addressRepo;
            ContactInfoRepository = contactInfoRepo;
            UserDetailRepository = userDetailRepo;
            BizInfoRepository = bizInfoRepo;
            ZoneNameRepository =zoneNameRepo;
            BizHourRepository= bizHourRepo;
            UserRepository = userRepo;
            UserLoginRepository = UserLoginRepo;
            CuisineTypeRepository = cuisineTypeRepo;
            BizCuisineRepository = bizCuisineRepo;
            FreeItemCouponRepository = freeItemCouponRepo;
            DiscountCouponRepository =discountCouponRepo;
            DayOfCloseRepository =dayOfCloseRepo;
            CategoryRepository =categoryRepo;
            MealSectionRepository =mealSectionRepo;
            FoodTypeRepository =foodTypeRepo;
            CookMethodRepository =cookMethodRepo;
            ProductRepository =productRepo;
            ToppingRepository= toppingRepo;
            ProductToppingRepository= productToppingRepo;
            ProductSizeRepository =productSizeRepo;
            SideChoiceRepository =sideChoiceRepo;
            DressingRepository= dressingRepo;
            ProductDressingRepository =productDressingRepo;
            OrderRepository= orderRepo;
            CreditCardRepository= creditCardRepo;
            OrderItemRepository =OrderItemRepo;
            GiftCardRepository =giftCardRepo;
            FamilyMealRepository = familyMealRepo;
            AddSideRepository = addSideRepo;
            CrustChoiceRepository = crustChoiceRepo;
            CheeseAmountRepository = cheeseAmountRepo;
            SauceChoiceRepository = sauceChoiceRepo;
            BizImageRepository = bizImageRepo;
            BizRVInfoRepository = bizRVInfoRepo;
            ReservationRepository = reservationRepo;
            HotelRepository =hotelRepo;
            HotelTypeRepository= hotelTypeRepo;
            DriverRepository= driverRepo;
        }

        public ActionResult Index()
        {
            /*
            UserRepository.UnlockUser(UserName);
             */
            return View();
        }
        public ActionResult RegisterBiz()
        {
            TempData["SentMsg"] = "";
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterBiz(RegisterViewModel model)
        {
            TempData["SentMsg"] = "";
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, AddedDate = DateTime.Now, UpdatedDate = DateTime.Now, Active = true };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        //WebSecurity.Login(model.UserName, model.Password);
                        Address ar = AddressRepository.AddAddress(0, model.AddressLine, "", model.City, model.State, model.ZipCode,
                                    "", DateTime.Now, model.Email, DateTime.Now, model.Email, true);
                        ContactInfo ci = ContactInfoRepository.AddContactInfo(0, Helper.FormatPhoneNumber(model.Phone.Replace(" ", ""), Helper.PhoneNumberFormat.Straight), "", model.Email, DateTime.Now, model.Email, DateTime.Now, model.Email, true);
                          UserDetail ud = UserDetailRepository.InsertUserDetail(0, user.Id, model.FirstName, model.LastName, ar.AddressId,
                                        ci.ContactInfoId, DateTime.Now, model.Email, DateTime.Now, model.Email, true, CryptionClass.Encrypt(model.Password));

                        await UserManager.AddToRoleAsync(user.Id, "Restaurant");
                        EmailManager em = new EmailManager();
                        EmailContents ec = new EmailContents("foodready.net", model.Email, Globals.Settings.ContactForm.MailFrom,
                                            "You account has been created", EmailManager.BuildRegisterEmailBody(model));
                        //em.Send(ec); // send to foodready.net
                        if (em.IsSent == false)
                        {
                            TempData["SentMsg"] = "Email has not been sent out for some reasons.";
                            return View(model);
                        }
                        else
                        {
                            TempData["SentMsg"] = "";
                        }
                        return RedirectToAction("Index", "Admin");
                    } 
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            TempData["SentMsg"] = "Register failed.";
            return View(model);
        }

        public ActionResult RegisterDriver()
        {
            TempData["driverMsg"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult RegisterDriver(DriverModel model)
        {
            TempData["driverMsg"] = "";
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    if (DriverRepository.IsSigninNameExist(model.Name))
                    {
                        TempData["driverMsg"] = "The name already exist.";
                        return View(model);
                    }
                    DriverRepository.AddDriver(0, model.FirstName, model.LastName, model.Name, model.Password, false, model.WorkCity, model.AddressLine, model.City, model.State, model.ZipCode,
                                               model.Phone, model.Email, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    EmailManager em = new EmailManager();
                    EmailContents ec = new EmailContents("foodready.net", model.Email, Globals.Settings.ContactForm.MailFrom,
                                        "You driver account has been created", EmailManager.BuildRegisterDriverEmailBody(model));
                    em.Send(ec); // send to driver
                    if (em.IsSent == false)
                    {
                        TempData["driverMsg"] = "Email has not been sent out for some reasons.";
                        return View(model);
                    }
                    else
                    {
                        TempData["driverMsg"] = "";
                    }
                    return RedirectToAction("Index", "Admin");

                }
                catch 
                {
                    TempData["driverMsg"] = "Something wrong";
                }
            }

            TempData["driverMsg"] = "Something wrong";
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
        public ActionResult AllRestaurantUsers(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "phone_desc" : "Phone";
            List<UserDetail> lud = UserDetailRepository.GetAllBizUserDetails(true);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                lud = lud.Where(s => s.UserName.ToUpper().Contains(searchString.ToUpper()) || s.ContactInfo.Phone.Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    lud = lud.OrderByDescending(s => s.UserName).ToList();
                    break;
                case "Phone":
                    lud = lud.OrderBy(s => s.ContactInfo.Phone).ToList();
                    break;
                case "phone_desc":
                    lud = lud.OrderByDescending(s => s.ContactInfo.Phone).ToList();
                    break;
                default:  // Name ascending 
                    lud = lud.OrderBy(s => s.UserName).ToList();
                    break;
            }

            int pageSize = Globals.Settings.Articles.PageSize < 5 ? 5 : Globals.Settings.Articles.PageSize;
            int pageNumber = (page ?? 1);
            return View(lud.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult CreateRestaurant(int id) // id=UserDetailId
        {
            UserDetail ud = UserDetailRepository.GetUserDetailById(id);
            ViewBag.username = ud.UserName;
            string yl = ud.Address.AddressLine.Trim().Replace(" ", "+") + "%2C" + ud.Address.City.Trim().Replace(" ", "+") + "%2C" + ud.Address.State + " " + ud.Address.ZipCode;
            RestaurantModel rm = new RestaurantModel();
            rm.LZoneNames = ZoneNameRepository.GetAllZoneNames(true);
            rm.UserDetailId = id;
            rm.DeliveryFee = 5.00m;
            rm.OrderMinimum = 20.00m;
            rm.Radius = 5;
            ViewBag.findYelpId=@"http://www.yelp.com/search?find_desc=restaurants&find_loc=" +  @yl;
            return View(rm);
        }
        [HttpPost]
        public ActionResult CreateRestaurant(RestaurantModel model, HttpPostedFileBase uploadimage)
        { 

            model.LZoneNames = ZoneNameRepository.GetAllZoneNames(true);
            if (ModelState.IsValid)
            {
                ViewBag.createMsg = "";
                string imagename = "";
                UserDetail ud = UserDetailRepository.GetUserDetailById(model.UserDetailId);
                if (ud != null)
                {
                    if (uploadimage == null)
                    {
                        ViewBag.createMsg = "You must upload the image.";
                        return View(model);
                    }
                    imagename = UploadImage(uploadimage, ud.UserId, "Content/BizImages");
                }

                GeocoderLocation gl = new GeocoderLocation();
                gl = Geocoding.Locate(ud.UserAddressLine);
                string lat = gl == null ? "" : gl.Latitude.ToString();
                string log = gl == null ? "" : gl.Longitude.ToString();
                try
                {
                    var bizH = BizHourRepository.AddBizHour(0, model.LMonStart, model.LMonClose, model.LTueStart, model.LTueClose,
                             model.LWedStart, model.LWedClose, model.LThuStart, model.LTueClose, model.LFriStart, model.LFriClose,
                             model.LSatStart, model.LSatClose, model.LSunStart, model.LSunClose, model.MonStart, model.MonClose,
                             model.TueStart, model.TueClose, model.WedStart, model.WedClose, model.ThuStart, model.TueClose,
                             model.FriStart, model.FriClose, model.SatStart, model.SatClose, model.SunStart, model.SunClose,
                             model.ZoneNameId, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    var bizinfo = BizInfoRepository.AddBizInfo(0, ud.UserId, model.BizTitle, model.Description, imagename, ud.AddressId,
                        ud.ContactInfoId, bizH.BizHourId,model.YelpBizID, lat, log, model.Fax,string.IsNullOrEmpty(model.FoodCost)?"$$":model.FoodCost, model.HasDiscountCoupons == "Yes" ? true : false,
                        model.HasFreeItemCoupons == "Yes" ? true : false, model.HasBreakfast == "Yes" ? true : false,
                        model.HasLunchSpecial == "Yes" ? true : false, model.CanOrderForNextday == "Yes" ? true : false, model.Website,
                        model.Delivery == "Yes" ? true : false, model.Radius, model.DeliveryFee, model.OrderMinimum, model.TaxPercentageRate, 0, 0, 0, 0, 0, 0, 0,
                        DateTime.Now, UserName, DateTime.Now, UserName, true);

                    string pathString = System.IO.Path.Combine(System.IO.Path.Combine(Request.PhysicalApplicationPath, "Content/BizImages"), "Biz_" + bizinfo.BizInfoId);
                    if (!System.IO.Directory.Exists(pathString))
                    {
                        System.IO.Directory.CreateDirectory(pathString);
                    }

                    ViewBag.createMsg = "Create action successed!";
                    return View(model);

                }
                catch
                {
                    ViewBag.createMsg = "Create action failed!";
                }
                return View(model);
            }
            ViewBag.createMsg = "Create action failed!";
            return View(model);
        }

        [NonAction]
        public string UploadImage(HttpPostedFileBase image,string vUserId,string vFoldPath)
        {
            if (image == null)
            {
                return "imageSoon.png";

            }
            string imgname = image.FileName.Substring(image.FileName.LastIndexOf(@"\") + 1);
            if (string.IsNullOrEmpty(vUserId)==false)
            {
                imgname = imgname.Substring(0, imgname.LastIndexOf(".")) + "_" + vUserId + imgname.Substring(imgname.LastIndexOf("."));
            }
            string fileMimeType = image.ContentType;
            byte[] fileData = new byte[image.ContentLength];
            image.InputStream.Read(fileData, 0, image.ContentLength);
            //string imgfolderpath = "Content/BizImages";
            System.IO.File.WriteAllBytes(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, vFoldPath), imgname), fileData);
           
            return imgname;
        }
        public ActionResult ManageRestaurant(int id) // id=BizInfoId
        {
            ViewBag.updateMsg = "";
            BizInfo bi = BizInfoRepository.GetBizInfoById(id);
            ViewBag.username = UserDetailRepository.GetUserDetailByUserId(bi.UserId).UserName;
            RestaurantModel rm = new RestaurantModel();
            rm.Bizinfo = bi;
            rm.LZoneNames = ZoneNameRepository.GetAllZoneNames(true);
            rm.BizInfoID = bi.BizInfoId;
            rm.UserID = bi.UserId;
            rm.BizHourID = bi.BizHourId;
            rm.YelpBizID = bi.YelpBizId;
            rm.AddressID = bi.AddressId;
            rm.ContactInfoID = bi.ContactInfoId;
            rm.BizTitle = bi.BizTitle;
            rm.ImageUrl = bi.ImageUrl;
            rm.ZoneNameId = bi.BizHour.ZoneNameId;
            rm.Description = bi.Description;
            rm.LMonStart = bi.BizHour.LMonStart;
            rm.LMonClose = bi.BizHour.LMonClose;
            rm.LTueStart = bi.BizHour.LTueStart;
            rm.LTueClose = bi.BizHour.LTueClose;
            rm.LWedStart = bi.BizHour.LWedStart;
            rm.LWedClose = bi.BizHour.LWedClose;
            rm.LThuStart = bi.BizHour.LThuStart;
            rm.LThuClose = bi.BizHour.LThuClose;
            rm.LFriStart = bi.BizHour.LFriStart;
            rm.LFriClose = bi.BizHour.LFriClose;
            rm.LSatStart = bi.BizHour.LSatStart;
            rm.LSatClose = bi.BizHour.LSatClose;
            rm.LSunStart = bi.BizHour.LSunStart;
            rm.LSunClose = bi.BizHour.LSunClose;

            rm.MonStart = bi.BizHour.MonStart;
            rm.MonClose = bi.BizHour.MonClose;
            rm.TueStart = bi.BizHour.TueStart;
            rm.TueClose = bi.BizHour.TueClose;
            rm.WedStart = bi.BizHour.WedStart;
            rm.WedClose = bi.BizHour.WedClose;
            rm.ThuStart = bi.BizHour.ThuStart;
            rm.ThuClose = bi.BizHour.ThuClose;
            rm.FriStart = bi.BizHour.FriStart;
            rm.FriClose = bi.BizHour.FriClose;
            rm.SatStart = bi.BizHour.SatStart;
            rm.SatClose = bi.BizHour.SatClose;
            rm.SunStart = bi.BizHour.SunStart;
            rm.SunClose = bi.BizHour.SunColse;

            rm.Fax = bi.Fax;
            rm.HasDiscountCoupons = bi.HasDiscountCoupons == true ? "Yes" : "No";
            rm.HasFreeItemCoupons = bi.HasFreeItemCoupons == true ? "Yes" : "No";
            rm.HasBreakfast = bi.HasBreakfast == true ? "Yes" : "No";
            rm.HasLunchSpecial = bi.HasLunchSpecial == true ? "Yes" : "No";
            rm.CanOrderForNextday = bi.CanOrderForNextday == true ? "Yes" : "No";
            rm.Website = bi.Website;
            rm.FoodCost = bi.FoodCost;
            rm.Delivery = bi.Delivery == true ? "Yes" : "No";
            rm.Radius = bi.DeliveryRadius;
            rm.DeliveryFee = bi.DeliveryFee;
            rm.OrderMinimum = bi.DeliveryMinimum;
            rm.TaxPercentageRate = bi.TaxPercentageRate;
            return View(rm);
        }
        [HttpPost]
        public ActionResult ManageRestaurant(RestaurantModel model, HttpPostedFileBase uploadimage)
        {
            ViewBag.updateMsg = "";
            ViewBag.username = UserDetailRepository.GetUserDetailByUserId(model.UserID).UserName;
            model.LZoneNames = ZoneNameRepository.GetAllZoneNames(true);
            if (ModelState.IsValid)
            {
                model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
                string imagename = "";
                imagename = uploadimage == null ? "imageSoon.png" : UploadImage(uploadimage, model.UserID, "Content/BizImages");
                model.ImageUrl = imagename;
                try
                {
                    BizHour bh = BizHourRepository.GetBizHourById(model.BizHourID);
                   bh = BizHourRepository.AddBizHour(model.BizHourID, model.LMonStart, model.LMonClose, model.LTueStart, model.LTueClose,
                             model.LWedStart, model.LWedClose, model.LThuStart, model.LTueClose, model.LFriStart, model.LFriClose,
                             model.LSatStart, model.LSatClose, model.LSunStart, model.LSunClose, model.MonStart, model.MonClose,
                             model.TueStart, model.TueClose, model.WedStart, model.WedClose, model.ThuStart, model.TueClose,
                             model.FriStart, model.FriClose, model.SatStart, model.SatClose, model.SunStart, model.SunClose,
                             model.ZoneNameId, bh.AddedDate, bh.AddedBy, DateTime.Now, UserName, true);
                    BizInfo bi = BizInfoRepository.GetBizInfoById(model.BizInfoID);
                    bi = BizInfoRepository.AddBizInfo(model.BizInfoID, model.UserID, model.BizTitle, model.Description, imagename, model.AddressID,
                        model.ContactInfoID, model.BizHourID, model.YelpBizID, bi.Latitude, bi.Longitude, model.Fax, string.IsNullOrEmpty(model.FoodCost) ? "$$" : model.FoodCost, model.HasDiscountCoupons == "Yes" ? true : false,
                        model.HasFreeItemCoupons == "Yes" ? true : false, model.HasBreakfast == "Yes" ? true : false,
                        model.HasLunchSpecial == "Yes" ? true : false, model.CanOrderForNextday == "Yes" ? true : false, model.Website,
                        model.Delivery == "Yes" ? true : false, model.Radius, model.DeliveryFee, model.OrderMinimum, model.TaxPercentageRate, 
                        bi.RatingVotes, bi.TotalRating, bi.FiveStarVotes, bi.FourStarVotes, bi.ThreeStarVotes, bi.TwoStarVotes, bi.OneStarVotes,
                        bi.AddedDate, bi.AddedBy, DateTime.Now, UserName, true);
                    ViewBag.updateMsg = "Update action successed!";
                    return View(model);

                }
                catch (Exception e)
                {
                    ViewBag.updateMsg = e.Message;
                }
                return View(model);
            }
            ViewBag.updateMsg = "Update action failed!";
            return View(model);
        }

        public ActionResult CreateReservationInfo(int id) // id=BizInfoId
        {
            ViewBag.createMsg = "";
            BizInfo bi = BizInfoRepository.GetBizInfoById(id);
            BizRVInfoModel brvm = new BizRVInfoModel();
            brvm.Bizinfo = bi;
            brvm.BizInfoID = id;
            return View(brvm);
        }

        [HttpPost]
        public ActionResult CreateReservationInfo(BizRVInfoModel model)
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Bizinfo = bi;
            ViewBag.createMsg = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var bizRV = BizRVInfoRepository.AddBizRVInfo(0, model.BizInfoID, model.BizPrice, model.StandingCapacity, model.SeatedCapacity,
                        model.StartTime, model.EndTime, model.DiningStyle, model.PaymentOptions, model.AcceptWalkIn == "Yes" ? true : false, model.ExecutiveChef,
                        model.Facilities, model.Events, model.Parking, model.Details, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    if (bizRV != null)
                    {
                        ViewBag.createMsg = "Create action successed!";
                    }
                    else
                    {
                        ViewBag.createMsg = "Create action failed!";
                    }
                    return View(model);

                }
                catch
                {
                    ViewBag.createMsg = "Create action failed!";
                }
                return View(model);
            }
            ViewBag.createMsg = "Create action failed!";
            return View(model);
        }

        public ActionResult ManageReservationInfo(int id) // id=BizInfoId
        {
            ViewBag.updateMsg = "";
            BizInfo bi = BizInfoRepository.GetBizInfoById(id);
            BizRVInfoModel brvm = new BizRVInfoModel();
            BizRVInfo brv = bi.GetBizRVInfo;
            brvm.Bizinfo = bi;
            brvm.BizInfoID = id;
            brvm.BizRVInfoId = brv.BizRVInfoId;
            brvm.BizPrice = brv.BizPrice;
            brvm.StandingCapacity = brv.StandingCapacity;
            brvm.SeatedCapacity = brv.SeatedCapacity;
            brvm.StartTime = brv.StartTime;
            brvm.EndTime = brv.EndTime;
            brvm.DiningStyle = brv.DiningStyle;
            brvm.PaymentOptions = brv.PaymentOptions;
            brvm.AcceptWalkIn = brv.AcceptWalkIn ? "Yes" : "No"; ;
            brvm.ExecutiveChef = brv.ExecutiveChef;
            brvm.Facilities = brv.PrivatePartyFacilities;
            brvm.Events = brv.SpecialEvents;
            brvm.Parking = brv.Parking;
            brvm.Details = brv.AdditionalDetails;
            return View(brvm);
        }
        [HttpPost]
        public ActionResult ManageReservationInfo(BizRVInfoModel model)
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Bizinfo = bi;
            BizRVInfo brv = bi.GetBizRVInfo;
            ViewBag.updateMsg = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var bizRV = BizRVInfoRepository.AddBizRVInfo(model.BizRVInfoId, model.BizInfoID, model.BizPrice, model.StandingCapacity, model.SeatedCapacity,
                        model.StartTime, model.EndTime, model.DiningStyle, model.PaymentOptions, model.AcceptWalkIn == "Yes" ? true : false, model.ExecutiveChef,
                        model.Facilities, model.Events, model.Parking, model.Details, brv.AddedDate, brv.AddedBy, DateTime.Now, UserName, true);
                    if (bizRV != null)
                    {
                        ViewBag.updateMsg = "Update action successed!";
                    }
                    else
                    {
                        ViewBag.updateMsg = "Update action failed!";
                    }
                    return View(model);

                }
                catch
                {
                    ViewBag.updateMsg = "Update action failed!";
                }
                return View(model);
            }
            ViewBag.updateMsg = "Update action failed!";
            return View(model);
        }
        public ViewResult AllOfUsers(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date"; 
            UserViewModel uvm = new UserViewModel();
            uvm.Users = UserRepository.GetAllUsers();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                uvm.Users = uvm.Users.Where(s => s.UserName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    uvm.Users = uvm.Users.OrderByDescending(s => s.UserName).ToList();
                    break;
                case "Date":
                    uvm.Users = uvm.Users.OrderBy(s => s.AddedDate).ToList();
                    break;
                case "date_desc":
                    uvm.Users = uvm.Users.OrderByDescending(s => s.AddedDate).ToList();
                    break;
                default:  // Name ascending 
                    uvm.Users = uvm.Users.OrderBy(s => s.UserName).ToList();
                    break;
            }

            int pageSize = Globals.Settings.Articles.PageSize < 5 ? 5 : Globals.Settings.Articles.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.WebHit = HttpContext.Application["WebHit"].ToString();
            return View(uvm.Users.ToPagedList(pageNumber, pageSize));


        }
        public ViewResult AllUsers(string username, string userSearch = "")
        {
            UserViewModel uvm = new UserViewModel();
            uvm.Alphabet = new string[]{
                              "A", "a", "B", "b", "C", "c", "D", "d", "E", "e",
                              "F", "f", "G", "g", "H", "h", "I", "i", "J", "j",
                              "K", "k", "L", "l", "M", "m","N", "n", "O", "o", 
                              "P", "p", "Q", "q", "R", "r",
                              "S", "s", "T", "t", "U", "u", "V", "v", "W", "w",
                              "X", "x", "Y", "y", "Z", "z", "All"
                          };
            if (string.IsNullOrEmpty(username) == false)
            {
                if (UserManager.IsInRoleAsync(username, "Admin").Result)
                {
                    uvm.Users = UserRepository.GetAllUsers();
                    ViewBag.WebHit = HttpContext.Application["WebHit"].ToString();
                    return View(uvm);
                }

                if (UserRepository.LockUser(username))
                {
                    uvm.Users = UserRepository.GetAllUsers();
                }
                else
                {
                    if (string.IsNullOrEmpty(userSearch) || userSearch == "All")
                    {
                        uvm.Users = UserRepository.GetAllUsers();
                    }
                    else
                    {
                        uvm.Users = UserRepository.GetAllUserStartWith(userSearch);
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(userSearch) || userSearch == "All")
                {
                    uvm.Users = UserRepository.GetAllUsers();
                }
                else
                {
                    uvm.Users = UserRepository.GetAllUserStartWith(userSearch);
                }
            }
            ViewBag.WebHit = HttpContext.Application["WebHit"].ToString();
            return View(uvm);
        }
        public ActionResult UserNames(string term) // have to use parameter named 'term'
        {
            List<string> mu = new List<string>();
            foreach (var x in UserRepository.GetAllUsers())
            {
                mu.Add(x.UserName);
            }
            var userNames = (from p in mu
                             where p.StartsWith(term,true,null)
                             select p).Distinct().ToArray();

            return Json(userNames, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetUser(string userSearch)
        {
            AspNetUser user = UserRepository.GetUserByUserName(userSearch);
            string un = user.UserName;
            string str = "";
            if (user!=null)
            {
                foreach (var r in UserManager.GetRolesAsync(user.Id).Result)
                {
                    str += r + ",";
                }
            }
            return Json(new
            {
                uUserName = un,
                uCreateDate = user.AddedDate.ToString(),
                uChangedDate = user.UpdatedDate.ToString(),
                uEmailConfirmed = user.EmailConfirmed.ToString(),
                uLockoutEnabled=user.LockoutEnabled.ToString(),
                uRoles = str
            });
        }
        public ActionResult DoApprove(string id)
        {
            DoApproveModel dam = new DoApproveModel();
            AspNetUser up = UserRepository.GetUserByUserId(id);
            if (UserManager.IsInRoleAsync(id, "Admin").Result == false)
            {
                if (up.Active)
                {
                    UserRepository.LockUser(id);
                }
                else
                {
                   UserRepository.UnlockUser(id);
                }
            }
            dam.User = UserRepository.GetUserByUserId(id);
            return PartialView(dam);
        }
        public ActionResult UserDetails(string username)
        {
            UserDetailModel udm = new UserDetailModel();
            udm.User = UserRepository.GetUserByUserName(username);
            udm.Userdetail=UserDetailRepository.GetUserDetailByUserId(udm.User.Id);
            udm.UserLogin = UserLoginRepository.GetUserLoginByUserId(udm.User.Id);
           // ViewBag.UserFrom = UserLoginRepository.IsExternalUser(username) == true ? "External User" : "Local User";

            ViewBag.psw = udm.Userdetail == null ? "" : udm.Userdetail.DecryptPassword;
            string str =string.Empty;
            foreach (var r in udm.User.AspNetRoles)
            {
                str = str + " " + r.Name;
            }
            ViewBag.Role = string.IsNullOrEmpty(str)? "none": str;
            return View(udm);
        }
        public ActionResult RestaurantUserDetails(int id)
        {
            RestaurantUserModel rum = new RestaurantUserModel();
            UserDetail ud= UserDetailRepository.GetUserDetailById(id);
            if (ud!=null)
            {
            rum.User = UserRepository.GetUserByUserId(ud.UserId);
            rum.UserDetailID = ud.UserDetailId;
            rum.UserID = ud.UserId;
            rum.AddressID = ud.AddressId;
            rum.ContactInfoID = ud.ContactInfoId;
            rum.FirstName = ud.FirstName;
            rum.LastName = ud.LastName;
            rum.AddressLine = ud.Address.AddressLine;
            rum.AddressLine2 = ud.Address.AddressLine2;
            rum.CrossStreet = ud.Address.CrossStreet;
            rum.City = ud.Address.City;
            rum.State = ud.Address.State;
            rum.ZipCode = ud.Address.ZipCode;
            rum.Phone = ud.ContactInfo.Phone;
            rum.Phone2 = ud.ContactInfo.Phone2;
            rum.Email = ud.ContactInfo.Email;
            ViewBag.password = ud == null ? "" : ud.DecryptPassword;
            return View(rum);
            }
            return Redirect("/Admin");
        }
        [HttpPost]
        public ActionResult RestaurantUserDetails(RestaurantUserModel rum)
        {
            rum.User = UserRepository.GetUserByUserId(rum.UserID);
            if (ModelState.IsValid)
            {
                try
                {
                    Address ar=AddressRepository.GetAddressById(rum.AddressID);
                    ar = AddressRepository.AddAddress(rum.AddressID, rum.AddressLine, rum.AddressLine2, rum.City,
                              rum.State, rum.ZipCode, rum.CrossStreet, ar.AddedDate, ar.AddedBy,
                              DateTime.Now, UserName, ar.Active);
                    ContactInfo ci = ContactInfoRepository.GetContactInfoById(rum.ContactInfoID);
                    ci = ContactInfoRepository.AddContactInfo(rum.ContactInfoID, rum.Phone, rum.Phone2, rum.Email,
                              ci.AddedDate, ci.AddedBy, DateTime.Now, UserName, ci.Active);
                    UserDetail ud = UserDetailRepository.GetUserDetailById(rum.UserDetailID);
                    ud = UserDetailRepository.InsertUserDetail(rum.UserDetailID, rum.UserID, rum.FirstName,
                              rum.LastName, rum.AddressID, rum.ContactInfoID, ud.AddedDate, ud.AddedBy,
                              DateTime.Now, UserName, ud.Active, ud.Password);
                    if (rum.User.UserHasBizInfo)
                    {
                        GeocoderLocation gl = new GeocoderLocation();
                        string str = rum.AddressLine + ", " + rum.City + ", " + rum.State + " " + rum.ZipCode;
                        gl = Geocoding.Locate(str);
                        string lat = gl == null ? "" : gl.Latitude.ToString();
                        string log = gl == null ? "" : gl.Longitude.ToString();
                        BizInfoRepository.UpdateLatLon(rum.UserID, lat, log);
                    }
                    ViewBag.result = "Update has been successfully done";
                }
                catch
                {
                    ViewBag.result = "Update has been failed";
                    return View(rum);
                }    
            }
            return View(rum);
        }
        public ActionResult ManageMenu(int id) // id=BizInfoId
        {
            BizCuisineModel bcm = new BizCuisineModel();
            bcm.BizInfoID = id;
            bcm.Bizinfo = BizInfoRepository.GetBizInfoById(id);
           bcm.Bizcuisines = BizCuisineRepository.GetBizCuisinesByBizInfoId(id);
            return View(bcm);
        }
        [HttpPost]
        public ActionResult ManageMenu(BizCuisineModel model)
        {
            ViewBag.result = "";
            model.Bizcuisines = BizCuisineRepository.GetBizCuisinesByBizInfoId(model.BizInfoID);
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            if (model.Bizcuisines.Count>=3)
            {
                ViewBag.result = "can not choose more than 3 menus.";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                if (BizCuisineRepository.GetBizCuisineByBizID_CuisineTypeID(model.BizInfoID,model.CuisineID) == null)
                {
                    BizCuisineRepository.AddBizCuisine(0, model.BizInfoID, model.CuisineID, DateTime.Now, UserName,
                              DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                else
                {
                    ViewBag.result = "The menu already exist";
                }
                return View(model);
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }

        public ActionResult DoMenu(int id) // id=BizCuisineId
        {
            BizCuisine bc = new BizCuisine();
            bc = BizCuisineRepository.GetBizCuisineById(id);
                if (bc.Active)
                {
                    BizCuisineRepository.LockBizCuisine(bc);
                }
                else
                {
                    BizCuisineRepository.UnlockBizCuisine(bc);
                }
                bc = BizCuisineRepository.GetBizCuisineById(id);
            return PartialView(bc);
        }
        public ActionResult ManageFreeItems(int id) // id=BizInfoId
        {
            FreeItemModel fim = new FreeItemModel();
            fim.BizInfoID = id;
            fim.Bizinfo = BizInfoRepository.GetBizInfoById(id);
            fim.StartDate = DateTime.Now.ToShortDateString();
            fim.EndDate = DateTime.MaxValue.ToShortDateString();
            fim.FreeItems = FreeItemCouponRepository.GetFreeItemCouponsByBizId(id);
            return View(fim);
        }
        [HttpPost]
        public ActionResult ManageFreeItems(FreeItemModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.FreeItems = FreeItemCouponRepository.GetFreeItemCouponsByBizId(model.BizInfoID); 
            if (ModelState.IsValid)
            {
                try
                {
                    FreeItemCouponRepository.AddFreeItemCoupon(0, model.BizInfoID, model.Title, model.Description, model.UnitPrice,
                                               model.OrderMinimum, DateTime.Parse(model.StartDate), DateTime.Parse(model.EndDate),
                                               DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        public ActionResult DoFreeItem(int id) // id=FreeItemId
        {
            FreeItemCoupon fic = new FreeItemCoupon();
            fic = FreeItemCouponRepository.GetFreeItemCouponById(id);
                if (fic.Active)
                {
                    FreeItemCouponRepository.LockFreeItemCoupon(fic);
                }
                else
                {
                    FreeItemCouponRepository.UnlockFreeItemCoupon(fic);
                }
                fic = FreeItemCouponRepository.GetFreeItemCouponById(id);
            return PartialView(fic);
        }
        public ActionResult EditFreeItem(int id,int bizid) // id=FreeItemId
        {
             FreeItemModel fim = new FreeItemModel();
             fim.BizInfoID = bizid;
             fim.FreeItemID = id;
             fim.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
             FreeItemCoupon fic = FreeItemCouponRepository.GetFreeItemCouponById(id);
             fim.Title = fic.Title;
             fim.Description = fic.Description;
             fim.UnitPrice = fic.UnitPrice;
             fim.OrderMinimum =(int)fic.OrderMinimum;
             fim.StartDate = fic.StartDate.ToShortDateString();
             fim.EndDate = fic.ExpirationDate.ToShortDateString();
            return View(fim);
        }
        [HttpPost]
        public ActionResult EditFreeItem(FreeItemModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            if (ModelState.IsValid)
            {
                try
                {
                    FreeItemCouponRepository.AddFreeItemCoupon(model.FreeItemID, model.BizInfoID, model.Title, model.Description, model.UnitPrice,
                                               model.OrderMinimum, DateTime.Parse(model.StartDate), DateTime.Parse(model.EndDate),
                                               DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }
        public ActionResult ManageCoupons(int id) // id=BizInfoId
        {
            DiscountModel dm = new DiscountModel();
            dm.BizInfoID = id;
            dm.Bizinfo = BizInfoRepository.GetBizInfoById(id);
            dm.StartDate = DateTime.Now.ToShortDateString();
            dm.EndDate = DateTime.MaxValue.ToShortDateString();
            dm.DiscountCoupons = DiscountCouponRepository.GetDiscountCouponsByBizId(id);
            return View(dm);
        }
        [HttpPost]
        public ActionResult ManageCoupons(DiscountModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.DiscountCoupons = DiscountCouponRepository.GetDiscountCouponsByBizId(model.BizInfoID);
            if (ModelState.IsValid)
            {
                try
                {
                    DiscountCouponRepository.AddDiscountCoupon(0, model.BizInfoID, model.Title, model.Description, model.DiscountPercentage,
                                               model.OrderMinimum, DateTime.Parse(model.StartDate), DateTime.Parse(model.EndDate),
                                               DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        public ActionResult DoDiscount(int id) // id=DiscountCouponId
        {
            DiscountCoupon dc = new DiscountCoupon();
            dc = DiscountCouponRepository.GetDiscountCouponById(id);
            if (dc.Active)
            {
                DiscountCouponRepository.LockDiscountCoupon(dc);
            }
            else
            {
                DiscountCouponRepository.UnlockDiscountCoupon(dc);
            }
            dc = DiscountCouponRepository.GetDiscountCouponById(id);
            return PartialView(dc);
        }
        public ActionResult EditDiscount(int id, int bizid) // id=DiscountCouponId
        {
            DiscountModel dm = new DiscountModel();
            dm.BizInfoID = bizid;
            dm.DiscountCouponID = id;
            dm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            DiscountCoupon dc = DiscountCouponRepository.GetDiscountCouponById(id);
            dm.Title = dc.Title;
            dm.Description = dc.Description;
            dm.DiscountPercentage = dc.DiscountPercentage;
            dm.OrderMinimum = dc.OrderMinimum;
            dm.StartDate = dc.StartDate.ToShortDateString();
            dm.EndDate = dc.ExpirationDate.ToShortDateString();
            return View(dm);
        }
        [HttpPost]
        public ActionResult EditDiscount(DiscountModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            if (ModelState.IsValid)
            {
                try
                {
                    DiscountCouponRepository.AddDiscountCoupon(model.DiscountCouponID, model.BizInfoID, model.Title, model.Description, model.DiscountPercentage,
                                               model.OrderMinimum, DateTime.Parse(model.StartDate), DateTime.Parse(model.EndDate),
                                               DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult ManageBizImages(int id) // id=BizInfoId
        {
            BizImageModel bim = new  BizImageModel();
            bim.BizInfoID = id;
            bim.Bizinfo = BizInfoRepository.GetBizInfoById(id);
            bim.BizImages = bim.Bizinfo.BizImages.ToList();
            return View(bim);
        }
        [HttpPost]
        public ActionResult ManageBizImages(BizImageModel model, HttpPostedFileBase uploadimage)
        {
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.BizImages = model.Bizinfo.BizImages.ToList();
            if (uploadimage==null)
            {
                ViewBag.result = "Please choose an image.";
                return View(model);
            }
            string imagename = "";
            imagename = UploadImage(uploadimage, model.Bizinfo.UserId, model.Bizinfo.ProductFoldPath);
            model.SmallImageName = imagename;
            model.BigImageName = imagename; 
            if (ModelState.IsValid)
            {
                try
                {
                    BizImageRepository.AddBizImage(0, model.BizInfoID, model.SmallImageName, model.BigImageName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Add action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }

        public ActionResult DoBizImage(int id) // id=BizImageId
        {
            BizImage dc = new BizImage();
            dc = BizImageRepository.GetBizImageById(id);
            if (dc.Active)
            {
                BizImageRepository.LockBizImage(dc);
            }
            else
            {
                BizImageRepository.UnlockBizImage(dc);
            }
            dc = BizImageRepository.GetBizImageById(id);
            return PartialView(dc);
        }

        public ActionResult EditBizImage(int id, int bizid) // id=BizImageId
        {
            BizImageModel bim = new  BizImageModel();
            bim.BizInfoID = bizid;
            bim.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            bim.BizImageID = id;
            bim.Bizimage = BizImageRepository.GetBizImageById(id);
            return View(bim);
        }
        [HttpPost]
        public ActionResult EditBizImage(BizImageModel model, HttpPostedFileBase uploadimage)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Bizimage = BizImageRepository.GetBizImageById(model.BizImageID);
            string imagename = "";
            imagename = uploadimage == null ? model.Bizimage.SmallImageName: UploadImage(uploadimage, model.Bizinfo.UserId, model.Bizinfo.ProductFoldPath);
            model.SmallImageName = imagename;
            model.BigImageName = imagename; 
            if (ModelState.IsValid)
            {
                try
                {
                    BizImageRepository.AddBizImage(model.BizImageID, model.BizInfoID, model.SmallImageName, model.BigImageName, model.Bizimage.Active);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Add action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }
        public ActionResult ManageCloseDays(int id) // id=BizInfoId
        {
            DayOfCloseModel dcm = new DayOfCloseModel();
            dcm.LZoneNames = ZoneNameRepository.GetAllZoneNames(true);
            dcm.BizInfoID = id;
            dcm.Bizinfo = BizInfoRepository.GetBizInfoById(id);
            dcm.CloseDays = DayOfCloseRepository.GetDayOfClosesByBizId(id);
            return View(dcm);
        }
        [HttpPost]
        public ActionResult ManageCloseDays(DayOfCloseModel model)
        {
            model.LZoneNames = ZoneNameRepository.GetAllZoneNames(true);
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.CloseDays = DayOfCloseRepository.GetDayOfClosesByBizId(model.BizInfoID); ;
            if (ModelState.IsValid)
            {
                try
                {
                    DayOfCloseRepository.AddDayOfClose(0, model.BizInfoID, model.Title,DateTime.Parse(model.CloseDay), model.ZoneNameId,
                                               DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Add action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }

        public ActionResult DoDayOfClose(int id) // id=DayOfCloseId
        {
            DayOfClose dc = new DayOfClose();
            dc = DayOfCloseRepository.GetDayOfCloseById(id);
            if (dc.Active)
            {
                DayOfCloseRepository.LockDayOfClose(dc);
            }
            else
            {
                DayOfCloseRepository.UnlockDayOfClose(dc);
            }
            dc = DayOfCloseRepository.GetDayOfCloseById(id);
            return PartialView(dc);
        }

        public ActionResult EditCloseDay(int id, int bizid) // id=DayOfCloseId
        {
            DayOfCloseModel dcm = new DayOfCloseModel();
            dcm.BizInfoID = bizid;
            dcm.DayOfCloseID = id;
            dcm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            dcm.LZoneNames = ZoneNameRepository.GetAllZoneNames(true);
            DayOfClose dc = DayOfCloseRepository.GetDayOfCloseById(id);
            dcm.Title = dc.Title;
            dcm.CloseDay = dc.CloseDay.ToShortDateString();
            dcm.ZoneNameId = dc.ZoneNameId;
            return View(dcm);
        }
        [HttpPost]
        public ActionResult EditCloseDay(DayOfCloseModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.LZoneNames = ZoneNameRepository.GetAllZoneNames(true);
            if (ModelState.IsValid)
            {
                try
                {
                    DayOfCloseRepository.AddDayOfClose(model.DayOfCloseID, model.BizInfoID, model.Title, DateTime.Parse(model.CloseDay), model.ZoneNameId,
                                               DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Edit action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }
        
        public ActionResult ManageCategories(int id) // id=BizCuisineId
        {
            CategoryModel cm = new CategoryModel();
            BizCuisine bc = BizCuisineRepository.GetBizCuisineById(id);
            cm.Bizinfo = bc.BizInfo;
            cm.BizCuisineID = id;
            cm.MenuName = bc.CuisineTypeName;
            cm.ListCategories = CategoryRepository.GetCategoriesByBizCuisineId(id);
            return View(cm);
        }
        [HttpPost]
        public ActionResult ManageCategories(CategoryModel model, HttpPostedFileBase uploadimage)
        {
            ViewBag.result = "";

            BizCuisine bc = BizCuisineRepository.GetBizCuisineById(model.BizCuisineID);
            model.Bizinfo = bc.BizInfo;
            model.MenuName = bc.CuisineTypeName;
            model.ListCategories = CategoryRepository.GetCategoriesByBizCuisineId(model.BizCuisineID);
            string imagename = "";
            imagename = uploadimage == null ? "imageSoon.png" : UploadImage(uploadimage, model.Bizinfo.UserId, model.Bizinfo.ProductFoldPath); ;
            if (ModelState.IsValid)
            {
                try
                {
                   CategoryRepository.AddCategory(0, model.BizCuisineID, model.Title, model.Description, imagename,
                                              DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Add action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        public ActionResult DoBizInfo(int id) // id=BizInfoId
        {
            BizInfo dc = new BizInfo();
            dc =BizInfoRepository.GetBizInfoById(id);
            if (dc.Active)
            {
                BizInfoRepository.LockBizInfo(dc);
            }
            else
            {
                BizInfoRepository.UnlockBizInfo(dc);
            }
            dc = BizInfoRepository.GetBizInfoById(id);
            return PartialView(dc);
        }
        public ActionResult DoCategory(int id) // id=CategoryId
        {
            Category dc = new Category();
            dc = CategoryRepository.GetCategoryById(id);
            if (dc.Active)
            {
                CategoryRepository.LockCategory(dc);
            }
            else
            {
                CategoryRepository.UnlockCategory(dc);
            }
            dc = CategoryRepository.GetCategoryById(id);
            return PartialView(dc);
        }
        public ActionResult EditCategory(int id, int bizid) // id=CategoryId
        {
            CategoryModel cm = new CategoryModel();
            cm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            cm.CategoryID = id;
            Category c=CategoryRepository.GetCategoryById(id);
            cm.BizCuisineID = c.BizCuisineId;
            cm.MenuName = c.CuisineTypeName;
            cm.Title = c.Title;
            cm.Description = c.Description;
            cm.ImageUrl = c.ImageUrl;
            return View(cm);
        }
        [HttpPost]
        public ActionResult EditCategory(CategoryModel model, HttpPostedFileBase uploadimage)
        {
            ViewBag.result = "";
            Category c=CategoryRepository.GetCategoryById(model.CategoryID);
            model.Bizinfo = BizInfoRepository.GetBizInfoById(c.BizId);
            model.MenuName = c.CuisineTypeName;
            string imagename = "";
            imagename = uploadimage == null ? "imageSoon.png" : UploadImage(uploadimage, model.Bizinfo.UserId, model.Bizinfo.ProductFoldPath);
            model.ImageUrl = imagename;
            if (ModelState.IsValid)
            {
                try
                {
                    CategoryRepository.AddCategory(model.CategoryID, model.BizCuisineID, model.Title, model.Description, imagename,
                                              c.AddedDate, c.AddedBy, DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Edit action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult ManageProducts(int id,int bizid) // id=CategoryId
        {
            ProductModel pm = new ProductModel();
            pm.BizInfoID = bizid;
            pm.CategoryID = id;
            Category c = CategoryRepository.GetCategoryById(id);
            pm.BizCuisineID = c.BizCuisineId;
            pm.LProducts = c.Products.ToList();
            pm.Bizinfo = BizInfoRepository.GetBizInfoById(c.BizId);
            pm.LCookMethods = CookMethodRepository.GetAllCookMethods(true);
            pm.LFoodTypes = FoodTypeRepository.GetAllFoodTypes(true);
            pm.LMealSections = MealSectionRepository.GetAllMealSections(true);

            return View(pm);
        }
        [HttpPost]
        public ActionResult ManageProducts(ProductModel model, HttpPostedFileBase uploadimage)
        {
            ViewBag.result = ""; 
            Category c = CategoryRepository.GetCategoryById(model.CategoryID);
            model.LProducts = c.Products.ToList();
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.LCookMethods = CookMethodRepository.GetAllCookMethods(true);
            model.LFoodTypes = FoodTypeRepository.GetAllFoodTypes(true);
            model.LMealSections = MealSectionRepository.GetAllMealSections(true);
            string imagename = "";
            imagename = uploadimage == null ? "imageSoon.png" : UploadImage(uploadimage, model.Bizinfo.BizInfoId.ToString(), model.Bizinfo.ProductFoldPath);
            model.SmallImage = imagename;
            model.BigImage = imagename; 
            if (ModelState.IsValid)
            {
                decimal up = model.UnitPrice;
                if (model.UnitPrice<=0.00m)
                {
                    up = model.BizPrice + base.ProductIncreasement;
                }
                try
                {
                    ProductRepository.AddProduct(0, model.CategoryID, model.CookMethodID, model.FoodTypeID, model.MealSectionID,
                              model.Title, model.Description, up, model.BizPrice, model.DiscountPercentage, model.SmallImage,
                               model.BigImage,model.MaxNumOfFreeTopping, model.IsSpicy == "Yes" ? true : false, model.IsVegetarian == "Yes" ? true : false, model.IsMostPopular == "Yes" ? true : false,
                               model.IsFamilyDinner == "Yes" ? true : false, DateTime.Now, UserName, DateTime.Now,UserName,true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Add action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }


        public ActionResult EditProduct(int id, int bizid) // id=ProductId
        {
            ProductModel pm = new ProductModel();
            pm.BizInfoID = bizid;
            Product p = ProductRepository.GetProductById(id);
            pm.Product = p;
            pm.ProductId = p.ProductId;
            pm.CategoryID = p.CategoryId;
            pm.BizCuisineID = p.Category.BizCuisineId;
            pm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            pm.LCookMethods = CookMethodRepository.GetAllCookMethods(true);
            pm.LFoodTypes = FoodTypeRepository.GetAllFoodTypes(true);
            pm.LMealSections = MealSectionRepository.GetAllMealSections(true);
            pm.CookMethodID = p.CookMethodId;
            pm.FoodTypeID = p.FoodTypeId;
            pm.MealSectionID = p.MealSectionId;
            pm.Title = p.Title;
            pm.Description = p.Description;
            pm.SmallImage = p.SmallImage;
            pm.UnitPrice = p.UnitPrice;
            pm.BizPrice = p.BizPrice;
            pm.MaxNumOfFreeTopping = p.MaxNumOfFreeTopping;
            pm.DiscountPercentage = pm.DiscountPercentage;
            pm.IsSpicy = p.IsSpicy == true ? "Yes" : "No";
            pm.IsVegetarian = p.IsVegetarian == true ? "Yes" : "No";
            pm.IsMostPopular = p.IsMostPopular == true ? "Yes" : "No";
            pm.IsFamilyDinner = p.IsFamilyDinner == true ? "Yes" : "No";

            return View(pm);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductModel model, HttpPostedFileBase uploadimage)
        {
            ViewBag.result = "";
            Product p = ProductRepository.GetProductById(model.ProductId);
            model.Product = p;
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.LCookMethods = CookMethodRepository.GetAllCookMethods(true);
            model.LFoodTypes = FoodTypeRepository.GetAllFoodTypes(true);
            model.LMealSections = MealSectionRepository.GetAllMealSections(true);
            string imagename = "";
            imagename = uploadimage == null ? p.SmallImage : UploadImage(uploadimage, model.Bizinfo.BizInfoId.ToString(), model.Bizinfo.ProductFoldPath);
            model.SmallImage = imagename;
            model.BigImage = imagename;
            if (ModelState.IsValid)
            {
                decimal up = model.UnitPrice;
                if (model.UnitPrice == 0.00m)
                {
                    up = model.BizPrice + base.ProductIncreasement;
                }
                try
                {
                    ProductRepository.AddProduct(model.ProductId, model.CategoryID, model.CookMethodID, model.FoodTypeID, model.MealSectionID,
                        model.Title, model.Description, up, model.BizPrice, model.DiscountPercentage, model.SmallImage, model.BigImage,
                        model.MaxNumOfFreeTopping, model.IsSpicy == "Yes" ? true : false, model.IsVegetarian == "Yes" ? true : false, model.IsMostPopular == "Yes" ? true : false,
                        model.IsFamilyDinner == "Yes" ? true : false,p.AddedDate, p.AddedBy, DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult DoProduct(int id) // id=ProductId
        {
            Product p = new Product();
            p = ProductRepository.GetProductById(id);
            if (p.Active)
            {
                ProductRepository.LockProduct(p);
            }
            else
            {
                ProductRepository.UnlockProduct(p);
            }
            p = ProductRepository.GetProductById(id);
            return PartialView(p);
        }
        public ActionResult ManageProductTopping(int id, int bizid) // id=ProductId
        {
            ProductToppingModel ptm = new ProductToppingModel();
            ptm.BizInfoID = bizid;
            ptm.ProductID = id;
            ptm.Product = ProductRepository.GetProductById(id);
            ptm.LProductToppings = ptm.Product.ProductToppings.ToList();
            ptm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            List<Topping> lt = new List<Topping>();
            lt = ToppingRepository.GetAllToppings(true);
            foreach (var t in lt)
            {
                ptm.ToppingAssistances.Add(new CheckBoxModel { BoxName = t.Title, BoxID = t.ToppingId.ToString(), Checked = false });
            }
            List<Product> lp = ProductRepository.GetProductsWithToppingByBizInfoId(bizid, true);
            foreach (var p in lp)
            {
                ptm.ToppingProducts.Add(new SelectListItem { Text = p.Title, Value = p.ProductId.ToString()});
            }
            return View(ptm);
        }
        [HttpPost]
        public ActionResult ManageProductTopping(ProductToppingModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            model.LProductToppings = model.Product.ProductToppings.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    List<CheckBoxModel> selectedToppings = new List<CheckBoxModel>();
                    selectedToppings = model.ToppingAssistances.Where(e => e.Checked == true).ToList();
                    using (FRShoppingEntities frenty =new FRShoppingEntities())
                    {
                        foreach (var t in selectedToppings)
                        {
                            if (model.Product.ExistTopping(int.Parse(t.BoxID)) == false)
                            {
                                ProductTopping pt = new ProductTopping
                                {
                                    ProductId = model.ProductID,
                                    ToppingId = int.Parse(t.BoxID),
                                    ExtraToppingPrice = model.ExtraPrice,
                                    AddedDate = DateTime.Now,
                                    AddedBy = UserName,
                                    UpdatedDate = DateTime.Now,
                                    UpdatedBy = UserName,
                                    Active = true,
                                    ToppingTitle = t.BoxName,
                                    Increment = model.Increment
                                };
                                frenty.ProductToppings.Add(pt);
                            }
                        }
                        frenty.SaveChanges();
                    }
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        [HttpPost] 
        public ActionResult CopyProductTopping(int idto, int idfrom, int bizid) // idto=ProductId, idfrom=cope from
        {
            try
            {
                Product pto = ProductRepository.GetProductById(idto);
                Product pfrom = ProductRepository.GetProductById(idfrom);
                List<ProductTopping> lpfrom = pfrom.ProductToppings.ToList();
                using (FRShoppingEntities frenty = new FRShoppingEntities())
                {
                    foreach (var t in lpfrom)
                    {
                        if (pto.ExistTopping(t.ToppingId) == false)
                        {
                            ProductTopping pt = new ProductTopping
                            {
                                ProductId = idto,
                                ToppingId = t.ToppingId,
                                ExtraToppingPrice = t.ExtraToppingPrice,
                                AddedDate = DateTime.Now,
                                AddedBy = UserName,
                                UpdatedDate = DateTime.Now,
                                UpdatedBy = UserName,
                                Active = true,
                                ToppingTitle = t.ToppingName,
                                Increment = t.Increment
                            };

                            frenty.ProductToppings.Add(pt);
                        }
                    }
                    frenty.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("ManageProductTopping", "Admin", new { id = idto, bizid = bizid });
        }
        public ActionResult DoProductTopping(int id) // id=ProductToppingId
        {
            ProductTopping p = new ProductTopping();
            p = ProductToppingRepository.GetProductToppingById(id);
            if (p.Active)
            {
                ProductToppingRepository.LockProductTopping(p);
            }
            else
            {
                ProductToppingRepository.UnlockProductTopping(p);
            }
            p = ProductToppingRepository.GetProductToppingById(id);
            return PartialView(p);
        }
        public ActionResult EditProductTopping(int id, int bizid) // id=ProductToppingId
        {
            ProductToppingModel ptm = new ProductToppingModel();
            ptm.BizInfoID = bizid;
            ptm.ProductToppingID = id;
            ptm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            ProductTopping pt = ProductToppingRepository.GetProductToppingById(id);
            ptm.ProductID = pt.ProductId;
            ptm.ToppingID = pt.ToppingId;
            ptm.ToppingTitle = pt.ToppingTitle;
            ptm.ExtraPrice=pt.ExtraToppingPrice;
            ptm.Increment = pt.Increment;
            return View(ptm);
        }
        [HttpPost]
        public ActionResult EditProductTopping(ProductToppingModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            if (ModelState.IsValid)
            {
                try
                {
                    ProductToppingRepository.AddProductTopping(model.ProductToppingID, model.ProductID, model.ToppingID,model.ExtraPrice,
                                      model.Increment, model.ToppingTitle, DateTime.Now, "Any", DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult ManageProductDressing(int id, int bizid) // id=ProductId
        {
            ProductDressingModel ptm = new ProductDressingModel();
            ptm.BizInfoID = bizid;
            ptm.ProductID = id;
            ptm.Product = ProductRepository.GetProductById(id);
            ptm.LProductDressings = ptm.Product.ProductDressings.ToList();
            ptm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            List<Dressing> lt = new List<Dressing>();
            lt = DressingRepository.GetAllDressings(true); 
            foreach (var t in lt)
            {
                ptm.DressingAssistances.Add(new CheckBoxModel { BoxName = t.Title, BoxID = t.DressingId.ToString(), Checked = false });
            }

            List<Product> lp = ProductRepository.GetProductsWithDressingByBizInfoId(bizid, true);
            foreach (var p in lp)
            {
                ptm.DressingProducts.Add(new SelectListItem { Text = p.Title, Value = p.ProductId.ToString() });
            }
            return View(ptm);
        }

        [HttpPost]
        public ActionResult ManageProductDressing(ProductDressingModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            model.LProductDressings = model.Product.ProductDressings.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    List<CheckBoxModel> selectedDressings = new List<CheckBoxModel>();
                    selectedDressings = model.DressingAssistances.Where(e => e.Checked == true).ToList();
                    using (FRShoppingEntities frenty = new FRShoppingEntities())
                    {
                        foreach (var t in selectedDressings)
                        {
                            if (model.Product.ExistDressing(int.Parse(t.BoxID)) == false)
                            {
                                ProductDressing pd = new ProductDressing
                                {
                                    ProductId = model.ProductID,
                                    DressingId = int.Parse(t.BoxID),
                                    ExtraDressingPrice = model.ExtraPrice,
                                    AddedDate = DateTime.Now,
                                    AddedBy = UserName,
                                    UpdatedDate = DateTime.Now,
                                    UpdatedBy = UserName,
                                    Active = true,
                                    DressingTitle = t.BoxName
                                };
                                frenty.ProductDressings.Add(pd);
                            }
                        }
                        frenty.SaveChanges();
                    }
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }

        [HttpPost]
        public ActionResult CopyProductDressing(int idto, int idfrom, int bizid) // idto=ProductId, idfrom=cope from
        {
            try
            {
                Product pto = ProductRepository.GetProductById(idto);
                Product pfrom = ProductRepository.GetProductById(idfrom);
                List<ProductDressing> lpfrom = pfrom.ProductDressings.ToList();
                using (FRShoppingEntities frenty = new FRShoppingEntities())
                {
                    foreach (var t in lpfrom)
                    {
                        if (pto.ExistDressing(t.DressingId) == false)
                        {
                            ProductDressing pd = new ProductDressing
                            {
                                ProductId = idto,
                                DressingId = t.DressingId,
                                ExtraDressingPrice = t.ExtraDressingPrice,
                                AddedDate = DateTime.Now,
                                AddedBy = UserName,
                                UpdatedDate = DateTime.Now,
                                UpdatedBy = UserName,
                                Active = true,
                                DressingTitle = t.DressingTitle
                            };
                            frenty.ProductDressings.Add(pd);
                        }
                    }
                    frenty.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("ManageProductDressing", "Admin", new { id = idto, bizid = bizid });
        }
        public ActionResult DoProductDressing(int id) // id=ProductDressingId
        {
            ProductDressing p = new ProductDressing();
            p = ProductDressingRepository.GetProductDressingById(id);
            if (p.Active)
            {
                ProductDressingRepository.LockProductDressing(p);
            }
            else
            {
                ProductDressingRepository.UnlockProductDressing(p);
            }
            p = ProductDressingRepository.GetProductDressingById(id);
            return PartialView(p);
        }

        public ActionResult EditProductDressing(int id, int bizid) // id=ProductDressingId
        {
            ProductDressingModel ptm = new ProductDressingModel();
            ptm.BizInfoID = bizid;
            ptm.ProductDressingID = id;
            ptm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            ProductDressing pt = ProductDressingRepository.GetProductDressingById(id);
            ptm.ProductID = pt.ProductId;
            ptm.DressingID = pt.DressingId;
            ptm.DressingTitle = pt.DressingTitle;
            ptm.ExtraPrice = pt.ExtraDressingPrice;
            return View(ptm);
        }

        [HttpPost]
        public ActionResult EditProductDressing(ProductDressingModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            if (ModelState.IsValid)
            {
                try
                {
                    ProductDressingRepository.AddProductDressing(model.ProductDressingID, model.ProductID, model.DressingID, model.ExtraPrice,
                                       model.DressingTitle, DateTime.Now, "Any", DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult ManageSize(int id, int bizid) // id=ProductId
        {
            ProductSizeModel psm = new ProductSizeModel();
            psm.BizInfoID = bizid;
            psm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            psm.ProductID = id;
            psm.Product = ProductRepository.GetProductById(id);
            psm.LProductsize = psm.Product.ProductSizes.ToList();

            List<Product> lp = ProductRepository.GetProductsWithSizeByBizInfoId(bizid, true);
            foreach (var p in lp)
            {
                psm.SizeProducts.Add(new SelectListItem { Text = p.Title, Value = p.ProductId.ToString() });
            }
            return View(psm);
        }
        [HttpPost]
        public ActionResult ManageSize(ProductSizeModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            model.LProductsize = model.Product.ProductSizes.ToList(); 
            if (model.LProductsize.Count >= 4)
            {
                ViewBag.result = "can not choose more than 4 sizes.";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (ProductSizeRepository.GetProductSizeByProductID_Title(model.ProductID, model.Title) == null)
                    {
                       ProductSizeRepository.AddProductSize(0, model.ProductID, model.Title, model.Size, model.Price, model.BizPrice,
                                                 DateTime.Now, UserName, DateTime.Now, UserName, true);
                        ViewBag.result = "Add action is successfully done";
                    }
                    else
                    {
                        ViewBag.result =model.Title +  " already exist.";
                    }
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        [HttpPost]
        public ActionResult CopyProductSize(int idto, int idfrom, int bizid) // idto=ProductId, idfrom=cope from
        {
            try
            {
                Product pto = ProductRepository.GetProductById(idto);
                Product pfrom = ProductRepository.GetProductById(idfrom);
                List<ProductSize> lpfrom = pfrom.ProductSizes.ToList();
                using (FRShoppingEntities frenty = new FRShoppingEntities())
                {
                    foreach (var t in lpfrom)
                    {
                        if (pto.ExistSizeChoice(t.Title) == false)
                        {
                            ProductSize ps =new ProductSize{ProductId= idto,Title= t.Title,Size= t.Size,Price= t.Price,
                                             AddedDate= DateTime.Now,AddedBy= UserName,UpdatedDate= DateTime.Now,UpdatedBy= UserName,Active= true,BizPrice= t.BizPrice};
                            frenty.ProductSizes.Add(ps);
                        }
                    }
                    frenty.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("ManageProductTopping", "Admin", new { id = idto, bizid = bizid });
        }
        public ActionResult DoProductSize(int id) // id=ProductSizeId
        {
            ProductSize p = new ProductSize();
            p = ProductSizeRepository.GetProductSizeById(id);
            if (p.Active)
            {
                ProductSizeRepository.LockProductSize(p);
            }
            else
            {
                ProductSizeRepository.UnlockProductSize(p);
            }
            p = ProductSizeRepository.GetProductSizeById(id);
            return PartialView(p);
        }
        public ActionResult EditProductSize(int id, int bizid) // id=ProductSizeId
        {
            ProductSizeModel psm = new ProductSizeModel();
            psm.BizInfoID = bizid;
            psm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            ProductSize ps = ProductSizeRepository.GetProductSizeById(id);
            psm.ProductID = ps.ProductId;
            psm.ProductSizeID = id;
            psm.Title = ps.Title;
            psm.Size = ps.Size;
            psm.Price = ps.Price;
            psm.BizPrice = ps.BizPrice;
            psm.Product = ProductRepository.GetProductById(psm.ProductID);
            return View(psm);
        }
        [HttpPost]
        public ActionResult EditProductSize(ProductSizeModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            if (ModelState.IsValid)
            {
                try
                {
                    ProductSizeRepository.AddProductSize(model.ProductSizeID, model.ProductID, model.Title, model.Size,
                                       model.Price, model.BizPrice, DateTime.Now, "Any", DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }
        public ActionResult ManageAddSide(int id, int bizid) // id=ProductId
        {
            AddSideModel asm = new AddSideModel();
            asm.BizInfoID = bizid;
            asm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            asm.ProductID = id;
            asm.Product = ProductRepository.GetProductById(id);
            asm.LAddSide = asm.Product.AddSides.ToList();

            return View(asm);
        }
        [HttpPost]
        public ActionResult ManageAddSide(AddSideModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            model.LAddSide = model.Product.AddSides.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                        AddSideRepository.AddAddSide(0, model.ProductID, model.Title, model.Description, model.Price, model.BizPrice,
                                                  DateTime.Now, UserName, DateTime.Now, UserName, true);
                        ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        public ActionResult DoAddSide(int id) // id=ProductSizeId
        {
            AddSide p = new AddSide();
            p = AddSideRepository.GetAddSideById(id);
            if (p.Active)
            {
                AddSideRepository.LockAddSide(p);
            }
            else
            {
                AddSideRepository.UnlockAddSide(p);
            }
            p = AddSideRepository.GetAddSideById(id);
            return PartialView(p);
        }
        public ActionResult EditAddSide(int id, int bizid) // id=AddSideId
        {
            AddSideModel asm = new AddSideModel();
            asm.BizInfoID = bizid;
            asm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            AddSide ps = AddSideRepository.GetAddSideById(id);
            asm.ProductID = ps.ProductId;
            asm.AddSideID = id;
            asm.Title = ps.Title;
            asm.Description = ps.Description;
            asm.Price = ps.Price;
            asm.BizPrice = ps.BizPrice;
            asm.Product = ProductRepository.GetProductById(asm.ProductID);
            return View(asm);
        }
        [HttpPost]
        public ActionResult EditAddSide(AddSideModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            if (ModelState.IsValid)
            {
                try
                {
                    AddSideRepository.AddAddSide(model.AddSideID, model.ProductID, model.Title, model.Description,
                                       model.Price, model.BizPrice, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
            }
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult ManageCrustChoice(int id, int bizid) // id=ProductId
        {
            CrustChoiceModel ccm = new CrustChoiceModel();
            ccm.BizInfoID = bizid;
            ccm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            ccm.ProductID = id;
            ccm.Product = ProductRepository.GetProductById(id);
            ccm.LCrustChoice = ccm.Product.CrustChoices.ToList();

            List<Product> lp = ProductRepository.GetProductsWithCrustByBizInfoId(bizid, true);
            foreach (var p in lp)
            {
                ccm.CrustProducts.Add(new SelectListItem { Text = p.Title, Value = p.ProductId.ToString() });
            }
            return View(ccm);
        }
        [HttpPost]
        public ActionResult ManageCrustChoice(CrustChoiceModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            model.LCrustChoice = model.Product.CrustChoices.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    CrustChoiceRepository.AddCrustChoice(0, model.ProductID, model.Title, model.Description, model.Price, model.BizPrice,
                                              DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        [HttpPost]
        public ActionResult CopyProductCrust(int idto, int idfrom, int bizid) // idto=ProductId, idfrom=cope from
        {
            try
            {
                Product pto = ProductRepository.GetProductById(idto);
                Product pfrom = ProductRepository.GetProductById(idfrom);
                List<CrustChoice> lcfrom = pfrom.CrustChoices.ToList();
                using (FRShoppingEntities frenty = new FRShoppingEntities())
                {
                    foreach (var t in lcfrom)
                    {
                        if (pto.ExistCrustChoice(t.Title) == false)
                        {
                            CrustChoice cc =new CrustChoice{ProductId= idto,Title= t.Title,Description= t.Description,Price= t.Price,BizPrice= t.BizPrice,
                                             AddedDate= DateTime.Now,AddedBy= UserName,UpdatedDate= DateTime.Now,UpdatedBy= UserName,Active= true};
                            frenty.CrustChoices.Add(cc);
                        }
                    }
                    frenty.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("ManageCrustChoice", "Admin", new { id = idto, bizid = bizid });
        }
        public ActionResult DoCrustChoice(int id) // id=ProductSizeId
        {
            CrustChoice p = new CrustChoice();
            p = CrustChoiceRepository.GetCrustChoiceById(id);
            if (p.Active)
            {
                CrustChoiceRepository.LockCrustChoice(p);
            }
            else
            {
                CrustChoiceRepository.UnlockCrustChoice(p);
            }
            p = CrustChoiceRepository.GetCrustChoiceById(id);
            return PartialView(p);
        }
        public ActionResult EditCrustChoice(int id, int bizid) // id=CrustChoiceId
        {
            CrustChoiceModel ccm = new CrustChoiceModel();
            ccm.BizInfoID = bizid;
            ccm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            CrustChoice ps = CrustChoiceRepository.GetCrustChoiceById(id);
            ccm.ProductID = ps.ProductId;
            ccm.CrustChoiceID = id;
            ccm.Title = ps.Title;
            ccm.Description = ps.Description;
            ccm.Price = ps.Price;
            ccm.BizPrice = ps.BizPrice;
            ccm.Product = ProductRepository.GetProductById(ccm.ProductID);
            return View(ccm);
        }
        [HttpPost]
        public ActionResult EditCrustChoice(CrustChoiceModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            if (ModelState.IsValid)
            {
                try
                {
                    CrustChoiceRepository.AddCrustChoice(model.CrustChoiceID, model.ProductID, model.Title, model.Description,
                                       model.Price, model.BizPrice, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult ManageCheeseAmount(int id, int bizid) // id=ProductId 
        {
            CheeseAmountModel ccm = new CheeseAmountModel();
            ccm.BizInfoID = bizid;
            ccm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            ccm.ProductID = id;
            ccm.Product = ProductRepository.GetProductById(id);
            ccm.LCheeseAmount = ccm.Product.CheeseAmounts.ToList();
            List<Product> lp = ProductRepository.GetProductsWithCheeseAmountByBizInfoId(bizid, true);
            foreach (var p in lp)
            {
                ccm.CheeseProducts.Add(new SelectListItem { Text = p.Title, Value = p.ProductId.ToString() });
            }

            return View(ccm);
        }
        [HttpPost]
        public ActionResult ManageCheeseAmount(CheeseAmountModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            model.LCheeseAmount = model.Product.CheeseAmounts.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    CheeseAmountRepository.AddCheeseAmount(0, model.ProductID, model.Title, model.Description, model.Price, model.BizPrice,
                                              DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        [HttpPost]
        public ActionResult CopyProductCheeseAmount(int idto, int idfrom, int bizid) // idto=ProductId, idfrom=cope from
        {
            try
            {
                Product pto = ProductRepository.GetProductById(idto);
                Product pfrom = ProductRepository.GetProductById(idfrom);
                List<CheeseAmount> lcfrom = pfrom.CheeseAmounts.ToList();
                using (FRShoppingEntities frenty = new FRShoppingEntities())
                {
                    foreach (var t in lcfrom)
                    {
                        if (pto.ExistCheese(t.Title) == false)
                        {
                            CheeseAmount ca =new CheeseAmount{ProductId= idto,Title= t.Title,Description= t.Description,Price= t.Price,BizPrice= t.BizPrice,
                                             AddedDate= DateTime.Now,AddedBy= UserName,UpdatedDate= DateTime.Now,UpdatedBy= UserName,Active= true};
                            frenty.CheeseAmounts.Add(ca);
                        }
                    }
                    frenty.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("ManageCheeseAmount", "Admin", new { id = idto, bizid = bizid });
        }
        public ActionResult DoCheeseAmount(int id) // id=ProductSizeId
        {
            CheeseAmount p = new CheeseAmount();
            p = CheeseAmountRepository.GetCheeseAmountById(id);
            if (p.Active)
            {
                CheeseAmountRepository.LockCheeseAmount(p);
            }
            else
            {
                CheeseAmountRepository.UnlockCheeseAmount(p);
            }
            p = CheeseAmountRepository.GetCheeseAmountById(id);
            return PartialView(p);
        }
        public ActionResult EditCheeseAmount(int id, int bizid) // id=CheeseAmountId
        {
            CheeseAmountModel ccm = new CheeseAmountModel();
            ccm.BizInfoID = bizid;
            ccm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            CheeseAmount ps = CheeseAmountRepository.GetCheeseAmountById(id);
            ccm.ProductID = ps.ProductId;
            ccm.CheeseAmountID = id;
            ccm.Title = ps.Title;
            ccm.Description = ps.Description;
            ccm.Price = ps.Price;
            ccm.BizPrice = ps.BizPrice;
            ccm.Product = ProductRepository.GetProductById(ccm.ProductID);
            return View(ccm);
        }
        [HttpPost]
        public ActionResult EditCheeseAmount(CheeseAmountModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            if (ModelState.IsValid)
            {
                try
                {
                    CheeseAmountRepository.AddCheeseAmount(model.CheeseAmountID, model.ProductID, model.Title, model.Description,
                                       model.Price, model.BizPrice, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }


        public ActionResult ManageFamilyMeal(int id, int bizid) // id=ProductId
        {
            FamilyMealModel fmm = new FamilyMealModel();
            fmm.BizInfoID = bizid;
            fmm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            fmm.ProductID = id;
            fmm.Product = ProductRepository.GetProductById(id);
            fmm.LFamilyMeal = fmm.Product.FamilyMeals.ToList();

            return View(fmm);
        }
        [HttpPost]
        public ActionResult ManageFamilyMeal(FamilyMealModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            model.LFamilyMeal = model.Product.FamilyMeals.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime dt = DateTime.Now;
                    FamilyMealRepository.AddFamilyMeal(0, model.ProductID, model.NumberOfPeople,model.MealToAdd, dt, UserName, dt, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        public ActionResult DoFamilyMeal(int id) // id=FamilyMealId
        {
            FamilyMeal fm = new FamilyMeal();
            fm = FamilyMealRepository.GetFamilyMealById(id);
            if (fm.Active)
            {
                FamilyMealRepository.LockFamilyMeal(fm);
            }
            else
            {
                FamilyMealRepository.UnlockFamilyMeal(fm);
            }
            fm = FamilyMealRepository.GetFamilyMealById(id);
            return PartialView(fm);
        }
        public ActionResult EditFamilyMeal(int id, int bizid) // id=FamilyMealId
        {
            FamilyMealModel fmm = new FamilyMealModel();
            fmm.BizInfoID = bizid;
            fmm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            FamilyMeal fm = FamilyMealRepository.GetFamilyMealById(id);
            fmm.ProductID = fm.ProductId;
            fmm.Product = ProductRepository.GetProductById(fm.ProductId);
            fmm.FamilyMealID = id;
            fmm.NumberOfPeople = fm.NumberOfPeople;
            fmm.MealToAdd = fm.MealToAdd;
            return View(fmm);
        }
        [HttpPost]
        public ActionResult EditFamilyMeal(FamilyMealModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            if (ModelState.IsValid)
            {
                try
                { 
                    DateTime dt = DateTime.Now;
                    FamilyMealRepository.AddFamilyMeal(model.FamilyMealID, model.ProductID, model.NumberOfPeople, model.MealToAdd, dt, UserName, dt, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult ManageSideChoice(int id, int bizid) // id=ProductId
        {
            SideChoiceModel scm = new  SideChoiceModel();
            scm.BizInfoID = bizid;
            scm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            scm.ProductID = id;
            scm.Product = ProductRepository.GetProductById(id);
            scm.LSideChoice=  scm.Product.SideChoices.ToList();

            List<Product> lp = ProductRepository.GetProductsWithSideChoiceByBizInfoId(bizid, true);
            foreach (var p in lp)
            {
                scm.SideChoiceProducts.Add(new SelectListItem { Text = p.Title, Value = p.ProductId.ToString() });
            }
            return View(scm);
        }
        [HttpPost]
        public ActionResult ManageSideChoice(SideChoiceModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            model.LSideChoice = model.Product.SideChoices.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    SideChoiceRepository.AddSideChoice(0, model.ProductID, model.Title,  DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        [HttpPost]
        public ActionResult CopySideChoices(int idto, int idfrom, int bizid) // idto=ProductId, idfrom=cope from
        {
            try
            {
                Product pto = ProductRepository.GetProductById(idto);
                Product pfrom = ProductRepository.GetProductById(idfrom);
                List<SideChoice> lpfrom = pfrom.SideChoices.ToList();
                using (FRShoppingEntities frenty = new FRShoppingEntities())
                {
                    foreach (var t in lpfrom)
                    {
                        if (pto.ExistSideChoice(t.Title) == false)
                        {
                            SideChoice pt =new SideChoice{ProductId =idto,Title= t.Title,AddedDate= DateTime.Now,AddedBy= UserName,UpdatedDate= DateTime.Now,UpdatedBy= UserName,Active= true};
                            frenty.SideChoices.Add(pt);
                        }
                    }
                    frenty.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("ManageSideChoice", "Admin", new { id = idto, bizid = bizid });
        }

        public ActionResult DoSideChoice(int id) // id=SideChoiceId
        {
            SideChoice sc = new  SideChoice();
            sc = SideChoiceRepository.GetSideChoiceById(id);
            if (sc.Active)
            {
                SideChoiceRepository.LockSideChoice(sc);
            }
            else
            {
                SideChoiceRepository.UnlockSideChoice(sc);
            }
            sc = SideChoiceRepository.GetSideChoiceById(id);
            return PartialView(sc);
        }
        public ActionResult EditSideChoice(int id, int bizid) // id=SideChoiceId
        {
            SideChoiceModel scm = new SideChoiceModel(); 
            scm.BizInfoID = bizid;
            scm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            SideChoice sc = SideChoiceRepository.GetSideChoiceById(id);
            scm.ProductID = sc.ProductId;
            scm.Product = ProductRepository.GetProductById(sc.ProductId);
            scm.SideChoiceID = id;
            scm.Title = sc.Title;
            return View(scm);
        }
        [HttpPost]
        public ActionResult EditSideChoice(SideChoiceModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            if (ModelState.IsValid)
            {
                try
                {
                    SideChoiceRepository.AddSideChoice(model.SideChoiceID, model.ProductID, model.Title,  DateTime.Now, "Any", DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult ManageSauceChoice(int id, int bizid) // id=ProductId
        {
            SauceChoiceModel scm = new SauceChoiceModel();
            scm.BizInfoID = bizid; 
            scm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            scm.ProductID = id;
            scm.Product = ProductRepository.GetProductById(id);
            scm.LSauceChoice = scm.Product.SauceChoices.ToList();
            List<Product> lp = ProductRepository.GetProductsWithSauceByBizInfoId(bizid, true);
            foreach (var p in lp)
            {
                scm.SauceProducts.Add(new SelectListItem { Text = p.Title, Value = p.ProductId.ToString() });
            }

            return View(scm);
        }
        [HttpPost]
        public ActionResult ManageSauceChoice(SauceChoiceModel model)
        {
            ViewBag.result = "";
            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            model.LSauceChoice = model.Product.SauceChoices.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    SauceChoiceRepository.AddSauceChoice(0, model.ProductID, model.Title, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch
                {

                    ViewBag.result = "Add action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        [HttpPost]
        public ActionResult CopyProductSauce(int idto, int idfrom, int bizid) // idto=ProductId, idfrom=cope from
        {
            try
            {
                Product pto = ProductRepository.GetProductById(idto);
                Product pfrom = ProductRepository.GetProductById(idfrom);
                List<SauceChoice> lcfrom = pfrom.SauceChoices.ToList();
                using (FRShoppingEntities frenty = new FRShoppingEntities())
                {
                    foreach (var t in lcfrom)
                    {
                        if (pto.ExistSauceChoice(t.Title) == false)
                        {
                            SauceChoice sc =new SauceChoice{ProductId= idto,Title= t.Title,AddedDate= DateTime.Now,AddedBy= UserName,UpdatedDate= DateTime.Now,UpdatedBy= UserName,Active =true};
                            frenty.SauceChoices.Add(sc);
                        }
                    }
                    frenty.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("ManageSauceChoice", "Admin", new { id = idto, bizid = bizid });
        }
        public ActionResult DoSauceChoice(int id) // id=SauceChoiceId
        {
            SauceChoice sc = new SauceChoice();
            sc = SauceChoiceRepository.GetSauceChoiceById(id);
            if (sc.Active)
            {
                SauceChoiceRepository.LockSauceChoice(sc);
            }
            else
            {
                SauceChoiceRepository.UnlockSauceChoice(sc);
            }
            sc = SauceChoiceRepository.GetSauceChoiceById(id);
            return PartialView(sc);
        }
        public ActionResult EditSauceChoice(int id, int bizid) // id=SauceChoiceId
        {
            SauceChoiceModel scm = new SauceChoiceModel();
            scm.BizInfoID = bizid;
            scm.Bizinfo = BizInfoRepository.GetBizInfoById(bizid);
            SauceChoice sc = SauceChoiceRepository.GetSauceChoiceById(id);
            scm.ProductID = sc.ProductId;
            scm.Product = ProductRepository.GetProductById(sc.ProductId);
            scm.SauceChoiceID = id;
            scm.Title = sc.Title;
            return View(scm);
        }
        [HttpPost]
        public ActionResult EditSauceChoice(SauceChoiceModel model)
        {
            ViewBag.result = "";

            model.Bizinfo = BizInfoRepository.GetBizInfoById(model.BizInfoID);
            model.Product = ProductRepository.GetProductById(model.ProductID);
            if (ModelState.IsValid)
            {
                try
                {
                    SauceChoiceRepository.AddSauceChoice(model.SauceChoiceID, model.ProductID, model.Title, DateTime.Now, "Any", DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch
                {
                    ViewBag.result = "Edit action failed";
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult RestaurantOrders(int id) // id=BizInfoId
        {
            DateTime df = DateTime.Now.AddDays(-7);
            DateTime dt = DateTime.Now;
            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            RestaurantOrderViewModel rovm = new RestaurantOrderViewModel();
            rovm.Bizinfo = BizInfoRepository.GetBizInfoById(id);
            rovm.Orders = OrderRepository.GetOrdersByDateRangeWithBizId(df, dt, id);
            rovm.PeriodSubtotal = rovm.Orders.Sum(e => e.SubTotal);
            rovm.PeriodBizSubtotal = rovm.Orders.Sum(e => e.BizSubTotal);
            rovm.PeriodTotal = rovm.Orders.Sum(e => e.OrderTotal);
            rovm.PeriodBizTotal = rovm.Orders.Sum(e => e.BizOrderTotal);
            rovm.PeriodOrderTax = rovm.Orders.Sum(e => e.OrderTax);
            rovm.PeriodServiceCharge = rovm.Orders.Sum(e => e.ServiceCharge);
            rovm.PeriodDriverTip = rovm.Orders.Sum(e => e.DriverTip);
            rovm.PeriodDiscountAmount = rovm.Orders.Sum(e => e.DiscountAmount);
            rovm.PeriodDeliveryCharge = rovm.Orders.Sum(e => e.DeliveryCharge);
            ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() +" (" + rovm.Orders.Count.ToString() + ")";
            return View(rovm);
        }
        [HttpPost]
        public ActionResult RestaurantOrders(string vFromDate, string vToDate, int id) // id=BizInfoId
        {
            DateTime df;
            DateTime dt;
            if (string.IsNullOrEmpty(vFromDate))
            {
                df = DateTime.Now.AddDays(-7);
            }
            else
            {
                df = DateTime.Parse(vFromDate);
            }
            if (string.IsNullOrEmpty(vToDate))
            {
                dt = DateTime.Now;
            }
            else
            {
                dt = DateTime.Parse(vToDate);
            }
            if (df > dt)
            {
                df = DateTime.Now.AddDays(-7);
                dt = DateTime.Now;
            }

            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            RestaurantOrderViewModel rovm = new RestaurantOrderViewModel();
            rovm.Bizinfo = BizInfoRepository.GetBizInfoById(id);
            rovm.Orders = OrderRepository.GetOrdersByDateRangeWithBizId(df, dt, id);
            rovm.PeriodSubtotal = rovm.Orders.Sum(e => e.SubTotal);
            rovm.PeriodBizSubtotal = rovm.Orders.Sum(e => e.BizSubTotal);
            rovm.PeriodTotal = rovm.Orders.Sum(e => e.OrderTotal);
            rovm.PeriodBizTotal = rovm.Orders.Sum(e => e.BizOrderTotal);
            rovm.PeriodOrderTax = rovm.Orders.Sum(e => e.OrderTax);
            rovm.PeriodServiceCharge = rovm.Orders.Sum(e => e.ServiceCharge);
            rovm.PeriodDriverTip = rovm.Orders.Sum(e => e.DriverTip);
            rovm.PeriodDiscountAmount = rovm.Orders.Sum(e => e.DiscountAmount);
            rovm.PeriodDeliveryCharge = rovm.Orders.Sum(e => e.DeliveryCharge);
            ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " ( " + rovm.Orders.Count.ToString() +  ")";
            return View(rovm);
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
            OrderItem oi = new  OrderItem();
            oi = OrderItemRepository.GetOrderItemById(id);
             return PartialView(oi);
        }
        public ActionResult ManageOrders()
        {
            DateTime df = DateTime.Now.AddDays(-7);
            DateTime dt = DateTime.Now;
            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            RestaurantOrderViewModel rovm = new RestaurantOrderViewModel();
            rovm.Orders = OrderRepository.GetOrdersByDateRange(df, dt);
            rovm.PeriodSubtotal = rovm.Orders.Sum(e => e.SubTotal);
            rovm.PeriodBizSubtotal = rovm.Orders.Sum(e => e.BizSubTotal);
            rovm.PeriodTotal = rovm.Orders.Sum(e => e.OrderTotal);
            rovm.PeriodBizTotal = rovm.Orders.Sum(e => e.BizOrderTotal);
            rovm.PeriodOrderTax = rovm.Orders.Sum(e => e.OrderTax);
            rovm.PeriodServiceCharge = rovm.Orders.Sum(e => e.ServiceCharge);
            rovm.PeriodDriverTip = rovm.Orders.Sum(e => e.DriverTip);
            rovm.PeriodDiscountAmount = rovm.Orders.Sum(e => e.DiscountAmount);
            rovm.PeriodDeliveryCharge = rovm.Orders.Sum(e => e.DeliveryCharge);
            ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " ( " + rovm.Orders.Count.ToString() + ")";
            return View(rovm);
        }
        [HttpPost]
        public ActionResult ManageOrders(string vFromDate, string vToDate,string vEmail, string vInvoiceNumber,string vTransactionId)
        {
            RestaurantOrderViewModel rovm = new RestaurantOrderViewModel();
            DateTime df;
            DateTime dt;
            df = string.IsNullOrEmpty(vFromDate) ? DateTime.Now.AddDays(-7) : DateTime.Parse(vFromDate);
            dt = string.IsNullOrEmpty(vToDate) ? DateTime.Now : DateTime.Parse(vToDate);
            if (df > dt)
            {
                df = DateTime.Now.AddDays(-7);
                dt = DateTime.Now;
            }

            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            if (!string.IsNullOrEmpty(vEmail))
            {
                rovm.Orders = OrderRepository.GetOrdersByEmail(vEmail);
            }
            else if (!string.IsNullOrEmpty(vInvoiceNumber))
            {
                rovm.Orders = OrderRepository.GetOrdersByInvoiceNumber(vInvoiceNumber);
            }
            else if (!string.IsNullOrEmpty(vTransactionId))
            {
                rovm.Orders = OrderRepository.GetOrdersByTransactionId(vTransactionId);
            }
            else
            {
                rovm.Orders = OrderRepository.GetOrdersByDateRange(df, dt);
            }
            if (rovm.Orders != null)
            {
                rovm.PeriodSubtotal = rovm.Orders.Sum(e => e.SubTotal);
                rovm.PeriodBizSubtotal = rovm.Orders.Sum(e => e.BizSubTotal);
                rovm.PeriodTotal = rovm.Orders.Sum(e => e.OrderTotal);
                rovm.PeriodBizTotal = rovm.Orders.Sum(e => e.BizOrderTotal);
                rovm.PeriodOrderTax = rovm.Orders.Sum(e => e.OrderTax);
                rovm.PeriodServiceCharge = rovm.Orders.Sum(e => e.ServiceCharge);
                rovm.PeriodDriverTip = rovm.Orders.Sum(e => e.DriverTip);
                rovm.PeriodDiscountAmount = rovm.Orders.Sum(e => e.DiscountAmount);
                rovm.PeriodDeliveryCharge = rovm.Orders.Sum(e => e.DeliveryCharge);
                ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " ( " + rovm.Orders.Count.ToString() + ")";
            }
            else
            {
                rovm.PeriodSubtotal =0.0m;
                rovm.PeriodBizSubtotal = 0.0m;
                rovm.PeriodTotal = 0.0m;
                rovm.PeriodBizTotal = 0.0m;
                rovm.PeriodOrderTax = 0.0m;
                rovm.PeriodServiceCharge = 0.0m;
                rovm.PeriodDriverTip = 0.0m;
                rovm.PeriodDiscountAmount = 0.0m;
                rovm.PeriodDeliveryCharge = 0.0m;
                ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " (0)";
            }
                return View(rovm);
        }
        public ActionResult ManageReservations()
        {
            double days = double.Parse(ConfigurationManager.AppSettings["rvDays"].ToString());
            DateTime df =DateTime.Now;
            DateTime dt = DateTime.Now.AddDays(days); // upcoming 7 days from today
            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            List<Reservation> lr = new  List<Reservation>();
            lr=ReservationRepository.GetReservationsByDateRange(df, dt);
            ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " ( " + lr.Count.ToString() + ")";
             return View(lr);
        }
        [HttpPost]
        public ActionResult ManageReservations(string vFromDate, string vToDate)
        {
            double days = double.Parse(ConfigurationManager.AppSettings["rvDays"].ToString());
            DateTime df;
            DateTime dt;
            df = string.IsNullOrEmpty(vFromDate) ? DateTime.Now : DateTime.Parse(vFromDate);
            dt = string.IsNullOrEmpty(vToDate) ? DateTime.Now.AddDays(days) : DateTime.Parse(vToDate);
            if (df > dt)
            {
                df = DateTime.Now.AddDays(days);
                dt = DateTime.Now;
            }

            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            List<Reservation> lr = new List<Reservation>();
            lr = ReservationRepository.GetReservationsByDateRange(df, dt);
            ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " ( " + lr.Count.ToString() + ")";
            return View(lr);
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
        public ActionResult ManageGiftCards()
        {
            DateTime df = DateTime.Now.AddDays(-15);
            DateTime dt = DateTime.Now;
            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            GiftCardModel gcm = new  GiftCardModel();
            gcm.GiftCards=GiftCardRepository.GetGiftCardsByAddedDateRange(df, dt);
            ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " ( " + gcm.GiftCards.Count.ToString() + ")";
            return View(gcm);
        }
        [HttpPost]
        public ActionResult ManageGiftCards(string vFromDate, string vToDate, string vBuyerEmail, string vRecipientEmail, string vGiftCardCode)
        {
            GiftCardModel gcm = new GiftCardModel();
            DateTime df;
            DateTime dt;
            df = string.IsNullOrEmpty(vFromDate) ? DateTime.Now.AddDays(-15) : DateTime.Parse(vFromDate);
            dt = string.IsNullOrEmpty(vToDate) ? DateTime.Now : DateTime.Parse(vToDate);
            if (df > dt)
            {
                df = DateTime.Now.AddDays(-15);
                dt = DateTime.Now;
            }

            df = DateTime.Parse(df.ToShortDateString());
            dt = DateTime.Parse(dt.AddDays(1).ToShortDateString());
            if (!string.IsNullOrEmpty(vBuyerEmail))
            {
                gcm.GiftCards = GiftCardRepository.GetAllGiftCardByUserEmail(vBuyerEmail);
            }
            else if (!string.IsNullOrEmpty(vBuyerEmail))
            {
                gcm.GiftCards = GiftCardRepository.GetAllGiftCardByRecipientEmail(vRecipientEmail);
            }
            else if (!string.IsNullOrEmpty(vGiftCardCode))
            {
                gcm.GiftCards = GiftCardRepository.GetGiftCardsByCode(vGiftCardCode);
            }
            else
            {
                gcm.GiftCards =GiftCardRepository.GetGiftCardsByAddedDateRange(df, dt);
            }
            ViewBag.time = " from " + df.ToShortDateString() + " to " + dt.AddDays(-1).ToShortDateString() + " ( " + gcm.GiftCards.Count.ToString() + ")";
            return View(gcm);
        }
        public ActionResult DoGiftCard(int id) // id=GiftCardId
        {
            GiftCard gc = new GiftCard();
            gc = GiftCardRepository.GetGiftCardById(id); ;
            if (gc.Active)
            {
                GiftCardRepository.LockGiftCard(gc);
            }
            else
            {
                GiftCardRepository.UnlockGiftCard(gc);
            }
            gc = GiftCardRepository.GetGiftCardById(id); 
            return PartialView(gc);
        }
        public ActionResult GiftCardDetails(int id) // id=GiftCardId
        {
            GiftCard gc = new  GiftCard();
            gc = GiftCardRepository.GetGiftCardById(id);
            CreditCard cc = CreditCardRepository.GetCreditCardById(gc.CreditCardId);
            ViewBag.creditcard = cc == null ? "" : cc.CreditCardType.Title + "</br>" + cc.CreditCardNumber + "</br>" + cc.ExpirationMonth + "/" + cc.ExpirationYear + "</br>" + cc.SecurityCode;
            AspNetUser up = new  AspNetUser();
            up = UserRepository.GetUserByUserId(gc.UserId);
            ViewBag.buyername = up== null ?"": up.UserName;
            ViewBag.bizname = "";
            BizInfo bi = BizInfoRepository.GetBizInfoById(gc.LastPayBizInfoId);
            ViewBag.bizname = bi == null ? "" : bi.BizTitle;
            return PartialView(gc);
        }

        public ActionResult ManageHotelTypes() 
        {
            HotelTypeModel htm = new HotelTypeModel();
            htm.ListHotelTypes = HotelTypeRepository.GetAllHotelTypes();
            return View(htm); ;
        }
        [HttpPost]
        public ActionResult ManageHotelTypes(HotelTypeModel model, HttpPostedFileBase uploadimage)
        {
            ViewBag.result = "";

            model.ListHotelTypes = HotelTypeRepository.GetAllHotelTypes();
            string imagename = "";
            if (ModelState.IsValid)
            {
                if (uploadimage == null)
                {
                    ViewBag.result = "You must upload an image.";
                    return View(model);
                }
                try
                {
                    imagename = UploadImage(uploadimage, "", "Content/HotelImages"); 
                    var ht = HotelTypeRepository.AddHotelType(0, model.Name, imagename, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    string pathString = System.IO.Path.Combine(System.IO.Path.Combine(Request.PhysicalApplicationPath, "Content/HotelImages"), "Hotel_" + ht.HotelTypeId);
                    if (!System.IO.Directory.Exists(pathString))
                    {
                        System.IO.Directory.CreateDirectory(pathString);
                    }
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Add action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }
        public ActionResult DoHotelType(int id) // id=HotelTypeId
        {
            HotelType ht = new HotelType();
            ht = HotelTypeRepository.GetHotelTypeById(id);
            if (ht.Active)
            {
                HotelTypeRepository.LockHotelType(ht);
            }
            else
            {
                HotelTypeRepository.UnlockHotelType(ht);
            }
            ht = HotelTypeRepository.GetHotelTypeById(id);
            return PartialView(ht);
        }

        public ActionResult EditHotelType(int id) // id=HotelTypeId
        {
            HotelTypeModel htm = new HotelTypeModel();
            htm.HoteTypeID = id;
            HotelType ht =HotelTypeRepository.GetHotelTypeById(id);
            htm.Name = ht.Name;
            htm.ImageUrl = ht.ImageUrl;
            return View(htm);
        }
        [HttpPost]
        public ActionResult EditHotelType(HotelTypeModel model, HttpPostedFileBase uploadimage)
        {
            ViewBag.result = "";
            HotelType ht = HotelTypeRepository.GetHotelTypeById(model.HoteTypeID);
            string imagename = "";
            imagename = uploadimage == null ? model.ImageUrl : UploadImage(uploadimage, "", "Content/HotelImages");
            model.ImageUrl = imagename;
            if (ModelState.IsValid)
            {
                try
                {
                    HotelTypeRepository.AddHotelType(model.HoteTypeID, model.Name, imagename, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Edit action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Edit action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Edit action failed";
            return View(model);
        }

        public ActionResult ManageHotels(int id) // id=HotelTypeId
        {
            HotelsModel hm = new HotelsModel();
            hm.HoteTypeID = id;
            hm.ListHotels = HotelTypeRepository.GetHotelTypeById(id).Hotels.ToList();
            return View(hm); ;
        }
        [HttpPost]
        public ActionResult ManageHotels(HotelsModel model, HttpPostedFileBase uploadimage)
        {
            ViewBag.result = "";

            model.ListHotels = HotelTypeRepository.GetHotelTypeById(model.HoteTypeID).Hotels.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    GeocoderLocation gl = new GeocoderLocation();
                    gl = Geocoding.Locate(model.AddressLine + ", " + model.City + ", " + model.State + " " + model.ZipCode);
                    string lat = gl == null ? "" : gl.Latitude.ToString();
                    string log = gl == null ? "" : gl.Longitude.ToString();
                    string imagename = "";
                    imagename = uploadimage == null ? "imageSoon.png" : UploadImage(uploadimage, "", "Content/HotelImages/Hotel_" + model.HoteTypeID);
                    HotelRepository.AddHotel(0,model.HoteTypeID, model.Name,model.AddressLine,model.City,model.State,model.ZipCode, imagename,
                                            lat,log,model.Description, DateTime.Now, UserName, DateTime.Now, UserName, true);
                    ViewBag.result = "Add action is successfully done";
                    return View(model);
                }
                catch (Exception e)
                {

                    ViewBag.result = "Add action failed: " + e.Message;
                    return View(model);
                }
            }
            ViewBag.result = "Add action failed";
            return View(model);
        }

    }
}
