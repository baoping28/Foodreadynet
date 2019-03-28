using System;
using FR.Domain.Model.Abstract;
using System.Security.Principal;
using System.Web;

namespace FR.Domain.Model.Entities
{
    public partial class Order : IBaseEntity
    {

        private UserDetail _userdetail;
        private string _setName = "Orders";
        private bool bIsDirty = false;
        public string RoomNumber
        {
            get;
            set;
        }

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

        public string PaymentType
        {
            get
            {
                return this.IsPayPalPayment ? "PayPal Payment" : this.IsGiftCardPayment == false ? "Credit Card Payment" : this.GiftCardAmountPay == this.OrderTotal ? "Gift Card Payment" : "Credit Card and Gift Card Payment";
            }
        }
        public string InvioceNumber
        {
            get
            {
                if ((this.OrderId > 0))
                {
                    return (800000000 + this.OrderId).ToString();
                }
                return string.Empty;
            }
        }
        public string OrderType
        {
            get
            {
                return this.IsDelivery ? "Delivery" : "Pickup";
            }
        }
        public static IPrincipal CurrentUser
        {
            get { return HttpContext.Current.User; }
        }
        public static string CurrentUserName
        {
            get
            {
                string userName = string.Empty;
                if (CurrentUser.Identity.IsAuthenticated)
                {
                    userName = CurrentUser.Identity.Name;
                }
                return userName;
            }
        }
        public UserDetail Userdetail
        {
            get
            {
                if (_userdetail == null)
                {
                    _userdetail = new UserDetail();
                }
                return _userdetail;
            }
            set { _userdetail = value; }
        }
        public string OrderStatusTitle
        {
            get
            {
                if ((this.OrderStatus != null))
                {
                    return this.OrderStatus.Title;
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
                if (BizInfoId > 0 & OrderTotal > 0.0m && string.IsNullOrEmpty(LogonName) == false)
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
