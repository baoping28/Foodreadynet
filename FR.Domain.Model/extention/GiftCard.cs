using System;
using FR.Domain.Model.Abstract;
namespace FR.Domain.Model.Entities
{
    public partial class GiftCard : IBaseEntity
    {
        private string _setName = "GiftCards";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public bool Expired
        {
            get
            {
                return this.LastTimeUse.AddYears(1) < DateTime.Now;
            }
        }
        public bool Cover(decimal vAmount)
        {
            return this.Balance >= vAmount && this.Balance > 0 & this.Balance <= 500;
        }
        public string last4Digits
        {
            get
            {
                return "xxxx-xxxx-xxxx-" + this.GiftCardCode.Substring(this.GiftCardCode.Length - 4);
            }
        }
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.GiftCardCode) == false & this.Amount >= 10 & this.Amount <= 500)
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
