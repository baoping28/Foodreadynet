using System;
using FR.Domain.Model.Abstract;
using FR.Infrastructure.Helpers;
using System.Web.Security;
using System.Linq;

namespace FR.Domain.Model.Entities
{
    public partial class AspNetUser : IBaseEntity
    {
        private string _setName = "AspNetUsers";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public bool UserHasBizInfo
        {
            get
            {
                return this.BizInfos.Where(e => e.UserId == this.Id).Count() > 0;
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
