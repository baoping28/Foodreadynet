using System;

namespace FoodReady.WebUI.Models
{
    public class PayModel
    {
        public CheckoutModel CreditCardModel { get; set; }
        public CheckoutPayPalModel PaypalModel { get; set; }
    }
}