using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class Hotel : IBaseEntity
    {
        private string _setName = "Hotels";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public string ImagePath
        {
            get
            {
                return "~/" + HotelFoldPath + "/" + this.ImageUrl;
            }
        }
        public string HotelFoldPath
        {
            get
            {
                return "Content/HotelImages/Hotel_" + this.HotelTypeId;
            }
        }
        public string HotelAddressString
        {
            get
            {
                return this.AddressLine + ", " + this.City + ", " + this.State + " " + this.ZipCode;
            }
        }
        public bool IsValid
        {
            get
            {
                return true;
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
