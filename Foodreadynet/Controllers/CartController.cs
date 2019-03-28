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

namespace FoodReady.WebUI.Controllers
{
   // [SessionExpireFilter]
    public class CartController : BaseController
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
        private ICrustChoiceRepository CrustChoiceRepository;
        private ICheeseAmountRepository CheeseAmountRepository;
        public CartController( IProductRepository productRepo,IBizInfoRepository bizInfoRepo,
                               IBizCuisineRepository bizCuisineRepo,ICuisineTypeRepository cuisineTypeRepo,
                               IProductToppingRepository productToppingRepo, IProductSizeRepository productSizeRepo,
                               ISideChoiceRepository sideChoiceRepo, IProductDressingRepository productDressingRepo,
                               IDiscountCouponRepository discountCouponRepo, IFreeItemCouponRepository FreeItemCouponRepo,
                               ICrustChoiceRepository crustChoiceRepo, ICheeseAmountRepository cheeseAmountRepo)
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
            CrustChoiceRepository = crustChoiceRepo;
            CheeseAmountRepository = cheeseAmountRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToCart(int id)
        {
            AddItemModel model = new AddItemModel();
            model.Product = ProductRepository.GetProductById(id);
            model.ProductID = id;
            List<ProductSize> lps = ProductSizeRepository.GetProductSizesByProductId(true, id);
            model.SizeAssistances = new List<SelectListItem>();
            foreach (var s in lps)
            {
                model.SizeAssistances.Add(new SelectListItem { Text = s.Title + string.Format(" {0}{1}", s.Size > 0 ? " " + s.Size + "\"" : "", " ($" + s.Price.ToString("N2") + ")"), Value = s.ProductSizeId.ToString() });
            }
            List<ProductTopping> lpt = new List<ProductTopping>();
            lpt = ProductToppingRepository.GetProductToppingsByProductId(true,id);
            foreach (var t in lpt)
            {
                model.ToppingAssistances.Add(new CheckBoxViewModel { BoxName = t.ToppingTitle, BoxPrice = t.ExtraToppingPrice,BoxIncrementValue=t.Increment, Checked = false });
            }
            if (model.Product.MaxNumOfFreeTopping>0)
            {
                foreach (var t in lpt)
                {
                    model.FreeToppingAssistances.Add(new CheckBoxViewModel { BoxName = t.ToppingTitle, BoxPrice = t.ExtraToppingPrice, Checked = false });
                }
            }
            List<AddSide> las = new List<AddSide>();
            las = model.Product.AddSides.Where(e=>e.Active==true).ToList();
            foreach (var a in las)
            {
                model.AddSideAssistances.Add(new AddSideCheckBoxModel { BoxName = a.Title, BoxPrice = a.Price, BoxBizPrice =a.BizPrice,Checked = false });
            } 
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult UpdateCart(AddItemModel model, ShoppingCart cart)
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

            int ccID=string.IsNullOrEmpty(model.CrustChoiceID)?0:int.Parse(model.CrustChoiceID);
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
                if (string.IsNullOrEmpty(psTitle)==false)
                {
                    exp = psTitle.StartsWith("S", true, null) ? t.BoxPrice : (psTitle.StartsWith("M", true, null) ? t.BoxPrice + t.BoxIncrementValue : psTitle.StartsWith("L", true, null) ? t.BoxPrice + t.BoxIncrementValue*2 : t.BoxPrice + t.BoxIncrementValue*3);
                    t.BoxPrice = exp;
                }
             }
            int q = prod.IsFamilyDinner ? model.FamilyQty : model.Quantity;
            string side = prod.HasSideChoice ? model.SideChoice : string.Empty;
            string sauce = prod.HasSauceChoice ? model.SauceChoice : string.Empty;
            string pt = prod.MealSectionId == 3 ?"(Lunch)" + prod.Title : prod.Title;
            string sft = string.Empty;
            foreach (var s in selectedFreeToppings)
            {
                sft = sft + s.BoxName + ", ";
            }
            if (selectedFreeToppings.Count > 0)
            {
                sft=sft.Remove(sft.Length - 2, 2);
            }
            cart.InsertItem(model.ProductID, q,cart.PersonName, false, 0,0,0, 0, "","", pt, prod.BizPrice, prod.BizFinalUnitPrice, prod.UnitPrice, prod.FinalUnitPrice, prod.DiscountPercentage,
                prod.SmallImage, model.Instructions, prod.IsSpicy, model.HowSpicy, prod.IsFamilyDinner,side,sauce, psTitle,pSize, psPrice,
                psBizPrice,sft, selectedToppings, selectedDressings, selectedAddSides, model.DressingChoice, ccTitle, ccPrice, ccBizPrice, caTitle, caPrice, caBizPrice);
            model.Cart = cart;

