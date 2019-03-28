using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class Driver : IBaseEntity
    {
        private string _setName = "Drivers";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public string DriverFullAddress
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
                if (string.IsNullOrEmpty(this.SigninName) == false & string.IsNullOrEmpty(this.SigninPassword) == false & string.IsNullOrEmpty(this.Email) == false & string.IsNullOrEmpty(this.AddedBy) == false)
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
