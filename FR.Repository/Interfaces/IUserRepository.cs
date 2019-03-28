using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IUserRepository
    {
        List<AspNetUser> GetAllUsers();
        List<AspNetUser> GetAllUsers(bool vActive);
        List<AspNetUser> GetAllBizUsers(bool vActive);
        List<AspNetUser> GetAllUserStartWith(string vString);
        AspNetUser GetUserByUserId(string vUserID);
        AspNetUser GetUserByUserName(string vUserName);
        int GetAllUserCount();
        int GetAllUserCount(bool vActive);

        bool LockUser(AspNetUser vUser);
        bool LockUserByUserName(string vUserName);
        bool LockUser(string vUserId);
        bool UnlockUserByUserName(string vUserName);
        bool UnlockUser(string vUserId);
        bool UnlockUser(AspNetUser vUser);

        bool DeleteUser(AspNetUser vUser);
        bool DeleteUser(string vUserID);
        bool UnDeleteUser(AspNetUser vUser);
    }
}