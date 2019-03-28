using System.Web.Mvc;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using FR.Services.ViewServices;
using System.Linq;
using System.Text;
using FR.Domain.Model.Entities;
using System.Collections.Generic;
using FoodReady.WebUI.HtmlHelpers;

namespace FoodReady.WebUI.Controllers
{
    public class CityCuisineController   : BaseController
    {
        private IBizInfoRepository BizInfoRepository;
        private IBizCuisineRepository BizCuisineRepository;
        public CityCuisineController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo)
        {
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
        }

        public ActionResult Index(string city, string cuisine,ShoppingCart cart, BrowseHistory bh)
        {
            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(cuisine))
            {
                return RedirectToAction("Index", "AllCities");
            }

            ViewBag.delivery = bh.IsDelivery;
            BizInfoModel bim = new BizInfoModel();
            string ct = string.IsNullOrEmpty(city) ? string.Empty : DecodeString(city);
            bim.CityName = ct;
            bim.CuisineName = string.IsNullOrEmpty(cuisine) ? string.Empty : DecodeString(cuisine);
            bim.BizInfos = BizInfoRepository.GetBizInfosByCityCuisineName(ct, bim.CuisineName, true);
            bim.YelpTops = new List<TopYelpModel>();
            if ((HttpContext.Cache["Tops" + ct + bim.CuisineName] != null))
            {
                bim.YelpTops = (List<TopYelpModel>)HttpContext.Cache["Tops" + ct + bim.CuisineName];
            }
            else
            {
                YelpReviewModel yrm = new YelpReviewModel();
                foreach (var b in bim.BizInfos)
                {
                    yrm = YelpBizDetails.GetYelpBiz(b);
                    if (string.IsNullOrEmpty(b.YelpBizId) == false)
                    {
                        bim.YelpTops.Add(new TopYelpModel() { Biz = b, Yelp = yrm });
                    }
                }
                bim.YelpTops = bim.YelpTops.OrderByDescending(e => e.Yelp.Biz.rating).Take(3).ToList();
                HttpContext.Cache.Insert("Tops" + ct + bim.CuisineName, bim.YelpTops);
            }
           // bim.TopRatedBizInfos = BizInfoRepository.GetTopnTopRatedBizInfosByCityCuisine(3,ct,cuisine, true);
            //bim.BizInfos = BizInfoRepository.GetBizInfosByCityCuisineName(ct,cuisine, true).OrderByDescending(x => x.AverageRating).ToList();
            bim.BizInfo = bim.BizInfos.FirstOrDefault(); ;
            bim.ZipsInCityCuisine = BizInfoRepository.GetZipsInCityCuisine(ct, bim.CuisineName);
            if (string.IsNullOrEmpty(city) == false && string.IsNullOrEmpty(cuisine) == false)
            {
                bim.NewBiz = BizInfoRepository.GetLastnNewBizInfosByCityCuisineName(4, ct, cuisine, true);
            }
            bim.ShowZipCodes = AllZopCodesView.ShowZipCodesView_Bootstarp(BizInfoRepository.GetBizInfosByCityCuisineName(ct, cuisine, true), ct, cuisine);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var b in bim.YelpTops)
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
            if (bim.YelpTops.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            bim.MapMarkers = sb.ToString();
            ViewBag.bagitems = GetCartItems(cart);
            return View(bim); 
        }

    }
}
