using System;
using System.Web.Mvc;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "ShoppingCart";
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            // get the Cart from the session
            ShoppingCart cart = (ShoppingCart)controllerContext.HttpContext.Session[sessionKey];
            // create the Cart if there wasn't one in the session data
            if (cart == null)
            {
                cart = new ShoppingCart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }
            // return the cart
            return cart;
        }
    }

    public class HistoryModelBinder : IModelBinder
    {
        private const string sessionKey = "BorseHistory";
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            // get the history from the session
            BrowseHistory history = (BrowseHistory)controllerContext.HttpContext.Session[sessionKey];
            // create the history if there wasn't one in the session data
            if (history == null)
            {
                history = new BrowseHistory();
                history.IsDelivery = true;
                controllerContext.HttpContext.Session[sessionKey] = history;
            }
            // return the cart
            return history;
        }
    }
}