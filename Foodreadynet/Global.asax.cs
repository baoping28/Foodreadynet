using FoodReady.WebUI.Binders;
using FoodReady.WebUI.NinjectDependency;
using FR.Domain.Model.Entities;
using IdentitySample.Models;
using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IdentitySample
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Application["WebHit"] = 600;
            Application["AppRun"] = "startpbp590828";
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            ModelBinders.Binders.Add(typeof(ShoppingCart), new CartModelBinder());
            ModelBinders.Binders.Add(typeof(BrowseHistory), new HistoryModelBinder());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Session_Start(object sender, EventArgs e)
        {

            Application.Lock();
            Application["WebHit"] = (int)Application["WebHit"] + 1;
            Application.UnLock();
            string request_cookies = Request.Headers["Cookie"];

            if ((null != request_cookies) && (request_cookies.IndexOf("ASP.NET_SessionId") >= 0))
            {

                //cookie existed, so this new one is due to timeout. 

                //Redirect the user to the login page

                Response.Redirect("~/Home");
            }



        }

    }
}
