using System.Linq;
using System;
using System.Collections.Generic;
using FR.Repository.Interfaces;
using FR.Domain.Model.Entities;
using FR.Infrastructure.Helpers;


namespace FR.Repository.ShoppingRepository
{
    public class BizInfoRepository : BaseShoppingCartRepository, IBizInfoRepository
    {
        #region IBizInfoRepository Members
        public IQueryable<BizInfo> BizInfos
        {
            get
            {
                List<BizInfo> lBizInfos = default(List<BizInfo>);
                string lActiveBizInfosKey = CacheKey + "_IQueryBizInfos";

                if (base.EnableCaching && (Cache[lActiveBizInfosKey] != null))
                {
                    lBizInfos = (List<BizInfo>)Cache[lActiveBizInfosKey];
                }
                else
                {
                    lBizInfos = (from lBizInfo in Shoppingctx.BizInfos
                                 orderby lBizInfo.BizTitle
                                 select lBizInfo).ToList();
                    if (base.EnableCaching)
                    {
                        CacheData(lActiveBizInfosKey, lBizInfos, CacheDuration);
                    }
                }
                return lBizInfos.AsQueryable();
            }
        }
        public List<BizInfo> GetAllBizInfos()
        {
            List<BizInfo> lBizInfos = default(List<BizInfo>);
            string lBizInfosKey = CacheKey + "_AllBizInfos";

            if (base.EnableCaching && (Cache[lBizInfosKey] != null))
            {
                lBizInfos = (List<BizInfo>)Cache[lBizInfosKey];
            }
            else
            {
                lBizInfos = (from lBizInfo in Shoppingctx.BizInfos
                             orderby lBizInfo.BizTitle
                             select lBizInfo).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lBizInfosKey, lBizInfos, CacheDuration);
                }
            }
            return lBizInfos;
        }

        public List<BizInfo> GetAllBizInfos(bool vActive)
        {
            return GetAllBizInfos().Where(e => e.Active == vActive).ToList();
        }

        public Dictionary<string, List<CityBizInfosNumber>> GetAllStateCity(List<BizInfo> vBiList)
        {
            Dictionary<string, List<CityBizInfosNumber>> statecity = new Dictionary<string, List<CityBizInfosNumber>>();
            var bz = from b in vBiList
                     orderby b.Address.State, b.Address.City
                     group b by b.Address.State into g
                     select new { State = g.Key, Group = g, Count = g.Count() };
            foreach (var item in bz)
            {
                List<CityBizInfosNumber> st = new List<CityBizInfosNumber>();
                var ct = from i in item.Group
                         orderby i.Address.City
                         group i by i.Address.City into c
                         select new { City = c.Key, Group = c, Count = c.Count() };
                foreach (var e in ct)
                {
                    CityBizInfosNumber cbin = new CityBizInfosNumber();
                    cbin.CityName = e.City;
                    cbin.BizInfoNum = e.Count;
                    st.Add(cbin);
                }
                statecity.Add(item.State, st);
            }
            return statecity;
        }


        public List<string> GetAllZipCodes(bool vActive)
        {
            List<string> lzips = new List<string>();
            string lBizInfosKey = CacheKey + "_AllZips";

            if (base.EnableCaching && (Cache[lBizInfosKey] != null))
            {
                lzips = (List<string>)Cache[lBizInfosKey];
            }
            else
            {
                var lBizInfos = (from lBizInfo in Shoppingctx.BizInfos
                                 where lBizInfo.Active == vActive
                                 orderby lBizInfo.Address.ZipCode
                                 select new { ZipCode = lBizInfo.Address.ZipCode + "---" + lBizInfo.Address.City }).Distinct().ToList();
                foreach (var z in lBizInfos)
                {
                    lzips.Add(z.ZipCode);
                }
                if (base.EnableCaching)
                {
                    CacheData(lBizInfosKey, lzips, CacheDuration);
                }
            }
            return lzips;
        }

        public List<ZipCity> GetAllZipCities(bool vActive)
        {
            List<ZipCity> lzips = new List<ZipCity>();
            string lBizInfosKey = CacheKey + "_AllZips";

            if (base.EnableCaching && (Cache[lBizInfosKey] != null))
            {
                lzips = (List<ZipCity>)Cache[lBizInfosKey];
            }
            else
            {
                var lBizInfos = (from lBizInfo in Shoppingctx.BizInfos
                                 where lBizInfo.Active == vActive
                                 orderby lBizInfo.Address.ZipCode
                                 select new { ZipCode = lBizInfo.Address.ZipCode, City = lBizInfo.Address.City }).Distinct().ToList();
                foreach (var z in lBizInfos)
                {
                    lzips.Add(new ZipCity { ZipCode = z.ZipCode, City = z.City });
                }
                if (base.EnableCaching)
                {
                    CacheData(lBizInfosKey, lzips, CacheDuration);
                }
            }
            return lzips;
        }
        public List<string> GetBizAddressKeywords(bool vActive)
        {
            List<string> lkeys = new List<string>();
            string lKey = CacheKey + "_AllAddressKeywoods";

            if (base.EnableCaching && (Cache[lKey] != null))
            {
                lkeys = (List<string>)Cache[lKey];
            }
            else
            {
                var lBizKeys = (from l in Shoppingctx.BizInfos
                                where l.Active == vActive
                                select new
                                {
                                    bizTitleKey = l.BizTitle + "---" + l.Address.City,
                                    addressKey = l.Address.AddressLine,
                                    cityKey = l.Address.City,
                                    stateKey = l.Address.State,
                                    zipKey = l.Address.ZipCode + "---" + l.Address.City,
                                    phoneKey = l.ContactInfo.Phone + "---" + l.BizTitle,
                                    bizWholeAddress = l.Address.AddressLine + ", " + l.Address.City + ", " + l.Address.State + " " + l.Address.ZipCode
                                }).Distinct().ToList();
                foreach (var z in lBizKeys)
                {
                    lkeys.Add(z.bizTitleKey);
                    lkeys.Add(z.addressKey);
                    lkeys.Add(z.cityKey);
                    lkeys.Add(z.stateKey);
                    lkeys.Add(z.zipKey);
                    lkeys.Add(z.phoneKey);
                    lkeys.Add(z.bizWholeAddress);
                }
                if (base.EnableCaching)
                {
                    CacheData(lKey, lkeys, CacheDuration);
                }
            }
            return lkeys;
        }
        public int GetNumberOfCityInBizInfos(List<BizInfo> vList, bool vActive)
        {

            var lBizInfos = (from lBizInfo in vList
                             where lBizInfo.Active == vActive
                             orderby lBizInfo.BizTitle
                             select new { City = lBizInfo.Address.City.ToLower() }).Distinct().ToList();
            return lBizInfos.Count;
        }

        public List<string> GetAllCities()
        {
            List<string> lt = new List<string>();
            string lcityKey = CacheKey + "_Allcities";

            if (base.EnableCaching && (Cache[lcityKey] != null))
            {
                lt = (List<string>)Cache[lcityKey];
            }
            else
            {
                var l = (from b in GetAllBizInfos(true)
                         where b.Active == true
                         orderby b.Address.City
                         select new { City = b.Address.City }).Distinct().ToList();
                foreach (var c in l)
                {
                    lt.Add(c.City);
                }
                if (base.EnableCaching)
                {
                    CacheData(lcityKey, lt, CacheDuration);
                }
            }
            return lt;
        }
        public List<string> GetAllCities(bool vActive)
        {
            List<string> lt = new List<string>();
            var lBizInfos = (from lBizInfo in GetAllBizInfos(true)
                             where lBizInfo.Active == vActive
                             orderby lBizInfo.Address.City
                             select new { City = lBizInfo.Address.City, State = lBizInfo.Address.State }).Distinct().ToList();
            foreach (var c in lBizInfos)
            {
                lt.Add(c.City + "---" + c.State);
            }
            return lt;
        }
        public List<string> GetPopularCities(bool vActive)
        {
            List<string> lt = new List<string>();
            var lBizInfos = (from lBizInfo in GetAllBizInfos(true)
                             where lBizInfo.Active == vActive
                             orderby lBizInfo.Address.City
                             select new { City = lBizInfo.Address.City }).Distinct().ToList();
            foreach (var c in lBizInfos)
            {
                if (Helper.IsMostPopularCity(c.City))
                {
                    lt.Add(c.City);
                }
            }
            return lt;
        }

        public BizInfo GetBizInfoById(int vBizInfoID)
        {
            return GetAllBizInfos().Where(e => e.BizInfoId == vBizInfoID).FirstOrDefault();
        }

        public List<BizCuisine> GetBizCuisinesByBizInfoId(int vBizInfoID)
        {
            List<BizCuisine> lBizCuisines = default(List<BizCuisine>);
            string lBizCuisinesKey = CacheKey + "_BI" + vBizInfoID + "_BizCuisines";

            if (base.EnableCaching && (Cache[lBizCuisinesKey] != null))
            {
                lBizCuisines = (List<BizCuisine>)Cache[lBizCuisinesKey];
            }
            else
            {
                //Shoppingctx.BizCuisines.MergeOption = MergeOption.NoTracking;
                lBizCuisines = (from lBizCuisine in Shoppingctx.BizCuisines.Include("Categories")
                                where lBizCuisine.BizInfoId == vBizInfoID
                                select lBizCuisine).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lBizCuisinesKey, lBizCuisines, CacheDuration);
                }
            }
            return lBizCuisines;
        }

        public List<BizCuisine> GetBizCuisinesByBizInfoId(int vBizInfoID, bool vActive)
        {
            return GetBizCuisinesByBizInfoId(vBizInfoID).Where(e => e.Active == vActive).ToList();
        }

        public List<BizInfo> GetBizInfosByCuisineTypeId(int vCuisineTypeID)
        {
            List<BizInfo> lBizInfos = default(List<BizInfo>);
            string lBizInfosKey = CacheKey + "_CB" + vCuisineTypeID + "_BizInfos";

            if (base.EnableCaching && (Cache[lBizInfosKey] != null))
            {
                lBizInfos = (List<BizInfo>)Cache[lBizInfosKey];
            }
            else
            {
                lBizInfos = (from lBizInfo in Shoppingctx.BizInfos
                             from bc in lBizInfo.BizCuisines
                             where bc.CuisineTypeId == vCuisineTypeID
                             select lBizInfo).ToList();

                if (base.EnableCaching)
                {
                    CacheData(lBizInfosKey, lBizInfos, CacheDuration);
                }
            }
            return lBizInfos;
        }

        public List<BizInfo> GetBizInfosByCuisineTypeId(int vCuisineTypeID, bool vActive)
        {
            return GetBizInfosByCuisineTypeId(vCuisineTypeID).Where(e => e.Active == vActive).ToList();
        }

        public List<CuisineType> GetCuisineTypesByBizInfoId(int vBizInfoID)
        {
            List<CuisineType> lCuisineTypes = default(List<CuisineType>);
            string lCuisineTypesKey = CacheKey + "_CB" + vBizInfoID + "_CuisineTypes";

            if (base.EnableCaching && (Cache[lCuisineTypesKey] != null))
            {
                lCuisineTypes = (List<CuisineType>)Cache[lCuisineTypesKey];
            }
            else
            {
                lCuisineTypes = (from lCui in Shoppingctx.CuisineTypes
                                 from bc in lCui.BizCuisines
                                 where bc.BizInfoId == vBizInfoID
                                 select lCui).ToList();

                if (base.EnableCaching)
                {
                    CacheData(lCuisineTypesKey, lCuisineTypes, CacheDuration);
                }
            }
            return lCuisineTypes;
        }

        public List<BizInfo> GetBizInfosByCuisineType(string vCuisineType)
        {
            List<BizInfo> lBizInfos = default(List<BizInfo>);
            string lBizInfosKey = CacheKey + "_CB" + vCuisineType + "_BizInfos";

            if (base.EnableCaching && (Cache[lBizInfosKey] != null))
            {
                lBizInfos = (List<BizInfo>)Cache[lBizInfosKey];
            }
            else
            {
                lBizInfos = (from lBizInfo in Shoppingctx.BizInfos
                             from bc in lBizInfo.BizCuisines
                             where bc.CuisineType.Title.ToLower() == vCuisineType.ToLower()
                             select lBizInfo).ToList();

                if (base.EnableCaching)
                {
                    CacheData(lBizInfosKey, lBizInfos, CacheDuration);
                }
            }
            return lBizInfos;
        }
        public List<BizInfo> GetBizInfosByCityCuisineName(string vCity, string vCuisineName)
        {
            List<BizInfo> lBizInfos = default(List<BizInfo>);
            string lBizInfosKey = CacheKey + "_CC" + vCity + "_" + vCuisineName;

            if (base.EnableCaching && (Cache[lBizInfosKey] != null))
            {
                lBizInfos = (List<BizInfo>)Cache[lBizInfosKey];
            }
            else
            {
                lBizInfos = (from lBizInfo in Shoppingctx.BizInfos
                             from bc in lBizInfo.BizCuisines
                             where bc.BizInfo.Address.City.ToLower() == vCity.ToLower() && bc.CuisineType.Title.ToLower() == vCuisineName.ToLower()
                             select lBizInfo).ToList();

                if (base.EnableCaching)
                {
                    CacheData(lBizInfosKey, lBizInfos, CacheDuration);
                }
            }
            return lBizInfos;
        }
        public List<BizInfo> GetBizInfosByCityCuisineName(string vCity, string vCuisineName, bool vActive)
        {
            return GetBizInfosByCityCuisineName(vCity, vCuisineName).Where(e => e.Active == vActive).ToList();
        }
        public List<BizInfo> GetBizInfosByCuisineType(string vCuisineType, bool vActive)
        {
            return GetBizInfosByCuisineType(vCuisineType).Where(e => e.Active == vActive).ToList();
        }
        public List<CuisineType> GetCuisineTypesByBizInfoId(int vBizInfoID, bool vActive)
        {
            return GetCuisineTypesByBizInfoId(vBizInfoID).Where(e => e.Active == vActive).ToList();
        }
        public int GetAllBizInfoCount()
        {
            return Shoppingctx.BizInfos.Count();
        }

        public int GetAllBizInfoCount(bool vActive)
        {
            return Shoppingctx.BizInfos.Where(e => e.Active == vActive).Count();
        }

        public BizInfo GetBizInfoByUserId(string vUserID)
        {
            return GetAllBizInfos().Where(e => e.UserId == vUserID).FirstOrDefault();
        }

        public BizInfo GetBizInfoByUserId(string vUserID, bool vActive)
        {
            return GetAllBizInfos().Where(e => e.UserId == vUserID && e.Active == vActive).FirstOrDefault();
        }

        public BizInfo GetBizInfosByLogonName(string vLogonName)
        {
            return GetAllBizInfos().Where(e => e.AspNetUser.UserName == vLogonName).FirstOrDefault();
        }

        public BizInfo GetBizInfosByLogonName(string vLogonName, bool vActive)
        {
            return GetAllBizInfos().Where(e => e.AspNetUser.UserName == vLogonName && e.Active == vActive).FirstOrDefault();
        }

        public List<string> GetCitiesInCuisine(string vCuisine)
        {
            List<string> ls = new List<string>();
            var ct = from b in Shoppingctx.BizCuisines
                     where b.CuisineType.Title.ToLower() == vCuisine.ToLower() && b.Active == true
                     select new { City = b.BizInfo.Address.City };
            foreach (var c in ct)
            {
                ls.Add(c.City);
            }
            return ls.Distinct().ToList();
        }
        public List<string> GetCuisinesInCity(string vCity)
        {
            List<string> ls = new List<string>();
            var cus = from b in Shoppingctx.BizCuisines
                      where b.BizInfo.Address.City.ToLower() == vCity.ToLower() && b.Active == true
                      select new { Cuisine = b.CuisineType.Title };
            foreach (var c in cus)
            {
                ls.Add(c.Cuisine);
            }
            return ls.Distinct().ToList();
        }
        public List<string> GetCuisinesInZip(string vZip)
        {
            List<string> ls = new List<string>();
            var cus = from b in Shoppingctx.BizCuisines
                      where b.BizInfo.Address.ZipCode == vZip && b.Active == true
                      select new { Cuisine = b.CuisineType.Title };
            foreach (var c in cus)
            {
                ls.Add(c.Cuisine);
            }
            return ls.Distinct().ToList();
        }

        public List<string> GetZipsInCity(string vCity)
        {
            List<string> ls = new List<string>();
            var cus = from b in Shoppingctx.BizInfos
                      where b.Address.City.ToLower() == vCity.ToLower() && b.Active == true
                      select new { Zip = b.Address.ZipCode };
            foreach (var c in cus)
            {
                ls.Add(c.Zip);
            }
            return ls.Distinct().ToList();
        }
        public List<string> GetZipsInCityCuisine(string vCity, string vCuisine)
        {
            List<string> ls = new List<string>();
            List<BizInfo> lbi = new List<BizInfo>();
            lbi = GetBizInfosByCityCuisineName(vCity, vCuisine);
            var cus = from b in lbi
                      where b.Address.City.ToLower() == vCity.ToLower() && b.Active == true
                      select new { Zip = b.Address.ZipCode };
            foreach (var c in cus)
            {
                ls.Add(c.Zip);
            }
            return ls.Distinct().ToList();
        }
        public List<BizInfo> GetBizInfosWithOfferByCityZip(string vCity,string vZip)
        {
            if (string.IsNullOrEmpty(vCity))
            {
                return GetBizInfoByZip(vZip, true).Where(e => e.HasDiscountCoupons || e.HasFreeItemCoupons).ToList();
            }
            else if (string.IsNullOrEmpty(vZip))
            {
                return GetBizInfoByCity(vCity, true).Where(e => e.HasDiscountCoupons || e.HasFreeItemCoupons).ToList();
            }
            return GetAllBizInfos().Where(e => e.Address.City.ToLower() == vCity.ToLower() && e.Address.ZipCode==vZip && (e.HasDiscountCoupons || e.HasFreeItemCoupons)).ToList();
        }
        public List<BizInfo> GetBizInfosByCityZip(string vCity, string vZip)
        {
            if (string.IsNullOrEmpty(vCity))
            {
                return GetBizInfoByZip(vZip, true);
            }
            else if (string.IsNullOrEmpty(vZip))
            {
                return GetBizInfoByCity(vCity, true);
            }
            return GetAllBizInfos().Where(e => e.Address.City.ToLower() == vCity.ToLower() && e.Address.ZipCode == vZip && e.Active == true).ToList();
        }
        public List<BizInfo> GetBizInfoByCity(string vCity)
        {
            return GetAllBizInfos().Where(e => e.Address.City.ToLower() == vCity.ToLower()).ToList();
        }
        public List<BizInfo> GetBizInfoByCity(string vCity, bool vActive)
        {
            return GetBizInfoByCity(vCity).Where(e => e.Active == vActive).ToList();
        }
        public List<BizInfo> GetBizInfoByZip(string vZip)
        {
            return GetAllBizInfos().Where(e => e.Address.ZipCode == vZip).ToList();
        }
        public List<BizInfo> GetBizInfoByZip(string vZip, bool vActive)
        {
            return GetBizInfoByZip(vZip).Where(e => e.Active == vActive).ToList();
        }
        public List<BizInfo> GetBizInfoInMostPopularCities(bool vActive)
        {
            return GetAllBizInfos().Where(e => e.Active == true && Helper.IsMostPopularCity(e.Address.City)).OrderBy(e => e.Address.City).ToList();
        }

        public List<BizInfo> GetBizInfoInMostPopularLocalCities(bool vActive)
        {
            return GetAllBizInfos().Where(e => e.Active == true && Helper.IsMostPopularLocalCity(e.Address.City)).OrderBy(e => e.Address.City).ToList();
        }
        public List<string> GetMostPopularCities(bool vActive)
        {
            List<string> l = new List<string>();
            foreach (var m in GetBizInfoInMostPopularCities(vActive))
            {
                l.Add(m.Address.City);
            }
            return l.Distinct().ToList();
        }
        public List<string> GetMostPopularLocalCities(bool vActive)
        {
            List<string> l = new List<string>();
            foreach (var m in GetBizInfoInMostPopularLocalCities(vActive))
            {
                l.Add(m.Address.City);
            }
            return l.Distinct().ToList();
        }
        public BizInfo GetFirstBizInfoByCity(string vCity)
        {
            return GetBizInfoByCity(vCity).FirstOrDefault();
        }
        public BizInfo GetFirstBizInfoByCity(string vCity, bool vActive)
        {
            return GetBizInfoByCity(vCity, vActive).FirstOrDefault();
        }
        public List<BizInfo> GetTopnTopRatedBizInfosInCity(int vTopN, string vCity, bool vActive)
        {
            List<BizInfo> lBizs = default(List<BizInfo>);
            if (string.IsNullOrEmpty(vCity)) // All BizInfos
            {
                lBizs = GetAllBizInfos(vActive).OrderByDescending(x => x.AverageRating).Take(vTopN).ToList();

            }
            else
            {
                // In City
                lBizs = GetAllBizInfos(vActive).Where(e => e.Address.City.ToLower() == vCity.ToLower()).OrderByDescending(x => x.AverageRating).Take(vTopN).ToList();
            }
            return lBizs;
        }
        public List<BizInfo> GetTopnTopRatedBizInfosInzip(int vTopN, string vZip, bool vActive)
        {
            List<BizInfo> lBizs = default(List<BizInfo>);
            if (string.IsNullOrEmpty(vZip)) // All BizInfos
            {
                lBizs = GetAllBizInfos(vActive).OrderByDescending(x => x.AverageRating).Take(vTopN).ToList();

            }
            else
            {
                // In City
                lBizs = GetBizInfoByZip(vZip, vActive).OrderByDescending(x => x.AverageRating).Take(vTopN).ToList();
            }
            return lBizs;
        }
        public List<BizInfo> GetLastnNewBizInfosByCity(int vN, string vCity, bool vActive)
        {
            return GetBizInfoByCity(vCity, vActive).Where(e => e.AddedDate > DateTime.Now.AddMonths(-6)).OrderByDescending(x => x.AddedDate).Take(vN).ToList();
        }
        public List<BizInfo> GetLastnNewBizInfosByCityCuisineName(int vN, string vCity, string vCuisineName, bool vActive)
        {
            return GetBizInfosByCityCuisineName(vCity, vCuisineName, vActive).Where(e => e.AddedDate > DateTime.Now.AddMonths(-6)).OrderByDescending(x => x.AddedDate).Take(vN).ToList();
        }
        public List<BizInfo> GetLastnNewBizInfosByZip(int vN, string vZip, bool vActive)
        {
            return GetBizInfoByZip(vZip, vActive).Where(e => e.AddedDate > DateTime.Now.AddMonths(-6)).OrderByDescending(x => x.AddedDate).Take(vN).ToList();
        }

        public List<BizInfo> GetTopnTopRatedBizInfosByCityCuisine(int vN, string vCity, string vCuisine, bool vActive)
        {
            return GetBizInfosByCityCuisineName(vCity, vCuisine, vActive).OrderByDescending(x => x.AverageRating).Take(vN).ToList();
        }
        public BizInfo SetBizInfoRating(int vBizInfoID, int vRating)
        {
            BizInfo bi = new BizInfo();
            bi = Shoppingctx.BizInfos.FirstOrDefault(b => b.BizInfoId == vBizInfoID);
            bi.RatingVotes++;
            bi.TotalRating = bi.TotalRating + vRating;
            switch (vRating)
            {
                case 1: bi.OneStarVotes++;
                    break;
                case 2: bi.TwoStarVotes++;
                    break;
                case 3: bi.ThreeStarVotes++;
                    break;
                case 4: bi.FourStarVotes++;
                    break;
                case 5: bi.FiveStarVotes++;
                    break;
                default: break;
            }
            bi.UpdatedDate = DateTime.Now;
            return Shoppingctx.SaveChanges() > 0 ? bi : null;
        }
        public BizInfo AddBizInfo(int vBizInfoID, string vUserID, string vBizTitle, string vDescription, string vImageUrl,
                       int vAddressID, int vContactInfoID, int vBizHourID, string vYelpBizId, string vLatitude, string vLongitude, string vFax,string vFoodCost, bool vHasDiscountCoupons,
                       bool vHasFreeItemCoupons, bool vBreakfast, bool vHasLunchSpecial, bool vCanOrderForNextday, string vWebsite, bool vDelivery, int vDeliveryRadius,
                       decimal vDeliveryFee, decimal vDeliveryMinimum, decimal vTaxPercentageRate, int vRatingVotes,
                       int vTotalRating, int vFiveStarVotes, int vFourStarVotes, int vThreeStarVotes, int vTwoStarVotes,
                       int vOneStarVotes, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy,
                       bool vActive)
        {
            BizInfo lBizInfo = new BizInfo();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vBizInfoID > 0)
                {
                    lBizInfo = frctx.BizInfos.FirstOrDefault(u => u.BizInfoId == vBizInfoID);
                    lBizInfo.UserId = vUserID;
                    lBizInfo.BizTitle = vBizTitle;
                    lBizInfo.Description = vDescription;
                    lBizInfo.ImageUrl = vImageUrl;
                    lBizInfo.AddressId = vAddressID;
                    lBizInfo.ContactInfoId = vContactInfoID;
                    lBizInfo.BizHourId = vBizHourID;
                    lBizInfo.YelpBizId = vYelpBizId;
                    lBizInfo.Latitude = vLatitude;
                    lBizInfo.Longitude = vLongitude;
                    lBizInfo.Fax = vFax;
                    lBizInfo.FoodCost = vFoodCost;
                    lBizInfo.HasDiscountCoupons = vHasDiscountCoupons;
                    lBizInfo.HasFreeItemCoupons = vHasFreeItemCoupons;
                    lBizInfo.HasBreakfast = vBreakfast;
                    lBizInfo.HasLunchSpecial = vHasLunchSpecial;
                    lBizInfo.CanOrderForNextday = vCanOrderForNextday;
                    lBizInfo.Website = vWebsite;


                    lBizInfo.Delivery = vDelivery;
                    lBizInfo.DeliveryRadius = vDeliveryRadius;
                    lBizInfo.DeliveryFee = vDeliveryFee;
                    lBizInfo.DeliveryMinimum = vDeliveryMinimum;
                    lBizInfo.TaxPercentageRate = vTaxPercentageRate;

                    lBizInfo.RatingVotes = vRatingVotes;
                    lBizInfo.TotalRating = vTotalRating;

                    lBizInfo.FiveStarVotes = vFiveStarVotes;
                    lBizInfo.FourStarVotes = vFourStarVotes;
                    lBizInfo.ThreeStarVotes = vThreeStarVotes;
                    lBizInfo.TwoStarVotes = vTwoStarVotes;
                    lBizInfo.OneStarVotes = vOneStarVotes;

                    lBizInfo.UpdatedDate = vUpdatedDate;
                    lBizInfo.UpdatedBy = vUpdatedBy;
                    lBizInfo.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lBizInfo : null;
                }
                else
                {
                    lBizInfo.UserId = vUserID;
                    lBizInfo.BizTitle = vBizTitle;
                    lBizInfo.Description = vDescription;
                    lBizInfo.ImageUrl = vImageUrl;
                    lBizInfo.AddressId = vAddressID;
                    lBizInfo.ContactInfoId = vContactInfoID;
                    lBizInfo.BizHourId = vBizHourID;
                    lBizInfo.YelpBizId = vYelpBizId;
                    lBizInfo.Latitude = vLatitude;
                    lBizInfo.Longitude = vLongitude;
                    lBizInfo.Fax = vFax;
                    lBizInfo.FoodCost = vFoodCost;
                    lBizInfo.HasDiscountCoupons = vHasDiscountCoupons;
                    lBizInfo.HasFreeItemCoupons = vHasFreeItemCoupons;
                    lBizInfo.HasBreakfast = vBreakfast;
                    lBizInfo.HasLunchSpecial = vHasLunchSpecial;
                    lBizInfo.CanOrderForNextday = vCanOrderForNextday;
                    lBizInfo.Website = vWebsite;


                    lBizInfo.Delivery = vDelivery;
                    lBizInfo.DeliveryRadius = vDeliveryRadius;
                    lBizInfo.DeliveryFee = vDeliveryFee;
                    lBizInfo.DeliveryMinimum = vDeliveryMinimum;
                    lBizInfo.TaxPercentageRate = vTaxPercentageRate;

                    lBizInfo.RatingVotes = vRatingVotes;
                    lBizInfo.TotalRating = vTotalRating;

                    lBizInfo.FiveStarVotes = vFiveStarVotes;
                    lBizInfo.FourStarVotes = vFourStarVotes;
                    lBizInfo.ThreeStarVotes = vThreeStarVotes;
                    lBizInfo.TwoStarVotes = vTwoStarVotes;
                    lBizInfo.OneStarVotes = vOneStarVotes;

                    lBizInfo.AddedDate = vAddedDate;
                    lBizInfo.AddedBy = vAddedBy;
                    lBizInfo.UpdatedDate = vUpdatedDate;
                    lBizInfo.UpdatedBy = vUpdatedBy;
                    lBizInfo.Active = vActive;
                    return AddBizInfo(lBizInfo);
                }
            }
        }
        public bool UpdateLatLon(string vUserId, string vLat, string vLon)
        {
            BizInfo up = default(BizInfo);
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                up = frenty.BizInfos.FirstOrDefault(e => e.UserId == vUserId);
                up.UpdatedDate = DateTime.Now;
                up.Latitude = vLat;
                up.Longitude = vLon;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public BizInfo AddBizInfo(BizInfo vBizInfo)
        {
            try
            {
                Shoppingctx.BizInfos.Add(vBizInfo);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vBizInfo : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockBizInfo(BizInfo vBizInfo)
        {
            return ChangeLockState(vBizInfo, false);
        }
        public bool UnlockBizInfo(BizInfo vBizInfo)
        {
            return ChangeLockState(vBizInfo, true);
        }
        private bool ChangeLockState(BizInfo vBizInfo, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                BizInfo up = frenty.BizInfos.FirstOrDefault(e => e.BizInfoId == vBizInfo.BizInfoId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteBizInfo(BizInfo vBizInfo)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBizInfo(int vBizInfoID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteBizInfo(BizInfo vBizInfo)
        {
            throw new NotImplementedException();
        }

        public BizInfo UpdateBizInfo(BizInfo vBizInfo)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
