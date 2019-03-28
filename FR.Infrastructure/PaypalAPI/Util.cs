using System;
using System.Collections;
using com.paypal.sdk.util;

namespace FR.Infrastructure.PaypalAPI
{
    public class Util
    {
        public static string BuildResponse(object response, string header1, string header2)
        {
            if (response != null)
            {
                NVPCodec decoder = new NVPCodec();
                decoder = (NVPCodec)response;



                string res = "<center>";
                if (!string.IsNullOrEmpty(header1))
                    res = res + "<font size=2 color=black face=Verdana><b>" + header1 + "</b></font>";
                res = res + "<br>";
                res = res + "<br>";

                if (!string.IsNullOrEmpty(header2))
                    res = res + "<b>" + header2 + "</b><br>";

                res = res + "<br>";

                res = res + "<table width=400 class=api>";


                for (int i = 0; i < decoder.Keys.Count; i++)
                {
                    res = res + "<tr><td class=field> " + decoder.Keys[i].ToString() + ":</td>";
                    res = res + "<td>" + decoder.GetValues(i)[0] + "</td>";
                    res = res + "</tr>";
                    res = res + "<tr>";
                }

                res = res + "</table>";
                res = res + "</center>";
                return res;
            }
            else
            {
                return "Requested action not allowed";
            }

        }
    }
}
