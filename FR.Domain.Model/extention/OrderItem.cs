using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class OrderItem : IBaseEntity
    {


        private string _setName = "OrderItems";
        private bool bIsDirty = false;


        public string BizName
        {
            get
            {
                if ((this.Order != null))
                {
                    return this.Order.BizInfo.BizTitle;
                }
                return string.Empty;
            }
        }

        public string GetItemNote
        {
            get
            {
                string str = string.Empty;
                if (string.IsNullOrEmpty(this.DressingChoice) == false)
                {

                    str = "Dressing:" + this.DressingChoice + "<br />";
                }
                if (string.IsNullOrEmpty(this.SelectedFreeToppings) == false)
                {
                    str += "Free toppings:<br />" + this.SelectedFreeToppings + "<br/>";
                }
                if (string.IsNullOrEmpty(this.SelectedToppings) == false)
                {
                    str += "Extra toppings for each:<br />" + this.SelectedToppings;
                }
                if (string.IsNullOrEmpty(this.HowSpicy) == false)
                {
                    str += "Spicy: " + this.HowSpicy + "<br />";
                }
                if (string.IsNullOrEmpty(this.SideChoice) == false)
                {
                    str += "Side choice: " + this.SideChoice + "<br />";
                }
                if (string.IsNullOrEmpty(this.SauceChoice) == false)
                {
                    str += "Sauce choice: " + this.SauceChoice + "<br />";
                }
                if (string.IsNullOrEmpty(this.CrustChoice) == false)
                {
                    str += "Crust choice: " + this.CrustChoice + "<br />";
                }
                if (string.IsNullOrEmpty(this.CheeseAmount) == false)
                {
                    str += "Cheese Amount: " + this.CheeseAmount + "<br />";
                }
                if (string.IsNullOrEmpty(this.SelectedAddSides) == false)
                {
                    str += this.SelectedAddSides + "<br />";
                }
                if (string.IsNullOrEmpty(this.Instruction) == false)
                {
                    str += "Instructions: " + this.Instruction;
                }
                return str;
            }
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
                    UnitPrice >= 0 & this.Quantity > 0)
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
