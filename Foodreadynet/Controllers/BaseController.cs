using System.Web;
using System.Web.Mvc;
using System.Linq;
using FR.Infrastructure.Config;
using System.Web.Configuration;
using FR.Domain.Model.Entities;
//using WebMatrix.WebData;
using FR.Infrastructure.Helpers;
using System.Collections.Generic;
using IdentitySample.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace FoodReady.WebUI.Controllers
{
    public class BaseController : Controller
    {
        internal const string KEY_CURRENTUSER = "Current Logged In User";
        internal const string KEY_CURRENTORDER = "Current Order";
        internal const string KEY_CURRENTCREDITCARD = "Current CreditCard";
        internal const string KEY_RETURNURL = "Return Url";
        internal const string KEY_SHAREDINFO = "Shared_Info";
        internal const string KEY_CHECKOUTSHAREDCART = "Checkout_Shared_Cart";
        internal const string KEY_COMBINEDCART = "Combined_Cart";
        public static readonly FoodReadySection Settings =
            (FoodReadySection)WebConfigurationManager.GetSection("FRWebSettings");

        public BaseController()
        {
        }
        public BaseController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        public SharedShoppingCart GetGroupShoppingCart(string cartkey)
        {
            SharedShoppingCart sc;
            if (GroupCart.GroupCarts.TryGetValue(cartkey, out sc))
            {
                return sc;
            }
            return null;
        }
        public void SetGroupShoppingCart(string cartkey, SharedShoppingCart cart)
        {
            GroupCart.GroupCarts[cartkey] = cart;
        }
        public SharedShoppingCart GetSharedCart(string cartkey)
        {
            if (HttpContext.Application[cartkey] == null)
            {
                SharedShoppingCart ssc = new SharedShoppingCart();
                ssc.SharedCartKey = cartkey;
                HttpContext.Application.Lock();
                HttpContext.Application[cartkey] = ssc;
                HttpContext.Application.UnLock();
            }
            return (SharedShoppingCart)HttpContext.Application[cartkey];
        }
        public bool CanAddGroupItemToCart(string cartkey,decimal amount)
        {
            SharedShoppingCart sc = GetGroupShoppingCart(cartkey);
            if (sc==null)
            {
                return false;
            }
            bool b = (sc.IsSharedCartLocked || (sc.PartySubTotal() + amount) > sc.MaxOrder);
            return !b;
        }
        public void SetSharedCart(string cartkey, SharedShoppingCart cart)
        {
            HttpContext.Application.Lock();
            HttpContext.Application[cartkey] = cart;
            HttpContext.Application.UnLock();
        }
        public MySharedInfo MyInfo
        {
            get
            {
                if (Session[KEY_SHAREDINFO] == null)
                {
                    Session[KEY_SHAREDINFO] = new MySharedInfo();
                }
                return (MySharedInfo)Session[KEY_SHAREDINFO];
            }

            set
            {
                if (value == null)
                {
                    Session.Remove(KEY_SHAREDINFO);
                }
                else
                {
                    Session[KEY_SHAREDINFO] = value;
                }
            }
        }
        public string MyLabel
        {
            get
            {
                return MyInfo.MyLabelName;
            }

            set
            {
                MySharedInfo msi = MyInfo;
                msi.MyLabelName = value;
                Session[KEY_SHAREDINFO] = msi;
            }
        }
        public string MySharedCartId
        {
            get
            {
                return MyInfo.SharedCartId;
            }

            set
            {
                MySharedInfo msi = MyInfo;
                msi.SharedCartId = value;
                Session[KEY_SHAREDINFO] = msi;
            }
        }
        public string MyBossName
        {
            get
            {
                return MyInfo.BossName;
            }

            set
            {
                MySharedInfo msi = MyInfo;
                msi.BossName = value;
                Session[KEY_SHAREDINFO] = msi;
            }
        }
        public SharedShoppingCart CheckoutSharedCart
        {
            get
            {
                try
                {
                    return (SharedShoppingCart)(Session[KEY_CHECKOUTSHAREDCART]);
                }
                catch
                {
                    return (null);  // for design time
                }
            }

            set
            {
                if (value == null)
                {
                    Session.Remove(KEY_CHECKOUTSHAREDCART);
                }
                else
                {
                    Session[KEY_CHECKOUTSHAREDCART] = value;
                }
            }
        }
        public ShoppingCart CombinedCart
        {
            get
            {
                try
                {
                    return (ShoppingCart)(Session[KEY_COMBINEDCART]);
                }
                catch
                {
                    return (null);  // for design time
                }
            }

            set
            {
                if (value == null)
                {
                    Session.Remove(KEY_COMBINEDCART);
                }
                else
                {
                    Session[KEY_COMBINEDCART] = value;
                }
            }
        }
        public string[] GetAddressCityState(string fullAddress)
        {
            string[] s = new string[5] { "", "", "", "", "" };
            if (string.IsNullOrEmpty(fullAddress) == false)
            {
                try
                {
                    string[] t = new string[10] { "", "", "", "", "", "", "", "", "", "" };
                    t = fullAddress.Split(',');
                    for (var n = 0; n < t.Length; n++)
                    {
                        s[n] = string.IsNullOrEmpty(t[n]) ? "" : t[n].Trim();
                    }
                }
                catch
                {
                    return s;
                }
            }
            return s;
        }
        public UserDetail CurrentEndUser
        {
            get
            {
                try
                {
                    return (UserDetail)(Session[KEY_CURRENTUSER]);
                }
                catch
                {
                    return (null);  // for design time
                }
            }

            set
            {
                if (value == null)
                {
                    Session.Remove(KEY_CURRENTUSER);
                }
                else
                {
                    Session[KEY_CURRENTUSER] = value;
                }
            }
        }
        public decimal ProductIncreasement
        {
            get
            {
                return decimal.Parse(WebConfigurationManager.AppSettings["productIncreasement"].ToString());
            }
        }
        public decimal ServiceCharge
        {
            get
            {
                return decimal.Parse(WebConfigurationManager.AppSettings["serviceCharge"].ToString());
            }
        }
        public decimal DefaultTaxRate
        {
            get { return decimal.Parse(WebConfigurationManager.AppSettings["defaultTaxRate"].ToString()); }
        }
        public decimal RewardRate
        {
            get { return decimal.Parse(WebConfigurationManager.AppSettings["rewardRate"].ToString()); }
        }
        public int DollarToPoints
        {
            get { return int.Parse(WebConfigurationManager.AppSettings["dollarToPoints"].ToString()); }
        }
        public Order CurrentOrder
        {
            get
            {
                try
                {
                    return (Order)(Session[KEY_CURRENTORDER]);
                }
                catch
                {
                    return (null);  // for design time
                }
            }

            set
            {
                if (value == null)
                {
                    Session.Remove(KEY_CURRENTORDER);
                }
                else
                {
                    Session[KEY_CURRENTORDER] = value;
                }
            }
        }

        public CreditCard CurrentCreditCard
        {
            get
            {
                try
                {
                    return (CreditCard)(Session[KEY_CURRENTCREDITCARD]);
                }
                catch
                {
                    return (null);  // for design time
                }
            }

            set
            {
                if (value == null)
                {
                    Session.Remove(KEY_CURRENTCREDITCARD);
                }
                else
                {
                    Session[KEY_CURRENTCREDITCARD] = value;
                }
            }
        }

        public string ReturnUrl
        {
            get
            {
                try
                {
                    return (string)(Session[KEY_RETURNURL]);
                }
                catch
                {
                    return "/";  // for design time
                }
            }

            set
            {
                if (value == null)
                {
                    Session.Remove(KEY_RETURNURL);
                }
                else
                {
                    Session[KEY_RETURNURL] = value;
                }
            }
        }
        public string BaseUrl
        {
            get
            {
                string url = Request.ApplicationPath;
                if (url.EndsWith("/"))
                {
                    return url;
                }
                return (url + "/");
            }
        }
        /*
        public string GetResourceObject(string name)
        {
            return Resources.SiteResource.ResourceManager.GetString(name, (CultureInfo)Session["CurrentLanguage"]);

        }
        
         */
        public string CurrentUserID
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return User.Identity.GetUserId();
                }
                return string.Empty;
            }
        }
        /*
        public string CurrentUserID
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    ApplicationUser u= UserManager.FindByNameAsync(User.Identity.Name).Result;
                    return u == null ? string.Empty : u.Id;
                }
                return string.Empty;
            }
        }
         */
        public int CurrentEndUserID
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    using (FRShoppingEntities fe = new FRShoppingEntities())
                    {
                        UserDetail eu = fe.UserDetails.Where(e => e.AspNetUser.UserName == UserName).FirstOrDefault();
                        return eu == null ? 0 : eu.UserDetailId;
                    }
                }

                return 0;
            }
        }
        
        public string FullBaseUrl
        {
            get
            {
                return (Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "") + BaseUrl);
            }
        }
        public string ServicePhone
        {
            get
            {
                return WebConfigurationManager.AppSettings["ServicePhone"].ToString();
            }
        }
        public string ServiceFax
        {
            get
            {
                return WebConfigurationManager.AppSettings["ServiceFax"].ToString();
            }
        }
        public bool IsAdmin
        {
            get
            {
                return (User.Identity.IsAuthenticated & User.IsInRole("Admin"));
            }
        }
        public string CurrentUserPassword
        {
            get;
            set;
        }
        public string CurrentUserLogonName
        {
            get;
            set;
        }
        public string UserName
        {
            get
            {
                return HttpContext.User.Identity.Name;
            }
        }
        public string GetUserNameByUserId(string vUserId)
        {
            var usr = UserManager.FindById(vUserId);
            return usr == null ? string.Empty : usr.UserName;
        }
        public static string ToUSD(string m)
        {
            return "$" + m;
        }
        public static string GoogleMapLink(BizInfo vBizInfo)
        {
            if (vBizInfo == null)
            {
                return string.Empty;
            }
            string s = vBizInfo.BizAddressString.Trim();
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ");
            s = s.Replace(" ", "+");
            return @"https://maps.google.com/maps?saddr=Your+Address&daddr=" + s + ",+USA&hl=en&ll=" + vBizInfo.Latitude + "," + vBizInfo.Longitude;
        }
        public string UrlBaseSSL
        {
            get { return Request.Url.AbsoluteUri.Replace(@"http://", @"https://"); }
        }
        public string CurrentUserIP
        {
            get
            {
                return Request.UserHostAddress;
            }
        }
        public static string EncodeString(string str)
        {
            return str.Replace(" ", "-");
        }
        public static string DecodeString(string str)
        {
            return str.Replace("-", " ");
        }
        public static void HandleCart(ShoppingCart cart)
        {

            if (cart == null)
            {
                ControllerContext cc = new ControllerContext();
                cart = new ShoppingCart();
                cc.HttpContext.Session["ShoppingCart"] = cart;
                cc.HttpContext.Response.Redirect("~/RedirectPage.htm");
            }
        }
        public static string DecryptPassword(string encryptPsw)
        {
            string result = string.Empty;
            try
            {
                if ((string.IsNullOrEmpty(encryptPsw) == false))
                {
                    result = CryptionClass.Decrypt(encryptPsw);
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }
        public  string GetCartItems(ShoppingCart cart)
        {
            return cart == null ? string.Empty : (cart.TotalItems == 0 ? string.Empty : cart.TotalItems.ToString());
        }
        public static Dictionary<int, ShoppingCart> GetCheckoutGroupCart(SharedShoppingCart GroupShoppingCart, ShoppingCart bosscart) //int=bizId
        {
            Dictionary<int, ShoppingCart> groupcart = new Dictionary<int, ShoppingCart>();
            foreach (var k in GroupShoppingCart.PartyCart.Keys)
            {
                ShoppingCart sc = new ShoppingCart();
                sc=GroupShoppingCart.PartyCart[k];
                if (groupcart.ContainsKey(sc.BizId) == false)
                {
                    ShoppingCart cart = CopyShoppingCart(sc,bosscart);
                    groupcart.Add(sc.BizId, cart);
                }
                else
                {
                    groupcart[sc.BizId] = UnionShoppingCartItems(groupcart[sc.BizId],sc);
                }
            }
            foreach (var j in groupcart.Keys)
            {
                if (GroupShoppingCart.PartySubTotal() > 0)
                {
                    groupcart[j].DriverTip = GroupShoppingCart.PartyDriverTip() * (groupcart[j].SubTotal() / GroupShoppingCart.PartySubTotal());
                    groupcart[j].TaxRate = GroupShoppingCart.PartyTaxRate;
                    groupcart[j].serviceCharge = GroupShoppingCart.PartyServiceCharge / groupcart.Keys.Count;
                }
            }
            return groupcart;
        }
        public static SharedShoppingCart GetCopyOfSharedShoppingCart(SharedShoppingCart fromSharedCart)
        {
            SharedShoppingCart ssc = new SharedShoppingCart();
            Dictionary<string, ShoppingCart> partycart = new Dictionary<string, ShoppingCart>();
            foreach (var k in fromSharedCart.PartyCart.Keys)
            {
                ShoppingCart fromcart = fromSharedCart.PartyCart[k];
                ShoppingCart cart = new ShoppingCart();
                foreach (var ci in fromSharedCart.PartyCart[k].Items)
                {
                    ShoppingCartItem sci = new ShoppingCartItem();
                    sci.LabelName = k;
                    sci.ItemId = ci.ItemId;
                    sci.ID = ci.ID;
                    sci.Quantity = ci.Quantity;
                    sci.ProductSize = ci.ProductSize;
                    sci.IsReorderItem = ci.IsReorderItem;
                    sci.ReExtraPriceTotal = ci.ReExtraPriceTotal;
                    sci.ReAddSideTotal = ci.ReAddSideTotal;
                    sci.ReBizAddSideTotal = ci.ReBizAddSideTotal;
                    sci.ReItemTotal = ci.ReItemTotal;
                    sci.ReSelectedToppings = ci.ReSelectedToppings;
                    sci.ReSelectedAddSides = ci.ReSelectedAddSides;
                    sci.Title = ci.Title;
                    sci.BizSizePrice = ci.BizSizePrice;
                    sci.BizUnitPrice = ci.BizUnitPrice;
                    sci.BizFinalPrice = ci.BizFinalPrice;
                    sci.UnitPrice = ci.UnitPrice;
                    sci.FinalPrice = ci.FinalPrice;
                    sci.CrustChoice = ci.CrustChoice;
                    sci.CrustChoicePrice = ci.CrustChoicePrice;
                    sci.CrustChoiceBizPrice = ci.CrustChoiceBizPrice;
                    sci.CheeseChoice = ci.CheeseChoice;
                    sci.CheeseChoicePrice = ci.CheeseChoicePrice;
                    sci.CheeseChoiceBizPrice = ci.CheeseChoiceBizPrice;
                    sci.DiscountPercentage = ci.DiscountPercentage;
                    sci.ProductImg = ci.ProductImg;
                    sci.Instruction = ci.Instruction;
                    sci.IsSpicy = ci.IsSpicy;
                    sci.HowSpicy = ci.HowSpicy;
                    sci.IsFamilyMeal = ci.IsFamilyMeal;
                    sci.SelectedFreeToppings = ci.SelectedFreeToppings;
                    sci.SideChoice = ci.SideChoice;
                    sci.SauceChoice = ci.SauceChoice;
                    sci.DressingChoice = ci.DressingChoice;
                    sci.ProductSizeTitle = ci.ProductSizeTitle;
                    sci.ProductSizePrice = ci.ProductSizePrice;
                    sci.ToppingList = ci.ToppingList;
                    sci.DressingList = ci.DressingList;
                    sci.AddSideList = ci.AddSideList;
                    cart.Items.Add(sci);
                }
                cart.PersonName = fromcart.PersonName;
                cart.CartKey = fromcart.CartKey;
                cart.BossName = fromcart.BossName;
                cart.ItemNum = fromcart.ItemNum;
                cart.BizId = fromcart.BizId;
                cart.IsFinishedShareOrdering = fromcart.IsFinishedShareOrdering;
                cart.ScheduleDate = fromcart.ScheduleDate;
                cart.ScheduleTime = fromcart.ScheduleTime;
                cart.IsBizDelivery = fromcart.IsBizDelivery;
                cart.IsDelivery = fromcart.IsDelivery;
                cart.FreeCoupon = fromcart.FreeCoupon;
                cart.FreeItem = fromcart.FreeItem;
                cart.DiscountCoupon = fromcart.DiscountCoupon;
                cart.DiscountPercentage = fromcart.DiscountPercentage;
                cart.CouponChoice = fromcart.CouponChoice;
                cart.CurrentDiscountMini = fromcart.CurrentDiscountMini;
                cart.TaxRate = fromcart.TaxRate;
                cart.OrderMinimum = fromcart.OrderMinimum;
                cart.DeliveryFee = fromcart.DeliveryFee;
                cart.DriverTip = fromcart.DriverTip;
                partycart.Add(k, cart);
            }
            ssc.PartyCart = partycart;
            ssc.SharedCartKey = fromSharedCart.SharedCartKey;
            ssc.IsSharedCartLocked = fromSharedCart.IsSharedCartLocked;
            ssc.PartyBossName = fromSharedCart.PartyBossName;
            ssc.PartyItemNum = fromSharedCart.PartyItemNum;
           // ssc.BizId = fromSharedCart.BizId;
            ssc.PartyScheduleDate = fromSharedCart.PartyScheduleDate;
            ssc.PartyScheduleTime = fromSharedCart.PartyScheduleTime;
            //ssc.IsBizDelivery = fromSharedCart.IsBizDelivery;
            ssc.IsPartyDelivery = fromSharedCart.IsPartyDelivery;
            ssc.PartyFreeCoupon = fromSharedCart.PartyFreeCoupon;
            ssc.PartyFreeItem = fromSharedCart.PartyFreeItem;
            ssc.PartyDiscountCoupon = fromSharedCart.PartyDiscountCoupon;
            ssc.PartyDiscountPercentage = fromSharedCart.PartyDiscountPercentage;
            ssc.PartyCouponChoice = fromSharedCart.PartyCouponChoice;
            ssc.PartyCurrentDiscountMini = fromSharedCart.PartyCurrentDiscountMini;
           // ssc.PartyDeliveryFee = fromSharedCart.PartyDeliveryFee;
           // ssc.PartyDriverTip = fromSharedCart.PartyDriverTip;
           // ssc.PartyOrderMinimum = fromSharedCart.PartyOrderMinimum;
            ssc.PartyTaxRate = fromSharedCart.PartyTaxRate;
            return ssc;
        }

        public static ShoppingCart UnionShoppingCartItems(ShoppingCart cart1, ShoppingCart cart2)
        {
            if (cart1 != null && cart2 != null)
            {
                List<ShoppingCartItem> cartitems = new List<ShoppingCartItem>();
                cartitems = cart1.Items.Union(cart2.Items).ToList();
                cart1.Items = cartitems;
            }
            return cart1;
        }
        public static ShoppingCart CopyShoppingCart(ShoppingCart copyFromCart,ShoppingCart bossCart)
        {
            ShoppingCart cart = new ShoppingCart();
            if (copyFromCart != null)
            {
                List<ShoppingCartItem> cartitems = new List<ShoppingCartItem>();
                foreach (var ci in copyFromCart.Items)
                {
                    ShoppingCartItem sci = new ShoppingCartItem();
                    sci.LabelName = ci.LabelName;
                    sci.ItemId = ci.ItemId;
                    sci.ID = ci.ID;
                    sci.Quantity = ci.Quantity;
                    sci.ProductSize = ci.ProductSize;
                    sci.IsReorderItem = ci.IsReorderItem;
                    sci.ReExtraPriceTotal = ci.ReExtraPriceTotal;
                    sci.ReAddSideTotal = ci.ReAddSideTotal;
                    sci.ReBizAddSideTotal = ci.ReBizAddSideTotal;
                    sci.ReItemTotal = ci.ReItemTotal;
                    sci.ReSelectedToppings = ci.ReSelectedToppings;
                    sci.ReSelectedAddSides = ci.ReSelectedAddSides;
                    sci.Title = ci.Title;
                    sci.BizSizePrice = ci.BizSizePrice;
                    sci.BizUnitPrice = ci.BizUnitPrice;
                    sci.BizFinalPrice = ci.BizFinalPrice;
                    sci.UnitPrice = ci.UnitPrice;
                    sci.FinalPrice = ci.FinalPrice;
                    sci.CrustChoice = ci.CrustChoice;
                    sci.CrustChoicePrice = ci.CrustChoicePrice;
                    sci.CrustChoiceBizPrice = ci.CrustChoiceBizPrice;
                    sci.CheeseChoice = ci.CheeseChoice;
                    sci.CheeseChoicePrice = ci.CheeseChoicePrice;
                    sci.CheeseChoiceBizPrice = ci.CheeseChoiceBizPrice;
                    sci.DiscountPercentage = ci.DiscountPercentage;
                    sci.ProductImg = ci.ProductImg;
                    sci.Instruction = ci.Instruction;
                    sci.IsSpicy = ci.IsSpicy;
                    sci.HowSpicy = ci.HowSpicy;
                    sci.IsFamilyMeal = ci.IsFamilyMeal;
                    sci.SelectedFreeToppings = ci.SelectedFreeToppings;
                    sci.SideChoice = ci.SideChoice;
                    sci.SauceChoice = ci.SauceChoice;
                    sci.DressingChoice = ci.DressingChoice;
                    sci.ProductSizeTitle = ci.ProductSizeTitle;
                    sci.ProductSizePrice = ci.ProductSizePrice;
                    sci.ToppingList = ci.ToppingList;
                    sci.DressingList = ci.DressingList;
                    sci.AddSideList = ci.AddSideList;
                    cartitems.Add(sci);
                }
                cart.Items = cartitems;
                cart.PersonName = bossCart.PersonName;
                cart.CartKey = bossCart.CartKey;
                cart.BossName = copyFromCart.BossName;
                cart.ItemNum = bossCart.ItemNum;
                cart.BizId = copyFromCart.BizId;
                cart.IsFinishedShareOrdering = bossCart.IsFinishedShareOrdering;
                cart.ScheduleDate = bossCart.ScheduleDate;
                cart.ScheduleTime = bossCart.ScheduleTime;
                cart.IsBizDelivery = bossCart.IsBizDelivery;
                cart.IsDelivery = bossCart.IsDelivery;
                cart.FreeCoupon = bossCart.FreeCoupon;
                cart.FreeItem = bossCart.FreeItem;
                cart.DiscountCoupon = bossCart.DiscountCoupon;
                cart.DiscountPercentage = bossCart.DiscountPercentage;
                cart.CouponChoice = bossCart.CouponChoice;
                cart.CurrentDiscountMini = bossCart.CurrentDiscountMini;
                cart.TaxRate = bossCart.TaxRate;
                cart.OrderMinimum = bossCart.OrderMinimum;
                cart.DeliveryFee = copyFromCart.DeliveryFee;
                cart.DriverTip = bossCart.DriverTip;
            }
            return cart;
        }
    }
}