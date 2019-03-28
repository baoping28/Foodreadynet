using System;
using System.Net;
using System.Xml;
using System.IO;
using FR.Infrastructure.Helpers;
namespace FR.Services.GoogleAPI
{
    public class GoogleHelps
    {
        public static decimal GetDistance(string startPoint, string endPoint, out string approxTime)
        {
            approxTime = string.Empty;
            decimal result = -1.0m;
            string xmlResult = null;

            //Pass request to google api with orgin and destination details
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + startPoint + "&destinations=" + endPoint + "&mode=Car&language=us-en&sensor=false");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //Get response as stream from httpwebresponse
            StreamReader resStream = new StreamReader(response.GetResponseStream());

            //Create instance for xml document
            XmlDocument doc = new XmlDocument();

            //Load response stream in to xml result
            xmlResult = resStream.ReadToEnd();

            //Load xmlResult variable value into xml documnet
            doc.LoadXml(xmlResult);

            try
            {
                //Get specified element value using select single node method and verify it return OK (success ) or failed
                if (doc.DocumentElement.SelectSingleNode("/DistanceMatrixResponse/row/element/status").InnerText.ToString().ToUpper() != "OK")
                {
                    result = -2.0m; //Invalid address please try again;
                    return result;
                }

                //Get DistanceMatrixResponse element and its values
                XmlNodeList xnList = doc.SelectNodes("/DistanceMatrixResponse");
                foreach (XmlNode xn in xnList)
                {
                    if (xn["status"].InnerText.ToString() == "OK")
                    { //finally bind it in the result label control
                        approxTime = doc.DocumentElement.SelectSingleNode("/DistanceMatrixResponse/row/element/duration/value").InnerText;
                        return Helper.ConvertKMtoMiles(doc.DocumentElement.SelectSingleNode("/DistanceMatrixResponse/row/element/distance/text").InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                return -3.0m; //Error during processing;
            }
            return result;
        }
    }
}
