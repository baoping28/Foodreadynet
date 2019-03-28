using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class ConvertedPoint : IBaseEntity
    {
        private string _setName = "ConvertedPoints";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.UserId)==false && this.RewardVoucherId > 0)
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
