using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text;

namespace FR.Infrastructure.Helpers
{
    public class Helper
    {
        #region PhoneNumberFormat enum

        /// <summary>
        /// Phone Number Format
        /// </summary>
        /// <remarks></remarks>
        public enum PhoneNumberFormat
        {
            Dashes = 2,
            Parenthises = 1,
            Periods = 3,
            Straight = 4
        }

        #endregion

        /// <summary>
        /// I left this in for those that might use this in their appliciations. 
        /// </summary>
        /// <remarks></remarks>
        private static readonly string[] _countries = new[]
                                                          {
                                                              "United States","Afghanistan", "Albania", "Algeria", "American Samoa",
                                                              "Andorra", "Angola", "Anguilla", "Antarctica",
                                                              "Antigua And Barbuda", "Argentina", "Armenia", "Aruba",
                                                              "Australia", "Austria", "Azerbaijan", "Bahamas",
                                                              "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium",
                                                              "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia",
                                                              "Bosnia Hercegovina", "Botswana", "Bouvet Island",
                                                              "Brazil", "Brunei Darussalam", "Bulgaria",
                                                              "Burkina Faso", "Burundi", "Byelorussian SSR", "Cambodia",
                                                              "Cameroon", "Canada", "Cape Verde", "Cayman Islands",
                                                              "Central African Republic", "Chad", "Chile", "China",
                                                              "Christmas Island", "Cocos (Keeling) Islands", "Colombia",
                                                              "Comoros",
                                                              "Congo", "Cook Islands", "Costa Rica", "Cote D'Ivoire",
                                                              "Croatia", "Cuba", "Cyprus", "Czech Republic",
                                                              "Czechoslovakia", "Denmark", "Djibouti", "Dominica",
                                                              "Dominican Republic", "East Timor", "Ecuador", "Egypt",
                                                              "El Salvador", "England", "Equatorial Guinea", "Eritrea",
                                                              "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands"
                                                              , "Fiji", "Finland", "France", "Gabon", "Gambia",
                                                              "Georgia", "Germany", "Ghana",
                                                              "Gibraltar", "Great Britain", "Greece", "Greenland",
                                                              "Grenada", "Guadeloupe", "Guam", "Guatemela", "Guernsey",
                                                              "Guiana", "Guinea", "Guinea-Bissau", "Guyana", "Haiti",
                                                              "Heard Islands", "Honduras",
                                                              "Hong Kong", "Hungary", "Iceland", "India", "Indonesia",
                                                              "Iran", "Iraq", "Ireland", "Isle Of Man", "Israel",
                                                              "Italy", "Jamaica", "Japan", "Jersey", "Jordan",
                                                              "Kazakhstan",
                                                              "Kenya", "Kiribati", "Korea, South", "Korea, North",
                                                              "Kuwait", "Kyrgyzstan", "Lao People's Dem. Rep.", "Latvia"
                                                              , "Lebanon", "Lesotho", "Liberia", "Libya",
                                                              "Liechtenstein", "Lithuania", "Luxembourg", "Macau",
                                                              "Macedonia", "Madagascar", "Malawi", "Malaysia",
                                                              "Maldives", "Mali", "Malta", "Mariana Islands",
                                                              "Marshall Islands", "Martinique", "Mauritania",
                                                              "Mauritius", "Mayotte", "Mexico", "Micronesia", "Moldova",
                                                              "Monaco", "Mongolia", "Montserrat", "Morocco",
                                                              "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal",
                                                              "Netherlands", "Netherlands Antilles", "Neutral Zone",
                                                              "New Caledonia", "New Zealand", "Nicaragua", "Niger",
                                                              "Nigeria", "Niue", "Norfolk Island", "Northern Ireland",
                                                              "Norway", "Oman", "Pakistan", "Palau", "Panama",
                                                              "Papua New Guinea", "Paraguay", "Peru", "Philippines",
                                                              "Pitcairn", "Poland", "Polynesia",
                                                              "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania",
                                                              "Russian Federation", "Rwanda", "Saint Helena",
                                                              "Saint Kitts", "Saint Lucia", "Saint Pierre",
                                                              "Saint Vincent", "Samoa", "San Marino",
                                                              "Sao Tome and Principe", "Saudi Arabia",
                                                              "Scotland", "Senegal", "Seychelles", "Sierra Leone",
                                                              "Singapore", "Slovakia", "Slovenia", "Solomon Islands",
                                                              "Somalia", "South Africa", "South Georgia", "Spain",
                                                              "Sri Lanka", "Sudan", "Suriname", "Svalbard",
                                                              "Swaziland", "Sweden", "Switzerland",
                                                              "Syrian Arab Republic", "Taiwan", "Tajikista", "Tanzania",
                                                              "Thailand", "Togo", "Tokelau", "Tonga",
                                                              "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan"
                                                              , "Turks and Caicos Islands",
                                                              "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates",
                                                              "United Kingdom", "Uruguay", "Uzbekistan"
                                                              , "Vanuatu", "Vatican City State", "Venezuela", "Vietnam",
                                                              "Virgin Islands", "Wales", "Western Sahara", "Yemen",
                                                              "Yugoslavia", "Zaire", "Zambia", "Zimbabwe"
                                                          };

        public static string[] Countries
        {
            get { return _countries; }
        }
        private static readonly string[] _mostPopularLocalCities = new[] {"antioch","brentwood", "concord", "danville", "lafayette", "oakland","orinda",
                                       "pittsburg", "pleasant hill", "sacramento", "san francisco", "san jose", "san ramon", "walnut creek" };

        public static string[] MostPopularLocalCities
        {
            get { return _mostPopularLocalCities; }
        }

        public static bool IsMostPopularLocalCity(string vCity)
        {
            if (string.IsNullOrEmpty(vCity))
            {
                return false;
            }
            return Array.IndexOf(MostPopularLocalCities, vCity.ToLower()) >= 0 ? true : false;
        }

        private static readonly string[] _mostPopularCities = new[] {"alexandria","atlanta","austin","baltimore",
            "berkeley","boston","brooklyn","chicago","concord","cleveland","dallas","denver","detroit","houston","jersey City","las vegas",
            "lafayette","los angeles","miami","montreal","nyc","new york","oakland","orlando","philadelphia","phoenix","pittsburgh","pleasant hill","sacramento",
            "san antonio","san diego","san francisco","san jose","san ramon","seattle","vancouver","walnut creek","washington dc" };

        public static string[] MostPopularCities
        {
            get { return _mostPopularCities; }
        }
        private static readonly string[] _mostPopularCuisines = new[] { "american", "chinese", "french", "india",
             "italian", "japanese", "korean", "mexican", "middle Eastern", "pizza", "sandwiches",
             "seafood", "sushi", "thai", "vegetarian", "vietnamese" };

