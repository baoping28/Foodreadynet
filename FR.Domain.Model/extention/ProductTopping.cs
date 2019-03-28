using System;
using FR.Domain.Model.Abstract;
namespace FR.Domain.Model.Entities
{
    public partial class ProductTopping : IBaseEntity
    {


        private string _setName = "ProductToppings";
        private bool bIsDirty = false;


        public string ProductName
        {
            get
            {
                if ((this.Product != null))
                {
                    return this.Product.Title;
                }
                return string.Empty;
            }
        }
        public string ToppingName
        {
            get
            {
                if ((this.Topping != null))
                {
                    return this.Topping.Title;
                }
                return string.Empty;
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
                if (this.ProductId > 0 & this.ToppingId > 0 && this.ExtraToppingPrice >= 0.0m)
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
