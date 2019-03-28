using System.Web.Mvc;
using System.Text;
using System.Linq;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using FR.Services.ViewServices;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
using System.Web.Helpers;
using FR.Services.FilterServces;
using System;
using FoodReady.WebUI.EmailServices;
using FR.Infrastructure.Helpers;

namespace FoodReady.WebUI.Controllers
{
    public class GroupController : BaseController
    {
        private IProductToppingRepository ProductToppingRepository;
        private IProductRepository ProductRepository;
        private IBizInfoRepository BizInfoRepository;
        private IBizCuisineRepository BizCuisineRepository;
        private ICuisineTypeRepository CuisineTypeRepository;
        private IProductSizeRepository ProductSizeRepository;
        private ISideChoiceRepository SideChoiceRepository;
        private IProductDressingRepository ProductDressingRepository;
        private IDiscountCouponRepository DiscountCouponRepository;
        private IFreeItemCouponRepository FreeItemCouponRepository;
        private ICategoryRepository CategoryRepository;
        private ICrustChoiceRepository CrustChoiceRepository;
        private ICheeseAmountRepository CheeseAmountRepository;
        public GroupController(IProductRepository productRepo, IBizInfoRepository bizInfoRepo,
                               IBizCuisineRepository bizCuisineRepo, ICuisineTypeRepository cuisineTypeRepo,
                               IProductToppingRepository productToppingRepo, IProductSizeRepository productSizeRepo,
                               ISideChoiceRepository sideChoiceRepo, IProductDressingRepository productDressingRepo,
                               IDiscountCouponRepository discountCouponRepo, IFreeItemCouponRepository FreeItemCouponRepo,
                               ICategoryRepository categoryRepo, ICrustChoiceRepository crustChoiceRepo, ICheeseAmountRepository cheeseAmountRepo)
        {
            ProductRepository = productRepo;
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
            CuisineTypeRepository = cuisineTypeRepo;
            ProductToppingRepository = productToppingRepo;
            ProductSizeRepository = productSizeRepo;
            SideChoiceRepository = sideChoiceRepo;
            ProductDressingRepository = productDressingRepo;
            DiscountCouponRepository = discountCouponRepo;
            FreeItemCouponRepository = FreeItemCouponRepo;
            CategoryRepository = categoryRepo;
            CrustChoiceRepository = crustChoiceRepo;
            CheeseAmountRepository = cheeseAmountRepo;
        }

