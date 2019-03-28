using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FR.Domain.Model.Entities;
using YelpSharp;
using FoodReady.WebUI.Configs;
using FoodReady.WebUI.Models;
using FR.Infrastructure.Helpers;

namespace FoodReady.WebUI.HtmlHelpers
{
    public static class YelpBizDetails
    {
        public static YelpReviewModel GetYelpBiz(BizInfo vBiz)
        {
            YelpReviewModel yrm = new YelpReviewModel();
            Yelp yelp = new Yelp(Config.Options);
          //  var results = yelp.Search("Restaurants " + vBiz.BizCuisines.FirstOrDefault().CuisineTypeName, vBiz.BizAddressString).Result;
            YelpSharp.Data.Business biz = new YelpSharp.Data.Business();
           // biz=results.businesses.FirstOrDefault(e => e.phone == vBiz.ContactInfo.Phone);
           // biz = (biz == null) ? null : yelp.GetBusiness(biz.id).Result;
            biz =string.IsNullOrEmpty(vBiz.YelpBizId)? null: yelp.GetBusiness(vBiz.YelpBizId).Result;
            yrm.Biz = biz;
            if (biz!= null)
            {
                if (biz.reviews != null && biz.reviews.Count > 0)
                {
                    int lRandIndex = Helper.GetRandItem(0, biz.reviews.Count - 1);
                    yrm.Review = biz.reviews[lRandIndex];
                    if (yrm.Review.user != null)
                    {
                        yrm.User = yrm.Review.user;
                    }
                }
            }
            return yrm;
        }
    }
}