using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class UserRepository : BaseShoppingCartRepository, IUserRepository
    {
        public List<AspNetUser> GetAllUsers()
        {
            List<AspNetUser> lAspNetUsers = default(List<AspNetUser>);
            string lAspNetUsersKey = CacheKey + "_AllAspNetUsers";

            if (base.EnableCaching && (Cache[lAspNetUsersKey] != null))
            {
                lAspNetUsers = (List<AspNetUser>)Cache[lAspNetUsersKey];
            }
            else
            {
                lAspNetUsers = (from lAspNetUser in Shoppingctx.AspNetUsers
                                 orderby lAspNetUser.UserName
                                 select lAspNetUser).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lAspNetUsersKey, lAspNetUsers, CacheDuration);
                }
            }
            return lAspNetUsers; 
        }

        public List<AspNetUser> GetAllUsers(bool vActive)
        {
            return GetAllUsers().Where(e => e.Active == vActive).ToList();
        }

        public List<AspNetUser> GetAllBizUsers(bool vActive)
        {
            throw new NotImplementedException();
        }

        public List<AspNetUser> GetAllUserStartWith(string vString)
        {
            return GetAllUsers().Where(e => e.UserName.StartsWith(vString)).ToList();
        }

        public AspNetUser GetUserByUserId(string vUserID)
        {
            return GetAllUsers().Where(e => e.Id == vUserID).FirstOrDefault();
        }

        public AspNetUser GetUserByUserName(string vUserName)
        {
            return GetAllUsers().Where(e => e.UserName == vUserName).FirstOrDefault();
        }

        public int GetAllUserCount()
        {
            return GetAllUsers().Count;
        }

        public int GetAllUserCount(bool vActive)
        {
            return GetAllUsers(vActive).Count;
        }

        public bool LockUser(AspNetUser vUser)
        {
            return ChangeLockState(vUser, false);
        }

        public bool LockUserByUserName(string vUserName)
        {
            return LockUser(GetUserByUserName(vUserName));
        }
        public bool LockUser(string vUserId)
        {
            return LockUser(GetUserByUserId(vUserId));
        }

        public bool UnlockUserByUserName(string vUserName)
        {
            return UnlockUser(GetUserByUserName(vUserName));
        }

        public bool UnlockUser(AspNetUser vUser)
        {
            return ChangeLockState(vUser, true);
        }

        public bool UnlockUser(string vUserId)
        {
            return UnlockUser(GetUserByUserId(vUserId));
        }
        private bool ChangeLockState(AspNetUser vUser, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                AspNetUser up = frenty.AspNetUsers.FirstOrDefault(e => e.UserName == vUser.UserName);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteUser(AspNetUser vUser)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string vUserID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteUser(AspNetUser vUser)
        {
            throw new NotImplementedException();
        }
    }
}
