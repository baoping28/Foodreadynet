using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Models
{
    public class GiftCardModel
    {
        public List<GiftCard> GiftCards { get; set; }
        public GiftCard GiftCard { get; set; }
    }
}