using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IContactInfoRepository
    {
        List<ContactInfo> GetAllContactInfos();
        List<ContactInfo> GetAllContactInfos(bool vActive);
        ContactInfo GetContactInfoById(int vContactInfoID);
        List<ContactInfo> GetAllContactInfoByPhone(string vPhone, bool vActive);
        List<ContactInfo> GetAllContactInfoByEmail(string vEmail, bool vActive);
        int GetContactInfosCount(bool vActive);
        ContactInfo AddContactInfo(int vContactInfoID, string vPhone, string vPhone2, string vEmail,
                                 DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        ContactInfo AddContactInfo(ContactInfo vContactInfo);
        bool DeleteContactInfo(ContactInfo vContactInfo);
        bool DeleteContactInfo(int vContactInfoID);
        bool UnDeleteContactInfo(ContactInfo vContactInfo);
        ContactInfo UpdateContactInfo(ContactInfo vContactInfo);
    }
}
