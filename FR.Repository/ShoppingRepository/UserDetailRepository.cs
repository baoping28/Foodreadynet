using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class UserDetailRepository : BaseShoppingCartRepository, IUserDetailRepository
    {
        #region IUserDetailRepository Members

        public List<UserDetail> GetAllUserDetails()
        {
            List<UserDetail> lUserDetails = default(List<UserDetail>);
            string lUserDetailsKey = CacheKey + "_AllUserDetails";

            if (base.EnableCaching && (Cache[lUserDetailsKey] != null))
            {
                lUserDetails = (List<UserDetail>)Cache[lUserDetailsKey];
            }
            else
            {
                lUserDetails = (from lUserDetail in Shoppingctx.UserDetails
                             select lUserDetail).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lUserDetailsKey, lUserDetails, CacheDuration);
                }
            }
            return lUserDetails; 
        }
        public List<UserDetail> GetAllUserDetails(bool vActive)
        {
            return GetAllUserDetails().Where(e => e.Active == vActive).ToList();
        }
        public List<UserDetail> GetAllBizUserDetails(bool vActive)
        {
            return GetAllUserDetails().Where(e => e.Active == vActive && e.IsRestaurantUser).ToList();
        }
        public UserDetail GetUserDetailById(int vUserDetailID)
        {
            return GetAllUserDetails().Where(e => e.UserDetailId == vUserDetailID).FirstOrDefault();
        }
        public UserDetail GetUserDetailByEmail(string vEmail,bool vActive)
        {
            return GetAllUserDetails().Where(e => e.ContactInfo.Email == vEmail && e.Active==vActive).FirstOrDefault();
        }
        public UserDetail GetUserDetailByUserId(string vUserID)
        {
            return GetAllUserDetails().Where(e => e.UserId == vUserID).FirstOrDefault();
        }
        public int GetAllUserDetailCount(bool vActive)
        {
            return Shoppingctx.UserDetails.Where(e => e.Active == vActive).Count();
        }

        public UserDetail InsertUserDetail(int vUserDetailID, string vUserId, string vFirstName,
               string vLastName, int vAddressId, int vContactInfoId, DateTime vAddedDate, string vAddedBy,
               DateTime vUpdatedDate, string vUpdatedBy, bool vActive,string vPassword)
        {
            UserDetail lUserDetail = new UserDetail();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vUserDetailID > 0)
                {
                    lUserDetail = frctx.UserDetails.FirstOrDefault(u => u.UserDetailId == vUserDetailID);
                    lUserDetail.UserId = vUserId;
                    lUserDetail.FirstName = vFirstName;
                    lUserDetail.LastName = vLastName;
                    lUserDetail.AddressId = vAddressId;
                    lUserDetail.ContactInfoId = vContactInfoId;

                    lUserDetail.UpdatedDate = vUpdatedDate;
                    lUserDetail.UpdatedBy = vUpdatedBy;
                    lUserDetail.Active = vActive;
                    //lUserDetail.Password = vPassword;
                    return frctx.SaveChanges() > 0 ? lUserDetail : null;
                }
                else
                {
                    lUserDetail.UserId = vUserId;
                    lUserDetail.FirstName = vFirstName;
                    lUserDetail.LastName = vLastName;
                    lUserDetail.AddressId = vAddressId;
                    lUserDetail.ContactInfoId = vContactInfoId;
                    lUserDetail.Password = vPassword;

                    lUserDetail.AddedDate = vAddedDate;
                    lUserDetail.AddedBy = vAddedBy;
                    lUserDetail.UpdatedDate = vUpdatedDate;
                    lUserDetail.UpdatedBy = vUpdatedBy;
                    lUserDetail.Active = vActive;
                    return AddUserDetail(lUserDetail);
                }
            }
        }




        public UserDetail AddUserDetail(UserDetail vUserDetail)
        {
            try
            {
                    Shoppingctx.UserDetails.Add(vUserDetail);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vUserDetail : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteUserDetail(UserDetail vUserDetail)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUserDetail(int vUserDetailID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteUserDetail(UserDetail vUserDetail)
        {
            throw new NotImplementedException();
        }

        public UserDetail UpdateUserDetail(UserDetail vUserDetail)
        {
            UserDetail ud = new UserDetail();
            ud = Shoppingctx.UserDetails.FirstOrDefault(u => u.UserDetailId == vUserDetail.UserDetailId);

            ud.FirstName = vUserDetail.FirstName;
            ud.LastName = vUserDetail.LastName;
            ud.AddressId = vUserDetail.AddressId;
            ud.ContactInfoId = vUserDetail.ContactInfoId;
            ud.Password = vUserDetail.Password;
            ud.UpdatedDate =vUserDetail.UpdatedDate;
            ud.UpdatedBy = vUserDetail.UpdatedBy;
            ud.Active = vUserDetail.Active;
            return Shoppingctx.SaveChanges() > 0 ? ud : null;
        }
        public UserDetail UpdateUserDetailPsw(string vUserId, string vNewPassword, string vUpdatedBy)
        {
            UserDetail ud = new UserDetail();
            ud = Shoppingctx.UserDetails.FirstOrDefault(u => u.UserId == vUserId);
            if (ud!=null)
            {
            ud.Password = vNewPassword;
            ud.UpdatedDate = DateTime.Now;
            ud.UpdatedBy = vUpdatedBy;
            return Shoppingctx.SaveChanges() > 0 ? ud : null;
            }
            return null;
        }
        #endregion
    }
}
