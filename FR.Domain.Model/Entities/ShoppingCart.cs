using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Configuration;

namespace FR.Domain.Model.Entities
{
    [Serializable()]
    public class RealTimeOrder
    {
        public string UserName { get; set; }
        public string BizTitle { get; set; }
        public string OrderId { get; set; }
        public string OrderList { get; set; }
    }

    [Serializable()]
    public class MySharedInfo
    {
        private string _bossName = string.Empty;
        public string BossName
        {
            get
            {
                return this._bossName;
            }
            set
            {
                this._bossName = value;
            }
        }
        private string _sharedCartId = string.Empty;
        public string SharedCartId
        {
            get
            {
                return this._sharedCartId;
            }
            set
            {
                this._sharedCartId = value;
            }
        }
        private string _myLabelName = string.Empty;
        public string MyLabelName
        {
            get
            {
                return this._myLabelName;
            }
            set
            {
                this._myLabelName = value;
            }
        }
    }
    [Serializable()]
    public class CheckBoxViewModel
    {
        public string BoxName { get; set; }
        public decimal BoxPrice { get; set; }
        public decimal BoxIncrementValue { get; set; }
        public bool Checked { get; set; }
    }
    [Serializable()]
    public class AddSideCheckBoxModel
    {
        public string BoxName { get; set; }
        public decimal BoxPrice { get; set; }
        public decimal BoxBizPrice { get; set; }
        public bool Checked { get; set; }
    }

    [Serializable()]
    public class ShoppingCartItem
    {
        private string _labelName = string.Empty;
        private string _itemId = string.Empty;
        private int _id = 0;
        private int _quantity = 1;
        private bool _isReorderItem = false;
        private decimal _reExtraPriceTotal = 0.0m;
        private decimal _reAddSideTotal = 0.0m;
        private decimal _reBizAddSideTotal = 0.0m;
        private decimal _reItemTotal = 0.0m;
        private string _reSelectedToppings = string.Empty;
        private string _reSelectedAddSides = string.Empty;

        private string _title = string.Empty;
        private decimal _bizUnitPrice;
        private decimal _bizFinalPrice;
        private decimal _unitPrice;
        private decimal _finalPrice;
        private int _discountPercentage = 0;
        private string _productImg = "";
        private string _instruction = string.Empty;
        private bool _isSpicy = false;
        private string _howSpicy = string.Empty;// regular
        private bool _isFamilyMeal = false;
        private string _sideChoice = string.Empty;
        private string _sauceChoice = string.Empty;
        private string _productSizeTitle = string.Empty;
        private string _productSize = string.Empty; //SizeId Title
        private decimal _productSizePrice = 0.0m;
        private decimal _bizSizePrice = 0.0m;
        private string _selectedFreeToppings = string.Empty;
        private List<CheckBoxViewModel> _toppingList = new List<CheckBoxViewModel>();
        private List<CheckBoxViewModel> _dressingList = new List<CheckBoxViewModel>();
        private List<AddSideCheckBoxModel> _addSideList = new List<AddSideCheckBoxModel>();
        private string _dressingChoice = string.Empty;
        private string _crustChoice = string.Empty;
        private decimal _crustChoicePrice = 0.0m;
        private decimal _crustChoiceBizPrice = 0.0m;
        private string _cheeseChoice = string.Empty;
        private decimal _cheeseChoicePrice = 0.0m;
        private decimal _cheeseChoiceBizPrice = 0.0m;


