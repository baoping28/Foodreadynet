using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FR.Domain.Model.Abstract;
using FR.Infrastructure.Helpers;

namespace FR.Domain.Model.Entities
{
    public partial class BizInfo : IBaseEntity
    {
        private string _setName = "BizInfos";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }

        public BizRVInfo GetBizRVInfo
        {
            get
            {
                if (AcceptReservation)
                {
                    return this.BizRVInfos.FirstOrDefault();
                }
                return null;
            }
        }
        public string ProductFoldPath
        {
            get
            {
                return "Content/BizImages/Biz_" + this.BizInfoId;
            }
        }
        public bool IsBizNew
        {
            get
            {
                return this.AddedDate > DateTime.Now.AddMonths(-6);
            }
        }
        public bool AcceptReservation
        {
            get
            {
                return this.BizRVInfos.Count() == 1;
            }
        }
        public bool ExistBizCuisine
        {
            get
            {
                return this.BizCuisines.Count() > 0;
            }
        }
        public bool ExistCloseDay
        {
            get
            {
                return this.DayOfCloses.Count() > 0;
            }
        }
        public bool ExistFreeItem
        {
            get
            {
                return this.FreeItemCoupons.Count() > 0;
            }
        }
        public bool ExistCoupon
        {
            get
            {
                return this.DiscountCoupons.Count() > 0;
            }
        }
        public string BizAddressString
        {
            get
            {
                if (this.Address != null)
                {
                    return this.Address.AddressLine + ", " + this.Address.City + ", " + this.Address.State + " " + this.Address.ZipCode;
                }
                return string.Empty;
            }
        }
        public string BizTwoLineAddressString
        {
            get
            {
                if (this.Address != null)
                {
                    return this.Address.AddressLine + "</br> " + this.Address.City + ", " + this.Address.State + " " + this.Address.ZipCode;
                }
                return string.Empty;
            }
        }
        public decimal AverageRating
        {
            get
            {
                if (RatingVotes > 0)
                {
                    return ((int)(((decimal)TotalRating) * 100m / RatingVotes)) / 100m;
                }

                return 0m;
            }
        }
        public string DiscountNote
        {
            get
            {
                string str = this.HasDiscountCoupons ? "Discounts" : "";
                string s = this.HasFreeItemCoupons ? (string.IsNullOrEmpty(str) ? "Free Items" : ", Free Items") : "";
                str = str + s;
                if (string.IsNullOrEmpty(str))
                {
                    return "None";
                }
                return str;
            }
        }
        public List<string> AllCuisineList
        {
            get
            {
                List<string> lct = new List<string>();
                List<BizCuisine> lbc = this.BizCuisines.Where(e => e.BizInfoId == this.BizHourId).ToList();
                foreach (var l in lbc)
                {
                    lct.Add(l.CuisineType.Title.ToLower());
                }
                return lct;
            }
        }
        public bool ContainsCuisine(string vCuisineName)
        {
            return AllCuisineList.Contains(vCuisineName.ToLower());
        }
        public string AllCuisines
        {
            get
            {
                List<string> lct = new List<string>();
                List<BizCuisine> lbc = this.BizCuisines.ToList();
                foreach (var l in lbc)
                {
                    lct.Add(l.CuisineType.Title);
                }
                StringBuilder sb = new StringBuilder();
                foreach (var s in lct)
                {
                    sb.Append(s + ", ");
                }
                string str = sb.ToString();
                if (string.IsNullOrEmpty(str) == false)
                {
                    str = str.Trim();
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
        }
        public bool FreeDelivery
        {
            get
            {
                return this.DeliveryFee == 0.0m;
            }
        }
        public bool IsValid
        {
            get
            {
                return true;
            }
        }

        public bool IsTodayHolidayClose
        {
            get
            {
                bool close = false;
                string mytoday = Helper.ConvertLocalToMyTime(this.BizHour.BizTimeZoneName).ToShortDateString(); ;
                foreach (var c in this.DayOfCloses)
                {
                    if (mytoday == c.CloseDay.ToShortDateString())
                    {
                        close = true;
                        break;
                    }
                }
                return close;
            }
        }

        public string GetBizHours()
        {
            if (IsTodayHolidayClose)
            {
                return "Closed";
            }
            DateTime timeNow = DateTime.Now;
            DateTime startTime = timeNow;
            DateTime endTime = timeNow;
            string dayofweeknow = Helper.ConvertLocalToMyTime(this.BizHour.BizTimeZoneName).DayOfWeek.ToString();
            string lStartBiz = string.Empty;
            string lEndBiz = string.Empty;
            string startBiz = string.Empty;
            string endBiz = string.Empty;
            switch (dayofweeknow)
            {
                case "Monday":
                    {
                        lStartBiz = this.BizHour.LMonStart;
                        lEndBiz = this.BizHour.LMonClose;
                        startBiz = this.BizHour.MonStart;
                        endBiz = this.BizHour.MonClose;
                        break;
                    }
                case "Tuesday":
                    {
                        lStartBiz = this.BizHour.LTueStart;
                        lEndBiz = this.BizHour.LTueClose;
                        startBiz = this.BizHour.TueStart;
                        endBiz = this.BizHour.TueClose;
                        break;
                    }
                case "Wednesday":
                    {
                        lStartBiz = this.BizHour.LWedStart;
                        lEndBiz = this.BizHour.LWedClose;
                        startBiz = this.BizHour.WedStart;
                        endBiz = this.BizHour.WedClose;
                        break;
                    }
                case "Thursday":
                    {
                        lStartBiz = this.BizHour.LThuStart;
                        lEndBiz = this.BizHour.LThuClose;
                        startBiz = this.BizHour.ThuStart;
                        endBiz = this.BizHour.ThuClose;
                        break;
                    }
                case "Friday":
                    {
                        lStartBiz = this.BizHour.LFriStart;
                        lEndBiz = this.BizHour.LFriClose;
                        startBiz = this.BizHour.FriStart;
                        endBiz = this.BizHour.FriClose;
                        break;
                    }
                case "Saturday":
                    {
                        lStartBiz = this.BizHour.LSatStart;
                        lEndBiz = this.BizHour.LSatClose;
                        startBiz = this.BizHour.SatStart;
                        endBiz = this.BizHour.SatClose;
                        break;
                    }
                case "Sunday":
                    {
                        lStartBiz = this.BizHour.LSunStart;
                        lEndBiz = this.BizHour.LSunClose;
                        startBiz = this.BizHour.SunStart;
                        endBiz = this.BizHour.SunColse;
                        break;
                    }
            }
            if (lStartBiz.ToLower() == lEndBiz.ToLower() && startBiz.ToLower() == endBiz.ToLower()) // whole day closed
            {
                return dayofweeknow + ": " + "Closed";
            }

            if (lStartBiz.ToLower() == lEndBiz.ToLower()) // Lunch closed; Dinner open
            {
                return dayofweeknow + ": " + startBiz + " -- " + endBiz;
            }
            if (startBiz.ToLower() == endBiz.ToLower()) // Dinner closed; Lunch open
            {
                return dayofweeknow + ": " + lStartBiz + " -- " + lEndBiz;

            }
            if (lEndBiz.ToLower() != startBiz.ToLower()) // Dinner open; Lunch open; has break time
            {
                return dayofweeknow + ": " + lStartBiz + " -- " + lEndBiz + " | " + startBiz + " -- " + endBiz;


            }
            return dayofweeknow + ": " + lStartBiz + " -- " + endBiz; // whole day open
        }

        public bool IsOpenNow
        {
            get
            {
                if (IsTodayHolidayClose)
                {
                    return false;
                }
                bool openNow = false;
                DateTime timeNow = DateTime.Now;
                string dayofweeknow = Helper.ConvertLocalToMyTime(this.BizHour.BizTimeZoneName).DayOfWeek.ToString();
                string lStartBiz = string.Empty;
                string lEndBiz = string.Empty;
                string startBiz = string.Empty;
                string endBiz = string.Empty;
                switch (dayofweeknow)
                {
                    case "Monday":
                        {
                            lStartBiz = this.BizHour.LMonStart;
                            lEndBiz = this.BizHour.LMonClose;
                            startBiz = this.BizHour.MonStart;
                            endBiz = this.BizHour.MonClose;
                            break;
                        }
                    case "Tuesday":
                        {
                            lStartBiz = this.BizHour.LTueStart;
                            lEndBiz = this.BizHour.LTueClose;
                            startBiz = this.BizHour.TueStart;
                            endBiz = this.BizHour.TueClose;
                            break;
                        }
                    case "Wednesday":
                        {
                            lStartBiz = this.BizHour.LWedStart;
                            lEndBiz = this.BizHour.LWedClose;
                            startBiz = this.BizHour.WedStart;
                            endBiz = this.BizHour.WedClose;
                            break;
                        }
                    case "Thursday":
                        {
                            lStartBiz = this.BizHour.LThuStart;
                            lEndBiz = this.BizHour.LThuClose;
                            startBiz = this.BizHour.ThuStart;
                            endBiz = this.BizHour.ThuClose;
                            break;
                        }
                    case "Friday":
                        {
                            lStartBiz = this.BizHour.LFriStart;
                            lEndBiz = this.BizHour.LFriClose;
                            startBiz = this.BizHour.FriStart;
                            endBiz = this.BizHour.FriClose;
                            break;
                        }
                    case "Saturday":
                        {
                            lStartBiz = this.BizHour.LSatStart;
                            lEndBiz = this.BizHour.LSatClose;
                            startBiz = this.BizHour.SatStart;
                            endBiz = this.BizHour.SatClose;
                            break;
                        }
                    case "Sunday":
                        {
                            lStartBiz = this.BizHour.LSunStart;
                            lEndBiz = this.BizHour.LSunClose;
                            startBiz = this.BizHour.SunStart;
                            endBiz = this.BizHour.SunColse;
                            break;
                        }
                }
                if (lStartBiz.ToLower() == lEndBiz.ToLower() && startBiz.ToLower() == endBiz.ToLower()) // whole day closed
                {
                    return false;
                }
                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;

                if (lStartBiz.ToLower() == lEndBiz.ToLower()) // Lunch closed; Dinner open
                {
                    startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(startBiz), this.BizHour.BizTimeZoneName);
                    endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(endBiz), this.BizHour.BizTimeZoneName);
                }
                else if (startBiz.ToLower() == endBiz.ToLower()) // Dinner closed; Lunch open
                {
                    startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lStartBiz), this.BizHour.BizTimeZoneName);
                    endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lEndBiz), this.BizHour.BizTimeZoneName);

                }
                else if (lEndBiz.ToLower() != startBiz.ToLower()) // Dinner open; Lunch open; has break time
                {
                    startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lEndBiz), this.BizHour.BizTimeZoneName);
                    endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(startBiz), this.BizHour.BizTimeZoneName);
                    int cd1 = DateTime.Compare(timeNow, startTime);
                    int cd2 = DateTime.Compare(timeNow, endTime);
                    if (cd1 >= 0 && cd2 <= 0)
                    {
                        return false; // fall in break time
                    }
                    else
                    {
                        startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lStartBiz), this.BizHour.BizTimeZoneName);
                        endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(endBiz), this.BizHour.BizTimeZoneName);
                    }

                }
                else // Whole day open
                {
                    startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lStartBiz), this.BizHour.BizTimeZoneName);
                    endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(endBiz), this.BizHour.BizTimeZoneName);

                }
                int conditon1 = DateTime.Compare(timeNow, startTime);
                int conditon2 = DateTime.Compare(timeNow, endTime);
                if (conditon1 >= 0 && conditon2 <= 0)
                {
                    openNow = true;
                }
                return openNow;
            }
        }

        public bool IsLunchTimeNow
        {
            get
            {
                if (IsTodayHolidayClose)
                {
                    return false;
                }
                bool openNow = false;
                DateTime timeNow = DateTime.Now;
                string dayofweeknow = Helper.ConvertLocalToMyTime(this.BizHour.BizTimeZoneName).DayOfWeek.ToString();
                string lStartBiz = string.Empty;
                string lEndBiz = string.Empty;
                switch (dayofweeknow)
                {
                    case "Monday":
                        {
                            lStartBiz = this.BizHour.LMonStart;
                            lEndBiz = this.BizHour.LMonClose;
                            break;
                        }
                    case "Tuesday":
                        {
                            lStartBiz = this.BizHour.LTueStart;
                            lEndBiz = this.BizHour.LTueClose;
                            break;
                        }
                    case "Wednesday":
                        {
                            lStartBiz = this.BizHour.LWedStart;
                            lEndBiz = this.BizHour.LWedClose;
                            break;
                        }
                    case "Thursday":
                        {
                            lStartBiz = this.BizHour.LThuStart;
                            lEndBiz = this.BizHour.LThuClose;
                            break;
                        }
                    case "Friday":
                        {
                            lStartBiz = this.BizHour.LFriStart;
                            lEndBiz = this.BizHour.LFriClose;
                            break;
                        }
                    case "Saturday":
                        {
                            lStartBiz = this.BizHour.LSatStart;
                            lEndBiz = this.BizHour.LSatClose;
                            break;
                        }
                    case "Sunday":
                        {
                            lStartBiz = this.BizHour.LSunStart;
                            lEndBiz = this.BizHour.LSunClose;
                            break;
                        }
                }
                if (lStartBiz.ToLower() == lEndBiz.ToLower()) // lunch closed
                {
                    return false;
                }
                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                startTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lStartBiz), this.BizHour.BizTimeZoneName);
                endTime = Helper.ConvertMyTimeToLocal(Convert.ToDateTime(lEndBiz), this.BizHour.BizTimeZoneName);

                int conditon1 = DateTime.Compare(timeNow, startTime);
                int conditon2 = DateTime.Compare(timeNow, endTime);
                if (conditon1 >= 0 && conditon2 <= 0)
                {
                    openNow = true;
                }
                return openNow;
            }
        }

        public bool HasAnyLunchTime
        {
            get
            {
                bool b = false;
                if (this.BizHour.LMonStart.ToLower() != this.BizHour.LMonClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.LTueStart.ToLower() != this.BizHour.LTueClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.LWedStart.ToLower() != this.BizHour.LWedClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.LThuStart.ToLower() != this.BizHour.LThuClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.LFriStart.ToLower() != this.BizHour.LFriClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.LSatStart.ToLower() != this.BizHour.LSatClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.LSunStart.ToLower() != this.BizHour.LSunClose.ToLower())
                {
                    return true;
                }
                return b;
            }
        }
        public bool HasAnyDinnerTime
        {
            get
            {
                bool b = false;
                if (this.BizHour.MonStart.ToLower() != this.BizHour.MonClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.TueStart.ToLower() != this.BizHour.TueClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.WedStart.ToLower() != this.BizHour.WedClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.ThuStart.ToLower() != this.BizHour.ThuClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.FriStart.ToLower() != this.BizHour.FriClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.SatStart.ToLower() != this.BizHour.SatClose.ToLower())
                {
                    return true;
                }
                if (this.BizHour.SunStart.ToLower() != this.BizHour.SunColse.ToLower())
                {
                    return true;
                }
                return b;
            }
        }
        public bool HasNoAnyBreakTime
        {
            get
            {
                bool b = true;
                if (this.BizHour.LMonClose.ToLower() != this.BizHour.MonStart.ToLower())
                {
                    return false;
                }
                if (this.BizHour.LTueClose.ToLower() != this.BizHour.TueStart.ToLower())
                {
                    return false;
                }
                if (this.BizHour.LWedClose.ToLower() != this.BizHour.WedStart.ToLower())
                {
                    return false;
                }
                if (this.BizHour.LThuClose.ToLower() != this.BizHour.ThuStart.ToLower())
                {
                    return false;
                }
                if (this.BizHour.LFriClose.ToLower() != this.BizHour.FriStart.ToLower())
                {
                    return false;
                }
                if (this.BizHour.LSatClose.ToLower() != this.BizHour.SatStart.ToLower())
                {
                    return false;
                }
                if (this.BizHour.LSunClose.ToLower() != this.BizHour.SunStart.ToLower())
                {
                    return false;
                }
                return b;
            }
        }
        public Vote GetRandomVote()
        {
            List<Vote> l = this.Votes.Where(e => e.BizInfoId == this.BizInfoId).ToList();
            if (l != null)
            {
                if (l.Count > 0)
                {
                    int lRandIndex = Helper.GetRandItem(0, l.Count - 1);
                    return l[lRandIndex];
                }
            }
            return null;
        }
        bool bIsDirty = false;
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsDirty
        {
            get { return bIsDirty; }
            set { bIsDirty = value; }
        }


        public bool CanAdd
        {
            get { return true; }
        }

        public bool CanDelete
        {
            get { return true; }
        }

        public bool CanEdit
        {
            get { return true; }
        }

        public bool CanRead
        {
            get { return true; }
        }


        public object List { get; set; }
    }
}
