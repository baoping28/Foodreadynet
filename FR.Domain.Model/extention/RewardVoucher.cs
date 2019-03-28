using System;
using FR.Domain.Model.Abstract;
using System.Web.Configuration;

namespace FR.Domain.Model.Entities
{
    public partial class RewardVoucher : IBaseEntity
    {
        private string _setName = "RewardVouchers";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public int Points
        {
            get
            {
                decimal r = decimal.Parse(WebConfigurationManager.AppSettings["rewardRate"].ToString());
                int p = int.Parse(WebConfigurationManager.AppSettings["dollarToPoints"].ToString());
                return (int)((this.Value / r) * p);
            }
        }
        public bool IsValid
        {
            get
            {
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
