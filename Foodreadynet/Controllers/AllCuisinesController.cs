using System.Web.Mvc;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using FR.Services.ViewServices;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Controllers
{
    public class AllCuisinesController : BaseController
    {
        private IBizInfoRepository BizInfoRepository;
        private IBizCuisineRepository BizCuisineRepository;
        private ICuisineTypeRepository CuisineTypeRepository;
        public AllCuisinesController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo,
                                     ICuisineTypeRepository cuisineTypeRepo)
        {
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
            CuisineTypeRepository = cuisineTypeRepo;
        }
        public ActionResult Index(ShoppingCart cart, BrowseHistory bh)
        {
            ViewBag.bagitems = GetCartItems(cart);
            ViewBag.delivery = bh.IsDelivery;
            BizInfoModel bim = new BizInfoModel();
           // bim.ShowCities = AllCitiesView.ShowAllCities(BizInfoRepository.GetAllBizInfos(true), 4);
            List<BizCuisine> lbc = BizCuisineRepository.GetAllBizCuisines(true);
            bim.ShowCuisines = AllCuisinesView.ShowCuisinesView_Bootatrap(lbc, null, null);
            bim.AllCuisines = BizCuisineRepository.GetAllCuisines(true);
            return View(bim);
        }
        [OutputCache(Duration = 600)]
        public ActionResult ShowAllCuisines()
        {
            BizInfoModel bim = new BizInfoModel();
            // bim.ShowCities = AllCitiesView.ShowAllCities(BizInfoRepository.GetAllBizInfos(true), 4);
            List<BizCuisine> lbc = BizCuisineRepository.GetAllBizCuisines(true);
            bim.ShowCuisines = AllCuisinesView.ShowCuisinesView_Bootatrap(lbc, null, null);
            return PartialView(bim);
        }
        //[OutputCache(Duration = 600, VaryByParam = "id")]
        public ActionResult Cuisine(string id,ShoppingCart cart, BrowseHistory bh)
        {
            string cuisine = string.IsNullOrEmpty(id) ? string.Empty : DecodeString(id);
            ViewBag.delivery = bh.IsDelivery;
            BizInfoModel bim = new BizInfoModel();
            bim.CuisineName =cuisine;
            // bim.ShowCities = AllCitiesView.ShowAllCities(BizInfoRepository.GetAllBizInfos(true), 4);
            List<BizInfo> lbc = BizInfoRepository.GetBizInfosByCuisineType(cuisine, true);
            int n = BizInfoRepository.GetNumberOfCityInBizInfos(lbc, true);
            bim.ShowCities = AllCitiesView.ShowAllCities_Bootstrap(lbc, n, cuisine);
            bim.CitiesInCuisine = BizInfoRepository.GetCitiesInCuisine(cuisine);
            ViewBag.bagitems = GetCartItems(cart);
            return View(bim);
        }
    }
}
