using System;
using FR.Domain.Model.Abstract;


namespace FR.Domain.Model.Entities
{
    public partial class Vote : IBaseEntity
    {


        private string _setName = "Votes";
        private bool bIsDirty = false;


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
                if (string.IsNullOrEmpty(this.AspNetUser.Id) == false
                    & BizInfoId > 0 && this.RatingVote >= 0 && this.RatingVote <= 5)
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
