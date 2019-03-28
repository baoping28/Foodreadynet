using System.Web.Mvc;
using System.Text;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using FR.Services.ViewServices;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Collections.Generic;
using FoodReady.WebUI.HtmlHelpers;
namespace FoodReady.WebUI.Controllers
{
    public class CityZipController  : BaseController
    {
        private IBizInfoRepository BizInfoRepository;
        private IBizCuisineRepository BizCuisineRepository;
        public CityZipController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo)
        {
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
        }


        public ActionResult Index(string city, string zip,ShoppingCart cart, BrowseHistory bh)
        {
            if (string.IsNullOrEmpty(city) && string.IsNullOrEmpty(zip))
            {
                return RedirectToAction("Index", "AllCities");
            }

            ViewBag.delivery = bh.IsDelivery;
            BizInfoModel bim = new BizInfoModel();
            string ct =string.IsNullOrEmpty(city)?string.Empty : DecodeString(city);
            bim.CityName = ct;
            bim.ZipCode = zip;
            bim.BizInfos = BizInfoRepository.GetBizInfoByZip(zip, true);
            bim.YelpTops = new List<TopYelpModel>();
            if ((HttpContext.Cache["CityZip" + ct + zip] != null))
            {
                bim.YelpTops = (List<TopYelpModel>)HttpContext.Cache["CityZip" + ct + zip];
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
                HttpContext.Cache.Insert("CityZip" + ct + zip, bim.YelpTops);
            }
            // bim.TopRatedBizInfos = BizInfoRepository.GetTopnTopRatedBizInfosInzip(3, zip, true);
            bim.BizInfo = bim.BizInfos.FirstOrDefault();
            bim.NewBiz = BizInfoRepository.GetLastnNewBizInfosByZip(4, zip, true);
           bim.ShowCuisines = AllCuisinesView.ShowCuisinesView_Bootatrap(BizCuisineRepository.GetBizCuisinesByZip(true,zip),ct,zip);
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
           bim.CuisinesInZip = BizInfoRepository.GetCuisinesInZip(zip);
           ViewBag.bagitems = GetCartItems(cart);
            return View(bim); 
        }

    }
}
