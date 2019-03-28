using System.Web.Mvc;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using FR.Services.ViewServices;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Controllers
{
    public class AllZipcodesController : BaseController
    {
         private IBizInfoRepository BizInfoRepository;
        private IBizCuisineRepository BizCuisineRepository;
        public AllZipcodesController(IBizInfoRepository bizInfoRepo, IBizCuisineRepository bizCuisineRepo)
        {
            BizInfoRepository = bizInfoRepo;
            BizCuisineRepository = bizCuisineRepo;
        }
        public ActionResult Index(ShoppingCart cart, BrowseHistory bh)
        {
            ViewBag.delivery = bh.IsDelivery;
            BizInfoModel bim = new BizInfoModel();
            bim.AllZips = BizInfoRepository.GetAllZipCities(true);
            ViewBag.bagitems = GetCartItems(cart);
            return View(bim);
        }

        [OutputCache(Duration = 600)]
        public ActionResult ShowAllZips()
        {
            BizInfoModel bim = new BizInfoModel();
            List<BizInfo> lzips = BizInfoRepository.GetAllBizInfos(true);
            bim.ShowZipCodes = AllZopCodesView.ShowZipCodesView_Bootstarp(lzips, null, null);
            return PartialView(bim);
        }
    }
}