        public static string[] MostPopularCuisines
        {
            get { return _mostPopularCuisines; }
        }
        public static bool IsMostPopularCity(string vCity)
        {
            if (string.IsNullOrEmpty(vCity))
            {
                return false;
            }
            return Array.IndexOf(MostPopularCities, vCity.ToLower()) >= 0 ? true : false;
        }
        public static bool IsPopularCuisine(string vCuisine)
        {
            if (string.IsNullOrEmpty(vCuisine))
            {
                return false;
            }
            return Array.IndexOf(MostPopularCuisines, vCuisine.ToLower()) >= 0 ? true : false;
        }
        public static string GetPayPalServerUrl()
        {
            if (Globals.Settings.PayPalSettings.SandboxMode)
            {
                return "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
            else
            {
                return "https://www.paypal.com/cgi-bin/webscr";
            }
        }
        public static string ApplicationPath
        {
            get { return HttpContext.Current.Request.PhysicalApplicationPath; }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        /// 
        /*
        public static CouponItem GetCouponItem(int id)
        {
            using (CouponItemsRepository cr = new CouponItemsRepository())
            {
                if (id > 0)
                {
                    return cr.GetCouponItemById(id);
                }
                return null;
            }
        }
        public static Product GetProductById(int id)
        {
            using (ProductRepository cr = new ProductRepository())
            {
                if (id > 0)
                {
                    return cr.GetProductById(id);
                }
                return null;
            }
        }
         */
        public static IPrincipal CurrentUser
        {
            get { return HttpContext.Current.User; }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string CurrentUserIP
        {
            get { return HttpContext.Current.Request.UserHostAddress; }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static decimal ConvertTipToDecimal(string tip)
        {
            decimal result = 0.00m;
            if (string.IsNullOrEmpty(tip) == false)
            {
                try
                {
                    result = decimal.Parse(tip);
                }
                catch
                {
                    result = 0.00m;
                }
            }
            else
            {
                result = 0.00m;
            }
            return result;
        }
        public static string CurrentUserName
        {
            get
            {
                string userName = string.Empty;
                if (CurrentUser.Identity.IsAuthenticated)
                {
                    userName = CurrentUser.Identity.Name;
                }
                return userName;
            }
        }

        public static string ToUSD(string m)
        {
            return "$" + m;
        }
        public static string FormatPhoneNumber(string sPhoneNumber)
        {
            return FormatPhoneNumber(sPhoneNumber, PhoneNumberFormat.Parenthises);
        }

        /// <summary>
        /// Returns a Formatted Phone Number based on the PhoneNumberFormat
        /// </summary>
        /// <param name="sPhoneNumber"></param>
        /// <param name="pnf"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatPhoneNumber(string sPhoneNumber, PhoneNumberFormat pnf)
        {
            var rg = new Regex("^\\(?(\\d{3})\\)?[/ - .]?(\\d{3})[- .]?(\\d{4})$");

            switch (pnf)
            {
                case PhoneNumberFormat.Dashes:
                    return rg.Replace(sPhoneNumber, "$1-$2-$3");
                case PhoneNumberFormat.Parenthises:
                    return rg.Replace(sPhoneNumber, "($1) $2-$3");
                case PhoneNumberFormat.Periods:
                    return rg.Replace(sPhoneNumber, "$1.$2.$3");
                case PhoneNumberFormat.Straight:
                    return rg.Replace(sPhoneNumber, "$1$2$3");
            }


            return sPhoneNumber;
        }


        /// <summary>
        /// </summary>
        /// <param name="vPrice"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        /// 
        public static string FormatPriceToPayPalStringFormat(object vPrice)
        {
            return Convert.ToDecimal(vPrice).ToString("N2");
        }
        public static string FormatPrice(object vPrice)
        {
            return Convert.ToDecimal(vPrice).ToString("N2"); //  + " " + Globals.Settings.Store.CurrencyCode;
        }
        public static string FormatPriceWithDollar(object vPrice)
        {
            return "$" + Convert.ToDecimal(vPrice).ToString("N2");
        }
        public static string GetSubdescription(string description)
        {
            if (description.Length < 120)
            {
                return description.Substring(0, description.Length);
            }
            return description.Substring(0, 120);
        }
        public static string GetSubTitle(string vSubTitle)
        {
            if (vSubTitle.Length < 70)
            {
                return vSubTitle.Substring(0, vSubTitle.Length);
            }
            return vSubTitle.Substring(0, 70) + "... ";
        }
        public static string GetShortSubdescription(string description)
        {
            if (description.Length < 60)
            {
                return description.Substring(0, description.Length);
            }
            return description.Substring(0, 60);
        }

        public static StringCollection GetCountries()
        {
            var countries = new StringCollection();
            countries.AddRange(_countries);
            return countries;
        }

        public static SortedList GetCountries(bool insertEmpty)
        {
            var countries = new SortedList();
            if (insertEmpty)
            {
                countries.Add("", "Please select one...");
            }
            foreach (string country in _countries)
            {
                countries.Add(country, country);
            }
            return countries;
        }
        public static IEnumerable<SelectListItem> CardTypeDropdownData()
        {

            return new[] {
                new SelectListItem{Text="VISA",Value="1"},
                new SelectListItem {Text="Master Card",Value="2"},
                new SelectListItem {Text="American Express",Value="3"},
                new SelectListItem {Text="Discover",Value="4"}
        };
        }
        public static string GetCardNameByTypeValue(string vTypeValue)
        {
            string v = vTypeValue;

            switch (v)
            {
                case "1":
                    return "VISA";
                case "2":

                    return "Master Card";
                case "3":
                    return "American Express";
                case "4":
                    return "Discover";
            }
            return v;
        }
        public static string GetScheduleDate(string vValue)
        {
            string v = vValue;

            switch (v)
            {
                case "1":
                    return "Today";
                case "2":

                    return "Tomorrow";
            }
            return v;
        }

        public static IEnumerable<SelectListItem> SizeDropdown()
        {
            return SizeDropdown(null);
        }
        public static IEnumerable<SelectListItem> SizeDropdown(string selectedV)
        {
            if (string.IsNullOrEmpty(selectedV))
            {
                selectedV = "";
            }
            return new[] {
                new SelectListItem{Text="Small",Value="Small",Selected=selectedV=="Small"},
                new SelectListItem {Text="Medium",Value="Medium",Selected=selectedV=="Medium"},
                new SelectListItem{Text="Large",Value="Large",Selected=selectedV=="Large"},
                new SelectListItem {Text="X-Large",Value="X-Large",Selected=selectedV=="X-Large"}
        };
        }
        public static IEnumerable<SelectListItem> YesNoDropdown()
        {
            return YesNoDropdown(null);
        }
        public static IEnumerable<SelectListItem> YesNoDropdown(string selectedV)
        {
            if (string.IsNullOrEmpty(selectedV))
            {
                selectedV = "";
            }
            return new[] {
                new SelectListItem{Text="Yes",Value="Yes",Selected=selectedV=="Yes"},
                new SelectListItem {Text="No",Value="No",Selected=selectedV=="No"}
        };
        }

        public static IEnumerable<SelectListItem> RestaurantPriceDropdown(string selectedV)
        {
            if (string.IsNullOrEmpty(selectedV))
            {
                selectedV = "";
            }
            return new[] {
                new SelectListItem{Text="Cheap, Under $10",Value="$",Selected=selectedV=="$"},
                new SelectListItem {Text="Moderate, $11--$30",Value="$$",Selected=selectedV=="$$"},
                new SelectListItem{Text="Spendy, $31-- $60",Value="$$$",Selected=selectedV=="$$$"},
                new SelectListItem {Text="Splurge, Above $61",Value="$$$$",Selected=selectedV=="$$$$"}
        };
        }
        public static IEnumerable<SelectListItem> DiningStyleDropdown(string selectedV)
        {
            if (string.IsNullOrEmpty(selectedV))
            {
                selectedV = "";
            }
            return new[] {
                new SelectListItem{Text="Casual",Value="Casual",Selected=selectedV=="Casual"},
                new SelectListItem {Text="Fine",Value="Fine",Selected=selectedV=="Fine"}
        };
        }
        public static List<SelectListItem> RVTimeDropdown(string startTime, string endTime)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            DateTime sTime = startTime == null ? Convert.ToDateTime("6:00 PM") : Convert.ToDateTime(startTime);
            DateTime eTime = endTime == null ? Convert.ToDateTime("8:00 PM") : Convert.ToDateTime(endTime);

            int diff = (int)((eTime - sTime).TotalMinutes);
            for (int i = 0; i <= diff; i = i + 15)
            {
                DateTime dt = sTime.AddMinutes(i);
                string tm = dt.ToString("h:mm tt");
                l.Add(new SelectListItem { Text = tm, Value = tm });
            }
            return l;
        }
        public static IEnumerable<SelectListItem> TimeDropdown()
        {
            return TimeDropdown(null);
        }

        public static IEnumerable<SelectListItem> TimeDropdown(string selectedV)
        {
            if (string.IsNullOrEmpty(selectedV))
            {
                selectedV = "";
            }
            return new[]
        {
            new SelectListItem{Text="12:00 AM",Value="12:00 AM",Selected=selectedV=="12:00 AM"},
            new SelectListItem {Text="12:10 AM",Value="12:10 AM",Selected=selectedV=="12:10 AM"},
            new SelectListItem {Text="12:20 AM",Value="12:20 AM",Selected=selectedV=="12:20 AM"},
            new SelectListItem{Text="12:30 AM",Value="12:30 AM",Selected=selectedV=="12:30 AM"},
            new SelectListItem {Text="12:40 AM",Value="12:40 AM",Selected=selectedV=="12:40 AM"},
            new SelectListItem{Text="12:50 AM",Value="12:50 AM",Selected=selectedV=="12:50 AM"},
            new SelectListItem {Text="1:00 AM",Value="1:00 AM",Selected=selectedV=="1:00 AM"},
            new SelectListItem {Text="1:10 AM",Value="1:10 AM",Selected=selectedV=="1:10 AM"},
            new SelectListItem {Text="1:20 AM",Value="1:20 AM",Selected=selectedV=="1:20 AM"},
            new SelectListItem{Text="1:30 AM",Value="1:30 AM",Selected=selectedV=="1:30 AM"},
            new SelectListItem {Text="1:40 AM",Value="1:40 AM",Selected=selectedV=="1:40 AM"},
            new SelectListItem{Text="1:50 AM",Value="1:50 AM",Selected=selectedV=="1:50 AM"},
            new SelectListItem {Text="2:00 AM",Value="2:00 AM",Selected=selectedV=="2:00 AM"},
            new SelectListItem {Text="2:10 AM",Value="2:10 AM",Selected=selectedV=="2:10 AM"},
            new SelectListItem {Text="2:20 AM",Value="2:20 AM",Selected=selectedV=="2:20 AM"},
            new SelectListItem{Text="2:30 AM",Value="2:30 AM",Selected=selectedV=="2:30 AM"},
            new SelectListItem {Text="2:40 AM",Value="2:40 AM",Selected=selectedV=="2:40 AM"},
            new SelectListItem{Text="2:50 AM",Value="2:50 AM",Selected=selectedV=="2:50 AM"},
            new SelectListItem {Text="3:00 AM",Value="3:00 AM",Selected=selectedV=="3:00 AM"},
            new SelectListItem {Text="3:10 AM",Value="3:10 AM",Selected=selectedV=="3:10 AM"},
            new SelectListItem {Text="3:20 AM",Value="3:20 AM",Selected=selectedV=="3:20 AM"},
            new SelectListItem{Text="3:30 AM",Value="3:30 AM",Selected=selectedV=="3:30 AM"},
            new SelectListItem {Text="3:40 AM",Value="3:40 AM",Selected=selectedV=="3:40 AM"},
            new SelectListItem{Text="3:50 AM",Value="3:50 AM",Selected=selectedV=="3:50 AM"},
            new SelectListItem {Text="4:00 AM",Value="4:00 AM",Selected=selectedV=="4:00 AM"},
            new SelectListItem {Text="4:10 AM",Value="4:10 AM",Selected=selectedV=="4:10 AM"},
            new SelectListItem {Text="4:20 AM",Value="4:20 AM",Selected=selectedV=="4:20 AM"},
            new SelectListItem{Text="4:30 AM",Value="4:30 AM",Selected=selectedV=="4:30 AM"},
            new SelectListItem {Text="4:40 AM",Value="4:40 AM",Selected=selectedV=="4:40 AM"},
            new SelectListItem{Text="4:50 AM",Value="4:50 AM",Selected=selectedV=="4:50 AM"},
            new SelectListItem {Text="5:00 AM",Value="5:00 AM",Selected=selectedV=="5:00 AM"},
            new SelectListItem {Text="5:10 AM",Value="5:10 AM",Selected=selectedV=="5:10 AM"},
            new SelectListItem {Text="5:20 AM",Value="5:20 AM",Selected=selectedV=="5:20 AM"},
            new SelectListItem{Text="5:30 AM",Value="5:30 AM",Selected=selectedV=="5:30 AM"},
            new SelectListItem {Text="5:40 AM",Value="5:40 AM",Selected=selectedV=="5:40 AM"},
            new SelectListItem{Text="5:50 AM",Value="5:50 AM",Selected=selectedV=="5:50 AM"},
            new SelectListItem {Text="6:00 AM",Value="6:00 AM",Selected=selectedV=="6:00 AM"},
            new SelectListItem {Text="6:10 AM",Value="6:10 AM",Selected=selectedV=="6:10 AM"},
            new SelectListItem {Text="6:20 AM",Value="6:20 AM",Selected=selectedV=="6:20 AM"},
            new SelectListItem{Text="6:30 AM",Value="6:30 AM",Selected=selectedV=="6:30 AM"},
            new SelectListItem {Text="6:40 AM",Value="6:40 AM",Selected=selectedV=="6:40 AM"},
            new SelectListItem{Text="6:50 AM",Value="6:50 AM",Selected=selectedV=="6:50 AM"},
            new SelectListItem {Text="7:00 AM",Value="7:00 AM",Selected=selectedV=="7:00 AM"},
            new SelectListItem {Text="7:10 AM",Value="7:10 AM",Selected=selectedV=="7:10 AM"},
            new SelectListItem {Text="7:20 AM",Value="7:20 AM",Selected=selectedV=="7:20 AM"},
            new SelectListItem{Text="7:30 AM",Value="7:30 AM",Selected=selectedV=="7:30 AM"},
            new SelectListItem {Text="7:40 AM",Value="7:40 AM",Selected=selectedV=="7:40 AM"},
            new SelectListItem{Text="7:50 AM",Value="7:50 AM",Selected=selectedV=="7:50 AM"},
            new SelectListItem {Text="8:00 AM",Value="8:00 AM",Selected=selectedV=="8:00 AM"},
            new SelectListItem {Text="8:10 AM",Value="8:10 AM",Selected=selectedV=="8:10 AM"},
            new SelectListItem {Text="8:20 AM",Value="8:20 AM",Selected=selectedV=="8:20 AM"},
            new SelectListItem{Text="8:30 AM",Value="8:30 AM",Selected=selectedV=="8:30 AM"},
            new SelectListItem {Text="8:40 AM",Value="8:40 AM",Selected=selectedV=="8:40 AM"},
            new SelectListItem{Text="8:50 AM",Value="8:50 AM",Selected=selectedV=="8:50 AM"},
            new SelectListItem {Text="9:00 AM",Value="9:00 AM",Selected=selectedV=="9:00 AM"},
            new SelectListItem {Text="9:10 AM",Value="9:10 AM",Selected=selectedV=="9:10 AM"},
            new SelectListItem {Text="9:20 AM",Value="9:20 AM",Selected=selectedV=="9:20 AM"},
            new SelectListItem{Text="9:30 AM",Value="9:30 AM",Selected=selectedV=="9:30 AM"},
            new SelectListItem {Text="9:40 AM",Value="9:40 AM",Selected=selectedV=="9:40 AM"},
            new SelectListItem{Text="9:50 AM",Value="9:50 AM",Selected=selectedV=="9:50 AM"},
            new SelectListItem {Text="10:00 AM",Value="10:00 AM",Selected=selectedV=="10:00 AM"},
            new SelectListItem {Text="10:10 AM",Value="10:10 AM",Selected=selectedV=="10:10 AM"},
            new SelectListItem {Text="10:20 AM",Value="10:20 AM",Selected=selectedV=="10:20 AM"},
            new SelectListItem{Text="10:30 AM",Value="10:30 AM",Selected=selectedV=="10:30 AM"},
            new SelectListItem {Text="10:40 AM",Value="10:40 AM",Selected=selectedV=="10:40 AM"},
            new SelectListItem{Text="10:50 AM",Value="10:50 AM",Selected=selectedV=="10:50 AM"},
            new SelectListItem {Text="11:00 AM",Value="11:00 AM",Selected=selectedV=="11:00 AM"},
            new SelectListItem {Text="11:10 AM",Value="11:10 AM",Selected=selectedV=="11:10 AM"},
            new SelectListItem {Text="11:20 AM",Value="11:20 AM",Selected=selectedV=="11:20 AM"},
            new SelectListItem{Text="11:30 AM",Value="11:30 AM",Selected=selectedV=="11:30 AM"},
            new SelectListItem {Text="11:40 AM",Value="11:40 AM",Selected=selectedV=="11:40 AM"},
            new SelectListItem{Text="11:50 AM",Value="11:50 AM",Selected=selectedV=="11:50 AM"},
            new SelectListItem {Text="12:00 PM",Value="12:00 PM",Selected=selectedV=="12:00 PM"},
            new SelectListItem {Text="12:10 PM",Value="12:10 PM",Selected=selectedV=="12:10 PM"},
            new SelectListItem {Text="12:20 PM",Value="12:20 PM",Selected=selectedV=="12:20 PM"},
            new SelectListItem{Text="12:30 PM",Value="12:30 PM",Selected=selectedV=="12:30 PM"},
            new SelectListItem {Text="12:40 PM",Value="12:40 PM",Selected=selectedV=="12:40 PM"},
            new SelectListItem{Text="12:50 PM",Value="12:50 PM",Selected=selectedV=="12:50 PM"},
            new SelectListItem {Text="1:00 PM",Value="1:00 PM",Selected=selectedV=="1:00 PM"},
            new SelectListItem {Text="1:10 PM",Value="1:10 PM",Selected=selectedV=="1:10 PM"},
            new SelectListItem {Text="1:20 PM",Value="1:20 PM",Selected=selectedV=="1:20 PM"},
            new SelectListItem{Text="1:30 PM",Value="1:30 PM",Selected=selectedV=="1:30 PM"},
            new SelectListItem {Text="1:40 PM",Value="1:40 PM",Selected=selectedV=="1:40 PM"},
            new SelectListItem{Text="1:50 PM",Value="1:50 PM",Selected=selectedV=="1:50 PM"},
            new SelectListItem {Text="2:00 PM",Value="2:00 PM",Selected=selectedV=="2:00 PM"},
            new SelectListItem {Text="2:10 PM",Value="2:10 PM",Selected=selectedV=="2:10 PM"},
            new SelectListItem {Text="2:20 PM",Value="2:20 PM",Selected=selectedV=="2:20 PM"},
            new SelectListItem{Text="2:30 PM",Value="2:30 PM",Selected=selectedV=="2:30 PM"},
            new SelectListItem {Text="2:40 PM",Value="2:40 PM",Selected=selectedV=="2:40 PM"},
            new SelectListItem{Text="2:50 PM",Value="2:50 PM",Selected=selectedV=="2:50 PM"},
            new SelectListItem {Text="3:00 PM",Value="3:00 PM",Selected=selectedV=="3:00 PM"},
            new SelectListItem {Text="3:10 PM",Value="3:10 PM",Selected=selectedV=="3:10 PM"},
            new SelectListItem {Text="3:20 PM",Value="3:20 PM",Selected=selectedV=="3:20 PM"},
            new SelectListItem{Text="3:30 PM",Value="3:30 PM",Selected=selectedV=="3:30 PM"},
            new SelectListItem {Text="3:40 PM",Value="3:40 PM",Selected=selectedV=="3:40 PM"},
            new SelectListItem{Text="3:50 PM",Value="3:50 PM",Selected=selectedV=="3:50 PM"},
            new SelectListItem {Text="4:00 PM",Value="4:00 PM",Selected=selectedV=="4:00 PM"},
            new SelectListItem {Text="4:10 PM",Value="4:10 PM",Selected=selectedV=="4:10 PM"},
            new SelectListItem {Text="4:20 PM",Value="4:20 PM",Selected=selectedV=="4:20 PM"},
            new SelectListItem{Text="4:30 PM",Value="4:30 PM",Selected=selectedV=="4:30 PM"},
            new SelectListItem {Text="4:40 PM",Value="4:40 PM",Selected=selectedV=="4:40 PM"},
            new SelectListItem{Text="4:50 PM",Value="4:50 PM",Selected=selectedV=="4:50 PM"},
            new SelectListItem {Text="5:00 PM",Value="5:00 PM",Selected=selectedV=="5:00 PM"},
            new SelectListItem {Text="5:10 PM",Value="5:10 PM",Selected=selectedV=="5:10 PM"},
            new SelectListItem {Text="5:20 PM",Value="5:20 PM",Selected=selectedV=="5:20 PM"},
            new SelectListItem{Text="5:30 PM",Value="5:30 PM",Selected=selectedV=="5:30 PM"},
            new SelectListItem {Text="5:40 PM",Value="5:40 PM",Selected=selectedV=="5:40 PM"},
            new SelectListItem{Text="5:50 PM",Value="5:50 PM",Selected=selectedV=="5:50 PM"},
            new SelectListItem {Text="6:00 PM",Value="6:00 PM",Selected=selectedV=="6:00 PM"},
            new SelectListItem {Text="6:10 PM",Value="6:10 PM",Selected=selectedV=="6:10 PM"},
            new SelectListItem {Text="6:20 PM",Value="6:20 PM",Selected=selectedV=="6:20 PM"},
            new SelectListItem{Text="6:30 PM",Value="6:30 PM",Selected=selectedV=="6:30 PM"},
            new SelectListItem {Text="6:40 PM",Value="6:40 PM",Selected=selectedV=="6:40 PM"},
            new SelectListItem{Text="6:50 PM",Value="6:50 PM",Selected=selectedV=="6:50 PM"},
            new SelectListItem {Text="7:00 PM",Value="7:00 PM",Selected=selectedV=="7:00 PM"},
            new SelectListItem {Text="7:10 PM",Value="7:10 PM",Selected=selectedV=="7:10 PM"},
            new SelectListItem {Text="7:20 PM",Value="7:20 PM",Selected=selectedV=="7:20 PM"},
            new SelectListItem{Text="7:30 PM",Value="7:30 PM",Selected=selectedV=="7:30 PM"},
            new SelectListItem {Text="7:40 PM",Value="7:40 PM",Selected=selectedV=="7:40 PM"},
            new SelectListItem{Text="7:50 PM",Value="7:50 PM",Selected=selectedV=="7:50 PM"},
            new SelectListItem {Text="8:00 PM",Value="8:00 PM",Selected=selectedV=="8:00 PM"},
            new SelectListItem {Text="8:10 PM",Value="8:10 PM",Selected=selectedV=="8:10 PM"},
            new SelectListItem {Text="8:20 PM",Value="8:20 PM",Selected=selectedV=="8:20 PM"},
            new SelectListItem{Text="8:30 PM",Value="8:30 PM",Selected=selectedV=="8:30 PM"},
            new SelectListItem {Text="8:40 PM",Value="8:40 PM",Selected=selectedV=="8:40 PM"},
            new SelectListItem{Text="8:50 PM",Value="8:50 PM",Selected=selectedV=="8:50 PM"},
            new SelectListItem {Text="9:00 PM",Value="9:00 PM",Selected=selectedV=="9:00 PM"},
            new SelectListItem {Text="9:10 PM",Value="9:10 PM",Selected=selectedV=="9:10 PM"},
            new SelectListItem {Text="9:20 PM",Value="9:20 PM",Selected=selectedV=="9:20 PM"},
            new SelectListItem{Text="9:30 PM",Value="9:30 PM",Selected=selectedV=="9:30 PM"},
            new SelectListItem {Text="9:40 PM",Value="9:40 PM",Selected=selectedV=="9:40 PM"},
            new SelectListItem{Text="9:50 PM",Value="9:50 PM",Selected=selectedV=="9:50 PM"},
            new SelectListItem {Text="10:00 PM",Value="10:00 PM",Selected=selectedV=="10:00 PM"},
            new SelectListItem {Text="10:10 PM",Value="10:10 PM",Selected=selectedV=="10:10 PM"},
            new SelectListItem {Text="10:20 PM",Value="10:20 PM",Selected=selectedV=="10:20 PM"},
            new SelectListItem{Text="10:30 PM",Value="10:30 PM",Selected=selectedV=="10:30 PM"},
            new SelectListItem {Text="10:40 PM",Value="10:40 PM",Selected=selectedV=="10:40 PM"},
            new SelectListItem{Text="10:50 PM",Value="10:50 PM",Selected=selectedV=="10:50 PM"},
            new SelectListItem {Text="11:00 PM",Value="11:00 PM",Selected=selectedV=="11:00 PM"},
            new SelectListItem {Text="11:10 PM",Value="11:10 PM",Selected=selectedV=="11:10 PM"},
            new SelectListItem {Text="11:20 PM",Value="11:20 PM",Selected=selectedV=="11:20 PM"},
            new SelectListItem{Text="11:30 PM",Value="11:30 PM",Selected=selectedV=="11:30 PM"},
            new SelectListItem {Text="11:40 PM",Value="11:40 PM",Selected=selectedV=="11:40 PM"},
            new SelectListItem{Text="11:50 PM",Value="11:50 PM",Selected=selectedV=="11:50 PM"}
        };

        }
        public static IEnumerable<SelectListItem> NumOfPeopleDropdown(string selectedV)
        {
            if (string.IsNullOrEmpty(selectedV))
            {
                selectedV = "2";
            }
            return new[] {
                new SelectListItem {Text="1 person",Value="1",Selected=selectedV=="1"},
                new SelectListItem {Text="2 people",Value="2",Selected=selectedV=="2"},
                new SelectListItem {Text="3 people",Value="3",Selected=selectedV=="3"},
                new SelectListItem {Text="4 people",Value="4",Selected=selectedV=="4"},
                new SelectListItem {Text="5 people",Value="5",Selected=selectedV=="5"},
                new SelectListItem {Text="6 people",Value="6",Selected=selectedV=="6"},
                new SelectListItem {Text="7 people",Value="7",Selected=selectedV=="7"},
                new SelectListItem {Text="8 people",Value="8",Selected=selectedV=="8"},
                new SelectListItem {Text="9 people",Value="9",Selected=selectedV=="9"},
                new SelectListItem {Text="10 people",Value="10",Selected=selectedV=="10"},
                new SelectListItem {Text="11 people",Value="11",Selected=selectedV=="11"},
                new SelectListItem {Text="12 people",Value="12",Selected=selectedV=="12"},
                new SelectListItem {Text="13 people",Value="13",Selected=selectedV=="13"},
                new SelectListItem {Text="14 people",Value="14",Selected=selectedV=="14"},
                new SelectListItem {Text="15 people",Value="15",Selected=selectedV=="15"},
                new SelectListItem {Text="16 people",Value="16",Selected=selectedV=="16"},
                new SelectListItem {Text="17 people",Value="17",Selected=selectedV=="17"},
                new SelectListItem {Text="18 people",Value="18",Selected=selectedV=="18"},
                new SelectListItem {Text="19 people",Value="19",Selected=selectedV=="19"},
                new SelectListItem {Text="20 people",Value="20",Selected=selectedV=="20"},
                new SelectListItem {Text="21 people",Value="21",Selected=selectedV=="21"},
                new SelectListItem {Text="22 people",Value="22",Selected=selectedV=="22"},
                new SelectListItem {Text="23 people",Value="23",Selected=selectedV=="23"},
                new SelectListItem {Text="24 people",Value="24",Selected=selectedV=="24"},
                new SelectListItem {Text="25 people",Value="25",Selected=selectedV=="25"},
                new SelectListItem {Text="26 people",Value="26",Selected=selectedV=="26"},
                new SelectListItem {Text="27 people",Value="27",Selected=selectedV=="27"},
                new SelectListItem {Text="28 people",Value="28",Selected=selectedV=="28"},
                new SelectListItem {Text="29 people",Value="29",Selected=selectedV=="29"},
                new SelectListItem {Text="30 people",Value="30",Selected=selectedV=="30"}
        };
        }
        public static IEnumerable<SelectListItem> MonthDropdownData()
        {

            return new[] {
                new SelectListItem{Text="01",Value="01"},
                new SelectListItem {Text="02",Value="02"},
                new SelectListItem {Text="03",Value="03"},
                new SelectListItem {Text="04",Value="04"},
                new SelectListItem {Text="05",Value="05"},
                new SelectListItem{Text="06",Value="06"},
                new SelectListItem {Text="07",Value="07"},
                new SelectListItem {Text="08",Value="08"},
                new SelectListItem {Text="09",Value="09"},
                new SelectListItem {Text="10",Value="10"},
                new SelectListItem{Text="11",Value="11"},
                new SelectListItem {Text="12",Value="12"}
        };

        }
        public static IEnumerable<SelectListItem> YearDropdownData()
        {
            int yr = DateTime.Now.Year;
            List<SelectListItem> lt = new List<SelectListItem>();
            for (var i = yr; i < yr + 11; i++)
            {
                lt.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return lt;
        }

        public static IEnumerable<SelectListItem> OccupationDropdownData(string selectedV)
        {

            return new[] {
                new SelectListItem{Text="Academic",Value="Academic",Selected=selectedV=="Academic"},
                new SelectListItem{Text="Accountant",Value="Accountant",Selected=selectedV=="Accountant"},
                new SelectListItem {Text="Actor",Value="Actor",Selected=selectedV=="Actor"},
                new SelectListItem {Text="Architect",Value="Architect",Selected=selectedV=="Architect"},
                new SelectListItem {Text="Artist",Value="Artist",Selected=selectedV=="Artist"},
                new SelectListItem {Text="Business Manager",Value="Business Manager",Selected=selectedV=="Business Manager"},
                new SelectListItem{Text="Carpenter",Value="Carpenter",Selected=selectedV=="Carpenter"},
                new SelectListItem {Text="Chief Executive",Value="Chief Executive",Selected=selectedV=="Chief Executive"},
                new SelectListItem {Text="Cinematographer",Value="Cinematographer",Selected=selectedV=="Cinematographer"},
                new SelectListItem {Text="Civil Servant",Value="Civil Servant",Selected=selectedV=="Civil Servant"},
                new SelectListItem {Text="Coach",Value="Coach",Selected=selectedV=="Coach"},
                new SelectListItem{Text="Composer",Value="Composer",Selected=selectedV=="Composer"},
                new SelectListItem {Text="Computer programme",Value="Computer programme",Selected=selectedV=="Computer programme"},
                new SelectListItem {Text="Cook",Value="Cook",Selected=selectedV=="Cook"},
                new SelectListItem {Text="Counsellor",Value="Counsellor",Selected=selectedV=="Counsellor"},
                new SelectListItem {Text="Doctor",Value="Doctor",Selected=selectedV=="Doctor"},
                new SelectListItem {Text="Driver",Value="Driver",Selected=selectedV=="Driver"},
                new SelectListItem{Text="Economist",Value="Economist",Selected=selectedV=="Economist"},
                new SelectListItem {Text="Editor",Value="Editor",Selected=selectedV=="Editor"},
                new SelectListItem {Text="Electrician",Value="Electrician",Selected=selectedV=="Electrician"},
                new SelectListItem {Text="Engineer",Value="Engineer",Selected=selectedV=="Engineer"},
                new SelectListItem {Text="Executive Producer",Value="Executive Producer",Selected=selectedV=="Executive Producer"},
                new SelectListItem {Text="Fixer",Value="Fixer",Selected=selectedV=="Fixer"},
                new SelectListItem {Text="Graphic Designer",Value="Graphic Designer",Selected=selectedV=="Graphic Designer"},
                new SelectListItem {Text="Hairdresser",Value="Hairdresser",Selected=selectedV=="Hairdresser"},
                new SelectListItem {Text="Headhunter",Value="Headhunter",Selected=selectedV=="Headhunter"},
                new SelectListItem {Text="HR - Recruitment",Value="HR - Recruitment",Selected=selectedV=="HR - Recruitment"},
                new SelectListItem {Text="Information Officer",Value="Information Officer",Selected=selectedV=="Information Officer"},
                new SelectListItem {Text="IT Consultant",Value="IT Consultant",Selected=selectedV=="IT Consultant"},
                new SelectListItem {Text="Journalist",Value="Journalist",Selected=selectedV=="Journalist"},
                new SelectListItem {Text="Lawyer / Solicitor",Value="Lawyer / Solicitor",Selected=selectedV=="Lawyer / Solicitor"},
                new SelectListItem {Text="Lecturer",Value="Lecturer",Selected=selectedV=="Lecturer"},
                new SelectListItem {Text="Librarian",Value="Librarian",Selected=selectedV=="Librarian"},
                new SelectListItem {Text="Mechanic",Value="Mechanic",Selected=selectedV=="Mechanic"},
                new SelectListItem {Text="Model",Value="Model",Selected=selectedV=="Model"},
                new SelectListItem {Text="Musician",Value="Musician",Selected=selectedV=="Musician"},
                new SelectListItem {Text="Office Worker",Value="Office Worker",Selected=selectedV=="Office Worker"},
                new SelectListItem {Text="Performer",Value="Performer",Selected=selectedV=="Performer"},
                new SelectListItem {Text="Photographer",Value="Photographer",Selected=selectedV=="Photographer"},
                new SelectListItem {Text="Presenter",Value="Presenter",Selected=selectedV=="Presenter"},
                new SelectListItem {Text="Producer / Director",Value="Producer / Director",Selected=selectedV=="Producer / Director"},
                new SelectListItem {Text="Project Manager",Value="Project Manager",Selected=selectedV=="Project Manager"},
                new SelectListItem {Text="Researcher",Value="Researcher",Selected=selectedV=="Researcher"},
                new SelectListItem {Text="Salesman",Value="Salesman",Selected=selectedV=="Salesman"},
                new SelectListItem {Text="Social Worker",Value="Social Worker",Selected=selectedV=="Social Worker"},
                new SelectListItem {Text="Soldier",Value="Soldier",Selected=selectedV=="Soldier"},
                new SelectListItem {Text="Sportsperson",Value="Sportsperson",Selected=selectedV=="Sportsperson"},
                new SelectListItem {Text="Student",Value="Student",Selected=selectedV=="Student"},
                new SelectListItem {Text="Teacher",Value="Teacher",Selected=selectedV=="Teacher"},
                new SelectListItem {Text="Technical Crew",Value="Technical Crew",Selected=selectedV=="Technical Crew"},
                new SelectListItem {Text="Technical Writer",Value="Technical Writer",Selected=selectedV=="Technical Writer"},
                new SelectListItem {Text="Therapist",Value="Therapist",Selected=selectedV=="Therapist"},
                new SelectListItem {Text="Translator",Value="Translator",Selected=selectedV=="Translator"},
                new SelectListItem {Text="Waitress / Waiter",Value="Waitress / Waiter",Selected=selectedV=="Waitress / Waiter"},
                new SelectListItem {Text="Web designer / author",Value="Web designer / author",Selected=selectedV=="Web designer / author"},
                new SelectListItem {Text="Writer",Value="Writer",Selected=selectedV=="Writer"},
                new SelectListItem {Text="Other",Value="Other",Selected=selectedV=="Other"}
        };
        }

        public static IEnumerable<SelectListItem> StateDropdownData(string selectedV)
        {
            if (string.IsNullOrEmpty(selectedV))
            {
                selectedV = "CA";
            }
            return new[] {
                new SelectListItem{Text="Alabama",Value="AL",Selected=selectedV=="AL"},
                new SelectListItem {Text="Alaska",Value="AK",Selected=selectedV=="AK"},
                new SelectListItem {Text="Arizona",Value="AZ",Selected=selectedV=="AZ"},
                new SelectListItem {Text="Arkansas",Value="AR",Selected=selectedV=="AR"},
                new SelectListItem {Text="California",Value="CA",Selected=selectedV=="CA"},
                new SelectListItem{Text="Colorado",Value="CO",Selected=selectedV=="CO"},
                new SelectListItem {Text="Connecticut",Value="CT",Selected=selectedV=="CT"},
                new SelectListItem {Text="Delaware",Value="DE",Selected=selectedV=="DE"},
                new SelectListItem {Text="Florida",Value="FL",Selected=selectedV=="FL"},
                new SelectListItem {Text="Georgia",Value="GA",Selected=selectedV=="GA"},
                
                new SelectListItem{Text="Hawaii",Value="HI",Selected=selectedV=="HI"},
                new SelectListItem {Text="Idaho",Value="ID",Selected=selectedV=="ID"},
                new SelectListItem {Text="Illinois",Value="IL",Selected=selectedV=="IL"},
                new SelectListItem {Text="Indiana",Value="IN",Selected=selectedV=="IN"},
                new SelectListItem {Text="Iowa",Value="IA",Selected=selectedV=="IA"},
                new SelectListItem{Text="Kansas",Value="KS",Selected=selectedV=="KS"},
                new SelectListItem {Text="Kentucky",Value="KY",Selected=selectedV=="KY"},
                new SelectListItem {Text="Louisiana",Value="LA",Selected=selectedV=="LA"},
                new SelectListItem {Text="Maine",Value="ME",Selected=selectedV=="ME"},
                new SelectListItem {Text="Maryland",Value="MD",Selected=selectedV=="MD"},
                
                new SelectListItem{Text="Massachusetts",Value="MA",Selected=selectedV=="MA"},
                new SelectListItem {Text="Michigan",Value="MI",Selected=selectedV=="MI"},
                new SelectListItem {Text="Minnesota",Value="MN",Selected=selectedV=="MN"},
                new SelectListItem {Text="Mississippi",Value="MS",Selected=selectedV=="MS"},
                new SelectListItem {Text="Missouri",Value="MO",Selected=selectedV=="MO"},
                new SelectListItem{Text="Montana",Value="MT",Selected=selectedV=="MT"},
                new SelectListItem {Text="Nebraska",Value="NE",Selected=selectedV=="NE"},
                new SelectListItem {Text="Nevada",Value="NV",Selected=selectedV=="NV"},
                new SelectListItem {Text="New Hampshire",Value="NH",Selected=selectedV=="NH"},
                new SelectListItem {Text="New Jersey",Value="NJ",Selected=selectedV=="NJ"},
                
                new SelectListItem{Text="New Mexico",Value="NM",Selected=selectedV=="NM"},
                new SelectListItem {Text="New York",Value="NY",Selected=selectedV=="NY"},
                new SelectListItem {Text="North Carolina",Value="NC",Selected=selectedV=="NC"},
                new SelectListItem {Text="North Dakota",Value="ND",Selected=selectedV=="ND"},
                new SelectListItem {Text="Ohio",Value="OH",Selected=selectedV=="OH"},
                new SelectListItem{Text="Oklahoma",Value="OK",Selected=selectedV=="OK"},
                new SelectListItem {Text="Oregon",Value="OR",Selected=selectedV=="OR"},
                new SelectListItem {Text="Pennsylvania",Value="PA",Selected=selectedV=="PA"},
                new SelectListItem {Text="Rhode Island",Value="RI",Selected=selectedV=="RI"},
                new SelectListItem {Text="South Carolina",Value="SC",Selected=selectedV=="SC"},
                
                new SelectListItem{Text="South Dakota",Value="SD",Selected=selectedV=="SD"},
                new SelectListItem {Text="Tennessee",Value="TN",Selected=selectedV=="TN"},
                new SelectListItem {Text="Texas",Value="TX",Selected=selectedV=="TX"},
                new SelectListItem {Text="Utah",Value="UT",Selected=selectedV=="UT"},
                new SelectListItem {Text="Vermont",Value="VT",Selected=selectedV=="VT"},
                new SelectListItem{Text="Virginia",Value="VA",Selected=selectedV=="VA"},
                new SelectListItem {Text="Washington",Value="WA",Selected=selectedV=="WA"},
                new SelectListItem {Text="West Virginia",Value="WV",Selected=selectedV=="WV"},
                new SelectListItem {Text="Wisconsin",Value="WI",Selected=selectedV=="WI"},
                new SelectListItem {Text="Wyoming",Value="WY",Selected=selectedV=="WY"},
                new SelectListItem {Text="Quebec, Canada",Value="QC",Selected=selectedV=="QC"},
                new SelectListItem {Text="British Columbia, Canada",Value="BC",Selected=selectedV=="BC"}
        };
        }
        public static IEnumerable<SelectListItem> DriverTip(decimal selectedTip, decimal subtotalV,bool isdelivery)
        {
            string selectedV = selectedTip.ToString("N2");
            string basetip15 = (subtotalV * 0.15m).ToString("N2");
            string tip16 = (subtotalV * 0.16m).ToString("N2");
            string tip17 = (subtotalV * 0.17m).ToString("N2");
            string tip18 = (subtotalV * 0.18m).ToString("N2");
            string tip19 = (subtotalV * 0.19m).ToString("N2");
            string tip20 = (subtotalV * 0.20m).ToString("N2");
            string tip21 = (subtotalV * 0.21m).ToString("N2");
            string tip22 = (subtotalV * 0.22m).ToString("N2");
            string tip23 = (subtotalV * 0.23m).ToString("N2");
            string tip24 = (subtotalV * 0.24m).ToString("N2");
            string tip25 = (subtotalV * 0.25m).ToString("N2");
            string tipnote = isdelivery ? "Tip the driver 15% of subtotal" : "Tip? No, not this time";
            decimal intTip =isdelivery? Decimal.Round(subtotalV * 0.15m) + 1:2.0m;
            if (selectedTip == 0.0m || subtotalV == 0.0m)
            {
                selectedV = "0.00";
            }
            return new[] {
                new SelectListItem{Text=tipnote,Value="0.00",Selected=selectedV=="0.00"},
                new SelectListItem {Text="15% ---  $" + string.Format("{0} tip",basetip15),Value=basetip15,Selected=selectedV==basetip15},
                new SelectListItem {Text="16% ---  $" + string.Format("{0} tip",tip16),Value=tip16,Selected=selectedV==tip16},
                new SelectListItem {Text="17% ---  $" + string.Format("{0} tip",tip17),Value=tip17,Selected=selectedV==tip17},
                new SelectListItem {Text="18% ---  $" + string.Format("{0} tip",tip18),Value=tip18,Selected=selectedV==tip18},
                new SelectListItem {Text="19% ---  $" + string.Format("{0} tip",tip19),Value=tip19,Selected=selectedV==tip19},
                new SelectListItem {Text="20% ---  $" + string.Format("{0} tip",tip20),Value=tip20,Selected=selectedV==tip20},
                new SelectListItem {Text="21% ---  $" + string.Format("{0} tip",tip21),Value=tip21,Selected=selectedV==tip21},
                new SelectListItem {Text="22% ---  $" + string.Format("{0} tip",tip22),Value=tip23,Selected=selectedV==tip22},
                new SelectListItem {Text="23% ---  $" + string.Format("{0} tip",tip23),Value=tip23,Selected=selectedV==tip23},
                new SelectListItem {Text="24% ---  $" + string.Format("{0} tip",tip24),Value=tip24,Selected=selectedV==tip24},
                new SelectListItem {Text="25% ---  $" + string.Format("{0} tip",tip25),Value=tip25,Selected=selectedV==tip25},

                new SelectListItem {Text="$" + intTip.ToString("N") + " --- tip",Value=intTip.ToString("N"),Selected=selectedV==intTip.ToString("N")},
                new SelectListItem {Text="$" + (intTip + 1.00m).ToString("N") + " --- tip",Value=(intTip + 1.00m).ToString("N"),Selected=selectedV==(intTip + 1.00m).ToString("N")},
                new SelectListItem {Text="$" + (intTip + 2.00m).ToString("N") + " --- tip",Value=(intTip + 2.00m).ToString("N"),Selected=selectedV==(intTip + 2.00m).ToString("N")},
                new SelectListItem {Text="$" + (intTip + 3.00m).ToString("N") + " --- tip",Value=(intTip + 3.00m).ToString("N"),Selected=selectedV==(intTip + 3.00m).ToString("N")},
                new SelectListItem {Text="$" + (intTip + 4.00m).ToString("N") + " --- tip",Value=(intTip + 4.00m).ToString("N"),Selected=selectedV==(intTip + 4.00m).ToString("N")},
                new SelectListItem {Text="$" + (intTip + 5.00m).ToString("N") + " --- tip",Value=(intTip + 5.00m).ToString("N"),Selected=selectedV==(intTip + 5.00m).ToString("N")},
                new SelectListItem {Text="$" + (intTip + 6.00m).ToString("N") + " --- tip",Value=(intTip + 6.00m).ToString("N"),Selected=selectedV==(intTip + 6.00m).ToString("N")},
                new SelectListItem {Text="$" + (intTip + 7.00m).ToString("N") + " --- tip",Value=(intTip + 7.00m).ToString("N"),Selected=selectedV==(intTip + 7.00m).ToString("N")},
                new SelectListItem {Text="$" + (intTip + 8.00m).ToString("N") + " --- tip",Value=(intTip + 8.00m).ToString("N"),Selected=selectedV==(intTip + 8.00m).ToString("N")},
                new SelectListItem {Text="$" + (intTip + 9.00m).ToString("N") + " --- tip",Value=(intTip + 9.00m).ToString("N"),Selected=selectedV==(intTip + 9.00m).ToString("N")}
        };
        }
        public static IEnumerable<SelectListItem> GenderDropdownData(string selectedV)
        {

            return new[] {
                new SelectListItem{Text="Male",Value="Male",Selected=selectedV=="Male"},
                new SelectListItem {Text="Female",Value="Female",Selected=selectedV=="Female"}
        };
        }
        public static IEnumerable<SelectListItem> AttendStatusData()
        {
            return new[] {
                new SelectListItem{Text="I will be attending",Value="1"},
                new SelectListItem {Text="I will not be attending",Value="2"},
                new SelectListItem {Text="I am not sure",Value="3"}
        };

        }
        public static decimal SetUnitPrice(decimal inputValue, int useTypeId)
        {
            decimal unitprice = (inputValue < 0.0m) ? 0.0m : inputValue;
            unitprice = (unitprice > 1000.0m) ? 1000.0m : unitprice;
            unitprice = (useTypeId == 4 || useTypeId == 2) ? 0.0m : unitprice;
            return unitprice;
        }
        public static int SetDiscountPercentage(int inputValue, int useTypeId)
        {
            int discountperc = (inputValue < 0 || inputValue > 100) ? 0 : inputValue;
            discountperc = (useTypeId == 4 || useTypeId == 2) ? 0 : discountperc;
            return discountperc;
        }
        public static int SetMaxDays(int inputValue, int useTypeId)
        {
            int maxdays = (inputValue < 1) ? 30 : inputValue;
            maxdays = (maxdays > 30) ? 30 : maxdays;
            maxdays = (useTypeId == 4) ? 9999 : maxdays;
            return maxdays;
        }
        public static int SetMaxCount(int inputValue, int useTypeId)
        {
            int maxcount = (inputValue < 1) ? 10 : inputValue;
            maxcount = (maxcount > 10) ? 10 : maxcount;
            maxcount = (useTypeId == 4) ? 9999 : maxcount;
            return maxcount;
        }
        /// <summary>
        /// </summary>
        /// <param name="vFolder"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetPhyscialPath(string vFolder)
        {
            return Path.Combine(ApplicationPath, vFolder);
        }


        public static string GetURLPath(string vURL)
        {
            var _Regex = new Regex("://[^/]+/(?<path>[^?\\s<>#\"]+)");
            if (_Regex.Matches(vURL).Count > 0)
            {
                return _Regex.Match(vURL).Groups[1].ToString();
            }
            return vURL;
        }


        public static bool IsValidPhoneNumber(string sPhoneNumber)
        {
            return IsValidUSPhoneNumber(sPhoneNumber);
        }

        public static bool IsValidUSPhoneNumber(string sPhoneNumber)
        {
            RegexOptions options = RegexOptions.Multiline;

            MatchCollection matches = Regex.Matches(sPhoneNumber,
                                                    "^\\d{3}/\\d{3}-\\d{4}$^\\(\\d{3}\\)\\d{3}-\\d{4}$|^\\d{3}-\\d{3}-\\d{4}$|\\d{10}",
                                                    options);

            foreach (object found in matches)
            {
                return true;
            }


            return false;
        }

        public static bool IsValidString(string vValue, int vMax)
        {
            return IsValidString(vValue, 0, vMax);
        }

        public static bool IsValidString(string vValue, int vMin, int vMax)
        {
            if (vMin > 0)
            {
                if (string.IsNullOrEmpty(vValue) && vMin <= vValue.Length)
                {
                    return false;
                }
            }
            if (vMax < vValue.Length)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// </summary>
        /// <param name="vDirectoryPath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DirectoryInfo MakeDirectory(string vDirectoryPath)
        {
            if (Directory.Exists(vDirectoryPath))
            {
                return new DirectoryInfo(vDirectoryPath);
            }
            return Directory.CreateDirectory(vDirectoryPath);
        }

        public static string ParsePhoneNumber(string sPhoneNumber)
        {
            if (IsValidUSPhoneNumber(sPhoneNumber))
            {
                return sPhoneNumber.Replace("(", "").Replace(")", "").Replace("-", "");
            }
            return string.Empty;
        }

        public static string ProperCase(string Text)
        {
            var objCulture = new CultureInfo("en-US");
            return objCulture.TextInfo.ToTitleCase(Text.ToLower());
        }

        /// <summary>
        /// This is a utlity function I used to wrap the Regex.Replace
        /// method. It just provides a shorthand way to do a replace.
        /// </summary>
        /// <param name="strin"></param>
        /// <param name="strExp"></param>
        /// <param name="strReplace"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RegexReplace(string strin, string strExp, string strReplace)
        {
            return Regex.Replace(strin, strExp, strReplace);
        }


        public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
            {
                return null;
            }
            if (originalUrl.IndexOf("://") != -1)
            {
                return originalUrl;
            }
            if (!originalUrl.StartsWith("~"))
            {
                return originalUrl;
            }
            if (HttpContext.Current == null)
            {
                throw new ArgumentException("Invalid URL: Relative URL not allowed.");
            }
            return (HttpContext.Current.Request.ApplicationPath + originalUrl.Substring(1).Replace("//", "/"));
        }


        public static string getRatingImgUrl(string averageRating, int num)
        {
            int ar = (int)(decimal.Parse(averageRating) * 2 + 0.5m);
            if (ar >= num * 2)
            {
                return "App_Themes/CHThemes/Images/ratingStar.png";
            }
            else if (ar == num * 2 - 1)
            {
                return "App_Themes/CHThemes/Images/ratingHalf.png";
            }
            else
            {
                return "App_Themes/CHThemes/Images/ratingEmpty.png";
            }

        }
        public static decimal ConvertKMtoMiles(string km)
        {
            return string.IsNullOrEmpty(km) ? 0m : (decimal.Parse(km.ToLower().Replace("km", "").Replace("m", "").Replace("mi", "").Trim())) * 0.621371m;
        }
        public static string EncodeString(string str)
        {
            return str.Replace(" ", "-");
        }
        public static string DecodeString(string str)
        {
            return str.Replace("-", " ");
        }
        public static string[] TimeZoneStandardName
        {
            get { return _timeZoneStandardName; }
        }
        private static readonly string[] _timeZoneStandardName = new[]
                           {
            "Hawaiian Standard Time",
            "Alaskan Standard Time",
            "Pacific Standard Time",
            "Mountain Standard Time",
            "Central Standard Time",
            "Eastern Standard Time"
                                                          
                           };
        public static string ListAllTimeZoneStandardName()
        {
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            //Console.WriteLine("All time zones:");
            StringBuilder sb = new StringBuilder();
            foreach (TimeZoneInfo timeZoneInfo in timeZones)
            {
                sb.Append(string.Format("   {0}", timeZoneInfo.StandardName) + "<br>");
            }
            return sb.ToString();
        }
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
        public static TimeSpan DifferenceBetweenMyzoneAndLocal(string myZoneName)
        {
            var myzone = TimeZoneInfo.FindSystemTimeZoneById(myZoneName);
            var local = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.StandardName);

            var now = DateTimeOffset.UtcNow;
            TimeSpan myzoneOffset = myzone.GetUtcOffset(now);
            TimeSpan localOffset = local.GetUtcOffset(now);
            TimeSpan difference = myzoneOffset - localOffset;
            return difference;
        }

        public static string DeliveryFee(decimal vFee)
        {
            return vFee > 0.0m ? Helper.ToUSD(vFee.ToString("N2")) : "Free";
        }

        public static string CouponNote(int vPercentage, string vFreeItem, decimal vSubTotal)
        {

            if (vPercentage > 0 && vSubTotal > 0.0m)
            {
                return "Discount( " + vPercentage + "% ):  -$" + (vSubTotal * vPercentage / 100).ToString("N2");
            }
            else
            {
                return string.IsNullOrEmpty(vFreeItem) ? "" : vFreeItem;
            }
        }

        public static int GetRandItem(int min, int max)
        {
            Random rand = new Random();
            return rand.Next(min, max);
        }
        public static string GetReview(string vWords, int vLength)
        {
            if (vWords.Length < vLength)
            {
                return vWords;
            }
            return vWords.Substring(0, vLength) + "...";
        }
        public static string RemoveHyphens(string vWords)
        {
            int idx = vWords.IndexOf("---");
            if (vWords.Length > 0)
            {
                return vWords.Substring(0, vWords.IndexOf("---"));
            }
            return vWords;
        }
    }
}
