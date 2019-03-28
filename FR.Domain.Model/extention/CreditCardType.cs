using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class CreditCardType : IBaseEntity
    {
        private string _setName = "CreditCardTypes";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public string CreditCardTypeTitle
        {
            get
            {
                switch (this.CreditCardTypeId)
                {
                    case 1:
                        return "VISA";
                    case 2:
                        return "Master Card";
                    case 3:
                        return "American Express";
                    case 4:
                        return "Discover";
                    default:

                        //Did not match any of the defaults, so now go and retrieve from the database
                        /*
                        using (OrderStatusesRepository lOrderStatusrpt = new OrderStatusesRepository())
                        {

                            OrderStatus lOrderStatus = lOrderStatusrpt.GetOrderStatusById(StatusID);

                            if ((lOrderStatus != null))
                            {
                                return lOrderStatus.Title;
                            }
                            return "Unknown Status";
                        }
                        */
                        return "Unknown Credit Card Type";
                }
            }
        }
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.Title) == false && string.IsNullOrEmpty(this.AddedBy) == false)
                {
                    return true;
                }
                return false;
            }
        }

        bool bIsDirty = false;
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

    }
}
