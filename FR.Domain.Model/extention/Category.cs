using System;
using FR.Domain.Model.Abstract;
using System.Linq;
using System.Web.Security;
using System.Collections.Generic;

namespace FR.Domain.Model.Entities
{
    public partial class Category : IBaseEntity
    {
        private string _setName = "Categories";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public string BizName
        {
            get
            {
                if ((this.BizCuisine.BizInfo.BizTitle != null))
                {
                    return this.BizCuisine.BizInfo.BizTitle;
                }
                return string.Empty;
            }
        }
        public int BizId
        {
            get
            {
                return this.BizCuisine.BizInfo.BizInfoId;
            }
        }
        public string CuisineTypeName
        {
            get
            {
                if ((this.BizCuisine.CuisineType.Title != null))
                {
                    return this.BizCuisine.CuisineType.Title;
                }
                return string.Empty;
            }
        }
        public List<Product> ActiveProducts
        {
            get
            {
                return this.Products.Where(e => e.Active == true).OrderBy(k => k.Title).ToList();
            }
        }
        public int ActiveProductsCount
        {
            get
            {
                return this.ActiveProducts.Count;
            }
        }
        public int ProductsCount
        {
            get
            {
                return this.Products.Count;
            }
        }
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.Title) == false & string.IsNullOrEmpty(this.Description) == false)
                {
                    return true;
                }
                return false;
            }
        }

        bool bIsDirty = false;
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
