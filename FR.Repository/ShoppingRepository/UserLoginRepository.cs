using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class UserLoginRepository : BaseShoppingCartRepository, IUserLoginRepository
    {
        public List<AspNetUserLogin> GetAllUserLogins()
        {
             List<AspNetUserLogin> lAspNetUserLogins = default(List<AspNetUserLogin>);
            string lAspNetUserLoginsKey = CacheKey + "_AllAspNetUserLogins";

            if (base.EnableCaching && (Cache[lAspNetUserLoginsKey] != null))
            {
                lAspNetUserLogins = (List<AspNetUserLogin>)Cache[lAspNetUserLoginsKey];
            }
            else
            {
                lAspNetUserLogins = (from lAspNetUserLogin in Shoppingctx.AspNetUserLogins
                                 orderby lAspNetUserLogin.LoginProvider
                                 select lAspNetUserLogin).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lAspNetUserLoginsKey, lAspNetUserLogins, CacheDuration);
                }
            }
            return lAspNetUserLogins; 
        }

        public AspNetUserLogin GetUserLoginByUserId(string vUserID)
        {
            return GetAllUserLogins().Where(e => e.UserId == vUserID).FirstOrDefault();
        }

        public bool IsExternalUser(string vUserID)
        {
            throw new NotImplementedException();
        }

        public bool IsExternalUserByUserName(string vUserName)
        {
            throw new NotImplementedException();
        }
        public AspNetUserLogin GetUserLoginByUserName(string vUserName)
        {
            throw new NotImplementedException();
        }

        public AspNetUserLogin GetUserLoginByProvider(string vProvider, string vProviderUserId)
        {
            throw new NotImplementedException();
        }

        public int GetUserIdByProvider(string vProvider, string vProviderUserId)
        {
            throw new NotImplementedException();
        }

        public bool IsProviderUserLocked(string vProvider, string vProviderUserId)
        {
            throw new NotImplementedException();
        }

        public int GetAllUserLoginCount()
        {
            throw new NotImplementedException();
        }

        public bool LockUserLogin(AspNetUserLogin vUserLogin)
        {
            throw new NotImplementedException();
        }

        public bool LockUserLogin(string vUserName)
        {
            throw new NotImplementedException();
        }

        public bool LockUserLoginByUserName(string vUserName)
        {
            throw new NotImplementedException();
        }

        public bool UnlockUserLoginByUserName(string vUserName)
        {
            throw new NotImplementedException();
        }
        public bool UnlockUserLogin(string vUserName)
        {
            throw new NotImplementedException();
        }

        public bool UnlockUserLogin(AspNetUserLogin vUserLogin)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUserLogin(AspNetUserLogin vUserLogin)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUserLogin(int vUserLoginID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteUserLogin(AspNetUserLogin vUserLogin)
        {
            throw new NotImplementedException();
        }
    }
}
