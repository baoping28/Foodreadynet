using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
using System.Linq;
using FR.Infrastructure.Helpers;

namespace FR.Repository.Interfaces
{
    public interface IBizInfoRepository
    {
        IQueryable<BizInfo> BizInfos { get; }
        List<BizInfo> GetAllBizInfos();
        List<BizInfo> GetAllBizInfos(bool vActive);
        int GetNumberOfCityInBizInfos(List<BizInfo> vList, bool vActive);
        List<string> GetAllCities();
        List<string> GetAllCities(bool vActive);
        List<string> GetPopularCities(bool vActive);
        Dictionary<string, List<CityBizInfosNumber>> GetAllStateCity(List<BizInfo> vBiList);
        List<string> GetAllZipCodes(bool vActive);
        List<ZipCity> GetAllZipCities(bool vActive);
        List<string> GetBizAddressKeywords(bool vActive);
        BizInfo GetBizInfoById(int vBizInfoID);
        List<BizCuisine> GetBizCuisinesByBizInfoId(int vBizInfoID);
        List<BizCuisine> GetBizCuisinesByBizInfoId(int vBizInfoID, bool vActive);
        List<BizInfo> GetBizInfosByCuisineTypeId(int vCuisineTypeID);
        List<BizInfo> GetBizInfosByCuisineTypeId(int vCuisineTypeID, bool vActive);
        List<BizInfo> GetBizInfosByCuisineType(string vCuisineType);
        List<BizInfo> GetBizInfosByCuisineType(string vCuisineType, bool vActive);
        List<BizInfo> GetBizInfosByCityCuisineName(string vCity, string vCuisineName);
        List<BizInfo> GetBizInfosByCityCuisineName(string vCity, string vCuisineName, bool vActive);
        List<CuisineType> GetCuisineTypesByBizInfoId(int vBizInfoID);
        List<CuisineType> GetCuisineTypesByBizInfoId(int vBizInfoID, bool vActive);
        int GetAllBizInfoCount();
        int GetAllBizInfoCount(bool vActive);
        BizInfo GetBizInfoByUserId(string vUserID);
        BizInfo GetBizInfoByUserId(string vUserID, bool vActive);
        BizInfo GetBizInfosByLogonName(string vLogonName);
        BizInfo GetBizInfosByLogonName( string vLogonName, bool vActive);
        List<string> GetCitiesInCuisine(string vCuisine);
        List<string> GetCuisinesInCity(string vCity);
        List<string> GetCuisinesInZip(string vZip);
        List<string> GetZipsInCity(string vCity);
        List<string> GetZipsInCityCuisine(string vCity, string vCuisine);
        List<BizInfo> GetBizInfosWithOfferByCityZip(string vCity, string vZip);
        List<BizInfo> GetBizInfosByCityZip(string vCity, string vZip);
        List<BizInfo> GetBizInfoByCity(string vCity);
        List<BizInfo> GetBizInfoByCity(string vCity, bool vActive);
        List<BizInfo> GetBizInfoInMostPopularCities(bool vActive);
        List<BizInfo> GetBizInfoInMostPopularLocalCities(bool vActive);
        List<string> GetMostPopularCities(bool vActive);
        List<string> GetMostPopularLocalCities(bool vActive);
        List<BizInfo> GetBizInfoByZip(string vZip);
         List<BizInfo>GetBizInfoByZip(string vZip, bool vActive);
        BizInfo GetFirstBizInfoByCity(string vCity);
        BizInfo GetFirstBizInfoByCity(string vCity, bool vActive);
        List<BizInfo> GetTopnTopRatedBizInfosInCity(int vTopN, string vCity, bool vActive);
        List<BizInfo> GetTopnTopRatedBizInfosInzip(int vTopN, string vZip, bool vActive);
        List<BizInfo> GetLastnNewBizInfosByCity(int vN, string vCity, bool vActive);
        List<BizInfo> GetLastnNewBizInfosByCityCuisineName(int vN, string vCity, string vCuisineName, bool vActive);
        List<BizInfo> GetLastnNewBizInfosByZip(int vN, string vZip, bool vActive);
        List<BizInfo> GetTopnTopRatedBizInfosByCityCuisine(int vN, string vCity, string vCuisine, bool vActive);
        BizInfo SetBizInfoRating(int vBizInfoID, int vRating);
        bool UpdateLatLon(string vUserId, string vLat, string vLon);
        BizInfo AddBizInfo(int vBizInfoID, string vUserID, string vBizTitle, string vDescription, string vImageUrl,
                       int vAddressID, int vContactInfoID, int vBizHourID, string vYelpBizId, string vLatitude, string vLongitude, string vFax, string vFoodCost, bool vHasDiscountCoupons,
                       bool vHasFreeItemCoupons, bool vBreakfast, bool vHasLunchSpecial, bool vCanOrderForNextday, string vWebsite, bool vDelivery, int vDeliveryRadius,
                       decimal vDeliveryFee, decimal vDeliveryMinimum, decimal vTaxPercentageRate, int vRatingVotes,
                       int vTotalRating, int vFiveStarVotes, int vFourStarVotes, int vThreeStarVotes, int vTwoStarVotes,
                       int vOneStarVotes, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy,
                       bool vActive);
        BizInfo AddBizInfo(BizInfo vBizInfo);
        bool LockBizInfo(BizInfo vBizInfo);
        bool UnlockBizInfo(BizInfo vBizInfo);
        bool DeleteBizInfo(BizInfo vBizInfo);
        bool DeleteBizInfo(int vBizInfoID);
        bool UnDeleteBizInfo(BizInfo vBizInfo);
        BizInfo UpdateBizInfo(BizInfo vBizInfo);
    }
}