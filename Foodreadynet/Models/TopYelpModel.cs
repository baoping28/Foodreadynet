using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Models
{
    public class TopYelpModel
    {
        public BizInfo Biz { get; set; }
        public YelpReviewModel Yelp { get; set; }
    }
}