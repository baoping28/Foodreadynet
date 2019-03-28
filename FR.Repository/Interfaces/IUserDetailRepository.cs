using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IUserDetailRepository
    {
        List<UserDetail> GetAllUserDetails();
        List<UserDetail> GetAllUserDetails(bool vActive);
        List<UserDetail> GetAllBizUserDetails(bool vActive);
        UserDetail GetUserDetailById(int vUserDetailID);
        UserDetail GetUserDetailByUserId(string vUserID);
        UserDetail GetUserDetailByEmail(string vEmail,bool vActive);
        int GetAllUserDetailCount(bool vActive);
        UserDetail InsertUserDetail(int vUserDetailID, string vUserId, string vFirstName,
                string vLastName, int vAddressId, int vContactInfoId, DateTime vAddedDate, string vAddedBy,
                DateTime vUpdatedDate, string vUpdatedBy, bool vActive, string vPassword);
        UserDetail AddUserDetail(UserDetail vUserDetail);
        bool DeleteUserDetail(UserDetail vUserDetail);
        bool DeleteUserDetail(int vUserDetailID);
        bool UnDeleteUserDetail(UserDetail vUserDetail);
        UserDetail UpdateUserDetail(UserDetail vUserDetail);
        UserDetail UpdateUserDetailPsw(string vUserId, string vNewPassword, string vUpdatedBy);
    }
}