using System;
using System.Net;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Web;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
namespace FR.Services.GoogleAPI
{
    [Serializable]
    public class GeocoderLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}", Latitude, Longitude);
        }
    }

    public class Geocoding
    {

        public static GeocoderLocation Locate(string query)
        {
            WebRequest request = WebRequest
               .Create("http://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address="
                  + HttpUtility.UrlEncode(query));

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    XDocument document = XDocument.Load(new StreamReader(stream));

                    XElement longitudeElement = document.Descendants("lng").FirstOrDefault();
                    XElement latitudeElement = document.Descendants("lat").FirstOrDefault();

                    if (longitudeElement != null && latitudeElement != null)
                    {
                        return new GeocoderLocation
                        {
                            Longitude = Double.Parse(longitudeElement.Value, CultureInfo.InvariantCulture),
                            Latitude = Double.Parse(latitudeElement.Value, CultureInfo.InvariantCulture)
                        };
                    }
                }
            }

            return null;
        }
    }
}
