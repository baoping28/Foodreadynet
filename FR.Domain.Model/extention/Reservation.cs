using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FR.Domain.Model.Abstract;
using FR.Infrastructure.Helpers;

namespace FR.Domain.Model.Entities
{
    public partial class Reservation : IBaseEntity
    {
        private string _setName = "Reservations";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public string BizName
        {
            get
            {
                if ((this.BizRVInfo != null))
                {
                    return this.BizRVInfo.BizName;
                }
                return string.Empty;
            }
        }
        public bool IsValid
        {
            get
            {
                if (this.BizRVInfoId > 0 & string.IsNullOrEmpty(this.AddedBy) == false)
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
