using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class AddressRepository : BaseShoppingCartRepository, IAddressRepository
    {
        #region IAddressRepository Members

        public List<Address> GetAllAddress()
        {
            List<Address> lAddress = default(List<Address>);
            string lAddressKey = CacheKey + "_AllAddresss";

            if (base.EnableCaching && (Cache[lAddressKey] != null))
            {
                lAddress = (List<Address>)Cache[lAddressKey];
            }
            else
            {
                lAddress = Shoppingctx.Addresses.ToList();
                if (base.EnableCaching)
                {
                    CacheData(lAddressKey, lAddress, CacheDuration);
                }
            }
            return lAddress;
        }
        public List<Address> GetAllAddress(bool vActive)
        {
            throw new NotImplementedException();
        }

        public Address GetAddressById(int vAddressID)
        {
            return GetAllAddress().FirstOrDefault(e => e.AddressId == vAddressID);
        }

        public List<Address> GetAllAddressByCity(string vCity, bool vActive)
        {
            throw new NotImplementedException();
        }

        public List<Address> GetAllAddressByZip(string vZip, bool vActive)
        {
            throw new NotImplementedException();
        }

        public List<Address> GetAllByAddressline(string vAddressline, bool vActive)
        {
            throw new NotImplementedException();
        }

        public int GetAllAddressCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public Address AddAddress(int vAddressID, string vAddressLine, string vAddressLine2, string vCity, string vState, string vZipCode, string vCrossStreet, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            Address lAddress = new Address();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vAddressID > 0)
                {
                    lAddress = frctx.Addresses.FirstOrDefault(u => u.AddressId == vAddressID);
                    lAddress.AddressLine = vAddressLine;
                    lAddress.AddressLine2 = vAddressLine2;
                    lAddress.City = vCity;
                    lAddress.State = vState;
                    lAddress.ZipCode = vZipCode;

                    lAddress.CrossStreet = vCrossStreet;

                    lAddress.UpdatedDate = vUpdatedDate;
                    lAddress.UpdatedBy = vUpdatedBy;
                    lAddress.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lAddress : null;
                }
                else
                {
                    lAddress.AddressLine = vAddressLine;
                    lAddress.AddressLine2 = vAddressLine2;
                    lAddress.City = vCity;
                    lAddress.State = vState;
                    lAddress.ZipCode = vZipCode;

                    lAddress.CrossStreet = vCrossStreet;

                    lAddress.UpdatedDate = vUpdatedDate;
                    lAddress.UpdatedBy = vUpdatedBy;
                    lAddress.AddedDate = vAddedDate;
                    lAddress.AddedBy = vAddedBy;
                    lAddress.Active = vActive;
                    return AddAddress(lAddress);
                }
            }
        }
        public Address AddAddress(Address vAddress)
        {
            try
            {
                Shoppingctx.Addresses.Add(vAddress);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vAddress : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteAddress(Address vAddress)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAddress(int vAddressID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteAddress(Address vAddress)
        {
            throw new NotImplementedException();
        }

        public Address UpdateAddress(Address vAddress)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
