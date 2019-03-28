using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using FR.Domain.Model.Entities;
using FR.Infrastructure.Helpers;
namespace FoodReady.WebUI.Models
{
    public class SearchModel
    {
        public BrowseHistory History;
        public List<BizInfo> BizInfos { get; set; }
        public List<Product> Products { get; set; }
    }
    public class BizInfoModel
    {
        public BrowseHistory History;
        public string CityName { get; set; }
        public string CuisineName { get; set; }
        public string ZipCode { get; set; }
        public BizInfo BizInfo { get; set; }
        public List<BizInfo> NewBiz { get; set; }
        public List<BizInfo> BizInfos { get; set; }
        public List<BizInfo> TopRatedBizInfos { get; set; }
        public string ShowCities { get; set; }
        public string ShowCuisines { get; set; }
        public string ShowZipCodes { get; set; }
        public string ShowMostPopularCities { get; set; }
        public string ShowMostPopularCuisines { get; set; }
        public string MapMarkers { get; set; }
        public List<string> CityState { get; set; }
        public List<string> AllCuisines { get; set; }
        public List<ZipCity> AllZips { get; set; }
        public List<string> CitiesInCuisine { get; set; }
        public List<string> CuisinesInZip { get; set; }
        public List<string> ZipsInCityCuisine { get; set; }
        public List<TopYelpModel> YelpTops { get; set; }
        public List<HotelType> HotelTypes { get; set; }
    }
    public class CityModel
    {
        public BrowseHistory History;
        public string CityName { get; set; }
        public string CuisineName { get; set; }
        public string ZipCode { get; set; }
        public BizInfo BizInfo { get; set; }
        public List<BizInfo> BizInfos { get; set; }
        public List<BizInfo> NewBiz { get; set; }
        public string MapMarkers { get; set; }
        public string ShowZipcodes { get; set; }
        public string ShowCuisines { get; set; }
        public List<string> CuisinesInCity { get; set; }
        public List<string> ZipsInCity { get; set; }
        public List<TopYelpModel> YelpTops { get; set; }
    }
}