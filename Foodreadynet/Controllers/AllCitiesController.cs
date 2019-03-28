using System.Web.Mvc;
using System.Text;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using FR.Services.ViewServices;
using System.Collections.Generic;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Helpers;
using FoodReady.WebUI.HtmlHelpers;
namespace FoodReady.WebUI.Controllers
{
    public class AllCitiesController : BaseController
    {
        private IBizInfoRepository BizInfoRepository;
        private IBizCuisineRepository BizCuisineRepository;
        public AllCitiesController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo)
        {
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
        }

        //[OutputCache(Duration = 600, VaryByCustom = "IsMobile")]
        public ActionResult Index(ShoppingCart cart, BrowseHistory bh)
        {
            ViewBag.delivery = bh.IsDelivery;
            BizInfoModel bim = new BizInfoModel();
            bim.CityState = BizInfoRepository.GetAllCities(true);
            ViewBag.bagitems = GetCartItems(cart);
            return View(bim);
        }

        [OutputCache(Duration = 600)]
        public ActionResult ShowAllCities()
        {
            BizInfoModel bim = new BizInfoModel();
            List<BizInfo> lbi = BizInfoRepository.GetAllBizInfos(true);
            int n = BizInfoRepository.GetNumberOfCityInBizInfos(lbi, true);
            // bim.ShowCities = AllCitiesView.ShowAllCities(lbi, n, null, 4);
            bim.ShowCities = AllCitiesView.ShowAllCities_Bootstrap(lbi, n, null);
            bim.ShowCuisines = AllCuisinesView.ShowCuisinesView(BizCuisineRepository.GetAllBizCuisines(true), null, null);
            bim.ShowMostPopularCities = AllCitiesView.ShowMostPopularCities(BizInfoRepository.GetBizInfoInMostPopularCities(true), 4);
            return PartialView(bim);

        }
        //[OutputCache(Duration = 600, VaryByParam = "id")]
        public ActionResult City(string id,ShoppingCart cart, BrowseHistory bh)
        {
            ViewBag.bagitems = GetCartItems(cart);
            ViewBag.delivery = bh.IsDelivery;
            string ct = string.IsNullOrEmpty(id) ? string.Empty : DecodeString(id);
            CityModel cm = new CityModel();
            cm.CityName = ct;
            cm.YelpTops = new List<TopYelpModel>();
            if ((HttpContext.Cache["CityTops_" + id] != null))
            {
                cm.YelpTops = (List<TopYelpModel>)HttpContext.Cache["CityTops_" + id];
            }
            else
            {
                List<BizInfo> lbi = BizInfoRepository.GetBizInfoByCity(ct, true);
                YelpReviewModel yrm = new YelpReviewModel();
                foreach (var b in lbi)
                {
                    yrm = YelpBizDetails.GetYelpBiz(b);
                    if (string.IsNullOrEmpty(b.YelpBizId) == false)
                    {
                        cm.YelpTops.Add(new TopYelpModel() { Biz = b, Yelp = yrm });
                    }
                }
                cm.YelpTops = cm.YelpTops.OrderByDescending(e => e.Yelp.Biz.rating).Take(3).ToList();
                HttpContext.Cache.Insert("CityTops_" + id, cm.YelpTops);
            }
            //cm.BizInfos = BizInfoRepository.GetTopnTopRatedBizInfosInCity(3, ct, true);
            cm.NewBiz = BizInfoRepository.GetLastnNewBizInfosByCity(4, ct, true);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var b in cm.YelpTops)
            {
                sb.Append("[");
                sb.Append("'" + b.Biz.BizTitle + "',");
                sb.Append("'" + b.Biz.Address.AddressLine + "',");
                sb.Append("'" + b.Biz.Address.City + "',");
                sb.Append("'" + b.Biz.Address.State + "',");
                sb.Append("'" + b.Biz.Address.ZipCode + "',");
                sb.Append("'" + b.Biz.Latitude + "',");
                sb.Append("'" + b.Biz.Longitude + "',");
                sb.Append("'" + b.Biz.BizInfoId + "',");
                sb.Append("'" + b.Biz.ImageUrl + "'],");

            }
            if (cm.YelpTops.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            cm.MapMarkers = sb.ToString();
            cm.BizInfo = BizInfoRepository.GetBizInfoByCity(ct, true).FirstOrDefault();
            cm.ShowCuisines = AllCuisinesView.ShowCuisinesView_Bootatrap(BizCuisineRepository.GetBizCuisinesByCity(true, ct), ct, null);
            cm.ShowZipcodes = AllZopCodesView.ShowZipCodesView_Bootstarp(BizInfoRepository.GetBizInfoByCity(ct, true), ct, null);
            cm.CuisinesInCity = BizInfoRepository.GetCuisinesInCity(ct);
            cm.ZipsInCity = BizInfoRepository.GetZipsInCity(ct);
            return View(cm);
        }
        [OutputCache(Duration = 600, VaryByParam = "id")]
        public ActionResult ShowThemeChart(int id)
        {
            BizInfo p = BizInfoRepository.GetBizInfoById(id);

            var myChart = new System.Web.Helpers.Chart(width: 200, height: 160, theme: ChartTheme.Green)
         .AddTitle(p.AverageRating.ToString() + " out of 5 stars")
         .AddSeries(
             name: "Reviews",
             xValue: new[] { "5 Star(" + p.FiveStarVotes.ToString() + "):", "4 Star(" + p.FourStarVotes.ToString() + "):", "3 Star(" + p.ThreeStarVotes.ToString() + "):", "2 Star(" + p.TwoStarVotes.ToString() + "):", "1 Star(" + p.OneStarVotes.ToString() + "):" },
             yValues: new[] { (p.FiveStarVotes * 5).ToString(), (p.FourStarVotes * 4).ToString(), (p.ThreeStarVotes * 3).ToString(), (p.TwoStarVotes * 2).ToString(), p.OneStarVotes.ToString() })
                .GetBytes("png");
            return File(myChart, "image/png");
        }
    }
}
