using System;
using FR.Domain.Model.Abstract;
using FR.Infrastructure.Helpers;
using System.Web.Security;
using System.Linq;

namespace FR.Domain.Model.Entities
{
    public partial class UserDetail : IBaseEntity
    {


        private string _setName = "UserDetails";
        private bool bIsDirty = false;

        public string DecryptPassword
        {
            get
            {
                if ((string.IsNullOrEmpty(this.Password) == false))
                {
                    return CryptionClass.Decrypt(this.Password);
                }
                return string.Empty;
            }
        }
        public string UserName
        {
            get
            {
                if ((this.AspNetUser != null))
                {
                    return this.AspNetUser.UserName;
                }
                return string.Empty;
            }
        }
        public string UserAddressLine
        {
            get
            {
                if (this.Address != null)
                {
                    return this.Address.AddressLine + ", " + this.Address.City + ", " + this.Address.State + " " + this.Address.ZipCode;
                }
                return string.Empty;
            }
        }
        public bool UserHasBizInfo
        {
            get
            {
                if ((this.AspNetUser != null))
                {
                    return (this.AspNetUser.BizInfos.Where(e => e.UserId == this.UserId).Count()) > 0;
                }
                return false;
            }
        }
        public string BizInfoTitle
        {
            get
            {
                if (UserHasBizInfo)
                {
                    BizInfo bi = this.AspNetUser.BizInfos.Where(e => e.UserId == this.UserId).FirstOrDefault();
                    return (bi == null ? string.Empty : bi.BizTitle);
                }
                return string.Empty;
            }
        }
        public BizInfo UserBizInfo
        {
            get
            {
                if (UserHasBizInfo)
                {
                    return this.AspNetUser.BizInfos.Where(e => e.UserId == this.UserId).FirstOrDefault();
                }
                return null;
            }
        }
        public bool IsRestaurantUser
        {
            get
            {

                if ((string.IsNullOrEmpty(this.UserId) == false))
                {
                    var re=this.AspNetUser.AspNetRoles.Where(e=>e.Name=="Restaurant").FirstOrDefault();
                    return re == null ? false : true;
                }
                return false;
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
                if (string.IsNullOrEmpty(this.UserId)==false & this.AddressId > 0 && this.ContactInfoId > 0)
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
