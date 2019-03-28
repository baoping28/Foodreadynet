using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
namespace FR.Repository.Interfaces
{
    public interface ICookMethodRepository
    {
        List<CookMethod> GetAllCookMethods(bool vActive);
        CookMethod GetCookMethodById(int vCookMethodID);
        int GetAllCookMethodCount(bool vActive);
        CookMethod AddCookMethod(int vCookMethodID, string vTitle, string vDescription,
                              DateTime vAddedDate, string vAddedBy,
                              DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        CookMethod AddCookMethod(CookMethod vCookMethod);
        bool DeleteCookMethod(CookMethod vCookMethod);
        bool DeleteCookMethod(int vCookMethodID);
        bool UnDeleteCookMethod(CookMethod vCookMethod);
        CookMethod UpdateCookMethod(CookMethod vCookMethod);
    }
}

