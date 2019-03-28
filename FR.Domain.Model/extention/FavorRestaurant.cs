using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FR.Domain.Model.Abstract;
using FR.Infrastructure.Helpers;

namespace FR.Domain.Model.Entities
{
    public partial class FavorRestaurant : IBaseEntity
    {
        private string _setName = "FavorRestaurants";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public bool IsValid
        {
            get
            {
                if (this.BizInfoId > 0 && string.IsNullOrEmpty(this.AspNetUser.Id) == false)
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