            List<DiscountCoupon> ldc = new List<DiscountCoupon>();
            List<FreeItemCoupon> lfc = new List<FreeItemCoupon>();
            if (model.BizInfo.HasDiscountCoupons)
            {
                ldc = DiscountCouponRepository.GetBizDiscountCouponsByMinimum(cart.BizId, cart.SubTotal(), true);
            }
            model.DiscountCouponList = ldc;
            if (model.BizInfo.HasFreeItemCoupons)
            {
                lfc = FreeItemCouponRepository.GetBizFreeItemCouponsByMinimum(cart.BizId, cart.SubTotal(), true);
            }
            model.FreeItemCouponList = lfc;
            ViewBag.items = cart.TotalItems == 0 ? string.Empty : cart.TotalItems.ToString();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult UpdateQuantity(ShoppingCart cart, string itemId, string qty = "0")
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
            int quty = int.Parse(qty);
            cart.UpdateItemQuantity(itemId, quty);
            List<DiscountCoupon> ldc = new List<DiscountCoupon>();
            List<FreeItemCoupon> lfc = new List<FreeItemCoupon>();

            string couponShow="NO";
            if (string.IsNullOrEmpty(itemId)==false)
            {

                if (bi.HasDiscountCoupons)
                {
                    ldc = DiscountCouponRepository.GetBizDiscountCouponsByMinimum(cart.BizId, cart.SubTotal(), true);
                }
                if (bi.HasFreeItemCoupons)
                {
                    lfc = FreeItemCouponRepository.GetBizFreeItemCouponsByMinimum(cart.BizId, cart.SubTotal(), true);
                }

                if (ldc.Count>0 || lfc.Count>0)
                {
                    couponShow = "OK";
                }

                if (ldc.Count == 0)
                {
                    cart.DiscountPercentage = 0;
                    cart.CurrentDiscountMini = 999999.0m;
                }
                else if (ldc.Max(e=>e.DiscountPercentage)<cart.DiscountPercentage)
                {
                    cart.DiscountPercentage = 0;
                    cart.CurrentDiscountMini = 999999.0m;
                }

                if (lfc.Count == 0)
                {
                    cart.FreeItem = "";
                }
                if (ldc.Count == 0 && lfc.Count == 0)
                {
                    
                    cart.CouponChoice = "";
                }
                decimal t = 0.0m;
                StringBuilder sb = new StringBuilder();
                sb.Append("<option value=''>Apply a coupon? No, Thanks</option>");
                string txt = "";
                string vl = "";
            foreach (var d in ldc)
            {
                t = d.DiscountPercentage == 0 ? 0.0m : (cart.SubTotal() * d.DiscountPercentage / 100);
                txt = d.Title + " ------ ( -$" + t.ToString("N2") + " )";
                vl = d.DiscountPercentage.ToString() + ";" + d.OrderMinimum.ToString("N2");
                if (cart.CouponChoice == d.DiscountPercentage.ToString())
                {
                    sb.Append("<option value='" + vl + "' selected='selected'>" + txt + "</option>");
                }
                else
                {
                    sb.Append("<option value='" + vl + "'>" + txt + "</option>");
                }
                
            }
            foreach (var f in lfc)
            {
                txt = f.Title + " ( save: $" + f.UnitPrice.ToString("N2") + " )";
                vl = f.Title;
                if (cart.CouponChoice == f.Title)
                {
                    sb.Append("<option value='" + vl + "' selected='selected'>" + txt + "</option>");
                }
                else
                {
                    sb.Append("<option value='" + vl + "'>" + txt + "</option>");
                }
             }
            cart.DriverTip = 0.0m;
            string v15 = (cart.SubTotal() * 0.15m).ToString("N2");
            string v16 = (cart.SubTotal() * 0.16m).ToString("N2");
            string v17 = (cart.SubTotal() * 0.17m).ToString("N2");
            string v18 = (cart.SubTotal() * 0.18m).ToString("N2");
            string v19 = (cart.SubTotal() * 0.19m).ToString("N2");
            string v20 = (cart.SubTotal() * 0.20m).ToString("N2");
            string v21 = (cart.SubTotal() * 0.21m).ToString("N2");
            string v22 = (cart.SubTotal() * 0.22m).ToString("N2");
            string v23 = (cart.SubTotal() * 0.23m).ToString("N2");
            string v24 = (cart.SubTotal() * 0.24m).ToString("N2");
            string v25 = (cart.SubTotal() * 0.25m).ToString("N2");
            decimal baseint = cart.IsDelivery ? Decimal.Round(cart.SubTotal() * 0.15m) + 1 : 2.0m;

            string percent15 = "<option value='" + v15 + "'>" + "15% ---  $" + string.Format("{0} tip", v15) + "</option>";
            string percent16 = "<option value='" + v16 + "'>" + "16% ---  $" + string.Format("{0} tip", v16) + "</option>";
            string percent17 = "<option value='" + v17 + "'>" + "17% ---  $" + string.Format("{0} tip", v17) + "</option>";
            string percent18 = "<option value='" + v18 + "'>" + "18% ---  $" + string.Format("{0} tip", v18) + "</option>";
            string percent19 = "<option value='" + v19 + "'>" + "19% ---  $" + string.Format("{0} tip", v19) + "</option>";
            string percent20 = "<option value='" + v20 + "'>" + "20% ---  $" + string.Format("{0} tip", v20) + "</option>";
            string percent21 = "<option value='" + v21 + "'>" + "21% ---  $" + string.Format("{0} tip", v21) + "</option>";
            string percent22 = "<option value='" + v22 + "'>" + "22% ---  $" + string.Format("{0} tip", v22) + "</option>";
            string percent23 = "<option value='" + v23 + "'>" + "23% ---  $" + string.Format("{0} tip", v23) + "</option>";
            string percent24 = "<option value='" + v24 + "'>" + "24% ---  $" + string.Format("{0} tip", v24) + "</option>";
            string percent25 = "<option value='" + v25 + "'>" + "25% ---  $" + string.Format("{0} tip", v25) + "</option>";

            string int0 = "<option value='" + baseint.ToString("N2") + "'>" + "$" + baseint.ToString("N2") + " --- tip" + "</option>";
            string int1 = "<option value='" + (baseint + 1.00m).ToString("N2") + "'>" + "$" + (baseint + 1.00m).ToString("N2") + " --- tip" + "</option>";
            string int2 = "<option value='" + (baseint + 2.00m).ToString("N2") + "'>" + "$" + (baseint + 2.00m).ToString("N2") + " --- tip" + "</option>";
            string int3 = "<option value='" + (baseint + 3.00m).ToString("N2") + "'>" + "$" + (baseint + 3.00m).ToString("N2") + " --- tip" + "</option>";
            string int4 = "<option value='" + (baseint + 4.00m).ToString("N2") + "'>" + "$" + (baseint + 4.00m).ToString("N2") + " --- tip" + "</option>";
            string int5 = "<option value='" + (baseint + 5.00m).ToString("N2") + "'>" + "$" + (baseint + 5.00m).ToString("N2") + " --- tip" + "</option>";
            string int6 = "<option value='" + (baseint + 6.00m).ToString("N2") + "'>" + "$" + (baseint + 6.00m).ToString("N2") + " --- tip" + "</option>";
            string int7 = "<option value='" + (baseint + 7.00m).ToString("N2") + "'>" + "$" + (baseint + 7.00m).ToString("N2") + " --- tip" + "</option>";
            string int8 = "<option value='" + (baseint + 8.00m).ToString("N2") + "'>" + "$" + (baseint + 8.00m).ToString("N2") + " --- tip" + "</option>";
            string int9 = "<option value='" + (baseint + 9.00m).ToString("N2") + "'>" + "$" + (baseint + 9.00m).ToString("N2") + " --- tip" + "</option>";

                  string btnShow = "show";
                  if (cart.Lines.Sum(x => x.Quantity) == 0 || (cart.OrderMinimum > cart.SubTotal() && cart.IsDelivery) || (bi.Delivery == false && cart.IsDelivery))
                  {
                      btnShow = "off";
                  }

                ShoppingCartItem sci = cart.GetCartLineByItemId(itemId);
                decimal productSubTotal = sci.ItemTotal;
                decimal cartTax = cart.Tax();
                decimal subTotal = cart.SubTotal();
                decimal cartTotal = cart.Total();
                int cartItems = cart.TotalItems;
                return Json(new
                {
                    timeexp=timeout,
                    couponDD=sb.ToString(),
                    delMin = cart.OrderMinimum.ToString("N2"),
                    isDelivery=cart.IsDelivery? "delivery" : "pickup",
                    proid = itemId,
                    qutty = quty.ToString(),
                    total = ToUSD(cartTotal.ToString("N2")),
                    cartitems = cartItems.ToString(),
                    carttax = ToUSD(cartTax.ToString("N2")),
                    subtotal = ToUSD(subTotal.ToString("N2")),
                    productsubtotal = ToUSD(productSubTotal.ToString("N2")),
                    tip15 = percent15,
                    tip16 = percent16,
                    tip17 = percent17,
                    tip18 = percent18,
                    tip19 = percent19,
                    tip20 = percent20,
                    tip21 = percent21,
                    tip22 = percent22,
                    tip23 = percent23,
                    tip24 = percent24,
                    tip25 = percent25,
                    inttip0 = int0,
                    inttip1 = int1,
                    inttip2 = int2,
                    inttip3 = int3,
                    inttip4 = int4,
                    inttip5 = int5,
                    inttip6 = int6,
                    inttip7 = int7,
                    inttip8 = int8,
                    inttip9 = int9,
                    couponshow = couponShow,
                    btnshow=btnShow
                });
            }
            return Json(new { total = ToUSD(cart.Total().ToString("N2")) });
        }
        [HttpPost]
        public ActionResult ProcessCoupon(ShoppingCart cart, string coupon="")
        {

            string timeout = "timein";
            BizInfo bi= BizInfoRepository.GetBizInfoById(cart.BizId);
            if (bi == null)
            {
                timeout = "timeout";
                return Json(new
                {
                    timeexp = timeout
                });
            }

            cart.CouponChoice = coupon;
            string[] words = { "", "" };
            if (coupon != "" && coupon != "Apply a coupon? No, Thanks" && coupon.Contains(";"))
            {
                words = coupon.Split(';');
            }
            int discountpercent = 0;
            if (int.TryParse(words[0], out discountpercent))
            {
                cart.CouponChoice = words[0];// DiscountPercentage
                cart.CurrentDiscountMini = decimal.Parse(words[1]);//discount order Minimum
                cart.DiscountPercentage = discountpercent;
                cart.FreeItem = "";
                cart.DiscountCoupon = true;
                cart.FreeCoupon = false;
            }
            else if (coupon == "" || coupon == "Apply a coupon? No, Thanks")
            {
                cart.CouponChoice = "";
                cart.CurrentDiscountMini = 999999.0m;//discount order Minimum
                cart.DiscountPercentage = 0;
                cart.FreeItem = "";
                cart.DiscountCoupon = false;
                cart.FreeCoupon = false;
            }
            else
            {
                cart.CurrentDiscountMini = 999999.0m;//discount order Minimum
                cart.DiscountPercentage = 0;
                cart.FreeItem = coupon;
                cart.DiscountCoupon = false;
                cart.FreeCoupon = true;
            }


            string btnShow = "show";
            if (cart.Lines.Sum(x => x.Quantity) == 0 || (cart.OrderMinimum > cart.SubTotal() && cart.IsDelivery) || (bi.Delivery == false && cart.IsDelivery))
            {
                btnShow = "off";
            }

            decimal cartTax = cart.Tax();
            decimal subTotal = cart.SubTotal();
            decimal cartTotal = cart.Total();
            return Json(new
            {
                timeexp = timeout,
                delMin = cart.OrderMinimum.ToString("N2"),
                isDelivery = cart.IsDelivery ? "delivery" : "pickup",
                total = ToUSD(cartTotal.ToString("N2")),
                carttax = ToUSD(cartTax.ToString("N2")),
                subtotal = ToUSD(subTotal.ToString("N2")),
                btnshow = btnShow
            });
        }
        [HttpPost]
        public ActionResult ProcessTip(ShoppingCart cart, string tip = "0.00")
        {
            string timeout = "timein";
            BizInfo bi= BizInfoRepository.GetBizInfoById(cart.BizId);
            if (bi == null)
            {
                timeout = "timeout";
                return Json(new
                {
                    timeexp = timeout
                });
            }

            decimal tipV = 0.0m;
            decimal result = 0.0m;
            if (decimal.TryParse(tip,out tipV))
            {
                result = tipV;
            }
            cart.DriverTip = result;

            string btnShow = "show";
            if (cart.Lines.Sum(x => x.Quantity) == 0 || (cart.OrderMinimum > cart.SubTotal() && cart.IsDelivery) || (bi.Delivery == false && cart.IsDelivery))
            {
                btnShow = "off";
            }
            return Json(new
            {
                timeexp = timeout,
                carttotal = ToUSD(cart.Total().ToString("N2")),
                btnshow = btnShow
            });
        }
        public ActionResult RemoveFromCart(ShoppingCart cart, string itemId)
        {
            HandleCart(cart);
            AddItemModel model = new AddItemModel();
            if (string.IsNullOrEmpty(itemId) == false)
            {
                cart.DeleteProduct(itemId);
                ShoppingCartItem sci = cart.GetCartLineByItemId(itemId);
                model.Cart = cart;
            }
                cart.DriverTip = 0.0m;
            
            model.BizInfo = BizInfoRepository.GetBizInfoById(cart.BizId);

            if (model.BizInfo == null)
            {
                return Redirect("~/Home");
            }
            if (cart.CurrentDiscountMini>cart.SubTotal())
            {
                cart.CouponChoice = "";
                cart.CurrentDiscountMini = 999999.0m;//discount order Minimum
                cart.DiscountPercentage = 0;
                cart.FreeItem = "";
                cart.DiscountCoupon = false;
                cart.FreeCoupon = false;
            }
            List<DiscountCoupon> ldc = new List<DiscountCoupon>();
            List<FreeItemCoupon> lfc = new List<FreeItemCoupon>();
            if (model.BizInfo.HasDiscountCoupons)
            {
                ldc = DiscountCouponRepository.GetBizDiscountCouponsByMinimum(cart.BizId, cart.SubTotal(), true);
            }
            model.DiscountCouponList = ldc;
            if (model.BizInfo.HasFreeItemCoupons)
            {
                lfc = FreeItemCouponRepository.GetBizFreeItemCouponsByMinimum(cart.BizId, cart.SubTotal(), true);
            }
            model.FreeItemCouponList = lfc;
            ViewBag.items = cart.TotalItems == 0 ? string.Empty : cart.TotalItems.ToString();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult UpdateDelivery(string del,ShoppingCart cart, BrowseHistory bh)
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

            bh.IsDelivery = del == "true" ? true : false;
            cart.IsDelivery = bh.IsDelivery;
            cart.DriverTip =cart.IsDelivery?cart.SubTotal() * 0.15m: 0.00m;
            string btnShow = "show";
            if (cart.Lines.Sum(x => x.Quantity) == 0 || (cart.OrderMinimum > cart.SubTotal() && cart.IsDelivery) || (bi.Delivery == false && cart.IsDelivery))
            {
                btnShow = "off";
            }

            decimal baseint = cart.IsDelivery ? Decimal.Round(cart.SubTotal() * 0.15m) + 1 : 2.0m;
            string int0 = "<option value='" + baseint.ToString("N2") + "'>" + "$" + baseint.ToString("N2") + " --- tip" + "</option>";
            string int1 = "<option value='" + (baseint + 1.00m).ToString("N2") + "'>" + "$" + (baseint + 1.00m).ToString("N2") + " --- tip" + "</option>";
            string int2 = "<option value='" + (baseint + 2.00m).ToString("N2") + "'>" + "$" + (baseint + 2.00m).ToString("N2") + " --- tip" + "</option>";
            string int3 = "<option value='" + (baseint + 3.00m).ToString("N2") + "'>" + "$" + (baseint + 3.00m).ToString("N2") + " --- tip" + "</option>";
            string int4 = "<option value='" + (baseint + 4.00m).ToString("N2") + "'>" + "$" + (baseint + 4.00m).ToString("N2") + " --- tip" + "</option>";
            string int5 = "<option value='" + (baseint + 5.00m).ToString("N2") + "'>" + "$" + (baseint + 5.00m).ToString("N2") + " --- tip" + "</option>";
            string int6 = "<option value='" + (baseint + 6.00m).ToString("N2") + "'>" + "$" + (baseint + 6.00m).ToString("N2") + " --- tip" + "</option>";
            string int7 = "<option value='" + (baseint + 7.00m).ToString("N2") + "'>" + "$" + (baseint + 7.00m).ToString("N2") + " --- tip" + "</option>";
            string int8 = "<option value='" + (baseint + 8.00m).ToString("N2") + "'>" + "$" + (baseint + 8.00m).ToString("N2") + " --- tip" + "</option>";
            string int9 = "<option value='" + (baseint + 9.00m).ToString("N2") + "'>" + "$" + (baseint + 9.00m).ToString("N2") + " --- tip" + "</option>";

            decimal subTotal = cart.SubTotal();
            decimal cartTotal = cart.Total();
            return Json(new
            {
                timeexp = timeout,
                delFee = cart.DeliveryFee.ToString("N2"),
                delMin = cart.OrderMinimum.ToString("N2"),
                isDelivery = cart.IsDelivery ? "delivery" : "pickup",
                total = subTotal <= 0 ? "$0.00" : ToUSD(cartTotal.ToString("N2")),
                subtotal = ToUSD(subTotal.ToString("N2")),
                basevalue = cart.DriverTip.ToString("N2"),
                inttip0 = int0,
                inttip1 = int1,
                inttip2 = int2,
                inttip3 = int3,
                inttip4 = int4,
                inttip5 = int5,
                inttip6 = int6,
                inttip7 = int7,
                inttip8 = int8,
                inttip9 = int9,
                btnshow = btnShow
            });

        }
        [HttpPost]
        public ActionResult loadImage(int ProductID)
        {
            Product p = ProductRepository.GetProductById(ProductID);
            return PartialView(p);
        }
    }
}
