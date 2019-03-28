using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IAddressRepository
    {
        List<Address> GetAllAddress();
        List<Address> GetAllAddress(bool vActive);
        Address GetAddressById(int vAddressID);
        List<Address> GetAllAddressByCity(string vCity, bool vActive);
        List<Address> GetAllAddressByZip(string vZip, bool vActive);
        List<Address> GetAllByAddressline(string vAddressline, bool vActive);
        int GetAllAddressCount(bool vActive);
        Address AddAddress(int vAddressID, string vAddressLine, string vAddressLine2, string vCity, string vState,
                                 string vZipCode, string vCrossStreet, DateTime vAddedDate, string vAddedBy,
                                  DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        Address AddAddress(Address vAddress);
        bool DeleteAddress(Address vAddress);
        bool DeleteAddress(int vAddressID);
        bool UnDeleteAddress(Address vAddress);
        Address UpdateAddress(Address vAddress);
    }
}
