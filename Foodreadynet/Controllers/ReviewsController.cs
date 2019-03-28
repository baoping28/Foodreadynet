using System.Web.Mvc;
using FR.Repository.Interfaces;
using FoodReady.WebUI.Models;
using FR.Domain.Model.Entities;
using System.Web.Helpers;
using System;
using System.Web;
using FR.Infrastructure.Helpers;

namespace FoodReady.WebUI.Controllers
{
    public class ReviewsController : BaseController
    {
        private IVoteRepository VoteRepository;
        private IBizInfoRepository BizInfoRepository;
        public ReviewsController(IBizInfoRepository bizInfoRepo,IVoteRepository VoteRepo)
        {
            ViewBag.TitleSortParam = "BizTitle";
            ViewBag.NewestSortParam = "AddedDate desc";
            ViewBag.SortParam = "BizTitle";
            BizInfoRepository = bizInfoRepo;
            VoteRepository = VoteRepo;
        }

        public ActionResult Index(ShoppingCart cart)
        {
            ViewBag.bagitems = GetCartItems(cart);
            return View();
        }
        [Authorize]
        public ActionResult AddReview(int id,ShoppingCart cart)
        {
            RatingViewModel rvm = new RatingViewModel();
            if (id > 0)
            {
                rvm.BizInfo = BizInfoRepository.GetBizInfoById(id);
            } 
            string rul = HttpContext.Request.UrlReferrer == null ? "~/Home" : HttpContext.Request.UrlReferrer.PathAndQuery;
            rvm.ReturnUrl = rul;
            ViewBag.bagitems = GetCartItems(cart);
            return View(rvm);
        }
        [HttpPost]
        public JsonResult SetRating(string penname, string title, int id, string comment, int rating)
        {
            try
            {
                int ratingValue = CanUserVote(id, rating);
                if (ratingValue > 0)
                {

                    return Json(new
                    {
                        Success = false,
                        Message = "Sorry, you already voted " + ratingValue.ToString() + " star(s) for this post."
                    });
                }

                BizInfo post = BizInfoRepository.SetBizInfoRating(id, rating);
                Vote v = VoteRepository.AddVote(0, id, title, penname, CurrentUserID, rating, CurrentUserIP,
                                          comment, DateTime.Now, UserName, DateTime.Now, UserName, true);
                return Json(new
                {
                    Success = true,
                    Message = "Your Vote for " + Math.Abs(ratingValue).ToString() + " star(s)  was cast successfully" //  , Result = new { Rating = post.AverageRating, Raters = post.Votes }
                });
            }
            catch
            {
                return Json(new
                {
                    Success = false,
                    Message = "your vote  was not casted successfully."
                });
            }
        }
        private int CanUserVote(int id, int rating)
        {
            var cookieName = "Rating_" + id.ToString() + "_" + UserName;
            HttpCookie voteCookie = Request.Cookies[cookieName];

            if (voteCookie != null)
            {
                return int.Parse(voteCookie.Value);
            }

            //create the cookie and set the value
            voteCookie = new HttpCookie(cookieName, rating.ToString());
            voteCookie.Expires = DateTime.Now.AddDays(Globals.Settings.PayPalSettings.RatingLockInterval);
            Response.Cookies.Add(voteCookie);

            return -rating;
        }

        public ActionResult ShowThemeChart(int id)
        {
            BizInfo b = BizInfoRepository.GetBizInfoById(id);

            var myChart = new System.Web.Helpers.Chart(width: 250, height: 200, theme: ChartTheme.Green)
         .AddTitle(b.AverageRating.ToString() + " out of 5 stars")
         .AddSeries(
              name: "Reviews",
             xValue: new[] { "5 Star(" + b.FiveStarVotes.ToString() + "):", "4 Star(" + b.FourStarVotes.ToString() + "):", "3 Star(" + b.ThreeStarVotes.ToString() + "):", "2 Star(" + b.TwoStarVotes.ToString() + "):", "1 Star(" + b.OneStarVotes.ToString() + "):" },
             yValues: new[] { (b.FiveStarVotes * 5).ToString(), (b.FourStarVotes * 4).ToString(), (b.ThreeStarVotes * 3).ToString(), (b.TwoStarVotes * 2).ToString(), b.OneStarVotes.ToString() })
                .GetBytes("png");
            return File(myChart, "image/png");
        }
        public ActionResult ShowChart(int id)
        {
            BizInfo b = BizInfoRepository.GetBizInfoById(id);
            string myTheme = @"<Chart BackColor=""#e6e5e5"" >
                                    <ChartAreas>
                                        <ChartArea Name=""Default"" BackColor=""Transparent""></ChartArea>
                                    </ChartAreas>
                                </Chart>";

            var myChart = new System.Web.Helpers.Chart(width: 220, height: 170, theme: myTheme)
         .AddTitle(b.AverageRating.ToString() + " out of 5 stars")
         .AddSeries(
          chartType: "bar",
             name: "Reviews",
             xValue: new[] { "5 Star(" + b.FiveStarVotes.ToString() + "):", "4 Star(" + b.FourStarVotes.ToString() + "):", "3 Star(" + b.ThreeStarVotes.ToString() + "):", "2 Star(" + b.TwoStarVotes.ToString() + "):", "1 Star(" + b.OneStarVotes.ToString() + "):" },
             yValues: new[] { (b.FiveStarVotes * 5).ToString(), (b.FourStarVotes * 4).ToString(), (b.ThreeStarVotes * 3).ToString(), (b.TwoStarVotes * 2).ToString(), b.OneStarVotes.ToString() })
                .GetBytes("png");
            return File(myChart, "image/png");
        }
    }
}