        public ShoppingCartItem() { }
        public ShoppingCartItem(string vItemId, int id, string labelName, int quantity, bool isReorderItem, decimal reExtraPriceTotal, decimal reAddSideTotal, decimal reBizAddSideTotal, decimal reItemTotal, string reSelectedToppings,
            string reSelectedAddSides, string title, decimal bizUnitPrice, decimal bizFinalPrice, decimal unitPrice, decimal finalPrice,
            int discountPercentage, string productImg, string instruction, bool isSpicy, string howSpicy, bool isFamilyMeal, string sideChoice, string sauceChoice, string productSizeTitle, string productSize, decimal productSizePrice,
            decimal bizSizePrice, string selectedFreeToppings, List<CheckBoxViewModel> vToppingList, List<CheckBoxViewModel> vDressingList, List<AddSideCheckBoxModel> vAddSideList, string vDressingChoice,
            string crustChoice, decimal crustChoicePrice, decimal crustChoiceBizPrice, string cheeseChoice, decimal cheeseChoicePrice, decimal cheeseChoiceBizPrice)
        {
            this.LabelName = labelName;
            this.ProductSizeTitle = productSizeTitle;
            this.ProductSize = productSize;
            this.ProductSizePrice = productSizePrice;
            this.BizSizePrice = bizSizePrice;
            this.ToppingList = vToppingList;
            this.DressingList = vDressingList;
            this.AddSideList = vAddSideList;

            this.ItemId = vItemId;
            this.ID = id;
            this.Quantity = quantity;
            this.IsReorderItem = isReorderItem;
            this.ReExtraPriceTotal = reExtraPriceTotal;
            this.ReAddSideTotal = reAddSideTotal;
            this.ReBizAddSideTotal = reBizAddSideTotal;
            this.ReItemTotal = reItemTotal;
            this.ReSelectedToppings = reSelectedToppings;
            this.ReSelectedAddSides = ReSelectedAddSides;

            this.Title = title;
            this.BizUnitPrice = bizUnitPrice;
            this.BizFinalPrice = bizFinalPrice;
            this.UnitPrice = unitPrice;
            this.FinalPrice = finalPrice;
            this.DiscountPercentage = discountPercentage;
            this.ProductImg = productImg;
            this.Instruction = instruction;
            this.IsSpicy = isSpicy;
            this.HowSpicy = howSpicy;

            this.IsFamilyMeal = isFamilyMeal;
            this.SelectedFreeToppings = selectedFreeToppings;
            this.SideChoice = sideChoice;
            this.SauceChoice = sauceChoice;
            this.DressingChoice = vDressingChoice;
            this.CrustChoice = crustChoice;
            this.CrustChoicePrice = crustChoicePrice;
            this.CrustChoiceBizPrice = crustChoiceBizPrice;
            this.CheeseChoice = cheeseChoice;
            this.CheeseChoicePrice = cheeseChoicePrice;
            this.CheeseChoiceBizPrice = cheeseChoiceBizPrice;

        }
        public string LabelName
        {
            get
            {
                return this._labelName;
            }
            set
            {
                this._labelName = value;
            }
        }
        public string ItemId
        {
            get
            {
                return this._itemId;
            }
            set
            {
                this._itemId = value;
            }
        }
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }
        public int Quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                this._quantity = value;
            }
        }

        public bool IsReorderItem
        {
            get
            {
                return this._isReorderItem;
            }
            set
            {
                this._isReorderItem = value;
            }
        }
        public decimal ReExtraPriceTotal
        {
            get
            {
                return this._reExtraPriceTotal;
            }
            set
            {
                this._reExtraPriceTotal = value;
            }
        }
        public decimal ReAddSideTotal
        {
            get
            {
                return this._reBizAddSideTotal;
            }
            set
            {
                this._reAddSideTotal = value;
            }
        }
        public decimal ReBizAddSideTotal
        {
            get
            {
                return this._reBizAddSideTotal;
            }
            set
            {
                this._reBizAddSideTotal = value;
            }
        }
        public decimal ReItemTotal
        {
            get
            {
                return this._reItemTotal;
            }
            set
            {
                this._reItemTotal = value;
            }
        }
        public string ReSelectedToppings
        {
            get
            {
                return this._reSelectedToppings;
            }
            set
            {
                this._reSelectedToppings = value;
            }
        }
        public string ReSelectedAddSides
        {
            get
            {
                return this._reSelectedAddSides;
            }
            set
            {
                this._reSelectedAddSides = value;
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }
        public decimal BizUnitPrice
        {
            get
            {
                return this._bizSizePrice > 0.0m ? this._bizSizePrice : this._bizUnitPrice;
            }
            set
            {
                this._bizUnitPrice = value;
            }
        }

        public decimal BizFinalPrice
        {
            get
            {
                return this._bizSizePrice > 0.0m ? this._bizSizePrice : this._bizFinalPrice; // ?
            }
            set
            {
                this._bizFinalPrice = value;
            }
        }
        public decimal UnitPrice
        {
            get
            {
                return this._productSizePrice > 0.0m ? this._productSizePrice : this._unitPrice;
            }
            set
            {
                this._unitPrice = value;
            }
        }

        public decimal FinalPrice
        {
            get
            {
                return this._productSizePrice > 0.0m ? this._productSizePrice : this._finalPrice; // ?
            }
            set
            {
                this._finalPrice = value;
            }
        }

        public decimal ExtraPriceTotal
        {
            get
            {

                if (IsReorderItem)
                {
                    return ReExtraPriceTotal;
                }
                if (ToppingList.Count > 0)
                {
                    return ToppingList.Sum(e => e.BoxPrice);
                }
                if (DressingList.Count > 0)
                {
                    return DressingList.Sum(e => e.BoxPrice);
                }
                return 0.0m;
            }
        }
        public decimal AddSidePriceTotal
        {
            get
            {

                if (IsReorderItem)
                {
                    return ReAddSideTotal;
                }
                if (AddSideList.Count > 0)
                {
                    return AddSideList.Sum(e => e.BoxPrice);
                }
                return 0.0m;
            }
        }
        public decimal BizAddSidePriceTotal
        {
            get
            {

                if (IsReorderItem)
                {
                    return ReBizAddSideTotal;
                }
                if (AddSideList.Count > 0)
                {
                    return AddSideList.Sum(e => e.BoxBizPrice);
                }
                return 0.0m;
            }
        }

        public string ToppingListNote
        {
            get
            {
                if (IsReorderItem)
                {
                    return ReSelectedToppings.Replace(";", "</br>");
                }
                string str = string.Empty;
                foreach (var t in this.ToppingList)
                {
                    str = str + t.BoxName + ": $" + t.BoxPrice.ToString("N2") + "<br/>"; ;
                }
                return str;
            }
        }
        public string AddSideListNote
        {
            get
            {
                if (IsReorderItem)
                {
                    return ReSelectedAddSides.Replace(";", "</br>");
                }
                string str = string.Empty;
                foreach (var t in this.AddSideList)
                {
                    str = str + "Add " + t.BoxName + ": $" + t.BoxPrice.ToString("N2") + "<br/>"; ;
                }
                return str;
            }
        }
        public string SelectedToppings
        {
            get
            {
                if (IsReorderItem)
                {
                    return ReSelectedToppings;
                }
                string str = string.Empty;
                foreach (var t in this.ToppingList)
                {
                    str = str + t.BoxName + ": $" + t.BoxPrice.ToString("N2") + ";";
                }
                if (string.IsNullOrEmpty(str) == false)
                {
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
        }
        public string SelectedAddSides
        {
            get
            {
                if (IsReorderItem)
                {
                    return ReSelectedAddSides;
                }
                string str = string.Empty;
                foreach (var t in this.AddSideList)
                {
                    str = str + "Add " + t.BoxName + ": $" + t.BoxPrice.ToString("N2") + ";";
                }
                if (string.IsNullOrEmpty(str) == false)
                {
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
        }
        public int DiscountPercentage
        {
            get
            {
                return this._discountPercentage;
            }
            set
            {
                this._discountPercentage = value;
            }
        }

        public string ProductImg
        {
            get
            {
                return this._productImg;
            }
            set
            {
                this._productImg = value;
            }
        }

        public decimal BizItemPrice
        {
            get
            {
                return this.BizFinalPrice + this.ExtraPriceTotal + this.BizAddSidePriceTotal + this.CrustChoiceBizPrice + this._cheeseChoiceBizPrice;
            }
        }
        public decimal BizItemTotal
        {
            get
            {
                return BizItemPrice * this._quantity;
            }
        }

        public decimal ItemPrice
        {
            get
            {
                return this.FinalPrice + this.ExtraPriceTotal + this.AddSidePriceTotal + this.CrustChoicePrice + this._cheeseChoicePrice;
            }
        }
        public decimal ItemTotal
        {
            get
            {
                if (IsReorderItem)
                {
                    return ReItemTotal;
                }
                return ItemPrice * this._quantity;
            }
        }
        public string Instruction
        {
            get
            {
                return this._instruction;
            }
            set
            {
                this._instruction = value;
            }
        }
        public bool IsSpicy
        {
            get
            {
                return this._isSpicy;
            }
            set
            {
                this._isSpicy = value;
            }
        }
        public string HowSpicy
        {
            get
            {
                return this._howSpicy;
            }
            set
            {
                this._howSpicy = value;
            }
        }

        public bool IsFamilyMeal
        {
            get
            {
                return this._isFamilyMeal;
            }
            set
            {
                this._isFamilyMeal = value;
            }
        }
        public string SelectedFreeToppings
        {
            get
            {
                return this._selectedFreeToppings;
            }
            set
            {
                this._selectedFreeToppings = value;
            }
        }
        public string SideChoice
        {
            get
            {
                return this._sideChoice;
            }
            set
            {
                this._sideChoice = value;
            }
        }
        public string SauceChoice
        {
            get
            {
                return this._sauceChoice;
            }
            set
            {
                this._sauceChoice = value;
            }
        }
        public string DressingChoice
        {
            get
            {
                return this._dressingChoice;
            }
            set
            {
                this._dressingChoice = value;
            }
        }
        public string ProductSizeTitle
        {
            get
            {
                return this._productSizeTitle;
            }
            set
            {
                this._productSizeTitle = value;
            }
        }
        public string ProductSize
        {
            get
            {
                return this._productSize;
            }
            set
            {
                this._productSize = value;
            }
        }
        public decimal ProductSizePrice
        {
            get
            {
                return this._productSizePrice;
            }
            set
            {
                this._productSizePrice = value;
            }
        }
        public decimal BizSizePrice
        {
            get
            {
                return this._bizSizePrice;
            }
            set
            {
                this._bizSizePrice = value;
            }
        }
        public List<CheckBoxViewModel> ToppingList
        {
            get
            {
                return this._toppingList;
            }
            set
            {
                this._toppingList = value;
            }
        }
        public List<CheckBoxViewModel> DressingList
        {
            get
            {
                return this._dressingList;
            }
            set
            {
                this._dressingList = value;
            }
        }
        public List<AddSideCheckBoxModel> AddSideList
        {
            get
            {
                return this._addSideList;
            }
            set
            {
                this._addSideList = value;
            }
        }
        public string CrustChoice
        {
            get
            {
                return this._crustChoice;
            }
            set
            {
                this._crustChoice = value;
            }
        }
        public decimal CrustChoicePrice
        {
            get
            {
                return this._crustChoicePrice;
            }
            set
            {
                this._crustChoicePrice = value;
            }
        }
        public decimal CrustChoiceBizPrice
        {
            get
            {
                return this._crustChoiceBizPrice;
            }
            set
            {
                this._crustChoiceBizPrice = value;
            }
        }
        public string CheeseChoice
        {
            get
            {
                return this._cheeseChoice;
            }
            set
            {
                this._cheeseChoice = value;
            }
        }
        public decimal CheeseChoicePrice
        {
            get
            {
                return this._cheeseChoicePrice;
            }
            set
            {
                this._cheeseChoicePrice = value;
            }
        }
        public decimal CheeseChoiceBizPrice
        {
            get
            {
                return this._cheeseChoiceBizPrice;
            }
            set
            {
                this._cheeseChoiceBizPrice = value;
            }
        }
    }

    [Serializable()]
    public class ShoppingCart
    {
        private bool _isFinalSharedCart = false;

        public bool IsFinalSharedCart
        {
            get { return this._isFinalSharedCart; }
            set { this._isFinalSharedCart = value; }
        }
        private string _personName = string.Empty;
        public string PersonName
        {
            get
            {
                return this._personName;
            }
            set
            {
                this._personName = value;
            }
        }
        private string _cartkey = string.Empty;
        public string CartKey
        {
            get
            {
                return this._cartkey;
            }
            set
            {
                this._cartkey = value;
            }
        }
        private string _bossName = string.Empty;
        public string BossName
        {
            get
            {
                return this._bossName;
            }
            set
            {
                this._bossName = value;
            }
        }
        private int _itemNum = 0;
        public int ItemNum
        {
            get
            {
                return this._itemNum;
            }
            set
            {
                this._itemNum = value;
            }
        }
        private int _bizId = 0;
        public int BizId
        {
            get
            {
                return this._bizId;
            }
            set
            {
                this._bizId = value;
            }
        }
        private string _bizName = string.Empty;
        public string BizName
        {
            get
            {
                return this._bizName;
            }
            set
            {
                this._bizName = value;
            }
        }
        private bool _isFinishedShareOrdering = false;

        public bool IsFinishedShareOrdering
        {
            get { return this._isFinishedShareOrdering; }
            set { this._isFinishedShareOrdering = value; }
        }
        private string _scheduleDate = "Today";

        public string ScheduleDate
        {
            get { return this._scheduleDate; }
            set { this._scheduleDate = value; }
        }
        private string _scheduleTime = "ASAP";

        public string ScheduleTime
        {
            get { return this._scheduleTime; }
            set { this._scheduleTime = value; }
        }
        private bool _isBizDelivery = false;

        public bool IsBizDelivery
        {
            get { return this._isBizDelivery; }
            set { this._isBizDelivery = value; }
        }
        private bool _isDelivery = true;

        public bool IsDelivery
        {
            get { return this._isDelivery; }
            set { this._isDelivery = value; }
        }
        private bool _freeCoupon = false;

        public bool FreeCoupon
        {
            get { return this._freeCoupon; }
            set { this._freeCoupon = value; }
        }
        private string _freeItem = string.Empty;

        public string FreeItem
        {
            get { return this._freeItem; }
            set { this._freeItem = value; }
        }
        private bool _discountCoupon = false;

        public bool DiscountCoupon
        {
            get { return this._discountCoupon; }
            set { this._discountCoupon = value; }
        }
        private int _discountPercentage = 0;

        public int DiscountPercentage
        {
            get { return this._discountPercentage; }
            set { this._discountPercentage = value; }
        }
        private string _couponChoice = string.Empty;

        public string CouponChoice
        {
            get { return this._couponChoice; }
            set { this._couponChoice = value; }
        }
        private decimal _currentDiscountMini = 999999.0m;

        public decimal CurrentDiscountMini
        {
            get { return this._currentDiscountMini; }
            set { this._currentDiscountMini = value; }
        }
        private decimal _taxRate = decimal.Parse(ConfigurationManager.AppSettings["defaultTaxRate"].ToString());
        public decimal TaxRate
        {
            get { return this._taxRate; }
            set { this._taxRate = value; }
        }
        private decimal _orderMinimum = 0.0M;
        public decimal OrderMinimum
        {
            get { return this._orderMinimum; }
            set { this._orderMinimum = value; }
        }
        private decimal _deliveryFee = 0.0M;
        public decimal DeliveryFee
        {
            get { return this._deliveryFee; }
            set { this._deliveryFee = value; }
        }
        private decimal _driverTip = 0.0M;
        public decimal DriverTip
        {
            get {return _driverTip==0.0M && this.IsDelivery? SubTotal()*0.15M:  this._driverTip; }
            set { this._driverTip = value; }
        }

        public decimal BizDiscountAmount
        {
            get
            {
                return (this.DiscountCoupon && this.DiscountPercentage > 0 && CurrentDiscountMini < 999990.0m) ? (this.BizSubTotal() * this.DiscountPercentage / 100) : 0.0m;
            }
        }
        public decimal DiscountAmount
        {
            get
            {
                return (this.DiscountCoupon && this.DiscountPercentage > 0 && CurrentDiscountMini < 999990.0m) ? (this.SubTotal() * this.DiscountPercentage / 100) : 0.0m;
            }
        }

        private decimal _serviceCharge = decimal.Parse(WebConfigurationManager.AppSettings["serviceCharge"].ToString());
        public decimal serviceCharge
        {
            get { return this._serviceCharge; }
            set { this._serviceCharge = value; }
        }
        /*
        public decimal serviceCharge
        {
            get { return decimal.Parse(WebConfigurationManager.AppSettings["serviceCharge"].ToString()); }
        }
         */
        private List<ShoppingCartItem> _items = new List<ShoppingCartItem>();

        public IEnumerable<ShoppingCartItem> Lines
        {
            get { return this._items; }
        }
        public List<ShoppingCartItem> Items
        {
            get { return this._items; }
            set { this._items = value; }
        }
        public ShoppingCartItem GetCartLineByItemId(string vItemId)
        {
            ShoppingCartItem line = new ShoppingCartItem();
            line = _items
             .Where(p => p.ItemId == vItemId)
             .FirstOrDefault();
            return line;
        }
        public List<ShoppingCartItem> GetCartLinesByProduct(Product product)
        {
            return _items.Where(p => p.ID == product.ProductId).ToList();
        }
        public List<ShoppingCartItem> GetCartLinesByProductID(int vProductID)
        {
            return _items.Where(p => p.ID == vProductID).ToList();
        }

        public string ToUSD(string m)
        {
            return "$" + m;
        }
        public decimal BizSubTotal()
        {
            return _items.Sum(e => (e.BizItemTotal));
        }
        public decimal SubTotal()
        {
            return _items.Sum(e => (e.ItemTotal));
        }
        public decimal Tax()
        {
            return (SubTotal() - DiscountAmount) * (TaxRate / 100);
        }
        // Gets the sum total of the items' prices
        public decimal BizTotal()
        {
            return (BizSubTotal() - BizDiscountAmount) + (this.IsDelivery ? DeliveryFee : 0.0m) + DriverTip;// DeliveryFee  belongs to delivery-company and  DriverTip belongs to delivry-guy
        }
        public decimal Total()
        {
            return (SubTotal() - DiscountAmount) + Tax() + serviceCharge + (this.IsDelivery ? DeliveryFee : 0.0m) + DriverTip;
        }
        public int TotalItems
        {
            get
            {
                return _items.Sum(x => x.Quantity);
            }
        }
        // Adds a new item to the shopping cart

        public void InsertItem(int vId, int vQuantity, string vLabelName, bool vIsReorderItem, decimal vReExtraPriceTotal, decimal vReAddSideTotal, decimal vReBizAddSideTotal, decimal vReItemTotal, string vReSelectedToppings,
           string vReSelectedAddSides, string vTitle, decimal vBizUnitPrice, decimal vBizFinalPrice, decimal vUnitPrice, decimal vFinalPrice,
                               int vDiscountPercentage, string vProductImg, string vInstruction, bool vIsSpicy,
                               string vHowSpicy, bool vIsFamilyMeal, string vSideChoice, string vSauceChoice, string vProductSizeTitle,
                               string vProductSize, decimal vProductSizePrice, decimal vBizSizePrice, string vSelectedFreeToppings, List<CheckBoxViewModel> vToppingList,
                                List<CheckBoxViewModel> vDressingList, List<AddSideCheckBoxModel> vAddSideList, string vDressingChoice,
                               string vCrustChoice, decimal vCrustChoicePrice, decimal vCrustChoiceBizPrice, string vCheeseChoice, decimal vCheeseChoicePrice, decimal vCheeseChoiceBizPrice)
        {
            if (vQuantity <= 0)
            {
                vQuantity = 0;
            }
            else if (vQuantity > 20)
            {
                vQuantity = 20;
            }
            if (vQuantity > 0)
            {
                _itemNum++;
                string id = _itemNum.ToString();
                ShoppingCartItem sci = new ShoppingCartItem(id, vId, vLabelName, vQuantity, vIsReorderItem, vReExtraPriceTotal,
                    vReAddSideTotal, vReBizAddSideTotal, vReItemTotal, vReSelectedToppings, vReSelectedAddSides, vTitle,
                    vBizUnitPrice, vBizFinalPrice, vUnitPrice, vFinalPrice, vDiscountPercentage, vProductImg, vInstruction,
                    vIsSpicy, vHowSpicy, vIsFamilyMeal, vSideChoice, vSauceChoice, vProductSizeTitle, vProductSize, vProductSizePrice,
                    vBizSizePrice, vSelectedFreeToppings, vToppingList, vDressingList, vAddSideList, vDressingChoice, vCrustChoice, vCrustChoicePrice, vCrustChoiceBizPrice,
                    vCheeseChoice, vCheeseChoicePrice, vCheeseChoiceBizPrice);
                _items.Add(sci);
            }
        }

        // Removes an item from the shopping cart
        public void DeleteItem(string vItemId)
        {
            DeleteProduct(vItemId);
        }

        // Removes all items of a specified product from the shopping cart
        public void DeleteProduct(string vItemId)
        {
            ShoppingCartItem line = Items
             .Where(i => i.ItemId == vItemId)
             .FirstOrDefault();
            if (line != null)
            {
                _items.RemoveAll(l => l.ItemId == vItemId);
            }
        }

        // Updates the quantity for an item
        public void UpdateItemQuantity(string vItemId, int vQuantity)
        {
            if (vQuantity <= 0)
            {
                vQuantity = 0;
            }
            if (vQuantity > 20)
            {
                vQuantity = 20;
            }
            ShoppingCartItem line = Items
             .Where(i => i.ItemId == vItemId)
             .FirstOrDefault();
            if (line != null)
            {
                line.Quantity = vQuantity;
                if (line.Quantity <= 0) _items.RemoveAll(l => l.ItemId == vItemId);

            }
        }
        public int GetItemQuantity(string vItemId)
        {
            ShoppingCartItem line = _items
               .Where(i => i.ItemId == vItemId)
               .FirstOrDefault();
            if (line == null)
            {
                return 0;
            }
            return line.Quantity;
        }
        // Clears the cart
        public void Clear()
        {
            this._isFinalSharedCart = false;
            this._items.Clear();
            this._itemNum = 0;
            this._isDelivery = true;
            this._freeCoupon = false;
            this._discountCoupon = false;
            this._discountPercentage = 0;
            this._orderMinimum = 0.0m;
            this._deliveryFee = 0.0m;
            this._driverTip = 0.0m;
            this._freeItem = "";
            this._couponChoice = "";
            this._currentDiscountMini = 999999.0m;
            this._scheduleDate = "Today";
            this._scheduleTime = "ASAP";
            this._cartkey = string.Empty;
            this._bossName = string.Empty;
            this._personName = string.Empty;
            this._isFinishedShareOrdering = false;
            this._taxRate = decimal.Parse(ConfigurationManager.AppSettings["defaultTaxRate"].ToString());
        }
    }
    [Serializable()]
    public class SharedShoppingCart
    {

        private Dictionary<string, ShoppingCart> _partyCart = new Dictionary<string, ShoppingCart>();
        public Dictionary<string, ShoppingCart> PartyCart
        {
            get
            {
                return this._partyCart;
            }
            set
            {
                this._partyCart = value;
            }
        }
        private decimal _maxOrder = 999999.0m;

        public decimal MaxOrder
        {
            get { return this._maxOrder; }
            set { this._maxOrder = value; }
        }
        private List<BizInfo> _selectBizInfos = new List<BizInfo>();
        public List<BizInfo> SelectBizInfos
        {
            get
            {
                return this._selectBizInfos;
            }
            set
            {
                this._selectBizInfos = value;
            }
        }
        private string _sharedCartKey = string.Empty;
        public string SharedCartKey
        {
            get
            {
                return this._sharedCartKey;
            }
            set
            {
                this._sharedCartKey = value;
            }
        }
        private string _partyAddress = string.Empty;
        public string PartyAddress
        {
            get
            {
                return this._partyAddress;
            }
            set
            {
                this._partyAddress = value;
            }
        }
        private string _partyZip = string.Empty;
        public string PartyZip
        {
            get
            {
                return this._partyZip;
            }
            set
            {
                this._partyZip = value;
            }
        }
        private bool _isSharedCartLocked = false;
        public bool IsSharedCartLocked
        {
            get
            {
                return this._isSharedCartLocked;
            }
            set
            {
                this._isSharedCartLocked = value;
            }
        }
        private string _partyBossName = string.Empty;
        public string PartyBossName
        {
            get
            {
                return this._partyBossName;
            }
            set
            {
                this._partyBossName = value;
            }
        }
        private int _partyItemNum = 0;
        public int PartyItemNum
        {
            get
            {
                return this._partyItemNum;
            }
            set
            {
                this._partyItemNum = value;
            }
        }
        private string _partyScheduleDate = "Today";

        public string PartyScheduleDate
        {
            get { return this._partyScheduleDate; }
            set { this._partyScheduleDate = value; }
        }
        private string _partyScheduleTime = "ASAP";

        public string PartyScheduleTime
        {
            get { return this._partyScheduleTime; }
            set { this._partyScheduleTime = value; }
        }
        public ICollection PartCartItems
        {
            get { return _partyCart.Values; }
        }
        public ICollection PartyCartIDs
        {
            get { return _partyCart.Keys; }
        }
        private bool _isPartyDelivery = true;

        public bool IsPartyDelivery
        {
            get { return this._isPartyDelivery; }
            set { this._isPartyDelivery = value; }
        }
        private bool _partyFreeCoupon = false;

        public bool PartyFreeCoupon
        {
            get { return this._partyFreeCoupon; }
            set { this._partyFreeCoupon = value; }
        }
        private string _partyFreeItem = string.Empty;

        public string PartyFreeItem
        {
            get { return this._partyFreeItem; }
            set { this._partyFreeItem = value; }
        }
        private bool _partyDiscountCoupon = false;

        public bool PartyDiscountCoupon
        {
            get { return this._partyDiscountCoupon; }
            set { this._partyDiscountCoupon = value; }
        }
        private int _partyDiscountPercentage = 0;

        public int PartyDiscountPercentage
        {
            get { return this._partyDiscountPercentage; }
            set { this._partyDiscountPercentage = value; }
        }
        private string _partyCouponChoice = string.Empty;

        public string PartyCouponChoice
        {
            get { return this._partyCouponChoice; }
            set { this._partyCouponChoice = value; }
        }
        private decimal _partyCurrentDiscountMini = 999999.0m;

        public decimal PartyCurrentDiscountMini
        {
            get { return this._partyCurrentDiscountMini; }
            set { this._partyCurrentDiscountMini = value; }
        }
        public decimal PartyDeliveryFee()
        {
            var bz = from b in this.PartyCart.Values
                     orderby b.BizId
                     group b by b.BizId into g
                     select new { Bizid = g.Key, Group = g, Count = g.Count() };
            decimal fee = 0.0m;
             foreach (var item in bz)
            {
                fee = fee + item.Group.FirstOrDefault().DeliveryFee;
            }
             return fee;
        }
        public decimal PartyDriverTip()
        {
            return this.IsPartyDelivery ? this.PartySubTotal() * decimal.Parse(ConfigurationManager.AppSettings["groupTip"].ToString()) : 0.00m;
        }
        public decimal PartyOrderMinimum()
        {
            var bz = from b in this.PartyCart.Values
                     orderby b.BizId
                     group b by b.BizId into g
                     select new { Bizid = g.Key, Group = g, Count = g.Count() };
            decimal fee = 0.0m;
            foreach (var item in bz)
            {
                fee = fee + item.Group.FirstOrDefault().OrderMinimum;
            }
            return fee;
        }
        private decimal _partyTaxRate = decimal.Parse(ConfigurationManager.AppSettings["defaultTaxRate"].ToString());
        public decimal PartyTaxRate
        {
            get { return this._partyTaxRate; }
            set { this._partyTaxRate = value; }
        }
        public void UpdatePartCart(string vKey, ShoppingCart vCart)
        {
            if (this.PartyCart.ContainsKey(vKey) && this.IsSharedCartLocked == false)
            {
                this.PartyCart[vKey] = vCart;
            }
        }
        public void DeleteCart(string vKey)
        {
            if (this.PartyCart.ContainsKey(vKey) && this.IsSharedCartLocked == false)
            {
                this.PartyCart.Remove(vKey);
            }
        }

        public decimal PartyBizSubTotal()
        {
            return this.PartyCart.Sum(e => e.Value.BizSubTotal());
        }
        public decimal PartySubTotal()
        {
            return this.PartyCart.Sum(e => e.Value.SubTotal());
        }
        public decimal PartyDiscountAmount
        {
            get
            {
                return (this.PartyDiscountCoupon && this.PartyDiscountPercentage > 0 && PartyCurrentDiscountMini < 999990.0m) ? (this.PartySubTotal() * this.PartyDiscountPercentage / 100) : 0.0m;
            }
        }
        public decimal PartyBizDiscountAmount
        {
            get
            {
                return (this.PartyDiscountCoupon && this.PartyDiscountPercentage > 0 && PartyCurrentDiscountMini < 999990.0m) ? (this.PartyBizSubTotal() * this.PartyDiscountPercentage / 100) : 0.0m;
            }
        }
        public decimal PartyTax()
        {
            return (PartySubTotal() - PartyDiscountAmount) * (PartyTaxRate / 100);
        }
        public decimal PartyServiceCharge
        {
            get { return decimal.Parse(ConfigurationManager.AppSettings["serviceCharge"].ToString()); }
        }
        public decimal PartyTotal()
        {
            return (this.PartySubTotal() - this.PartyDiscountAmount) + this.PartyTax() + this.PartyServiceCharge + (this.IsPartyDelivery ? this.PartyDeliveryFee() : 0.0m) + this.PartyDriverTip();
        }
        public int PartyTotalItems
        {
            get
            {
                return this.PartyCart.Sum(e => e.Value.TotalItems);
            }
        }

        public void Clear()
        {
            foreach (var c in this.PartyCart.Keys)
            {
                if (this._partyCart[c] != null)
                {
                    this._partyCart[c].Clear();
                }
            }
            this._partyCart.Clear();
            this._maxOrder = 999999.0m;
            this._sharedCartKey = string.Empty;
            this._isSharedCartLocked = false;
            this._partyAddress = string.Empty;
            this.PartyZip = string.Empty;
            this._selectBizInfos = new List<BizInfo>();
            this._partyItemNum = 0;
            this._isPartyDelivery = true;
            this._partyFreeCoupon = false;
            this._partyDiscountCoupon = false;
            this._partyDiscountPercentage = 0;
           // this._partyOrderMinimum = 0.0m;
           // this._partyDeliveryFee = 0.0m;
           // this._partyDriverTip = 0.0m;
            this._partyFreeItem = "";
            this._partyCouponChoice = "";
            this._partyCurrentDiscountMini = 999999.0m;
            this._partyScheduleDate = "Today";
            this._partyScheduleTime = "ASAP";
            this._partyBossName = string.Empty;
            this.PartyTaxRate = decimal.Parse(ConfigurationManager.AppSettings["defaultTaxRate"].ToString());
        }
    }
    public static class GroupCart
    {
        private static ConcurrentDictionary<string, SharedShoppingCart> _groupCarts = new ConcurrentDictionary<string, SharedShoppingCart>();
        public static ConcurrentDictionary<string, SharedShoppingCart> GroupCarts
        {
            get
            {
                return GroupCart._groupCarts;
            }
            set
            {
                GroupCart._groupCarts = value;
            }
        }
    }
}
