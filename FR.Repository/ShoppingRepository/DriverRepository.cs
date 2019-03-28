using System.Linq;
using System;
using System.Collections.Generic;
using FR.Repository.Interfaces;
using FR.Domain.Model.Entities;
using FR.Infrastructure.Helpers;

namespace FR.Repository.ShoppingRepository
{
    public class DriverRepository : BaseShoppingCartRepository, IDriverRepository
    {
        public List<Driver> GetAllDrivers()
        {

            List<Driver> lDrivers = default(List<Driver>);
            string lDriversKey = CacheKey + "_AllDrivers";

            if (base.EnableCaching && (Cache[lDriversKey] != null))
            {
                lDrivers = (List<Driver>)Cache[lDriversKey];
            }
            else
            {
                lDrivers = (from lDriver in Shoppingctx.Drivers
                             orderby lDriver.FirstName
                             select lDriver).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lDriversKey, lDrivers, CacheDuration);
                }
            }
            return lDrivers;
        }

        public List<Driver> GetAllDrivers(bool vActive)
        {
            return GetAllDrivers().Where(e => e.Active == vActive).ToList();
        }

        public List<Driver> GetAllDrivers(bool vStayin, bool vActive)
        {
            return GetAllDrivers().Where(e =>e.Stayin==vStayin && e.Active == vActive).ToList();
        }

        public Driver GetDriverById(int vDriverID)
        {
            return GetAllDrivers().Where(e => e.DriverId == vDriverID).FirstOrDefault();
        }

        public Driver GetDriverByName(string vSignName, bool vActive)
        {
            return GetAllDrivers().Where(e => e.SigninName == vSignName && e.Active==vActive).FirstOrDefault();
        }
        public Driver GetDriverByNamePassword(string vSignName, string vSignPassword, bool vActive)
        {
            return GetAllDrivers().Where(e => e.SigninName == vSignName && e.SigninPassword==vSignPassword && e.Active == vActive).FirstOrDefault();
        }
        public List<Driver> GetDriversByCity(string vCity, bool vStayin, bool vActive)
        {
            string ct = vCity.ToLower();
            return GetAllDrivers(vStayin, vActive).Where(e => e.Stayin == vStayin && e.Active == vActive && ct.StartsWith(e.WorkArea.ToLower())).ToList();
        }

        public List<Driver> GetDriversByOrder(Order vOrder, bool vStayin, bool vActive)
        {
            if (vOrder == null) return null;

            string oc = vOrder.City.ToLower();
            return GetAllDrivers(vStayin, vActive).Where(e => e.Stayin == vStayin && e.Active == vActive && oc.StartsWith(e.City.ToLower())).ToList();

        }

        public bool Signin(string vSigninName,string vSigninPassword)
        {
            bool success = false;
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                Driver dv = frctx.Drivers.FirstOrDefault(e => e.SigninName == vSigninName && e.SigninPassword == vSigninPassword && e.Active == true);
                if (dv == null) return success;
                dv.Stayin = true;
                dv.UpdatedDate = DateTime.Now;
                dv.UpdatedBy = vSigninName;
                try
                {
                    return frctx.SaveChanges() > 0 ? true : false;
                }
                catch
                {
                    return success;
                }
            }
        }
        public bool Signout(string vSigninName, string vSigninPassword)
        {
            bool success = false;
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                Driver dv = frctx.Drivers.FirstOrDefault(e => e.SigninName == vSigninName && e.SigninPassword == vSigninPassword && e.Active == true);
                if (dv == null) return success;
                dv.Stayin = false;
                dv.UpdatedDate = DateTime.Now;
                dv.UpdatedBy = vSigninName;
                try
                {
                    return frctx.SaveChanges() > 0 ? true : false;
                }
                catch
                {
                    return success;
                }
            }
        }
        public bool IsSigninNameExist(string vSigninName)
        {
            return GetAllDrivers().Where(e => e.SigninName == vSigninName).Count() > 0;
        }
        public int GetAllDriverCount(bool vActive)
        {
            return GetAllDrivers().Where(e => e.Active == vActive).Count();
        }

        public Driver AddDriver(int vDriverID, string vFirstName, string vLastName, string vSigninName, string vSigninPassword, bool vStayin, string vWorkArea, string vAddressLine, string vCity, 
                                 string vState, string vZipCode, string vPhone, string vEmail, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            Driver lDriver = new Driver();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vDriverID > 0)
                {
                    lDriver = frctx.Drivers.FirstOrDefault(u => u.DriverId == vDriverID);
                    lDriver.FirstName = vFirstName;
                    lDriver.LastName = vLastName;
                    lDriver.SigninName = vSigninName;
                    lDriver.SigninPassword = vSigninPassword;
                    lDriver.Stayin = vStayin;
                    lDriver.WorkArea = vWorkArea;
                    lDriver.AddressLine = vAddressLine;
                    lDriver.City = vCity;
                    lDriver.State = vState;
                    lDriver.ZipCode = vZipCode;
                    lDriver.Phone = vPhone;
                    lDriver.Email = vEmail;

                    lDriver.UpdatedDate = vUpdatedDate;
                    lDriver.UpdatedBy = vUpdatedBy;
                    lDriver.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lDriver : null;
                }
                else
                {
                    lDriver.FirstName = vFirstName;
                    lDriver.LastName = vLastName;
                    lDriver.SigninName = vSigninName;
                    lDriver.SigninPassword = vSigninPassword;
                    lDriver.Stayin = vStayin;
                    lDriver.WorkArea = vWorkArea;
                    lDriver.AddressLine = vAddressLine;
                    lDriver.City = vCity;
                    lDriver.State = vState;
                    lDriver.ZipCode = vZipCode;
                    lDriver.Phone = vPhone;
                    lDriver.Email = vEmail;

                    lDriver.AddedDate = vAddedDate;
                    lDriver.AddedBy = vAddedBy;
                    lDriver.UpdatedDate = vUpdatedDate;
                    lDriver.UpdatedBy = vUpdatedBy;
                    lDriver.Active = vActive;
                    return AddDriver(lDriver);
                }
            }
        }

        public Driver AddDriver(Driver vDriver)
        {
            try
            {
                Shoppingctx.Drivers.Add(vDriver);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vDriver : null;
            }
            catch
            {
                return null;
            }
        }
        private bool ChangeLockState(Driver vDriver, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                Driver up = frenty.Drivers.FirstOrDefault(e => e.DriverId == vDriver.DriverId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool LockDriver(Driver vDriver)
        {
            return ChangeLockState(vDriver, false);
        }

        public bool UnlockDriver(Driver vDriver)
        {
            return ChangeLockState(vDriver, true);
        }

        public bool DeleteDriver(Driver vDriver)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDriver(int vDriverID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteDriver(Driver vDriver)
        {
            throw new NotImplementedException();
        }

        public Driver UpdateDriver(Driver vDriver)
        {
            throw new NotImplementedException();
        }
    }
}
