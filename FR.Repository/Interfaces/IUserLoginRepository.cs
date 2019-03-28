using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IUserLoginRepository
    {
        List<AspNetUserLogin> GetAllUserLogins();
        AspNetUserLogin GetUserLoginByUserId(string vUserID);
        bool IsExternalUser(string vUserID);
        bool IsExternalUserByUserName(string vUserName);
        AspNetUserLogin GetUserLoginByUserName(string vUserName);
        AspNetUserLogin GetUserLoginByProvider(string vProvider, string vProviderUserId);
        int GetUserIdByProvider(string vProvider, string vProviderUserId);
        bool IsProviderUserLocked(string vProvider, string vProviderUserId);
        int GetAllUserLoginCount();

        bool LockUserLogin(AspNetUserLogin vUserLogin);
        bool LockUserLoginByUserName(string vUserName);
        bool LockUserLogin(string vUserId);
        bool UnlockUserLoginByUserName(string vUserName);
        bool UnlockUserLogin(string vUserId);
        bool UnlockUserLogin(AspNetUserLogin vUserLogin);

        bool DeleteUserLogin(AspNetUserLogin vUserLogin);
        bool DeleteUserLogin(int vUserLoginID);
        bool UnDeleteUserLogin(AspNetUserLogin vUserLogin);
    }
}