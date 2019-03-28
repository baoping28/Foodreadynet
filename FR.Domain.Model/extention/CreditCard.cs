using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class CreditCard : IBaseEntity
    {
        private string _setName = "CreditCards";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public string last4Digits
        {
            get
            {
                return this.CreditCardType.Title + ": </br>xxxx-xxxx-xxxx-" + this.CreditCardNumber.Substring(this.CreditCardNumber.Length - 4);
            }
        }
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.CreditCardNumber) == false && string.IsNullOrEmpty(this.AddedBy) == false)
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
