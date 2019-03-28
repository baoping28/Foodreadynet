using System;
using System.Collections.Generic;
using System.Linq;
using FR.Domain.Model.Abstract;
namespace FR.Domain.Model.Entities
{
    public partial class Product : IBaseEntity
    {


        private string _setName = "Products";
        private bool bIsDirty = false;


        public BizInfo Bizinfo
        {
            get
            {
                if ((this.Category != null))
                {
                    return this.Category.BizCuisine.BizInfo;
                }
                return null;
            }
        }
        public string CategoryName
        {
            get
            {
                if ((this.Category != null))
                {
                    return this.Category.Title;
                }
                return string.Empty;
            }
        }
        public string BizName
        {
            get
            {
                if ((this.Category != null))
                {
                    return this.Category.BizName;
                }
                return string.Empty;
            }
        }

        public string BigImagePath
        {
            get
            {
                return "~/Content/BizImages/" + "Biz_" + this.BizId + "/" + this.BigImage;
            }
        }

        public string SmallImagePath
        {
            get
            {
                return "~/Content/BizImages/" + "Biz_" + this.BizId + "/" + this.SmallImage;
            }
        }

        public bool HasSize
        {
            get
            {
                return this.ProductSizes.Count > 0;
            }
        }
        public bool HasAddSide
        {
            get
            {
                return this.AddSides.Count > 0;
            }
        }
        public bool HasCrustChoice
        {
            get
            {
                return this.CrustChoices.Count > 0;
            }
        }
        public bool HasCheeseChoice
        {
            get
            {
                return this.CheeseAmounts.Count > 0;
            }
        }
        public decimal SmallestSizePrice
        {
            get
            {
                return this.ProductSizes.Min(e => e.Price);
            }
        }
        public int BizId
        {
            get
            {
                return this.Category.BizId;
            }
        }
        public string CookMethodName
        {
            get
            {
                if ((this.CookMethod != null))
                {
                    return this.CookMethod.Title;
                }
                return string.Empty;
            }
        }
        public string FoodTypeName
        {
            get
            {
                if ((this.FoodType != null))
                {
                    return this.FoodType.Title;
                }
                return string.Empty;
            }
        }
        public string MealSectionName
        {
            get
            {
                if ((this.MealSection != null))
                {
                    return this.MealSection.Title;
                }
                return string.Empty;
            }
        }

        public bool HasSideChoice
        {
            get
            {
                return (this.SideChoices.Count()) > 0;
            }
        }
        public bool HasSauceChoice
        {
            get
            {
                return (this.SauceChoices.Count()) > 0;
            }
        }
        public decimal BizFinalUnitPrice
        {
            get
            {
                if (this.DiscountPercentage > 0)
                {
                    return this.BizPrice - (this.BizPrice * this.DiscountPercentage / 100);
                }
                else
                {
                    return this.BizPrice;
                }
            }
        }
        public decimal FinalUnitPrice
        {
            get
            {
                if (this.DiscountPercentage > 0)
                {
                    return this.UnitPrice - (this.UnitPrice * this.DiscountPercentage / 100);
                }
                else
                {
                    return this.UnitPrice;
                }
            }
        }
        public List<string> AllToppingList
        {
            get
            {
                List<string> lct = new List<string>();
                List<ProductTopping> lbc = this.ProductToppings.Where(e => e.ProductId == this.ProductId).ToList();
                foreach (var l in lbc)
                {
                    lct.Add(l.ToppingName.ToLower());
                }
                return lct;
            }
        }
        public bool ContainsTopping(string vToppingName)
        {
            return AllToppingList.Contains(vToppingName.ToLower());
        }

        public List<string> AllDressingList
        {
            get
            {
                List<string> lct = new List<string>();
                List<ProductDressing> lbc = this.ProductDressings.Where(e => e.ProductId == this.ProductId).ToList();
                foreach (var l in lbc)
                {
                    lct.Add(l.DressingName.ToLower());
                }
                return lct;
            }
        }
        public bool ContainsDressing(string vDressingName)
        {
            return AllDressingList.Contains(vDressingName.ToLower());
        }

        public bool ExistTopping(int vToppingID)
        {
            return this.ProductToppings.Where(e => e.ProductId == this.ProductId && e.ToppingId == vToppingID).Count() > 0;
        }
        public bool ExistDressing(int vDressingID)
        {
            return this.ProductDressings.Where(e => e.ProductId == this.ProductId && e.DressingId == vDressingID).Count() > 0;
        }
        public bool ExistSizeChoice(string vSizeTitle)
        {
            return this.ProductSizes.Where(e => e.ProductId == this.ProductId && e.Title.ToLower() == vSizeTitle.ToLower()).Count() > 0;
        }
        public bool ExistCrustChoice(string vCrustTitle)
        {
            return this.CrustChoices.Where(e => e.ProductId == this.ProductId && e.Title.ToLower() == vCrustTitle.ToLower()).Count() > 0;
        }
        public bool ExistSideChoice(string vSideChoiceTitle)
        {
            return this.SideChoices.Where(e => e.ProductId == this.ProductId && e.Title.ToLower() == vSideChoiceTitle.ToLower()).Count() > 0;
        }
        public bool ExistSauceChoice(string vSauceTitle)
        {
            return this.SauceChoices.Where(e => e.ProductId == this.ProductId && e.Title.ToLower() == vSauceTitle.ToLower()).Count() > 0;
        }
        public bool ExistCheese(string vCheeseAmountTitle)
        {
            return this.CheeseAmounts.Where(e => e.ProductId == this.ProductId && e.Title.ToLower() == vCheeseAmountTitle.ToLower()).Count() > 0;
        }
        #region IBaseEntity Members

        /// <summary>
        /// Returns the name of the Data Set the Entity belongs to. Needs to be set
        /// in the derived class.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Title) == false &
                    string.IsNullOrEmpty(Description) == false & this.ProductId > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsDirty
        {
            get { return bIsDirty; }
            set { bIsDirty = value; }
        }


        public bool CanAdd
        {
            get { return true; }
        }

        public bool CanDelete
        {
            get { return true; }
        }

        public bool CanEdit
        {
            get { return true; }
        }

        public bool CanRead
        {
            get { return true; }
        }
        #endregion


    }
}
