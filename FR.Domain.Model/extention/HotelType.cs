using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class HotelType : IBaseEntity
    {
        private string _setName = "HotelTypes";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public string ImagePath
        {
            get
            {
                return @"~/" + HotelTypeFoldPath + "/" + this.ImageUrl;
            }
        }
        public string HotelTypeFoldPath
        {
            get
            {
                return "Content/HotelImages";
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
