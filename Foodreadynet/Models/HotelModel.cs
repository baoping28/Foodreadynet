using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace FoodReady.WebUI.Models
{
    public class HotelModel
    {
        public Dictionary<string, List<Hotel>> DCityHotel { get; set; }
        public List<HotelType> LHotelType { get; set; }
        public string GoogleMapLink(Hotel vHotel)
        {
            if (vHotel == null)
            {
                return string.Empty;
            }
            string s = vHotel.HotelAddressString.Trim();
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ");
            s = s.Replace(" ", "+");
            return @"https://maps.google.com/maps?saddr=Your+Address&daddr=" + s + ",+USA&hl=en&ll=" + vHotel.Latitude + "," + vHotel.Longitude;
        }
    }
}