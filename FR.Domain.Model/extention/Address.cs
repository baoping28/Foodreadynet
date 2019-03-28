using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class Address : IBaseEntity
    {
        private string _setName = "Addresses";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }

        public string AddressString
        {
            get
            {
                return this.AddressLine + ", " + this.City + ", " + this.State + " " + this.ZipCode;

            }
        }
        public string TwoLineAddressString
        {
            get
            {
                return this.AddressLine + "</br> " + this.City + ", " + this.State + " " + this.ZipCode;

            }
        }
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.AddressLine) == false & string.IsNullOrEmpty(this.City) == false & string.IsNullOrEmpty(this.ZipCode) == false & string.IsNullOrEmpty(this.AddedBy) == false)
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

