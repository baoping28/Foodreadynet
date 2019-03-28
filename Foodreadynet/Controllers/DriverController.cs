using FoodReady.WebUI.Models;
using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodReady.WebUI.Controllers
{
    public class DriverController : BaseController
    {
        private IDriverRepository DriverRepository;
         public DriverController(IDriverRepository driverRepo)
        {
            DriverRepository = driverRepo;
        }
         public ActionResult Index(ShoppingCart cart)
         {
             ViewBag.bagitems = GetCartItems(cart);
             return View();
         }
        public ActionResult ResetDriverPassword()
        {
            TempData["resetMsg"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult ResetDriverPassword(ResetDriverPasswordModel model)
        {
            TempData["resetMsg"] = "";
            if (ModelState.IsValid)
            {
                Driver dr = DriverRepository.GetDriverByNamePassword(model.Name,model.OldPassword, true);
                if (dr==null)
                {
                    TempData["resetMsg"] = "The name or old password is incorrect.";
                    return View(model);
                }
                DriverRepository.AddDriver(dr.DriverId, dr.FirstName, dr.LastName, dr.SigninName, model.NewPassword, dr.Stayin, dr.WorkArea, dr.AddressLine, dr.City, dr.State, dr.ZipCode,
                                           dr.Phone, dr.Email, DateTime.Now, dr.SigninName, DateTime.Now, dr.SigninName, dr.Active);
                TempData["resetMsg"] = "Password has been successfully changed.";
                return RedirectToAction("Index", "Driver");
            }
            TempData["resetMsg"] = "Please check your input and try again.";
            return View(model);
        }
        [HttpPost]
        public ActionResult DriverIn(string vName, string vPassword)
        {
            string status = "unlock";
            string result = "";
            if (string.IsNullOrEmpty(vName) || string.IsNullOrEmpty(vPassword))
           {
               result = "Please input name and password.";
           }
            try
            {
                if (DriverRepository.Signin(vName, vPassword))
                {
                    result = "Successfully signed in !";
                    status = "lock";
                }
                else
                {
                    result = "Failed sign in.";
                }
            }
            catch
            {
                result = "Failed sign in.";
            }
            return Json(new
            {
                note = result,
                btn=status
            });
        }

        [HttpPost]
        public ActionResult DriverOut(string vName, string vPassword)
        {
            string status = "unlock";
            string result = "";
            if (string.IsNullOrEmpty(vName) || string.IsNullOrEmpty(vPassword))
            {
                result = "Please input name and password.";
            }
            try
            {
                if (DriverRepository.Signout(vName, vPassword))
                {
                    result = "Successfully signed out !";
                    status = "lock";
                }
                else
                {
                    result = "Failed sign out.";
                }
            }
            catch
            {
                result = "Failed sign out.";
            }
            return Json(new
            {
                note = result,
                btn = status
            });
        }
    }
}
