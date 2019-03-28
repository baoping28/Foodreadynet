using System.Web.Mvc;
using System.Text;
using System.Linq;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
using FR.Services.FilterServces;
using System;
using FoodReady.WebUI.HtmlHelpers;
using FoodReady.WebUI.EmailServices;
using FR.Infrastructure.Helpers;
namespace FoodReady.WebUI.Controllers
{
    //[SessionExpireFilter]
    public class RestaurantsController : BaseController
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
        private IFavorRestaurantRepository FavorRestaurantRepository;
        private IOrderRepository OrderRepository;
        private IBizImageRepository BizImageRepository;
        public RestaurantsController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo,
                                     ICuisineTypeRepository cuisineTypeRepo, ICategoryRepository categoryRepo,
                                     IProductRepository productRepo, IProductToppingRepository productToppingRepo,
                                     IProductDressingRepository productDressingRepo,IDiscountCouponRepository discountCouponRepo,
                                     IFreeItemCouponRepository freeItemCouponRepo, IFavorRestaurantRepository favorRestaurantRepo,
                                     IOrderRepository orderRepo, IBizImageRepository bizImageRepo)
        {
            ViewBag.TitleSortParam = "BizTitle";
            ViewBag.NewestSortParam = "AddedDate desc";
            ViewBag.SortParam = "BizTitle";
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
            CuisineTypeRepository = cuisineTypeRepo;
            CategoryRepository = categoryRepo;
            ProductRepository = productRepo;
            ProductToppingRepository = productToppingRepo;
            ProductDressingRepository = productDressingRepo;
            DiscountCouponRepository = discountCouponRepo;
            FreeItemCouponRepository = freeItemCouponRepo;
            FavorRestaurantRepository = favorRestaurantRepo;
            OrderRepository = orderRepo;
            BizImageRepository = bizImageRepo;
        }
        public ActionResult Index(string dlv,string street, string address, string city, string zip, string cuisine,ShoppingCart cart, BrowseHistory bh)
        {
            ViewBag.result = "y";
            string ct = string.IsNullOrEmpty(city) ? string.Empty : DecodeString(city);
            bh.City = ct;
            bh.Address = string.IsNullOrEmpty(street) ? "" : street;
            bh.AddressCityState = string.IsNullOrEmpty(address) ? "" : address;
            bh.Zip = zip;
            bh.Cuisine = string.IsNullOrEmpty(cuisine) ? string.Empty : DecodeString(cuisine);
            ViewBag.Address = bh.AddressCityState;
            ViewBag.Zipcode = bh.Zip;

            bh.IsDelivery = string.IsNullOrEmpty(dlv) ? bh.IsDelivery : (dlv == "true" ? true : false);
            ViewBag.delivery = bh.IsDelivery;
            FilterViewModels fvm = new FilterViewModels();
            fvm.cuisine = bh.Cuisine; // string.IsNullOrEmpty(cuisine) ? fvm.cuisine : cuisine;
            List<CuisineType> lct = CuisineTypeRepository.GetAllCuisineTypes(true);
            fvm.CuisineAssistances = new List<SelectListItem>();
            fvm.CuisineAssistances.Add(new SelectListItem { Text = "All Cuisines", Value = "All" });
            foreach (var c in lct)
            {
                fvm.CuisineAssistances.Add(new SelectListItem { Text = c.Title, Value = c.Title });
            }
            fvm.History = bh;
            fvm.city = ct;
            fvm.zip = zip;
            fvm.userFullAddress = bh.AddressCityState + " " + bh.Zip;
            fvm.BizInfos = BizInfoRepository.GetAllBizInfos(true)
             .Where(p => (string.IsNullOrEmpty(ct) ? true : p.Address.City.ToLower() == ct.ToLower())
              && (string.IsNullOrEmpty(zip) ? true : p.Address.ZipCode == zip)
             && (string.IsNullOrEmpty(cuisine) ? true : p.BizCuisines.Where(e => e.CuisineTypeName == bh.Cuisine && e.BizInfoId == p.BizInfoId).Count() > 0)).ToList();
            if (fvm.BizInfos==null || fvm.BizInfos.Count<=0 )
            {
                ViewBag.result = "n";
                return View(fvm);
            }
            bh.FilterSet = fvm.BizInfos;
            fvm.BizOpenSet = fvm.BizInfos.Where(e => e.IsOpenNow == true).ToList();
            fvm.BizCloseSet = fvm.BizInfos.Except(fvm.BizOpenSet).ToList();
            fvm.BizHiddenSet = new List<BizInfo>();
            bh.FilterOpenSet = fvm.BizOpenSet;
            bh.FilterCloseSet = fvm.BizCloseSet;
            bh.FilterHiddenSet = fvm.BizHiddenSet; 
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var b in fvm.BizInfos)
            {
                sb.Append("[");
                sb.Append("'" + b.BizTitle + "',");
                sb.Append("'" + b.Address.AddressLine + "',");
                sb.Append("'" + b.Address.City + "',");
                sb.Append("'" + b.Address.State + "',");
                sb.Append("'" + b.Address.ZipCode + "',");
                sb.Append("'" + b.Latitude + "',");
                sb.Append("'" + b.Longitude + "',");
                sb.Append("'" + b.BizInfoId + "',");
                sb.Append("'" + b.ImageUrl + "',");
                sb.Append("'" + "tr-" + b.BizInfoId + "'],");

            }
            if (fvm.BizInfos.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            fvm.MapMarkers = sb.ToString();
            ViewBag.bagitems = GetCartItems(cart);
            return View(fvm);
        }

        [HttpPost]
        public ActionResult FilterUpdate(string schedule, string time, string cuisine, string min, string dist, string rating, string freeDelivery, string breakfast, string lunchSpecial, string coupons, string freeItems, BrowseHistory bh)
        {
            string timeout = "timein";
            if (bh.FilterSet == null)
            {
                timeout = "timeout";
                return Json(new
                {
                    timeexp = timeout
                });
            }
            List<BizInfo> openset = new List<BizInfo>();
            List<BizInfo> closeset = new List<BizInfo>();
            List<BizInfo> hiddenset = new List<BizInfo>();
            int[] filterOn = { };
            int[] filterOff = { };
            int[] filterOpen = { };
            string[] localOn = { };
            string[] localOff = { };
            string[] localOpen = { };
            List<int> ltOn = new List<int>();
            List<int> ltOpen = new List<int>();
            List<int> ltOff = new List<int>();
            List<string> ltLocalOn = new List<string>();
            List<string> ltLocalOpen = new List<string>();
            List<string> ltLocalOff = new List<string>();
            int closeCount = 0;
            YelpReviewModel yrm = new YelpReviewModel();
            foreach (var p in bh.FilterSet)
            {
                string tm = SearchFilter.ConvertLocalToMyTime(p.BizHour.BizTimeZoneName).ToString();
                yrm = YelpBizDetails.GetYelpBiz(p);
                bool yrating= string.IsNullOrEmpty(rating)?true: int.Parse(rating) <= 0 ? true : yrm.Biz==null?p.AverageRating >= decimal.Parse(rating):yrm.Biz.rating >= double.Parse(rating);
                if (SearchFilter.ItemFilter(p, schedule, time, cuisine, min, dist, yrating, freeDelivery, breakfast, lunchSpecial, coupons, freeItems, bh))
                {
                    ltOn.Add(p.BizInfoId);
                    ltLocalOn.Add(tm);
                    if (p.IsOpenNow)
                    {
                        openset.Add(p);
                        ltOpen.Add(p.BizInfoId);
                        ltLocalOpen.Add(tm);
                    }
                    else
                    {
                        closeset.Add(p);
                        closeCount++;
                    }
                }
                else
                {
                    hiddenset.Add(p);
                    ltOff.Add(p.BizInfoId);
                    ltLocalOff.Add(tm);
                }
            }
            bh.FilterOpenSet = openset;
            bh.FilterCloseSet = closeset;
            bh.FilterHiddenSet = hiddenset;
            filterOn = ltOn.ToArray();
            filterOpen = ltOpen.ToArray();
            filterOff = ltOff.ToArray();
            localOn = ltLocalOn.ToArray();
            localOff = ltLocalOff.ToArray();
            localOpen = ltLocalOpen.ToArray();
            return Json(new
            {
                timeexp = timeout,
                featherOn = filterOn,
                featherOnCount = filterOn.Count().ToString(),
                featherCloseCount = closeCount,
                featherOpen = filterOpen,
                featherOpenCount = filterOpen.Count().ToString(),
                featherOff = filterOff,
                featherOffCount = filterOff.Count().ToString(),
                localOn = localOn,
                localOff = localOff,
                localOpen = localOpen
            });

            // return Json(new { sch = schedule, dlvtime = time, mini = min, distc = dist, rate = rating, re1 = freeDelivery, re2 = breakfast, re3 = lunchSpecial, re4 = coupons, re5 = freeItems });
        }

        public ActionResult SearchSort(string sortParam, BrowseHistory bh)
        {
            ViewBag.Address = bh.AddressCityState;
            ViewBag.Zipcode = bh.Zip;


            ViewBag.SortParam = sortParam ?? ViewBag.SortParam;
            string SortParameter = (string)ViewBag.SortParam;

            FilterViewModels fvm = new FilterViewModels();
            fvm.cuisine = bh.Cuisine; // string.IsNullOrEmpty(cuisine) ? fvm.cuisine : cuisine;
            List<CuisineType> lct = CuisineTypeRepository.GetAllCuisineTypes(true);
            fvm.CuisineAssistances = new List<SelectListItem>();
            fvm.CuisineAssistances.Add(new SelectListItem { Text = "All Cuisines", Value = "All" });
            foreach (var c in lct)
            {
                fvm.CuisineAssistances.Add(new SelectListItem { Text = c.Title, Value = c.Title });
            }
            fvm.History = bh;
            fvm.city = bh.City;
            fvm.zip = bh.Zip;
            fvm.userFullAddress = bh.AddressCityState + " " + bh.Zip;
           // fvm.BizInfos = BizInfoRepository.GetAllBizInfos(true);
            // .Where(p => (string.IsNullOrEmpty(ct) ? true : p.Address.City.ToLower() == ct.ToLower())
            //   && (string.IsNullOrEmpty(zip) ? true : p.Address.ZipCode == zip)
            //  && (string.IsNullOrEmpty(cuisine) ? true : p.BizCuisines.Where(e=>e.CuisineTypeName==cuisine && e.BizInfoId==p.BizInfoId).Count()>0)).ToList();
            fvm.BizInfos = bh.FilterSet;



            if (SortParameter.StartsWith("BizTitle"))
            {
                if (SortParameter != "BizTitle")
                {
                    fvm.BizOpenSet = bh.FilterOpenSet.OrderBy(e => e.BizTitle).ToList();
                    fvm.BizCloseSet = bh.FilterCloseSet.OrderBy(e => e.BizTitle).ToList();
                    fvm.BizHiddenSet = bh.FilterHiddenSet.OrderBy(e => e.BizTitle).ToList();
                }
                else
                {
                    fvm.BizOpenSet = bh.FilterOpenSet.OrderByDescending(e => e.BizTitle).ToList();
                    fvm.BizCloseSet = bh.FilterCloseSet.OrderByDescending(e => e.BizTitle).ToList();
                    fvm.BizHiddenSet = bh.FilterHiddenSet.OrderByDescending(e => e.BizTitle).ToList();
                }
                ViewBag.TitleSortParam = ((string)ViewBag.SortParam == "BizTitle") ? "BizTitle desc" : "BizTitle";
            }

            if (SortParameter.StartsWith("AddedDate"))
            {
                if (SortParameter != "AddedDate")
                {
                    fvm.BizOpenSet = bh.FilterOpenSet.OrderBy(e => e.AddedDate).ToList();
                    fvm.BizCloseSet = bh.FilterCloseSet.OrderBy(e => e.AddedDate).ToList();
                    fvm.BizHiddenSet = bh.FilterHiddenSet.OrderBy(e => e.AddedDate).ToList();
                }
                else
                {
                    fvm.BizOpenSet = bh.FilterOpenSet.OrderByDescending(e => e.AddedDate).ToList();
                    fvm.BizCloseSet = bh.FilterCloseSet.OrderByDescending(e => e.AddedDate).ToList();
                    fvm.BizHiddenSet = bh.FilterHiddenSet.OrderByDescending(e => e.AddedDate).ToList();
                }
                ViewBag.NewestSortParam = ((string)ViewBag.SortParam == "AddedDate") ? "AddedDate desc" : "AddedDate";
            }
            bh.FilterOpenSet = fvm.BizOpenSet;
            bh.FilterCloseSet = fvm.BizCloseSet;
            bh.FilterHiddenSet = fvm.BizHiddenSet;
            return PartialView(fvm);
        }
        public ActionResult Menu(ShoppingCart cart, BrowseHistory bh, int id = 0, int reorderid = 0) 
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
            if (string.IsNullOrEmpty(cart.CartKey)==false && string.IsNullOrEmpty(cart.BossName)==false)
            {
                return Redirect("~/Group");
            }

            if (id == 0)
            {
                return Redirect("~/Home");
            }
            if (cart.BizId != id)
            {
                cart.Clear();
                cart.IsDelivery = bh.IsDelivery;
                cart.BizId = id;

            }
            if (reorderid>0)
            {
                cart.Clear();
                SetupReorder(reorderid, cart,bh);
            }
            MenuViewModel mvm = new MenuViewModel();
            mvm.BizInfo = BizInfoRepository.GetBizInfoById(id);
            BizInfo bi = mvm.BizInfo;
            cart.BizName = bi.BizTitle;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("[");
            sb.Append("'" + bi.BizTitle + "',");
            sb.Append("'" + bi.Address.AddressLine + "',");
            sb.Append("'" + bi.Address.City + "',");
            sb.Append("'" + bi.Address.State + "',");
            sb.Append("'" + bi.Address.ZipCode + "',");
            sb.Append("'" + bi.Latitude + "',");
            sb.Append("'" + bi.Longitude + "',");
            sb.Append("'" + bi.BizInfoId + "',");
            sb.Append("'" + bi.ImageUrl + "'],");
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            mvm.MapMarkers = sb.ToString();
            mvm.Maplink = GoogleMapLink(bi); 
            cart.IsBizDelivery = mvm.BizInfo.Delivery;
            cart.TaxRate = mvm.BizInfo.TaxPercentageRate;
            cart.OrderMinimum = mvm.BizInfo.DeliveryMinimum;
            cart.DeliveryFee = mvm.BizInfo.DeliveryFee;
            mvm.Cart = cart;
            mvm.Cart.IsDelivery = bh.IsDelivery;
            mvm.Cart.BizId = id;
            mvm.Cart.DeliveryFee = mvm.BizInfo.DeliveryFee;
            mvm.Cart.OrderMinimum = mvm.BizInfo.DeliveryMinimum;
            mvm.Cart.BizName = mvm.BizInfo.BizTitle;
            mvm.History = bh;
            string rul = HttpContext.Request.UrlReferrer==null ? "~/Home" : HttpContext.Request.UrlReferrer.PathAndQuery;
            mvm.ReturnUrl = rul;
            mvm.MenuList = BizInfoRepository.GetBizCuisinesByBizInfoId(id,true).ToList();
            mvm.FirstSubmenu = mvm.MenuList.FirstOrDefault();
            List<DiscountCoupon> ldc = new List<DiscountCoupon>();
            List<FreeItemCoupon> lfc = new List<FreeItemCoupon>();
            if (mvm.BizInfo.HasDiscountCoupons)
            {
                ldc = DiscountCouponRepository.GetBizDiscountCouponsByMinimum(id, cart.SubTotal(), true);
            }
            mvm.DiscountCouponList=ldc;
            if (mvm.BizInfo.HasFreeItemCoupons)
            {
                lfc = FreeItemCouponRepository.GetBizFreeItemCouponsByMinimum(id, cart.SubTotal(), true);
            }
            mvm.FreeItemCouponList = lfc;
            ViewBag.rurl = HttpContext.Request.UrlReferrer == null ? "~/Home" : HttpContext.Request.Url.PathAndQuery;
            mvm.ProductsWithImage = ProductRepository.GetAllProductsByBizInfoId(bi.BizInfoId, true).Where(e => string.IsNullOrEmpty(e.SmallImage) == false && e.SmallImage.StartsWith("imageSoon", true, null) == false).ToList();
            mvm.BizImages =bi.BizImages.Where(e => string.IsNullOrEmpty(e.SmallImageName) == false && e.SmallImageName.StartsWith("imageSoon", true, null) == false && e.Active==true).ToList();

            ViewBag.bagitems = GetCartItems(cart);
            return View(mvm);
        }
        [NonAction]
        public void SetupReorder(int vReorderid, ShoppingCart vCart, BrowseHistory vBh)
        {
            Order od = OrderRepository.GetOrderById(vReorderid);
            if(od==null)
            {
                return;
            }
            vBh.AddressCityState = od.Street + ", " + od.City + ", " + od.State;
            vBh.Address = od.Street;
            vBh.City = od.City;
            vBh.State = od.State;
            vBh.Zip = od.ZipCode;
            vBh.IsDelivery = od.IsDelivery;

            foreach (var oi in od.OrderItems) //check if price has been changed
            {
                if (oi.FinalSalePrice < oi.Product.FinalUnitPrice)
                {
                    return;
                }
            }

            vCart.CouponChoice = od.CouponChoice;
            int discountpercent = 0;
            if (int.TryParse(vCart.CouponChoice, out discountpercent))
            {
                DiscountCoupon dc = DiscountCouponRepository.GetDiscountCouponsByBizIdPercent(od.BizInfoId, discountpercent, true);
                if (dc == null) // discount coupon expired
                {
                    vCart.CouponChoice = "";
                    return;
                }
                vCart.CouponChoice = vCart.CouponChoice;// DiscountPercentage
                vCart.CurrentDiscountMini = dc == null ? 0.0m : dc.OrderMinimum;//discount order Minimum
                vCart.DiscountPercentage = discountpercent;
                vCart.FreeItem = "";
                vCart.DiscountCoupon = true;
                vCart.FreeCoupon = false;
            }
            else if (od.CouponChoice == "" || od.CouponChoice == "Apply a coupon? No, Thanks")
            {
                vCart.CouponChoice = "";
                vCart.CurrentDiscountMini = 999999.0m;//discount order Minimum
                vCart.DiscountPercentage = 0;
                vCart.FreeItem = "";
                vCart.DiscountCoupon = false;
                vCart.FreeCoupon = false;
            }
            else
            {
                vCart.CurrentDiscountMini = 999999.0m;//discount order Minimum
                vCart.DiscountPercentage = 0;
                vCart.FreeItem = vCart.CouponChoice;
                vCart.DiscountCoupon = false;
                vCart.FreeCoupon = true;
            }
           
            vCart.DriverTip = od.DriverTip;
            foreach (var oi in od.OrderItems)
            {
               vCart.InsertItem(oi.ProductId, oi.Quantity,"",true,oi.ExtraListTotal,oi.AddSideListPrice,oi.BizAddSideListPrice, oi.ItemTotal,oi.SelectedToppings,oi.SelectedAddSides, oi.Title, oi.BizUnitPrice, oi.BizFinalPrice, oi.UnitPrice, oi.FinalSalePrice,oi.DiscountPercentage,
              oi.Product.SmallImage, oi.Instruction, oi.Product.IsSpicy, oi.HowSpicy, oi.IsFamilyMeal, oi.SideChoice,oi.SauceChoice, oi.ProductSizeTitle, oi.ProductSize, oi.ProductSizePrice,
              oi.BizSizePrice,oi.SelectedFreeToppings, new List<CheckBoxViewModel>(), new List<CheckBoxViewModel>(), new List<AddSideCheckBoxModel>(), oi.DressingChoice, oi.CrustChoice, oi.CrustChoicePrice, oi.CrustChoiceBizPrice, oi.CheeseAmount, oi.cheeseAmountPrice, oi.CheeseAmountBizPrice);
           
            }
        }
        [Authorize]
        public ActionResult AddRestaurant(int bizid,string returnUrl)
        {
            FavorRestaurantRepository.AddFavorRestaurant(0, CurrentUserID, bizid, DateTime.Now, UserName, DateTime.Now, UserName, true);
            return Redirect(returnUrl);
        }

        public ActionResult MenuSection(int id, ShoppingCart cart, string sortParam = "")
        {
            SubMenuViewModel smvm = new SubMenuViewModel();
            smvm.CategoryList = CategoryRepository.GetCategoriesByBizCuisineId(id, true).Where(e => e.Title.ToLower() != "lunch special").ToList();
            BizInfo cg = BizCuisineRepository.GetBizCuisineById(id).BizInfo;
            smvm.MenuList = BizCuisineRepository.GetBizCuisinesByBizInfoId(true, cg.BizInfoId);
            smvm.sortParameter = sortParam;
            smvm.CurrentBizCuisineId = id;
            return PartialView(smvm);
        }

        public ActionResult SubMenu(int id, ShoppingCart cart, string sortParam = "")
        {
            SubMenuViewModel smvm = new SubMenuViewModel();
            smvm.CategoryList = CategoryRepository.GetCategoriesByBizCuisineId(id, true).Where(e=>e.Title.ToLower()!="lunch special").ToList();
            BizInfo cg = BizCuisineRepository.GetBizCuisineById(id).BizInfo;
            smvm.MenuList = BizCuisineRepository.GetBizCuisinesByBizInfoId(true,cg.BizInfoId);
            smvm.sortParameter = sortParam;
            smvm.CurrentBizCuisineId = id;
            return PartialView(smvm);
        }

        public ActionResult LunchMenu(int id, ShoppingCart cart) // id=BizCuisineId
        {
            Category ctg = BizCuisineRepository.GetBizCuisineById(id).LunchSpecailCategory;
            List<Product> lp = ctg.Products.Where(e => e.CategoryId ==ctg.CategoryId && e.Active==true).ToList();
            return PartialView(lp);
        }
        public ActionResult KeyWordsList(string term) // have to use parameter named 'term'
        {
            List<string> addresskeys = new List<string>();
            List<string> cuisinekeys = new List<string>();
            List<string> productkeys = new List<string>();
            List<string> toppingkeys = new List<string>();
            List<string> dressingkeys = new List<string>();
            addresskeys = BizInfoRepository.GetBizAddressKeywords(true);
            cuisinekeys = BizCuisineRepository.GetCuisineKeywoods(true).Union(addresskeys).ToList();
            toppingkeys = ProductToppingRepository.GetToppingKeywoods(true).Union(cuisinekeys).ToList();
            dressingkeys = ProductDressingRepository.GetDressingKeywoods(true).Union(toppingkeys).ToList();
            productkeys = ProductRepository.GetProductKeywoods(true).Union(dressingkeys).ToList();
            productkeys.Distinct().ToList().Sort();
            string[] keys = productkeys.ToArray();
            return this.Json(keys.Where(t => t.ToUpper().Contains(term.ToUpper())), JsonRequestBehavior.AllowGet);

        }
        [OutputCache(Duration = 1000, VaryByParam = "keyword")]
        //[HttpPost]
        public ActionResult SearchList(string keyword, ShoppingCart cart, BrowseHistory bh)
        {
            if (bh == null)
            {
                ControllerContext cc = new ControllerContext();
                bh = new BrowseHistory();
                bh.IsDelivery = true;
                cc.HttpContext.Session["BorseHistory"] = bh;
                return Redirect("~/Home");
            }
            ViewBag.delivery = bh.IsDelivery;
            ViewBag.Keyword = keyword;
            ViewBag.Count = "0";
            SearchModel sm = new SearchModel();
            sm.History = bh;
            sm.BizInfos = BizInfoRepository.GetAllBizInfos(true)
                  .Where(b => (b.BizTitle.ToUpper().Contains(keyword.ToUpper())
                   || b.Address.AddressLine.ToUpper().Contains(keyword.ToUpper())
                   || b.Address.City.ToUpper().Contains(keyword.ToUpper())
                   || b.Address.State.ToUpper().Contains(keyword.ToUpper())
                   || b.Address.ZipCode.ToUpper().Contains(keyword.ToUpper())
                   || b.ContactInfo.Phone.ToUpper().Contains(keyword.ToUpper())
                   || b.BizAddressString.ToUpper().Contains(keyword.ToUpper())
                   || b.ContainsCuisine(keyword))).ToList();
            
            sm.Products = ProductRepository.GetAllProducts(true)
                  .Where(p => (p.Title.ToUpper().Contains(keyword.ToUpper()) || p.ContainsTopping(keyword) || p.ContainsDressing(keyword))).ToList();
            List<BizInfo> lbi = new List<BizInfo>();
            foreach (var p in sm.Products)
            {
                lbi.Add(p.Bizinfo);
            }
            lbi = lbi.Distinct().ToList();
             List<BizInfo> lresult = new List<BizInfo>();
            List<int> lid = new List<int>();
            sm.BizInfos = (sm.BizInfos.Union(lbi)).ToList();
            foreach (var b in sm.BizInfos)
            {
                if (!lid.Contains(b.BizInfoId))
                {
                    lid.Add(b.BizInfoId);
                    lresult.Add(b);
                }
            }
            sm.BizInfos = lresult;
            ViewBag.Count = sm.BizInfos.Count.ToString();
            ViewBag.bagitems = GetCartItems(cart);
            return View(sm);
        }
        public ActionResult CheckDelivery(int id, string bizname, BrowseHistory bh)
        {
            DeliveryCheckModel dcm = new DeliveryCheckModel();
            ViewBag.bizid = id.ToString();
            ViewBag.bizname = string.IsNullOrEmpty(bizname) ? "" : bizname;
            dcm.Address = string.IsNullOrEmpty(bh.AddressCityState) ? "" : bh.AddressCityState;
            dcm.ZipCode = string.IsNullOrEmpty(bh.Zip) ? "" : bh.Zip;
            dcm.BizInfoId=id;
            dcm.BizName = bizname;
            return PartialView(dcm);
        }
        [HttpPost]
        public ActionResult CheckResult(DeliveryCheckModel dcm)
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(dcm.BizInfoId);
            string approxtime = string.Empty;
            string mysAddress = dcm.Address + " " + dcm.ZipCode;
            string[] s = base.GetAddressCityState(dcm.Address);
            ViewBag.ValidateAddress = "Oops1, feel like something wrong with you input. Please check it and try again.";
            if (string.IsNullOrEmpty(dcm.Address) || string.IsNullOrEmpty(dcm.ZipCode) || dcm.ZipCode.Length != 5 || string.IsNullOrEmpty(s[0]) || string.IsNullOrEmpty(s[1]) || string.IsNullOrEmpty(s[2]))
            {
                ViewBag.ValidateAddress = "Oops2, feel like something wrong with you input. Please check it and try again.";
            }
            else
            {
                decimal st = SearchFilter.GetDistance(mysAddress, bi.BizAddressString, out approxtime);
                string xx = approxtime;
                if (st < 0.0m)
                {
                    ViewBag.ValidateAddress =st + "/Oops3, feel like something wrong with you input. Please check it and try again.";
                }
                else if (st > bi.DeliveryRadius && bi.Delivery)
                {
                    ViewBag.ValidateAddress = "Sorry, your address is beyond the area this restaurant deliveries to. Try different restaurants or you may need change your location and try again.";
                }
                else
                {

                    ViewBag.ValidateAddress = "Yes, " + dcm.BizName + " delivers to your area : " + mysAddress + ".";
                }
            }
            return PartialView(dcm);
        }
        public ActionResult CheckDeliveryOK(int id, string bizname, BrowseHistory bh)
        {
            DeliveryCheckModel dcm = new DeliveryCheckModel();
            ViewBag.bizid = id.ToString();
            ViewBag.bizname = string.IsNullOrEmpty(bizname) ? "" : bizname;
            dcm.Address = string.IsNullOrEmpty(bh.AddressCityState) ? "" : bh.AddressCityState;
            dcm.ZipCode = string.IsNullOrEmpty(bh.Zip) ? "" : bh.Zip;
            dcm.BizInfoId = id;
            dcm.BizName = bizname;
            return PartialView(dcm);
        }
        [HttpPost]
        public ActionResult CheckDeliveryOKResult(DeliveryCheckModel dcm)
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(dcm.BizInfoId);
            string approxtime = string.Empty;
            string mysAddress = dcm.Address + ", " + dcm.ZipCode;
            ViewBag.ValidateAddress = "Oops, feel like something wrong with you input. Please check it and try again.";
            if (string.IsNullOrEmpty(dcm.Address) || string.IsNullOrEmpty(dcm.ZipCode) || dcm.ZipCode.Length != 5)
            {
                ViewBag.ValidateAddress = "Oops, feel like something wrong with you input. Please check it and try again.";
            }
            else
            {
                decimal st = SearchFilter.GetDistance(mysAddress, bi.BizAddressString, out approxtime);
                string xx = approxtime;
                if (st < 0.0m)
                {
                    ViewBag.ValidateAddress = "Oops, feel like something wrong with you input. Please check it and try again.";
                }
                else if (st > bi.DeliveryRadius && bi.Delivery)
                {
                    ViewBag.ValidateAddress = "Sorry, your address is beyond the area this restaurant deliveries to. Try other restaurants or you may need change your location to try again.";
                }
                else
                {

                    ViewBag.ValidateAddress = "Yes, " + dcm.BizName + " delivers to your area : " + mysAddress + ".";
                }
            }
            return PartialView(dcm);
        }
        
        [OutputCache(Duration = 1000, VaryByParam = "id")]
        public ActionResult Coupons(int id) // id=BizInfoId
        {
            CouponViewModel cvm = new CouponViewModel();
            cvm.Bizinfo = BizInfoRepository.GetBizInfoById(id);
            cvm.Discounts = cvm.Bizinfo.DiscountCoupons.Where(e=>e.Active=true && e.IsDiscountCouponDateOK).ToList();
            cvm.FreeItems = cvm.Bizinfo.FreeItemCoupons.Where(e => e.Active = true && e.IsFreeItemCouponDateOK).ToList();
            return PartialView(cvm);
        }
        [HttpPost]
        public ActionResult loadBizImage(int bizImageId)
        {
            BizImage b = BizImageRepository.GetBizImageById(bizImageId);
            return PartialView(b);
        }
        public ActionResult ShowBizImages(int id) // id=BizImageId
        {
            BizImage b = BizImageRepository.GetBizImageById(id);
            return PartialView(b);
        }
        public ActionResult ShowProductImages(int id) // id=ProductId
        {
            Product p = ProductRepository.GetProductById(id);
            return PartialView(p);
        }
        public ActionResult ReferRestaurant(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }
        [HttpPost]
        public ActionResult ReferRestaurant(ReferRestaurantModel model,ShoppingCart cart)
        {
            ViewBag.result = "";
            if (ModelState.IsValid)
            {
                EmailManager em = new EmailManager();
                EmailContents ec = new EmailContents();
                ec.FromEmailAddress = model.YourEmail;
                ec.FromName ="Customer: " + model.YourName;
                ec.Subject = "Refer a restaurant";
                ec.To = Globals.Settings.ContactForm.MailTo; // send to FoodReady.Net
                ec.Body = "Refer a restayrant by customer: " + model.YourName + "<br />Customer Email: " + model.YourEmail + "<br />Restaurant Name: " + model.RestaurantName
                    + "<br />Restaurant Phone: " + model.RestaurantPhone + "<br />Restaurant City: " + model.RestaurantName + "<br />Restaurant State: " + model.RestaurantState;
                em.Send(ec);
                ViewBag.result = "Your message has been sent to foodready.net. Thank you so much  for refering a restaurant. we will be adding the restaurant into our restaurant system.";
            }
            ViewBag.bagitems = GetCartItems(cart);
            return View(model);
        }
        public ActionResult Discount(BrowseHistory bh)
        {
            DeliveryCheckModel dcm = new DeliveryCheckModel();
            dcm.Address = string.IsNullOrEmpty(bh.AddressCityState) ? "" : bh.AddressCityState;
            dcm.ZipCode = string.IsNullOrEmpty(bh.Zip) ? "" : bh.Zip;
            return PartialView(dcm);
        }
        [HttpPost]
        public ActionResult DiscountResult(DeliveryCheckModel dcm, BrowseHistory bh)
        {
            ViewBag.success = string.Empty;
            string[] s = base.GetAddressCityState(dcm.Address);
            string approxtime = string.Empty;
            string mysAddress = dcm.Address + " " + dcm.ZipCode;
            ViewBag.ValidateAddress = "Oops, feel like something wrong with you input. Please check it and try again.";
            if (!(string.IsNullOrEmpty(dcm.Address) || string.IsNullOrEmpty(dcm.ZipCode) || dcm.ZipCode.Length != 5 || string.IsNullOrEmpty(s[0]) || string.IsNullOrEmpty(s[1]) || string.IsNullOrEmpty(s[2])))
            {
                decimal st = SearchFilter.GetDistance(mysAddress, "1291 Parkside Dr.,Walnut Creek, CA 94596", out approxtime);
                string xx = approxtime;
                if (st >= 0.0m)
                {
                    ViewBag.success = "ok";
                    ViewBag.ValidateAddress = "";
                    bh.Address = s[0];
                    bh.City = s[1];
                    bh.State = s[2];
                    bh.Zip = dcm.ZipCode;
                    bh.AddressCityState = dcm.Address;
                    string goList = Url.Action("DiscountList", "Restaurants", new { vCity = s[1], vZip = dcm.ZipCode });
                    ViewBag.goList = goList;
                }
            }
            return PartialView(dcm);
        }
        public ActionResult DiscountList(string vCity, string vZip,ShoppingCart cart, BrowseHistory bh)
        {
            DiscountListViewModel dlv = new DiscountListViewModel();
            dlv.History = bh;
            dlv.LBizInfos = BizInfoRepository.GetBizInfosByCityZip(vCity, vZip).Where(e => e.HasDiscountCoupons || e.HasFreeItemCoupons).ToList();
            ViewBag.bagitems = GetCartItems(cart);
            return View(dlv);
        }
        [HttpPost]
        public ActionResult GetBizTime(int id)
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(id);
            DateTime dt = SearchFilter.ConvertLocalToMyTime(bi.BizHour.BizTimeZoneName);
            return Json(new
            {
                result=dt.ToLongTimeString()
            });
        }
    }
}
