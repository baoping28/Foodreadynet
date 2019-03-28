using System;
using FR.Domain.Model.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace FR.Domain.Model.Entities
{
    public partial class BizCuisine : IBaseEntity
    {


        private string _setName = "BizCuisines";
        private bool bIsDirty = false;


        public string BizName
        {
            get
            {
                if ((this.BizInfo != null))
                {
                    return this.BizInfo.BizTitle;
                }
                return string.Empty;
            }
        }
        public string CuisineTypeName
        {
            get
            {
                if ((this.CuisineType != null))
                {
                    return this.CuisineType.Title;
                }
                return string.Empty;
            }
        }
        public bool HasLunchSpecail
        {
            get
            {
                return (this.Categories.Where(e => e.Title.ToLower() == "lunch special").Count()) > 0;
            }
        }

        public Category LunchSpecailCategory
        {
            get
            {
                return (this.Categories.Where(e => e.Title.ToLower() == "lunch special" && this.Active == true).FirstOrDefault());
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
                if (this.BizInfoId > 0 & this.CuisineTypeId > 0)
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
