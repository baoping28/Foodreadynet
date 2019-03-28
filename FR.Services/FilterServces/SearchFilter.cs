using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FR.Repository.Interfaces;
using FR.Domain.Model.Entities;
using FR.Infrastructure.Helpers;
using FR.Services.GoogleAPI;
namespace FR.Services.FilterServces
{
    public class SearchFilter
    {
        public static DateTime ConvertMyTimeToLocal(DateTime time, string zoneName)
        {
            TimeZoneInfo myZone = TimeZoneInfo.FindSystemTimeZoneById(zoneName);
            return TimeZoneInfo.ConvertTime(time, myZone, TimeZoneInfo.Local);
        }



        public static DateTime ConvertLocalToMyTime(string myZoneName)
        {
            var myzone = TimeZoneInfo.FindSystemTimeZoneById(myZoneName);
            var local = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.StandardName);

            var now = DateTimeOffset.UtcNow;
            TimeSpan myzoneOffset = myzone.GetUtcOffset(now);
            TimeSpan localOffset = local.GetUtcOffset(now);
            TimeSpan difference = myzoneOffset - localOffset;
            return DateTime.Now.AddHours(difference.TotalHours);
        }

        public static bool IsTheDayHoliday(BizInfo vBizInfo, DateTime vDay)
        {
            bool close = false;
            DateTime dt1 = DateTime.Parse(vDay.ToShortDateString());
            DateTime dt2 = DateTime.Parse(DateTime.Now.ToShortDateString());
            double dayDiff = (dt1 - dt2).TotalDays;
            if (dayDiff < 0) return true;
            string mytoday = Helper.ConvertLocalToMyTime(vBizInfo.BizHour.BizTimeZoneName).AddDays(dayDiff).ToShortDateString();
            foreach (var c in vBizInfo.DayOfCloses)
            {
                if (mytoday == c.CloseDay.ToShortDateString())
                {
                    close = true;
                    break;
                }
            }
            return close;
        }
        public static bool IsHolidayClose(BizInfo vBizInfo, int vDay) // vDay==1---today, vDay==2---tomorrow
        {
            bool close = false;
            string mytoday = vDay == 1 ? Helper.ConvertLocalToMyTime(vBizInfo.BizHour.BizTimeZoneName).ToShortDateString() : Helper.ConvertLocalToMyTime(vBizInfo.BizHour.BizTimeZoneName).AddDays(1).ToShortDateString();
            foreach (var c in vBizInfo.DayOfCloses)
            {
                if (mytoday == c.CloseDay.ToShortDateString())
                {
                    close = true;
                    break;
                }
            }
            return close;
        }
        public static bool IsOpenNow(BizInfo vBizInfo)
        {
            if (IsHolidayClose(vBizInfo, 1))
            {
                return false;
            }
            bool openNow = false;
            DateTime timeNow = DateTime.Now;
            string dayofweeknow = Helper.ConvertLocalToMyTime(vBizInfo.BizHour.BizTimeZoneName).DayOfWeek.ToString();
            string lStartBiz = string.Empty;
            string lEndBiz = string.Empty;
            string startBiz = string.Empty;
            string endBiz = string.Empty;
            switch (dayofweeknow)
            {
                case "Monday":
                    {
                        lStartBiz = vBizInfo.BizHour.LMonStart;
                        lEndBiz = vBizInfo.BizHour.LMonClose;
                        startBiz = vBizInfo.BizHour.MonStart;
                        endBiz = vBizInfo.BizHour.MonClose;
                        break;
                    }
                case "Tuesday":
                    {
                        lStartBiz = vBizInfo.BizHour.LTueStart;
                        lEndBiz = vBizInfo.BizHour.LTueClose;
                        startBiz = vBizInfo.BizHour.TueStart;
                        endBiz = vBizInfo.BizHour.TueClose;
                        break;
                    }
                case "Wednesday":
                    {
                        lStartBiz = vBizInfo.BizHour.LWedStart;
                        lEndBiz = vBizInfo.BizHour.LWedClose;
                        startBiz = vBizInfo.BizHour.WedStart;
                        endBiz = vBizInfo.BizHour.WedClose;
                        break;
                    }
                case "Thursday":
                    {
                        lStartBiz = vBizInfo.BizHour.LThuStart;
                        lEndBiz = vBizInfo.BizHour.LThuClose;
                        startBiz = vBizInfo.BizHour.ThuStart;
                        endBiz = vBizInfo.BizHour.ThuClose;
                        break;
                    }
                case "Friday":
                    {
                        lStartBiz = vBizInfo.BizHour.LFriStart;
                        lEndBiz = vBizInfo.BizHour.LFriClose;
                        startBiz = vBizInfo.BizHour.FriStart;
                        endBiz = vBizInfo.BizHour.FriClose;
                        break;
                    }
                case "Saturday":
                    {
                        lStartBiz = vBizInfo.BizHour.LSatStart;
                        lEndBiz = vBizInfo.BizHour.LSatClose;
                        startBiz = vBizInfo.BizHour.SatStart;
                        endBiz = vBizInfo.BizHour.SatClose;
                        break;
                    }
                case "Sunday":
                    {
                        lStartBiz = vBizInfo.BizHour.LSunStart;
                        lEndBiz = vBizInfo.BizHour.LSunClose;
                        startBiz = vBizInfo.BizHour.SunStart;
                        endBiz = vBizInfo.BizHour.SunColse;
                        break;
                    }
            }
            if (lStartBiz.ToLower() == lEndBiz.ToLower() && startBiz.ToLower() == endBiz.ToLower()) // whole day closed
            {
                return false;
            }
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime;

            if (lStartBiz.ToLower() == lEndBiz.ToLower()) // Lunch closed; Dinner open
            {
                startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(startBiz), vBizInfo.BizHour.BizTimeZoneName);
                endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(endBiz), vBizInfo.BizHour.BizTimeZoneName);
            }
            else if (startBiz.ToLower() == endBiz.ToLower()) // Dinner closed; Lunch open
            {
                startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lStartBiz), vBizInfo.BizHour.BizTimeZoneName);
                endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lEndBiz), vBizInfo.BizHour.BizTimeZoneName);

            }
            else if (lEndBiz.ToLower() != startBiz.ToLower()) // Dinner open; Lunch open; has break time
            {
                startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lEndBiz), vBizInfo.BizHour.BizTimeZoneName);
                endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(startBiz), vBizInfo.BizHour.BizTimeZoneName);
                int cd1 = DateTime.Compare(timeNow, startTime);
                int cd2 = DateTime.Compare(timeNow, endTime);
                if (cd1 >= 0 && cd2 <= 0)
                {
                    return false; // fall in break time
                }
                else
                {
                    startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lStartBiz), vBizInfo.BizHour.BizTimeZoneName);
                    endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(endBiz), vBizInfo.BizHour.BizTimeZoneName);
                }

            }
            else // Whole day open
            {
                startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lStartBiz), vBizInfo.BizHour.BizTimeZoneName);
                endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(endBiz), vBizInfo.BizHour.BizTimeZoneName);

            }
            int conditon1 = DateTime.Compare(timeNow, startTime);
            int conditon2 = DateTime.Compare(timeNow, endTime);
            if (conditon1 >= 0 && conditon2 <= 0)
            {
                openNow = true;
            }
            return openNow;
        }


        public static bool IsTheDayOpen(BizInfo vBizInfo, DateTime vDay)
        {
            if (IsTheDayHoliday(vBizInfo, vDay))
            {
                return false;
            }

            DateTime dt1 = DateTime.Parse(vDay.ToShortDateString());
            DateTime dt2 = DateTime.Parse(DateTime.Now.ToShortDateString());
            double dayDiff = (dt1 - dt2).TotalDays;
            if (dayDiff < 0) return false;

            bool dayOpen = true;
            DateTime timeNow = DateTime.Now;
            string dayofweeknow = Helper.ConvertLocalToMyTime(vBizInfo.BizHour.BizTimeZoneName).AddDays(dayDiff).DayOfWeek.ToString();
            switch (dayofweeknow)
            {
                case "Monday":
                    {
                        if (vBizInfo.BizHour.LMonStart.ToLower() == vBizInfo.BizHour.LMonClose.ToLower() &&
                            vBizInfo.BizHour.MonStart.ToLower() == vBizInfo.BizHour.MonClose.ToLower())
                        {
                            dayOpen = false;
                        }
                        break;
                    }
                case "Tuesday":
                    {
                        if (vBizInfo.BizHour.LTueStart.ToLower() == vBizInfo.BizHour.LTueClose.ToLower() &&
                            vBizInfo.BizHour.TueStart.ToLower() == vBizInfo.BizHour.TueClose.ToLower())
                        {
                            dayOpen = false;
                        }
                        break;
                    }
                case "Wednesday":
                    {
                        if (vBizInfo.BizHour.LWedStart.ToLower() == vBizInfo.BizHour.LWedClose.ToLower() &&
                            vBizInfo.BizHour.WedStart.ToLower() == vBizInfo.BizHour.WedClose.ToLower())
                        {
                            dayOpen = false;
                        }
                        break;
                    }
                case "Thursday":
                    {
                        if (vBizInfo.BizHour.LThuStart.ToLower() == vBizInfo.BizHour.LThuClose.ToLower() &&
                            vBizInfo.BizHour.ThuStart.ToLower() == vBizInfo.BizHour.ThuClose.ToLower())
                        {
                            dayOpen = false;
                        }
                        break;
                    }
                case "Friday":
                    {
                        if (vBizInfo.BizHour.LFriStart.ToLower() == vBizInfo.BizHour.LFriClose.ToLower() &&
                             vBizInfo.BizHour.FriStart.ToLower() == vBizInfo.BizHour.FriClose.ToLower())
                        {
                            dayOpen = false;
                        }
                        break;
                    }
                case "Saturday":
                    {
                        if (vBizInfo.BizHour.LSatStart.ToLower() == vBizInfo.BizHour.LSatClose.ToLower() &&
                             vBizInfo.BizHour.SatStart.ToLower() == vBizInfo.BizHour.SatClose.ToLower())
                        {
                            dayOpen = false;
                        }
                        break;
                    }
                case "Sunday":
                    {
                        if (vBizInfo.BizHour.LSunStart.ToLower() == vBizInfo.BizHour.LSunClose.ToLower() &&
                              vBizInfo.BizHour.SunStart.ToLower() == vBizInfo.BizHour.SunColse.ToLower())
                        {
                            dayOpen = false;
                        }
                        break;
                    }
            }
            return dayOpen;
        }

        public static bool IsOpenTomorrow(BizInfo vBizInfo)
        {
            if (IsHolidayClose(vBizInfo, 2))
            {
                return false;
            }
            bool openTomorrow = true;
            DateTime timeNow = DateTime.Now;
            string dayofweeknow = Helper.ConvertLocalToMyTime(vBizInfo.BizHour.BizTimeZoneName).AddDays(1).DayOfWeek.ToString();
            switch (dayofweeknow)
            {
                case "Monday":
                    {
                        if (vBizInfo.BizHour.LMonStart.ToLower() == vBizInfo.BizHour.LMonClose.ToLower() &&
                            vBizInfo.BizHour.MonStart.ToLower() == vBizInfo.BizHour.MonClose.ToLower())
                        {
                            openTomorrow = false;
                        }
                        break;
                    }
                case "Tuesday":
                    {
                        if (vBizInfo.BizHour.LTueStart.ToLower() == vBizInfo.BizHour.LTueClose.ToLower() &&
                            vBizInfo.BizHour.TueStart.ToLower() == vBizInfo.BizHour.TueClose.ToLower())
                        {
                            openTomorrow = false;
                        }
                        break;
                    }
                case "Wednesday":
                    {
                        if (vBizInfo.BizHour.LWedStart.ToLower() == vBizInfo.BizHour.LWedClose.ToLower() &&
                            vBizInfo.BizHour.WedStart.ToLower() == vBizInfo.BizHour.WedClose.ToLower())
                        {
                            openTomorrow = false;
                        }
                        break;
                    }
                case "Thursday":
                    {
                        if (vBizInfo.BizHour.LThuStart.ToLower() == vBizInfo.BizHour.LThuClose.ToLower() &&
                            vBizInfo.BizHour.ThuStart.ToLower() == vBizInfo.BizHour.ThuClose.ToLower())
                        {
                            openTomorrow = false;
                        }
                        break;
                    }
                case "Friday":
                    {
                        if (vBizInfo.BizHour.LFriStart.ToLower() == vBizInfo.BizHour.LFriClose.ToLower() &&
                             vBizInfo.BizHour.FriStart.ToLower() == vBizInfo.BizHour.FriClose.ToLower())
                        {
                            openTomorrow = false;
                        }
                        break;
                    }
                case "Saturday":
                    {
                        if (vBizInfo.BizHour.LSatStart.ToLower() == vBizInfo.BizHour.LSatClose.ToLower() &&
                             vBizInfo.BizHour.SatStart.ToLower() == vBizInfo.BizHour.SatClose.ToLower())
                        {
                            openTomorrow = false;
                        }
                        break;
                    }
                case "Sunday":
                    {
                        if (vBizInfo.BizHour.LSunStart.ToLower() == vBizInfo.BizHour.LSunClose.ToLower() &&
                              vBizInfo.BizHour.SunStart.ToLower() == vBizInfo.BizHour.SunColse.ToLower())
                        {
                            openTomorrow = false;
                        }
                        break;
                    }
            }
            return openTomorrow;
        }

        public static decimal Distance(BizInfo vBizInfo, string vCustomerAddress, string vApproxTime)
        {
            return GoogleHelps.GetDistance(vBizInfo.BizAddressString, vCustomerAddress, out vApproxTime);
        }

        public static bool IsOpenAt(BizInfo vBizInfo, int vDay, string vDropDownTime)
        {
            if (IsHolidayClose(vBizInfo, vDay))
            {
                return false;
            }

            if (vDropDownTime.ToLower() == "asap")
            {
                if (vDay == 2)
                {
                    return IsOpenTomorrow(vBizInfo);
                }
                return true;
                //return IsOpenNow(vBizInfo);
            }
            TimeSpan ts = Helper.DifferenceBetweenMyzoneAndLocal(vBizInfo.BizHour.BizTimeZoneName);
            DateTime timeThen = vDay == 1 ? Convert.ToDateTime(vDropDownTime) : Convert.ToDateTime(vDropDownTime).AddDays(1);
            bool openThen = false;
            string dayofweekThen = timeThen.AddHours(ts.TotalHours).DayOfWeek.ToString(); // get my weeks
            string lStartBiz = string.Empty;
            string lEndBiz = string.Empty;
            string startBiz = string.Empty;
            string endBiz = string.Empty;
            switch (dayofweekThen)
            {
                case "Monday":
                    {
                        lStartBiz = vBizInfo.BizHour.LMonStart;
                        lEndBiz = vBizInfo.BizHour.LMonClose;
                        startBiz = vBizInfo.BizHour.MonStart;
                        endBiz = vBizInfo.BizHour.MonClose;
                        break;
                    }
                case "Tuesday":
                    {
                        lStartBiz = vBizInfo.BizHour.LTueStart;
                        lEndBiz = vBizInfo.BizHour.LTueClose;
                        startBiz = vBizInfo.BizHour.TueStart;
                        endBiz = vBizInfo.BizHour.TueClose;
                        break;
                    }
                case "Wednesday":
                    {
                        lStartBiz = vBizInfo.BizHour.LWedStart;
                        lEndBiz = vBizInfo.BizHour.LWedClose;
                        startBiz = vBizInfo.BizHour.WedStart;
                        endBiz = vBizInfo.BizHour.WedClose;
                        break;
                    }
                case "Thursday":
                    {
                        lStartBiz = vBizInfo.BizHour.LThuStart;
                        lEndBiz = vBizInfo.BizHour.LThuClose;
                        startBiz = vBizInfo.BizHour.ThuStart;
                        endBiz = vBizInfo.BizHour.ThuClose;
                        break;
                    }
                case "Friday":
                    {
                        lStartBiz = vBizInfo.BizHour.LFriStart;
                        lEndBiz = vBizInfo.BizHour.LFriClose;
                        startBiz = vBizInfo.BizHour.FriStart;
                        endBiz = vBizInfo.BizHour.FriClose;
                        break;
                    }
                case "Saturday":
                    {
                        lStartBiz = vBizInfo.BizHour.LSatStart;
                        lEndBiz = vBizInfo.BizHour.LSatClose;
                        startBiz = vBizInfo.BizHour.SatStart;
                        endBiz = vBizInfo.BizHour.SatClose;
                        break;
                    }
                case "Sunday":
                    {
                        lStartBiz = vBizInfo.BizHour.LSunStart;
                        lEndBiz = vBizInfo.BizHour.LSunClose;
                        startBiz = vBizInfo.BizHour.SunStart;
                        endBiz = vBizInfo.BizHour.SunColse;
                        break;
                    }
            }
            if (lStartBiz.ToLower() == lEndBiz.ToLower() && startBiz.ToLower() == endBiz.ToLower()) // whole day closed
            {
                return false;
            }
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime;

            if (lStartBiz.ToLower() == lEndBiz.ToLower()) // Lunch closed; Dinner open
            {
                startTime = vDay == 1 ? Convert.ToDateTime(startBiz) : Convert.ToDateTime(startBiz).AddDays(1);
                endTime = vDay == 1 ? Convert.ToDateTime(endBiz) : Convert.ToDateTime(endBiz).AddDays(1);
            }
            else if (startBiz.ToLower() == endBiz.ToLower()) // Dinner closed; Lunch open
            {
                startTime = vDay == 1 ? Convert.ToDateTime(lStartBiz) : Convert.ToDateTime(lStartBiz).AddDays(1);
                endTime = vDay == 1 ? Convert.ToDateTime(lEndBiz) : Convert.ToDateTime(lEndBiz).AddDays(1);

            }
            else if (lEndBiz.ToLower() != startBiz.ToLower()) // Dinner open; Lunch open; has break time
            {
                startTime = vDay == 1 ? Convert.ToDateTime(lEndBiz) : Convert.ToDateTime(lEndBiz).AddDays(1);
                endTime = vDay == 1 ? Convert.ToDateTime(startBiz) : Convert.ToDateTime(startBiz).AddDays(1);
                int cd1 = DateTime.Compare(timeThen, startTime);
                int cd2 = DateTime.Compare(timeThen, endTime);
                if (cd1 >= 0 && cd2 <= 0)
                {
                    return false; // fall in break time
                }
                else
                {
                    startTime = vDay == 1 ? Convert.ToDateTime(lStartBiz) : Convert.ToDateTime(lStartBiz).AddDays(1);
                    endTime = vDay == 1 ? Convert.ToDateTime(endBiz) : Convert.ToDateTime(endBiz).AddDays(1);
                }

            }
            else // Whole day open
            {
                startTime = vDay == 1 ? Convert.ToDateTime(lStartBiz) : Convert.ToDateTime(lStartBiz).AddDays(1);
                endTime = vDay == 1 ? Convert.ToDateTime(endBiz) : Convert.ToDateTime(endBiz).AddDays(1);

            }

            int conditon1 = DateTime.Compare(timeThen, startTime);
            int conditon2 = DateTime.Compare(timeThen, endTime);
            if (conditon1 >= 0 && conditon2 <= 0)
            {
                openThen = true;
            }
            return openThen;
        }
        public static string FormatMinites(int vMinites)
        {
            string time = string.Empty;
            if (vMinites <= 0)
            {
                return 0 + " mins";
            }
            if (vMinites < 60)
            {
                return vMinites + " mins";
            }
            int hour = vMinites / 60;
            int minites = vMinites % 60;
            string min = minites == 0 ? "" : minites + " minites";
            return hour + " hour " + min;
        }
        public static string FormatSeconds(string vSeconds)
        {
            int min = 0;
            try
            {
                min = int.Parse(vSeconds) / 60;
            }
            catch
            {
                min = 0;
            }
            return FormatMinites(min);
        }
        public static string FormatMinites(string vMinites)
        {
            int gl = 0;
            try
            {
                gl = int.Parse(vMinites);
            }
            catch
            {
                gl = 0;
            }
            return FormatMinites(gl);
        }
        public static int GetDeliveryTime(string vRestaurantTime, string vGoogleTime)
        {
            int gl = 1;
            try
            {
                gl = int.Parse(vGoogleTime);
            }
            catch
            {
                gl = 1;
            }
            return int.Parse(vRestaurantTime) + gl / 60; // return  by minites
        }
        public static decimal GetDistance(string vUserAddress, BizInfo vBizInfo, out string vApproxTime)
        {
            decimal g = GoogleHelps.GetDistance(vUserAddress, vBizInfo.BizAddressString, out vApproxTime);
            return g;
        }
        public static decimal GetDistance(string vUserAddress, string vBizAddress, out string vApproxTime)
        {
            decimal g = GoogleHelps.GetDistance(vUserAddress, vBizAddress, out  vApproxTime);
            return g;
        }
        public static bool ItemFilter(BizInfo vBizInfo, string schedule, string time, string cuisine, string min, string dist, bool yrating, string freeDelivery, string breakfast, string lunchSpecial, string coupons, string freeItems, BrowseHistory bh)
        {
            string approxtime = string.Empty;
            bool hascuisine = string.IsNullOrEmpty(cuisine) ? true : cuisine.ToLower() == "all" ? true : vBizInfo.ContainsCuisine(cuisine);
            bool isopen = IsOpenAt(vBizInfo, int.Parse(schedule), time);
            bool delmin = decimal.Parse(min) <= 0.0m ? true : vBizInfo.DeliveryMinimum < decimal.Parse(min);
            decimal g = GetDistance(bh.FullAddress, vBizInfo, out approxtime);
            bool distance = decimal.Parse(dist) <= 0.0m ? true : (g < 0.0m ? false : g < decimal.Parse(dist));
            bool freedel = freeDelivery == "checked" ? vBizInfo.DeliveryFee <= 0.0m : true;
            bool hasbreakfast = breakfast == "checked" ? vBizInfo.HasBreakfast : true;
            bool lunch = lunchSpecial == "checked" ? vBizInfo.HasLunchSpecial : true;
            bool coupon = coupons == "checked" ? vBizInfo.HasDiscountCoupons : true;
            bool fitem = freeItems == "checked" ? vBizInfo.HasFreeItemCoupons : true;
            return (isopen && hascuisine && delmin && distance && yrating && freedel && hasbreakfast && lunch && coupon && fitem);
        }
    }
}
