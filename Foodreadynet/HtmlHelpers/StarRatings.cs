using System;
using System.Text;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace FoodReady.WebUI.HtmlHelpers
{
    public static class StarRatings
    {
        public static MvcHtmlString Ratings(this HtmlHelper helper, BizInfo post)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<span class='rating' rating='{0}' post='{1}' title='{2}'>", post.AverageRating, post.BizInfoId, String.Format("{0}  {1} Click to cast vote ", post.AverageRating == 0.00M ? "No one rated yet." : "Rated " + post.AverageRating + " out of 5 stars by", post.RatingVotes == 0 ? "" : post.RatingVotes + " person(s)."));
            string formatStr = "<img src='/Content/Images/{0}' alt='star' width='12' height='12' class='star' value='{1}' />";
            int ar = (int)(post.AverageRating * 2 + 0.5m);
            for (int i = 1; i <= 5; i++)
            {
                if (ar >= i * 2)
                {
                    sb.AppendFormat(formatStr, "ratingStar.png", i);
                }
                else if (ar == i * 2 - 1)
                {
                    sb.AppendFormat(formatStr, "ratingHalf.png", i);
                }
                else
                {
                    sb.AppendFormat(formatStr, "ratingEmpty.png", i);
                }
            }
            sb.Append("&nbsp;<span></span></span>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString RatingsByVote(this HtmlHelper helper, int vote)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<span class='rating' title='{0}'>", vote + " star rated");
            string formatStr = "<img src='/Content/Images/{0}' alt='star' width='12' height='12' class='star' value='{1}' />";
            int ar = vote * 2; ;
            for (int i = 1; i <= 5; i++)
            {
                if (ar >= i * 2)
                {
                    sb.AppendFormat(formatStr, "ratingStar.png", i);
                }
                else if (ar == i * 2 - 1)
                {
                    sb.AppendFormat(formatStr, "ratingHalf.png", i);
                }
                else
                {
                    sb.AppendFormat(formatStr, "ratingEmpty.png", i);
                }
            }
            sb.Append("&nbsp;<span></span></span>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString YelpRatings(this HtmlHelper helper, double vote)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<span class='rating' title='{0}'>", vote + " star rated");
            string formatStr = "<img src='/Content/Images/{0}' alt='star' width='12' height='12' class='star' value='{1}' />";
            double ar = vote * 2; ;
            for (int i = 1; i <= 5; i++)
            {
                if (ar >= i * 2)
                {
                    sb.AppendFormat(formatStr, "ratingStar.png", i);
                }
                else if (ar == i * 2 - 1)
                {
                    sb.AppendFormat(formatStr, "ratingHalf.png", i);
                }
                else
                {
                    sb.AppendFormat(formatStr, "ratingEmpty.png", i);
                }
            }
            sb.Append("&nbsp;<span></span></span>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static string ListCuisineNames(List<BizCuisine> vBc)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var bz in vBc)
            {
                sb.Append(bz.CuisineTypeName + ", ");
            } 
            if (vBc.Count > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            } 
            return sb.ToString();
        }
        public static MvcHtmlString TimePrint(this HtmlHelper helper, string vStartTime, string vEndTime, bool vBreak)
        {
            StringBuilder sb = new StringBuilder();
            if (vBreak)
            {
                if (vStartTime.ToLower() == vEndTime.ToLower())
                {
                    sb.Append("<td colspan=\"3\"> &nbsp;</td>");
                }
                else
                {
                    sb.Append("<td>" + vStartTime + " </td><td> to </td><td> " + vEndTime + "</td>");
                }
            }
            else
            {
                if (vStartTime.ToLower() == vEndTime.ToLower())
                {
                    sb.Append("<td colspan=\"3\"> &nbsp;</td>");
                }
                else
                {
                    sb.Append("<td>" + vStartTime + " </td><td> to </td><td> " + vEndTime + "</td>");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        public static string PrintTime(string vStartTime, string vEndTime, bool vBreak)
        {
            StringBuilder sb = new StringBuilder();
            if (vBreak)
            {
                if (vStartTime.ToLower() == vEndTime.ToLower())
                {
                    sb.Append("<td colspan=\"3\"> &nbsp;</td>");
                }
                else
                {
                    sb.Append("<td>" + vStartTime + " </td><td> to </td><td> " + vEndTime + "</td>");
                }
            }
            else
            {
                if (vStartTime.ToLower() == vEndTime.ToLower())
                {
                    sb.Append("<td colspan=\"3\"> Closed</td>");
                }
                else
                {
                    sb.Append("<td>" + vStartTime + " </td><td> to </td><td> " + vEndTime + "</td>");
                }
            }
            return sb.ToString();
        }
        public static MvcHtmlString PrintLunchSchedule(this HtmlHelper helper, BizInfo vBizInfo,bool vBreak)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            sb.Append("<tr><td>Mon.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LMonStart, vBizInfo.BizHour.LMonClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Tue.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LTueStart, vBizInfo.BizHour.LTueClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Wed.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LWedStart, vBizInfo.BizHour.LWedClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Thu.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LThuStart, vBizInfo.BizHour.LThuClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Fri.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LFriStart, vBizInfo.BizHour.LFriClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Sat.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LSatStart, vBizInfo.BizHour.LSatClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Sun.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LSunStart, vBizInfo.BizHour.LSunClose, vBreak) + "</tr></table>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString PrintDinnerSchedule(this HtmlHelper helper, BizInfo vBizInfo,bool vBreak)
        {
            StringBuilder sb = new StringBuilder();
            if (vBreak)
            {
                sb.Append("<table>");
                sb.Append("<tr><td> &nbsp;</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.MonStart, vBizInfo.BizHour.MonClose, vBreak) + "</tr>");
                sb.Append("<tr><td> &nbsp;</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.TueStart, vBizInfo.BizHour.TueClose, vBreak) + "</tr>");
                sb.Append("<tr><td> &nbsp;</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.WedStart, vBizInfo.BizHour.WedClose, vBreak) + "</tr>");
                sb.Append("<tr><td> &nbsp;</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.ThuStart, vBizInfo.BizHour.ThuClose, vBreak) + "</tr>");
                sb.Append("<tr><td> &nbsp;</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.FriStart, vBizInfo.BizHour.FriClose, vBreak) + "</tr>");
                sb.Append("<tr><td> &nbsp;</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.SatStart, vBizInfo.BizHour.SatClose, vBreak) + "</tr>");
                sb.Append("<tr><td> &nbsp;</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.SunStart, vBizInfo.BizHour.SunColse, vBreak) + "</tr></table>");
            }
            else
            {
                sb.Append("<table>");
                sb.Append("<tr><td>Mon.</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.MonStart, vBizInfo.BizHour.MonClose, vBreak) + "</tr>");
                sb.Append("<tr><td>Tue.</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.TueStart, vBizInfo.BizHour.TueClose, vBreak) + "</tr>");
                sb.Append("<tr><td>Wed.</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.WedStart, vBizInfo.BizHour.WedClose, vBreak) + "</tr>");
                sb.Append("<tr><td>Thu.</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.ThuStart, vBizInfo.BizHour.ThuClose, vBreak) + "</tr>");
                sb.Append("<tr><td>Fri.</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.FriStart, vBizInfo.BizHour.FriClose, vBreak) + "</tr>");
                sb.Append("<tr><td>Sat.</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.SatStart, vBizInfo.BizHour.SatClose, vBreak) + "</tr>");
                sb.Append("<tr><td>Sun.</td>");
                sb.Append(PrintTime(vBizInfo.BizHour.SunStart, vBizInfo.BizHour.SunColse, vBreak) + "</tr></table>");
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString PrintStrightSchedule(this HtmlHelper helper, BizInfo vBizInfo, bool vBreak)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            sb.Append("<tr><td>Mon.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LMonStart, vBizInfo.BizHour.MonClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Tue.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LTueStart, vBizInfo.BizHour.TueClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Wed.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LWedStart, vBizInfo.BizHour.WedClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Thu.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LThuStart, vBizInfo.BizHour.ThuClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Fri.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LFriStart, vBizInfo.BizHour.FriClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Sat.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LSatStart, vBizInfo.BizHour.SatClose, vBreak) + "</tr>");
            sb.Append("<tr><td>Sun.</td>");
            sb.Append(PrintTime(vBizInfo.BizHour.LSunStart, vBizInfo.BizHour.SunColse, vBreak) + "</tr></table>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static string GoogleMapLink(BizInfo vBizInfo)
        {
            if (vBizInfo == null)
            {
                return string.Empty;
            }
            string s = vBizInfo.BizAddressString.Trim();
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ");
            s = s.Replace(" ", "+");
            return @"https://maps.google.com/maps?saddr=Your+Address&daddr=" + s + ",+USA&hl=en&ll=" + vBizInfo.Latitude + "," + vBizInfo.Longitude;
        }
    }
}