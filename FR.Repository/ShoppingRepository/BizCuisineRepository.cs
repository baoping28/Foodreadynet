using FR.Domain.Model.Entities;
using FR.Infrastructure.Helpers;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class BizCuisineRepository : BaseShoppingCartRepository, IBizCuisineRepository
    {
        #region IBizCuisineRepository Members

        public List<BizCuisine> GetAllBizCuisines()
        {
            List<BizCuisine> lBizCuisines = default(List<BizCuisine>);
            string lBizCuisinesKey = CacheKey + "_AllBizCuisines";

            if (base.EnableCaching && (Cache[lBizCuisinesKey] != null))
            {
                lBizCuisines = (List<BizCuisine>)Cache[lBizCuisinesKey];
            }
            else
            {
                lBizCuisines = (from lBizCuisine in Shoppingctx.BizCuisines.Include("BizInfo").Include("CuisineType")
                                select lBizCuisine).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lBizCuisinesKey, lBizCuisines, CacheDuration);
                }
            }
            return lBizCuisines;
        }

        public List<BizCuisine> GetAllBizCuisines(bool vActive)
        {
            return GetAllBizCuisines().Where(e => e.Active == vActive).ToList();
        }

        public List<BizCuisine> GetBizCuisinesInMostPopularCuisines(bool vActive)
        {
            return GetAllBizCuisines(true).Where(e => Helper.IsPopularCuisine(e.CuisineTypeName)).OrderBy(e => e.CuisineTypeName).ToList();
        }
        public List<string> GetMostPopularCuisines(bool vActive)
        {
            List<string> l = new List<string>();
            foreach (var m in GetBizCuisinesInMostPopularCuisines(vActive))
            {
                l.Add(m.CuisineTypeName);
            }
            return l.Distinct().ToList();
        }
        public BizCuisine GetBizCuisineById(int vBizCuisineID)
        {
            return GetAllBizCuisines().Where(e => e.BizCuisineId == vBizCuisineID).FirstOrDefault();
        }

        public int GetNumberOfCuisineInBizCuisines(List<BizCuisine> vList, bool vActive)
        {
            var lb = (from l in vList
                      where l.Active == vActive
                      select new { CuisineType = l.CuisineType.Title }).Distinct().ToList();
            return lb.Count;

        }
        public List<string> GetAllCuisines(bool vActive)
        {
            List<string> lt = new List<string>();
            var lts = (from l in GetAllBizCuisines(true)
                       where l.Active == vActive
                       orderby l.CuisineTypeName
                       select new { CuisineName = l.CuisineTypeName }).Distinct().ToList();
            foreach (var c in lts)
            {
                lt.Add(c.CuisineName);
            }
            return lt;
        }
        public List<string> GetPopularCuisines(bool vActive)
        {
            List<string> lt = new List<string>();
            var lts = (from l in GetBizCuisinesInMostPopularCuisines(true)
                       where l.Active == vActive
                       orderby l.CuisineTypeName
                       select new { CuisineName = l.CuisineTypeName }).Distinct().ToList();
            foreach (var c in lts)
            {
                lt.Add(c.CuisineName);
            }
            return lt;
        }
        public int GetAllBizCuisineCount()
        {
            return Shoppingctx.BizCuisines.Count();
        }

        public int GetAllBizCuisineCount(bool vActive)
        {
            return Shoppingctx.BizCuisines.Where(e => e.Active == vActive).Count();
        }
        public List<BizCuisine> GetBizCuisinesByBizInfoId(int vBizInfoId)
        {
            return GetAllBizCuisines().Where(e => e.BizInfoId == vBizInfoId).ToList();
        }
        public List<BizCuisine> GetBizCuisinesByBizInfoId(bool vActive, int vBizInfoId) // break sometimes
        {
            return GetAllBizCuisines().Where(e => e.BizInfoId == vBizInfoId && e.Active == vActive).ToList();
        }
        public BizCuisine GetBizCuisineByBizID_CuisineTypeID(int vBizInfoID, int vCuisineTypeID)
        {
            return GetAllBizCuisines().Where(e => e.BizInfoId == vBizInfoID && e.CuisineTypeId == vCuisineTypeID).FirstOrDefault();
        }
        public List<BizCuisine> GetBizCuisinesByCity(bool vActive, string vCity)
        {
            return GetAllBizCuisines().Where(e => e.BizInfo.Address.City.ToLower() == vCity.ToLower() && e.Active == vActive).ToList();
        }
        public List<BizCuisine> GetBizCuisinesByZip(bool vActive, string vZip)
        {
            return GetAllBizCuisines(true).Where(e => e.BizInfo.Address.ZipCode == vZip).ToList();
        }
        public List<string> GetCuisineKeywoods(bool vActive)
        {
            List<string> lkeys = new List<string>();
            string lKey = CacheKey + "_AllCuisineKeywoods";

            if (base.EnableCaching && (Cache[lKey] != null))
            {
                lkeys = (List<string>)Cache[lKey];
            }
            else
            {
                var lCuisineKeys = (from l in Shoppingctx.BizCuisines
                                    where l.Active == vActive
                                    select new
                                    {
                                        cuisineKey = l.CuisineType.Title
                                    }).Distinct().ToList();
                foreach (var z in lCuisineKeys)
                {
                    lkeys.Add(z.cuisineKey);
                }
                if (base.EnableCaching)
                {
                    CacheData(lKey, lkeys, CacheDuration);
                }
            }
            return lkeys;
        }
        public BizCuisine AddBizCuisine(int vBizCuisineID, int vBizInfoID, int vCuisineTypeID, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            BizCuisine lBizCuisine = new BizCuisine();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vBizCuisineID > 0)
                {
                    lBizCuisine = frctx.BizCuisines.FirstOrDefault(u => u.BizCuisineId == vBizCuisineID);
                    lBizCuisine.UpdatedDate = vUpdatedDate;
                    lBizCuisine.UpdatedBy = vUpdatedBy;
                    lBizCuisine.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lBizCuisine : null;
                }
                else
                {
                    lBizCuisine.AddedDate = vAddedDate;
                    lBizCuisine.AddedBy = vAddedBy;
                    lBizCuisine.BizInfoId = vBizInfoID;
                    lBizCuisine.CuisineTypeId = vCuisineTypeID;
                    lBizCuisine.UpdatedDate = vUpdatedDate;
                    lBizCuisine.UpdatedBy = vUpdatedBy;
                    lBizCuisine.Active = vActive;
                    return AddBizCuisine(lBizCuisine);
                }
            }
        }

        public BizCuisine AddBizCuisine(BizCuisine vBizCuisine)
        {
            try
            {
                Shoppingctx.BizCuisines.Add(vBizCuisine);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vBizCuisine : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockBizCuisine(BizCuisine vBizCuisine)
        {
            return ChangeLockState(vBizCuisine, false);
        }
        public bool UnlockBizCuisine(BizCuisine vBizCuisine)
        {
            return ChangeLockState(vBizCuisine, true);
        }
        private bool ChangeLockState(BizCuisine vBizCuisine, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                BizCuisine up = frenty.BizCuisines.FirstOrDefault(e => e.BizCuisineId == vBizCuisine.BizCuisineId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteBizCuisine(BizCuisine vBizCuisine)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBizCuisine(int vBizCuisineID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteBizCuisine(BizCuisine vBizCuisine)
        {
            throw new NotImplementedException();
        }

        public BizCuisine UpdateBizCuisine(BizCuisine vBizCuisine)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
