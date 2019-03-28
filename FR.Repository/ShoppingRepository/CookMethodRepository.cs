using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace FR.Repository.ShoppingRepository
{
    public class CookMethodRepository : BaseShoppingCartRepository, ICookMethodRepository
    {
        #region ICookMethod Members
        public List<CookMethod> GetAllCookMethods()
        {
            List<CookMethod> lCookMethod = default(List<CookMethod>);
            string lCookMethodKey = CacheKey + "_AllCookMethods";

            if (base.EnableCaching && (Cache[lCookMethodKey] != null))
            {
                lCookMethod = (List<CookMethod>)Cache[lCookMethodKey];
            }
            else
            {
                lCookMethod = Shoppingctx.CookMethods.ToList();
                if (base.EnableCaching)
                {
                    CacheData(lCookMethodKey, lCookMethod, CacheDuration);
                }
            }
            return lCookMethod;
        }
        public List<CookMethod> GetAllCookMethods(bool vActive)
        {
            return GetAllCookMethods().Where(e => e.Active == vActive).ToList();
        }

        public CookMethod GetCookMethodById(int vCookMethodID)
        {
            return GetAllCookMethods().FirstOrDefault(e => e.CookMethodId == vCookMethodID);
        }

        public int GetAllCookMethodCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public CookMethod AddCookMethod(int vCookMethodID, string vTitle, string vDescription, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            CookMethod lCookMethod = new CookMethod();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vCookMethodID > 0)
                {
                    lCookMethod = frctx.CookMethods.FirstOrDefault(u => u.CookMethodId == vCookMethodID);
                    lCookMethod.Title = vTitle;
                    lCookMethod.Description = vDescription;

                    lCookMethod.UpdatedDate = vUpdatedDate;
                    lCookMethod.UpdatedBy = vUpdatedBy;
                    lCookMethod.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lCookMethod : null;
                }
                else
                {
                    lCookMethod.Title = vTitle;
                    lCookMethod.Description = vDescription;

                    lCookMethod.AddedDate = vAddedDate;
                    lCookMethod.AddedBy = vAddedBy;
                    lCookMethod.UpdatedDate = vUpdatedDate;
                    lCookMethod.UpdatedBy = vUpdatedBy;
                    lCookMethod.Active = vActive;
                    return AddCookMethod(lCookMethod);
                }
            }
        }

        public CookMethod AddCookMethod(CookMethod vCookMethod)
        {
            try
            {
                Shoppingctx.CookMethods.Add(vCookMethod);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vCookMethod : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteCookMethod(CookMethod vCookMethod)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCookMethod(int vCookMethodID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteCookMethod(CookMethod vCookMethod)
        {
            throw new NotImplementedException();
        }

        public CookMethod UpdateCookMethod(CookMethod vCookMethod)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
