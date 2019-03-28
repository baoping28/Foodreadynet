using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IDriverRepository
    {
        List<Driver> GetAllDrivers();
        List<Driver> GetAllDrivers(bool vActive);
        List<Driver> GetAllDrivers(bool vStayin, bool vActive);

        Driver GetDriverById(int vDriverID);
        Driver GetDriverByName(string vSignName, bool vActive);
        Driver GetDriverByNamePassword(string vSignName, string vSignPassword, bool vActive);
        List<Driver> GetDriversByCity(string vCity, bool vStayin, bool vActive);
        List<Driver> GetDriversByOrder(Order vOrder, bool vStayin, bool vActive);
        bool Signin(string vSigninName, string vSigninPassword);
        bool Signout(string vSigninName, string vSigninPassword);
        bool IsSigninNameExist(string vSigninName);
        int GetAllDriverCount(bool vActive);
       
        Driver AddDriver(int vDriverID, string vFirstName, string vLastName, string vSigninName,string vSigninPassword, bool vStayin, string vWorkArea, 
                         string vAddressLine, string vCity,string vState, string vZipCode, string vPhone,string vEmail,
                         DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        Driver AddDriver(Driver vDriver);
        bool LockDriver(Driver vDriver);
        bool UnlockDriver(Driver vDriver);
        bool DeleteDriver(Driver vDriver);
        bool DeleteDriver(int vDriverID);
        bool UnDeleteDriver(Driver vDriver);
        Driver UpdateDriver(Driver vDriver);
    }
}
