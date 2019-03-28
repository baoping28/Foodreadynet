using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class BizImage : IBaseEntity
    {
        private string _setName = "BizImages";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public string SmallImagePath
        {
            get
            {
                return "~/Content/BizImages/" + "Biz_" + this.BizInfoId + "/" + this.SmallImageName;
            }
        }
        public string BigImagePath
        {
            get
            {
                return "~/Content/BizImages/" + "Biz_" + this.BizInfoId + "/" + this.BigImageName;
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
