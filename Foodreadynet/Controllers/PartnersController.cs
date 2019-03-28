using System.Web.Mvc;
using System.Text;
using System.Linq;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
using FR.Services.FilterServces;
using FoodReady.WebUI.EmailServices;
using FR.Infrastructure.Helpers;
using System.Web.Configuration;
namespace FoodReady.WebUI.Controllers
{
    public class PartnersController : BaseController
    {
        private ICuisineTypeRepository CuisineTypeRepository;
        public PartnersController(ICuisineTypeRepository cuisineTypeRepo)
        {
            CuisineTypeRepository = cuisineTypeRepo;
        }
        //[OutputCache(Duration = 1000)]
        public ActionResult Index(ShoppingCart cart)
        {
            RestaurantJoinModel rjm = new RestaurantJoinModel();
            List<CuisineType> lct = CuisineTypeRepository.GetAllCuisineTypes(true);
            rjm.CuisineAssistances = new List<SelectListItem>();
            foreach (var c in lct)
            {
                rjm.CuisineAssistances.Add(new SelectListItem { Text = c.Title, Value = c.Title });
            }
            ViewBag.servicephone = ServicePhone;
            ViewBag.bagitems = GetCartItems(cart);
            return View(rjm);
        }
        [HttpPost]
        public ActionResult InfoSubmit(RestaurantJoinModel model)
        {
            bool sendSuccess = false;
            if (ModelState.IsValid)
            {
            //ViewBag.qqq = EmailManager.BuildPartnerInfoHtmlBody(model);
            EmailManager em = new EmailManager();
            EmailContents ec = new EmailContents(model.Name, Globals.Settings.ContactForm.MailTo, model.Email,
                                "Restaurant Application", EmailManager.BuildPartnerInfoHtmlBody(model));

            em.FaxBody = EmailManager.BuildPartnerInfoHtmlBody(model);
            em.SendFax(WebConfigurationManager.AppSettings["OnlineFaxNumber"].ToString());
            em.Send(ec); // send to foodready.net
            if (em.IsSent == false)
            {
                sendSuccess = false;
                TempData["SentIMEALMsg"] = "Your message has not been sent out for some reasons.";
            }
            else
            {
                sendSuccess = true;
                TempData["SentIMEALMsg"] = "Your message has  been sent out successfully.";
            }
            ec = new EmailContents("Foodready.net", model.Email, Globals.Settings.ContactForm.MailTo, "Restaurant Application", EmailManager.BuildImealHtmlBody());
            em.Send(ec); // send to Partner
            if (em.IsSent == false)
            {
                sendSuccess = false;
                TempData["SentPartnerMsg"] = "Your message has not been sent out for some reasons.";
            }
            else
            {
                sendSuccess = true;
                TempData["SentPartnerMsg"] = "Your message has  been sent out successfully.";
            }
            }
            ViewBag.SendingSuccess = sendSuccess;
            return PartialView(model);
        }
    }
}