        // GET: Group
        public ActionResult Index(ShoppingCart cart,BrowseHistory bh)
        {
            if (cart == null)
            {
                ControllerContext cc = new ControllerContext();
                cart = new ShoppingCart();
                cc.HttpContext.Session["ShoppingCart"] = cart;
                return Redirect("/Home");
            }

            ViewBag.status = 0; // new
            GroupInfoModel gim = new GroupInfoModel();
            if (string.IsNullOrEmpty(cart.CartKey)==false) // come back
            {
                gim.Name = cart.PersonName;
                if (GroupCart.GroupCarts[cart.CartKey].IsSharedCartLocked)
                {
                    if (cart.PersonName == GroupCart.GroupCarts[cart.CartKey].PartyBossName)
                    {
                        ViewBag.status = 11; // locked & boss
                    }
                    else
                    {
                        ViewBag.status = 12; //locked & guest
                    }
                }
                else
                {
                    ViewBag.status = 1; // unlocked
                }
                gim.LBizInfo = GroupCart.GroupCarts[cart.CartKey].SelectBizInfos;
            }
            gim.Address = string.IsNullOrEmpty(bh.AddressCityState) ? "" : bh.AddressCityState;
            gim.ZipCode = string.IsNullOrEmpty(bh.Zip) ? "" : bh.Zip;
            gim.Cart = cart;
            return View(gim);
        }
        [HttpPost]
        public ActionResult Start(GroupInfoModel gim,ShoppingCart cart, BrowseHistory bh)
        {
            string approxtime = string.Empty;
            string mysAddress = gim.Address + " " + gim.ZipCode;
            string[] s = base.GetAddressCityState(gim.Address);
            ViewBag.ValidateAddress = string.Empty;
            List<BizInfo> lb = new List<BizInfo>();
            if (string.IsNullOrEmpty(gim.Address) || string.IsNullOrEmpty(gim.ZipCode) || gim.ZipCode.Length != 5 || string.IsNullOrEmpty(s[0]) || string.IsNullOrEmpty(s[1]) || string.IsNullOrEmpty(s[2]))
            {
                ViewBag.ValidateAddress = "Oops, feel like something wrong with you address. Please check it and try again.";
            }
            else
            {
                bh.Address = s[0];
                bh.City = s[1];
                bh.State = s[2];
                bh.Zip = gim.ZipCode;
                bh.AddressCityState=s[0] + ", " + s[1] + ", " + s[2];
                List<BizInfo> lbi = BizInfoRepository.GetBizInfoByZip(gim.ZipCode, true);
                foreach (var b in lbi)
                {
                    decimal st = SearchFilter.GetDistance(mysAddress, b.BizAddressString, out approxtime);
                    if (st >= 0 && st <= b.DeliveryRadius && b.Delivery)
                    {
                        lb.Add(b);
                    }
                }
                bh.GroupBizOption = lb;
            }
            gim.LBizInfo = lb;
            gim.Cart = cart;
            return PartialView(gim);
        }
        [HttpPost]
        public ActionResult Join(GroupGuestModel ggm, BrowseHistory bh)
        {
            string approxtime = string.Empty;
            ViewBag.ValidateAddress = string.Empty;
            ViewBag.gid = ggm.GroupId;
            ViewBag.lname = ggm.LableName;
            List<BizInfo> lb = new List<BizInfo>();
            if (string.IsNullOrEmpty(ggm.GroupId) || string.IsNullOrEmpty(ggm.LableName))
            {
                ViewBag.ValidateAddress = "Oops, feel like something wrong with your input. Please check it and try again.";
            }
            else if (GroupCart.GroupCarts.ContainsKey(ggm.GroupId) == false)
            {
                ViewBag.ValidateAddress = "Oops, Group ID is not correct. Please check it and try again.";
            }
            else
            {
                List<BizInfo> lbi = BizInfoRepository.GetBizInfoByZip(GroupCart.GroupCarts[ggm.GroupId].PartyZip, true);
                foreach (var b in lbi)
                {
                    decimal st = SearchFilter.GetDistance(GroupCart.GroupCarts[ggm.GroupId].PartyAddress, b.BizAddressString, out approxtime);
                    if (st >= 0 && st <= b.DeliveryRadius && b.Delivery)
                    {
                        lb.Add(b);
                    }
                }
                ViewBag.ValidateAddress = ggm.LableName + ", to join the group ordering, select a restaurant to start.";
            }
            return PartialView(lb);
        }
        public ActionResult GoGroup(string name,string address,string zip, int bizid,decimal maxorder, ShoppingCart cart, BrowseHistory bh)
        {
            string cartkey = Guid.NewGuid().ToString();
            if (cart.BizId !=bizid )
            {
                cart.Clear();
                BizInfo bi = BizInfoRepository.GetBizInfoById(bizid);
                cart.BizId = bizid;
                cart.IsBizDelivery = bi.Delivery;
                cart.TaxRate = bi.TaxPercentageRate;
                cart.OrderMinimum = bi.DeliveryMinimum;
                cart.DeliveryFee = bi.DeliveryFee;
                cart.BizName = bi.BizTitle;
            }
            MySharedCartId = cartkey;
            MyLabel = name;
            MyInfo.BossName = name;
            MyInfo.MyLabelName = name;
            MyInfo.SharedCartId = cartkey;
            cart.BossName = name;
            cart.CartKey = cartkey;
            cart.PersonName = name;
            cart.IsFinishedShareOrdering = false;
            SharedShoppingCart ssc = new SharedShoppingCart();
            ssc.MaxOrder = maxorder <= 0.0m ? 999999.0m : maxorder;
            ssc.PartyBossName = name;
            ssc.SharedCartKey = cartkey;
            ssc.PartyAddress = address + " " + zip;
            ssc.PartyZip = zip;
            ssc.IsSharedCartLocked = false;
            ssc.PartyCart.Add(name, cart);
            ssc.SelectBizInfos = bh == null ? new List<BizInfo>() : bh.GroupBizOption;
            SetGroupShoppingCart(cartkey, ssc);
            string url = Url.Action("Share", new { groupid = cartkey, bizid = bizid });
            return Redirect(url);
        }
        public ActionResult GoJoin(string name, string groupid,  int bizid, ShoppingCart cart)
        {
            if (GroupCart.GroupCarts.ContainsKey(groupid) == false)
            {
                return Redirect("/Group");
            }
            else if (GroupCart.GroupCarts[groupid].IsSharedCartLocked && cart.PersonName != GroupCart.GroupCarts[groupid].PartyBossName)
            {
                return Redirect("/Group");
            }
            if (cart.BizId != bizid)
            {
                cart.Clear();
                BizInfo bi = BizInfoRepository.GetBizInfoById(bizid);
                cart.BizId = bizid;
                cart.IsBizDelivery = bi.Delivery;
                cart.TaxRate = bi.TaxPercentageRate;
                cart.OrderMinimum = bi.DeliveryMinimum;
                cart.DeliveryFee = bi.DeliveryFee;
                cart.BizName = bi.BizTitle;
            }
            MySharedCartId = groupid;
            MyLabel = name;
            MyInfo.BossName = GroupCart.GroupCarts[groupid].PartyBossName;
            MyInfo.MyLabelName = name;
            MyInfo.SharedCartId = groupid;
            cart.BossName = MyInfo.BossName;
            cart.CartKey = groupid;
            cart.PersonName = name;
            cart.IsFinishedShareOrdering = false;
            if (GroupCart.GroupCarts[groupid].PartyCart.ContainsKey(name)==false && GroupCart.GroupCarts[groupid].IsSharedCartLocked == false)
            {
                GroupCart.GroupCarts[groupid].PartyCart.Add(name, cart);
            }
            string url = Url.Action("Share", new { groupid = groupid, bizid = bizid });
            return Redirect(url);
        }
        public ActionResult Share(string groupid, ShoppingCart cart, BrowseHistory bh, int bizid = 0)
        {
            if (bh == null)
            {
                ControllerContext cc = new ControllerContext();
                bh = new BrowseHistory();
                bh.IsDelivery = true;
                cc.HttpContext.Session["BorseHistory"] = bh;
                return Redirect("~/Group");
            }
            if (string.IsNullOrEmpty(groupid))
            {
                return Redirect("~/Group");
            }
            if (GroupCart.GroupCarts.ContainsKey(groupid) == false || bizid <= 0)
            {
                return Redirect("~/Group");
            }

            if ((GroupCart.GroupCarts[groupid].IsSharedCartLocked && GroupCart.GroupCarts[groupid].PartyBossName != cart.PersonName))
            {
                return Redirect("~/Group");
            }
            BizInfo bi = GroupCart.GroupCarts[groupid].SelectBizInfos.Where(e => e.BizInfoId == bizid).FirstOrDefault();
            if (bi == null)
            {
                return Redirect("~/Group");
            }
            if (bizid != cart.BizId)
            {
                MyInfo.BossName = GroupCart.GroupCarts[groupid].PartyBossName; ;
                MyInfo.MyLabelName = cart.PersonName;
                cart.Clear();
                cart.BossName = MyInfo.BossName;
                cart.CartKey = groupid;
                cart.PersonName = MyInfo.MyLabelName;
                cart.IsFinishedShareOrdering = false;
                cart.BizId = bizid;
                cart.IsBizDelivery = bi.Delivery;
                cart.TaxRate = bi.TaxPercentageRate;
                cart.OrderMinimum = bi.DeliveryMinimum;
                cart.DeliveryFee = bi.DeliveryFee;
                cart.BizName = bi.BizTitle;
            }
            ViewBag.cartkey = groupid;
            cart.IsFinishedShareOrdering = false;
            SharedShoppingCart ssc = new SharedShoppingCart();
            ssc = GetGroupShoppingCart(groupid);
            //ssc.IsPartyDelivery = ssc.PartyCart[ssc.PartyBossName].IsBizDelivery;
            ViewBag.bossname = ssc.PartyBossName;
            ViewBag.myname = MyLabel;
            ViewBag.isBoss = ssc.PartyBossName == cart.PersonName ? "yes" : "no";
            ViewBag.bizinfoid = bizid;
            ViewBag.bizname = bi.BizTitle;
            ShareMenuViewModel smvm = new ShareMenuViewModel();

            smvm.Cart = cart;
            smvm.SharedCart = ssc;
            smvm.Cart.IsDelivery = ssc.IsPartyDelivery;
            smvm.History = bh;
            smvm.BizInfo = bi;

            string rul = HttpContext.Request.UrlReferrer == null ? "~/Home" : HttpContext.Request.UrlReferrer.PathAndQuery;
            smvm.ReturnUrl = rul;
            smvm.MenuList = BizInfoRepository.GetBizCuisinesByBizInfoId(cart.BizId, true).ToList();
            smvm.FirstSubmenu = smvm.MenuList.FirstOrDefault();
            ViewBag.rurl = HttpContext.Request.UrlReferrer == null ? "~/Home" : HttpContext.Request.Url.PathAndQuery;
            return View(smvm);
        }
        public ActionResult SubMenu(int id, ShoppingCart cart, string sortParam = "")
        {
            SubMenuViewModel smvm = new SubMenuViewModel();
            smvm.CategoryList = CategoryRepository.GetCategoriesByBizCuisineId(id, true).Where(e => e.Title.ToLower() != "lunch special").ToList();
            BizInfo cg = BizCuisineRepository.GetBizCuisineById(id).BizInfo;
            smvm.MenuList = BizCuisineRepository.GetBizCuisinesByBizInfoId(true, cg.BizInfoId);
            smvm.sortParameter = sortParam;
            smvm.CurrentBizCuisineId = id;
            return PartialView(smvm);
        }
        public ActionResult LunchMenu(int id, ShoppingCart cart) // id=BizCuisineId
        {
            Category ctg = BizCuisineRepository.GetBizCuisineById(id).LunchSpecailCategory;
            List<Product> lp = ctg.Products.Where(e => e.CategoryId == ctg.CategoryId && e.Active == true).ToList();
            return PartialView(lp);
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
        public ActionResult AddToSharedCart(int id, ShoppingCart cart)
        {
            AddItemModel model = new AddItemModel();
            ViewBag.canAdd = string.IsNullOrEmpty(cart.CartKey) ? "n" : "y";
            model.Product = ProductRepository.GetProductById(id);
            model.ProductID = id;
            List<ProductSize> lps = ProductSizeRepository.GetProductSizesByProductId(true, id);
            model.SizeAssistances = new List<SelectListItem>();
            foreach (var s in lps)
            {
                model.SizeAssistances.Add(new SelectListItem { Text = s.Title + string.Format(" {0}{1}", s.Size > 0 ? " " + s.Size + "\"" : "", " ($" + s.Price.ToString("N2") + ")"), Value = s.ProductSizeId.ToString() });
            }
            List<ProductTopping> lpt = new List<ProductTopping>();
            lpt = ProductToppingRepository.GetProductToppingsByProductId(true, id);
            foreach (var t in lpt)
            {
                model.ToppingAssistances.Add(new CheckBoxViewModel { BoxName = t.ToppingTitle, BoxPrice = t.ExtraToppingPrice, Checked = false });
            }
            if (model.Product.MaxNumOfFreeTopping > 0)
            {
                foreach (var t in lpt)
                {
                    model.FreeToppingAssistances.Add(new CheckBoxViewModel { BoxName = t.ToppingTitle, BoxPrice = t.ExtraToppingPrice, Checked = false });
                }
            }
            List<AddSide> las = new List<AddSide>();
            las = model.Product.AddSides.Where(e => e.Active == true).ToList();
            foreach (var a in las)
            {
                model.AddSideAssistances.Add(new AddSideCheckBoxModel { BoxName = a.Title, BoxPrice = a.Price, BoxBizPrice = a.BizPrice, Checked = false });
            }
            SharedShoppingCart ssc = new SharedShoppingCart();
            if (string.IsNullOrEmpty(cart.CartKey) == false)
            {
               ssc = GetGroupShoppingCart(cart.CartKey);
            }
            model.SharedCart = ssc;
            model.Cart = cart;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult UpdateSharedCart(AddItemModel model, ShoppingCart cart)
        {
            HandleCart(cart);
            model.BizInfo = BizInfoRepository.GetBizInfoById(cart.BizId);
            if (model.BizInfo == null)
            {
                return Redirect("~/RedirectPage.htm");
            }

            cart.DriverTip = 0.0m;
            List<CheckBoxViewModel> selectedFreeToppings = new List<CheckBoxViewModel>();
            List<CheckBoxViewModel> selectedToppings = new List<CheckBoxViewModel>();
            List<CheckBoxViewModel> selectedDressings = new List<CheckBoxViewModel>();
            List<AddSideCheckBoxModel> selectedAddSides = new List<AddSideCheckBoxModel>();
            Product prod = ProductRepository.GetProductById(model.ProductID);
            selectedFreeToppings = model.FreeToppingAssistances.Where(e => e.Checked == true).ToList();
            selectedToppings = model.ToppingAssistances.Where(e => e.Checked == true).ToList();
            selectedDressings = model.DressingAssistances.Where(e => e.Checked == true).ToList();
            selectedAddSides = model.AddSideAssistances.Where(e => e.Checked == true).ToList();

            int ccID = string.IsNullOrEmpty(model.CrustChoiceID) ? 0 : int.Parse(model.CrustChoiceID);
            string ccTitle = string.Empty;
            decimal ccPrice = 0.0m;
            decimal ccBizPrice = 0.0m;
            if (ccID > 0)
            {
                CrustChoice cc = CrustChoiceRepository.GetCrustChoiceById(ccID);
                ccTitle = cc.Title;
                ccPrice = cc.Price;
                ccBizPrice = cc.BizPrice;
            }
            int caID = string.IsNullOrEmpty(model.CheeseAmountID) ? 0 : int.Parse(model.CheeseAmountID);
            string caTitle = string.Empty;
            decimal caPrice = 0.0m;
            decimal caBizPrice = 0.0m;
            if (caID > 0)
            {
                CheeseAmount c = CheeseAmountRepository.GetCheeseAmountById(caID);
                caTitle = c.Title;
                caPrice = c.Price;
                caBizPrice = c.BizPrice;
            }
            ProductSize ps = ProductSizeRepository.GetProductSizeById(int.Parse(string.IsNullOrEmpty(model.ProductSize) ? "0" : model.ProductSize));
            string psTitle = ps == null ? string.Empty : ps.Title;
            string pSize = ps == null ? string.Empty : ps.Size.ToString();
            decimal psPrice = ps == null ? 0.0m : ps.Price;
            decimal psBizPrice = ps == null ? 0.0m : ps.BizPrice;
            foreach (var t in selectedToppings)
            {
                decimal exp = 0.0m;
                if (string.IsNullOrEmpty(psTitle) == false)
                {
                    exp = psTitle.StartsWith("S", true, null) ? t.BoxPrice : (psTitle.StartsWith("M", true, null) ? t.BoxPrice + 0.50m : psTitle.StartsWith("L", true, null) ? t.BoxPrice + 1.00m : t.BoxPrice + 1.50m);
                    t.BoxPrice = exp;
                }
            }
            int q = prod.IsFamilyDinner ? model.FamilyQty : model.Quantity;
            string side = prod.HasSideChoice ? model.SideChoice : string.Empty;
            string sauce = prod.HasSauceChoice ? model.SauceChoice : string.Empty;
            string pt = prod.MealSectionId == 3 ? "(Lunch)" + prod.Title : prod.Title;
            string sft = string.Empty;
            foreach (var s in selectedFreeToppings)
            {
                sft = sft + s.BoxName + ", ";
            }
            if (selectedFreeToppings.Count > 0)
            {
                sft = sft.Remove(sft.Length - 2, 2);
            }
            ShoppingCartItem sci = new ShoppingCartItem("9999", model.ProductID, cart.PersonName, q, false, 0, 0, 0, 0, "", "", pt, prod.BizPrice, prod.BizFinalUnitPrice, prod.UnitPrice, prod.FinalUnitPrice, prod.DiscountPercentage,
               prod.SmallImage, model.Instructions, prod.IsSpicy, model.HowSpicy, prod.IsFamilyDinner, side, sauce, psTitle, pSize, psPrice,
               psBizPrice, sft, selectedToppings, selectedDressings, selectedAddSides, model.DressingChoice, ccTitle, ccPrice, ccBizPrice, caTitle, caPrice, caBizPrice);

            bool bl = CanAddGroupItemToCart(cart.CartKey, sci.ItemTotal);
            if (bl)
            {
            cart.InsertItem(model.ProductID, q, cart.PersonName, false, 0, 0, 0, 0, "", "", pt, prod.BizPrice, prod.BizFinalUnitPrice, prod.UnitPrice, prod.FinalUnitPrice, prod.DiscountPercentage,
               prod.SmallImage, model.Instructions, prod.IsSpicy, model.HowSpicy, prod.IsFamilyDinner, side, sauce, psTitle, pSize, psPrice,
               psBizPrice, sft, selectedToppings, selectedDressings, selectedAddSides, model.DressingChoice, ccTitle, ccPrice, ccBizPrice, caTitle, caPrice, caBizPrice);
            }
            model.Cart = cart;
            ViewBag.canadditem = bl?"y":"n";
            ViewBag.itemid = bl? cart.Lines.LastOrDefault().ItemId:"";
            ViewBag.quantity = q.ToString();
            ViewBag.title = bl? cart.Lines.LastOrDefault().Title:"";
            ViewBag.itemtotal = bl? cart.Lines.LastOrDefault().ItemTotal.ToString("N2"):"";
            ViewBag.price = prod.FinalUnitPrice.ToString("N2");
            ViewBag.subtotal = cart.SubTotal().ToString("N2");
            ViewBag.bizname = cart.BizName;
            ViewBag.tax = GroupCart.GroupCarts[cart.CartKey].PartyTax().ToString("N2");
            ViewBag.drivertip = GroupCart.GroupCarts[cart.CartKey].PartyDriverTip().ToString("N2");
            ViewBag.globalsubtotal = GroupCart.GroupCarts[cart.CartKey].PartySubTotal().ToString("N2");
            ViewBag.globaltotal = GroupCart.GroupCarts[cart.CartKey].PartyTotal().ToString("N2");

            string btnShow = "show";
            bool isBoss = (string.IsNullOrEmpty(cart.PersonName) == false && cart.PersonName == GroupCart.GroupCarts[cart.CartKey].PartyBossName);
            if (isBoss)
            {
                btnShow = "off";
            }
            else if ( GroupCart.GroupCarts[cart.CartKey].PartyTotalItems == 0 || (GroupCart.GroupCarts[cart.CartKey].PartyOrderMinimum() > GroupCart.GroupCarts[cart.CartKey].PartySubTotal() && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery) || (GroupCart.GroupCarts[cart.CartKey].PartyCart[cart.BossName].IsBizDelivery == false && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery))
            {
                btnShow = "off";
            }
            ViewBag.btnshow = btnShow;

            SharedShoppingCart ssc = new SharedShoppingCart();
            if (string.IsNullOrEmpty(cart.CartKey) == false)
            {
                ssc = GetGroupShoppingCart(cart.CartKey);
            }
            model.SharedCart = ssc;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult UpdateDelivery(string del, ShoppingCart cart, BrowseHistory bh)
        {
            string timeout = "timein";
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            if (bi == null)
            {
                timeout = "timeout";
                return Json(new
                {
                    timeexp = timeout
                });
            }
            SharedShoppingCart ssc = new SharedShoppingCart();
            if (string.IsNullOrEmpty(cart.CartKey) == false)
            {
                ssc = GetGroupShoppingCart(cart.CartKey);
            }
            /*
            if (ssc.IsSharedCartLocked)
            {
                return Json(new
                {
                    timeexp = timeout,
                    sharelocked = "yes"
                });
            }
            */
            decimal partysubtotal = ssc.PartySubTotal();
            bh.IsDelivery = del == "true" ? true : false;
            ssc.IsPartyDelivery = bh.IsDelivery;

            string btnShow = "show";
            bool isBoss = (string.IsNullOrEmpty(cart.PersonName) == false && cart.PersonName == GroupCart.GroupCarts[cart.CartKey].PartyBossName);
            if (GroupCart.GroupCarts[cart.CartKey].PartyTotalItems == 0 || (GroupCart.GroupCarts[cart.CartKey].PartyOrderMinimum() > GroupCart.GroupCarts[cart.CartKey].PartySubTotal() && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery) || (GroupCart.GroupCarts[cart.CartKey].PartyCart[cart.BossName].IsBizDelivery == false && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery))
            {
                btnShow = "off";
            }
            decimal cartTax = ssc.PartyTax();
            decimal subTotal = cart.SubTotal(); ;
            decimal globalsubTotal = partysubtotal;
            decimal cartTotal = ssc.PartyTotal();
            return Json(new
            {
                timeexp = timeout,
                sharelocked = ssc.IsSharedCartLocked ? "yes" : "no",
                isboss = isBoss ? "yes" : "no",
                isdelivery = ssc.IsPartyDelivery ? "delivery" : "pickup",
                delfee = ssc.PartyDeliveryFee().ToString("N2"),
                drivertip = ToUSD(ssc.PartyDriverTip().ToString("N2")),
                delMin = ssc.PartyOrderMinimum().ToString("N2"),
                globaltotal = ToUSD(cartTotal.ToString("N2")),
               // globalsubtotal = ToUSD(globalsubTotal.ToString("N2")),
               // carttax = ToUSD(cartTax.ToString("N2")),
               // subtotal = ToUSD(subTotal.ToString("N2")),
                btnshow = btnShow
            });

        }
        public ActionResult RemoveFromCart(ShoppingCart cart, string itemId)
        {
            HandleCart(cart);

            AddItemModel model = new AddItemModel();
            ViewBag.cando = string.IsNullOrEmpty(cart.CartKey) ? "n" : "y";
            model.BizInfo = BizInfoRepository.GetBizInfoById(cart.BizId);

            if (model.BizInfo == null)
            {
                return Redirect("~/Home");
            }
            SharedShoppingCart ssc = new SharedShoppingCart();
            if (string.IsNullOrEmpty(cart.CartKey))
            {
                model.SharedCart = ssc;
                model.Cart = cart;
                ViewBag.canremoveitem = "n";
                return PartialView(model);
            }
            ssc = GetGroupShoppingCart(cart.CartKey);
            bool bl = (string.IsNullOrEmpty(itemId) == false && !ssc.IsSharedCartLocked);
            if (bl)
            {
                cart.DeleteProduct(itemId);
            }

            ViewBag.canremoveitem = bl?"y":"n";
            ViewBag.itemid = itemId;
            ViewBag.subtotal = cart.SubTotal().ToString("N2");
            ViewBag.tax = GroupCart.GroupCarts[cart.CartKey].PartyTax().ToString("N2");
            ViewBag.globalsubtotal = GroupCart.GroupCarts[cart.CartKey].PartySubTotal().ToString("N2");
            ViewBag.drivertip = GroupCart.GroupCarts[cart.CartKey].PartyDriverTip().ToString("N2");
            ViewBag.globaltotal = GroupCart.GroupCarts[cart.CartKey].PartyTotal().ToString("N2");

            string btnShow = "show";
            bool isBoss = (string.IsNullOrEmpty(cart.PersonName) == false && cart.PersonName == GroupCart.GroupCarts[cart.CartKey].PartyBossName);
            if (isBoss)
            {
                btnShow = "off";
            }
            else if (GroupCart.GroupCarts[cart.CartKey].PartyTotalItems == 0 || (GroupCart.GroupCarts[cart.CartKey].PartyOrderMinimum() > GroupCart.GroupCarts[cart.CartKey].PartySubTotal() && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery) || (GroupCart.GroupCarts[cart.CartKey].PartyCart[cart.BossName].IsBizDelivery == false && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery))
            {
                btnShow = "off";
            }
            ViewBag.btnshow = btnShow;
            model.SharedCart = ssc;
            model.Cart = cart;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult UpdateQuantity(ShoppingCart cart, string itemId, string qty = "0")
        {
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            ShoppingCartItem sci = cart.GetCartLineByItemId(itemId);
            string timeout = "timein";
            if (bi == null || sci == null)
            {
                timeout = "timeout";
                return Json(new
                {
                    timeexp = timeout,
                    canupdatequantity = "n"
                });
            }

            if (string.IsNullOrEmpty(cart.CartKey))
            {
                return Json(new
                {
                    timeexp = timeout,
                    canupdatequantity = "n"
                });
            }
            SharedShoppingCart ssc = new SharedShoppingCart();
            if (string.IsNullOrEmpty(cart.CartKey) == false)
            {
                ssc = GetGroupShoppingCart(cart.CartKey);
            }
            int quty = int.Parse(qty);
            int beforeQuty = sci.Quantity;
             bool bl =CanAddGroupItemToCart(cart.CartKey,(quty-beforeQuty)* sci.ItemPrice);
             if (bl)
             {
                 cart.UpdateItemQuantity(itemId, quty);
             }
            decimal partysubtotal = ssc.PartySubTotal();

            string btnShow = "show";
            if (string.IsNullOrEmpty(itemId) == false)
            {
                bool isBoss = (string.IsNullOrEmpty(cart.PersonName) == false && cart.PersonName == GroupCart.GroupCarts[cart.CartKey].PartyBossName);
                if (GroupCart.GroupCarts[cart.CartKey].PartyTotalItems == 0 || (GroupCart.GroupCarts[cart.CartKey].PartyOrderMinimum() > GroupCart.GroupCarts[cart.CartKey].PartySubTotal() && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery) || (GroupCart.GroupCarts[cart.CartKey].PartyCart[cart.BossName].IsBizDelivery == false && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery))
                {
                    btnShow = "off";
                }

                decimal productSubTotal = sci.ItemTotal;
                decimal cartTax = ssc.PartyTax();
                decimal subTotal = cart.SubTotal(); ;
                decimal globalsubTotal = partysubtotal;
                decimal globalTotal = ssc.PartyTotal();
                int cartItems = ssc.PartyTotalItems;
                return Json(new
                {
                    timeexp = timeout,
                    canupdatequantity = bl ? "y" : "n",
                    isboss = isBoss ? "yes" : "no",
                    delMin = ssc.PartyOrderMinimum().ToString("N2"),
                    isDelivery = ssc.IsPartyDelivery ? "delivery" : "pickup",
                    itemid = itemId,
                    qutty = quty.ToString(),
                    producttitle = sci.Title,
                    uniteprice =ToUSD(sci.FinalPrice.ToString("N2")),
                    total = ToUSD(globalTotal.ToString("N2")),
                    cartitems = cartItems.ToString(),
                    carttax = ToUSD(cartTax.ToString("N2")),
                    drivertip = ToUSD(ssc.PartyDriverTip().ToString("N2")),
                    subtotal = ToUSD(subTotal.ToString("N2")),
                    globalsubtotal = ToUSD(globalsubTotal.ToString("N2")),
                    productsubtotal = ToUSD(productSubTotal.ToString("N2")),
                    btnshow = btnShow
                });
            }
            return Json(new { total = ToUSD(ssc.PartyTotal().ToString("N2")) });
        }
        [HttpPost]
        public ActionResult loadImage(int ProductID, ShoppingCart cart)
        {
            Product p = ProductRepository.GetProductById(ProductID);
            return PartialView(p);
        }
        public ActionResult GoCheckout(ShoppingCart cart)
        {
            HandleCart(cart);
            BizInfo bi = BizInfoRepository.GetBizInfoById(cart.BizId);
            AddItemModel aim = new AddItemModel();

            SharedShoppingCart ssc = new SharedShoppingCart();
            if (string.IsNullOrEmpty(cart.CartKey) == false)
            {
                ssc = GetGroupShoppingCart(cart.CartKey);
            }
            decimal partysubtotal = ssc.PartySubTotal();
            ViewBag.okcheckout = "show";
            bool isBoss = (string.IsNullOrEmpty(cart.PersonName) == false && cart.PersonName == GroupCart.GroupCarts[cart.CartKey].PartyBossName);
            if (!isBoss || GroupCart.GroupCarts[cart.CartKey].PartyTotalItems == 0 || (GroupCart.GroupCarts[cart.CartKey].PartyOrderMinimum() > GroupCart.GroupCarts[cart.CartKey].PartySubTotal() && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery) || (GroupCart.GroupCarts[cart.CartKey].PartyCart[cart.BossName].IsBizDelivery == false && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery))
            {
                ViewBag.okcheckout = "off";
            }
            ssc.IsSharedCartLocked = true;
            aim.SharedCart = ssc;
            aim.Cart = cart;
            return PartialView(aim);
        }
        [HttpPost]
        public ActionResult ChangeLock(ShoppingCart cart)
        {
            string isLocked = "n";
            if (string.IsNullOrEmpty(cart.CartKey) == false)
            {
                GroupCart.GroupCarts[cart.CartKey].IsSharedCartLocked = !(GroupCart.GroupCarts[cart.CartKey].IsSharedCartLocked);
                isLocked = GroupCart.GroupCarts[cart.CartKey].IsSharedCartLocked?"y":"n";
            }
            return Json(new { islocked = isLocked});
        }
        [HttpPost]
        public ActionResult IamDone(ShoppingCart cart)
        {
            HandleCart(cart);
            cart.IsFinishedShareOrdering = true;
            return Json(new {});
        }
        [HttpPost]
        public ActionResult RemovePerson(string id, ShoppingCart cart) // id=PersonName
        {
            HandleCart(cart);
            string islocked = "yes";
            SharedShoppingCart ssc = new SharedShoppingCart();
            if (string.IsNullOrEmpty(cart.CartKey) == false)
            {
                ssc = GetGroupShoppingCart(cart.CartKey);
            }
            if (ssc.IsSharedCartLocked == false)
            {
                islocked = "no";
                ShoppingCart sc = new ShoppingCart();
                sc = ssc.PartyCart[id];
                ssc.DeleteCart(id);
                // sc = null;
                sc.Clear();
            }
            string btnShow = "show";
            bool isBoss = (string.IsNullOrEmpty(cart.PersonName) == false && cart.PersonName == GroupCart.GroupCarts[cart.CartKey].PartyBossName);
            if (!isBoss || GroupCart.GroupCarts[cart.CartKey].PartyTotalItems == 0 || (GroupCart.GroupCarts[cart.CartKey].PartyOrderMinimum() > GroupCart.GroupCarts[cart.CartKey].PartySubTotal() && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery) || (GroupCart.GroupCarts[cart.CartKey].PartyCart[cart.BossName].IsBizDelivery == false && GroupCart.GroupCarts[cart.CartKey].IsPartyDelivery))
            {
                btnShow = "off";
            } 
            decimal cartTax = ssc.PartyTax();
            decimal globalsubTotal = ssc.PartySubTotal();
            decimal globalTotal = ssc.PartyTotal();
            int cartItems = ssc.PartyTotalItems;
            return Json(new
            {
                islocked=islocked,
                name = id,
                total = ToUSD(globalTotal.ToString("N2")),
                carttax = ToUSD(cartTax.ToString("N2")),
                drivertip = ToUSD(ssc.PartyDriverTip().ToString("N2")),
                globalsubtotal = ToUSD(globalsubTotal.ToString("N2")),
                btnshow = btnShow

            });
        }
        public ActionResult SendGroupKey(ShoppingCart cart)
        {
            EmailGroupKeyModel egkm = new EmailGroupKeyModel();
            ViewBag.cartkey = cart.CartKey;
            egkm.Name = cart.PersonName;
            return PartialView(egkm);
        }
        public ActionResult EmailKey(EmailGroupKeyModel egkm, ShoppingCart cart)
        {
            HandleCart(cart);
            ViewBag.result = string.Empty;
            ViewBag.cartkey = cart.CartKey;
            if (ModelState.IsValid)
            {
                EmailManager em = new EmailManager();
                EmailContents ec = new EmailContents();
                ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
                ec.FromName = "FoodReady.Net";
                ec.Subject = egkm.Name + " is inviting you to join food orderong";
                ec.To = egkm.Email; // send to FoodReady.Net
                ec.Body = "<p>The group key:" + cart.CartKey + "</p><p>" + egkm.Message + "</p>";
                em.Send(ec);
                ViewBag.result = "Email has been successfully sent.";
            }
            return PartialView(egkm);
        }
    }
}