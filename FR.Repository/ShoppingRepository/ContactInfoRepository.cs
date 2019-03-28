using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class ContactInfoRepository : BaseShoppingCartRepository, IContactInfoRepository
    {
        #region IContactInfoRepository Members

        public List<ContactInfo> GetAllContactInfos()
        {
            List<ContactInfo> lContactInfo = default(List<ContactInfo>);
            string lContactInfoKey = CacheKey + "_AllContactInfos";

            if (base.EnableCaching && (Cache[lContactInfoKey] != null))
            {
                lContactInfo = (List<ContactInfo>)Cache[lContactInfoKey];
            }
            else
            {
                lContactInfo = Shoppingctx.ContactInfos.ToList();
                if (base.EnableCaching)
                {
                    CacheData(lContactInfoKey, lContactInfo, CacheDuration);
                }
            }
            return lContactInfo;
        }
        public List<ContactInfo> GetAllContactInfos(bool vActive)
        {
            return GetAllContactInfos().Where(e => e.Active == vActive).ToList();
        }

        public ContactInfo GetContactInfoById(int vContactInfoID)
        {
            return GetAllContactInfos().FirstOrDefault(e => e.ContactInfoId == vContactInfoID);
        }

        public List<ContactInfo> GetAllContactInfoByPhone(string vPhone, bool vActive)
        {
            throw new NotImplementedException();
        }

        public List<ContactInfo> GetAllContactInfoByEmail(string vEmail, bool vActive)
        {
            throw new NotImplementedException();
        }

        public int GetContactInfosCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public ContactInfo AddContactInfo(int vContactInfoID, string vPhone, string vPhone2, string vEmail, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            ContactInfo lContactInfo = new ContactInfo();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vContactInfoID > 0)
                {
                    lContactInfo = frctx.ContactInfos.FirstOrDefault(u => u.ContactInfoId == vContactInfoID);
                    lContactInfo.Phone = vPhone;
                    lContactInfo.Email = vEmail;

                    lContactInfo.UpdatedDate = vUpdatedDate;
                    lContactInfo.UpdatedBy = vUpdatedBy;
                    lContactInfo.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lContactInfo : null;
                }
                else
                {
                    lContactInfo.Phone = vPhone;
                    lContactInfo.Email = vEmail;

                    lContactInfo.AddedDate = vAddedDate;
                    lContactInfo.AddedBy = vAddedBy;
                    lContactInfo.Phone2 = vPhone2;
                    lContactInfo.UpdatedDate = vUpdatedDate;
                    lContactInfo.UpdatedBy = vUpdatedBy;
                    lContactInfo.Active = vActive;
                    return AddContactInfo(lContactInfo);
                }
            }
        }

        public ContactInfo AddContactInfo(ContactInfo vContactInfo)
        {
            try
            {
                Shoppingctx.ContactInfos.Add(vContactInfo);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vContactInfo : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteContactInfo(ContactInfo vContactInfo)
        {
            throw new NotImplementedException();
        }

        public bool DeleteContactInfo(int vContactInfoID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteContactInfo(ContactInfo vContactInfo)
        {
            throw new NotImplementedException();
        }

        public ContactInfo UpdateContactInfo(ContactInfo vContactInfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
