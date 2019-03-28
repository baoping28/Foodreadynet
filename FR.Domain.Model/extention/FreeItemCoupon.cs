using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class FreeItemCoupon : IBaseEntity
    {


        private string _setName = "FreeItemCoupons";
        private bool bIsDirty = false;

        public string BizName
        {
            get
            {
                if ((this.BizInfo != null))
                {
                    return this.BizInfo.BizTitle;
                }
                return string.Empty;
            }
        }

        public string ExpirationDateNote
        {
            get
            {
                return this.ExpirationDate > DateTime.MaxValue.AddDays(-2) ? "" : "( Expire: " + this.ExpirationDate.ToShortDateString() + " )";
            }
        }
        public bool IsFreeItemCouponDateOK
        {
            get
            {
                DateTime dt = DateTime.Now;
                return (this.StartDate < dt && this.ExpirationDate > dt);
            }
        }
        public bool IsFreeItemCouponOK(decimal vAmount)
        {
            return (IsFreeItemCouponDateOK && this.OrderMinimum >= vAmount);
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
                if (this.BizInfoId > 0 && this.OrderMinimum >= 0)
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
