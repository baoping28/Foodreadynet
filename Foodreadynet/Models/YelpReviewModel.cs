using System;
using YelpSharp;
namespace FoodReady.WebUI.Models
{
    public class YelpReviewModel
    {
        public YelpSharp.Data.Business Biz { get; set; }
        public YelpSharp.Data.Review Review { get; set; }
        public YelpSharp.Data.User User { get; set; }
    }
}