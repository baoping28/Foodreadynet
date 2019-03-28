using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YelpSharp;
using System.Web.Configuration;

namespace FoodReady.WebUI.Configs
{
    public class Config
    {
        private static Options _options;

        public static Options Options
        {
            get
            {
                if (_options == null)
                {
                    // get all of the options out of EnvironmentSettings.  You can easily just put your own keys in here without
                    // doing the env dance, if you so choose
                    _options = new Options()
                    {
                        AccessToken =WebConfigurationManager.AppSettings["YELP_ACCESS_TOKEN"].ToString(),
                        AccessTokenSecret = WebConfigurationManager.AppSettings["YELP_ACCESS_TOKEN_SECRET"].ToString(),
                        ConsumerKey = WebConfigurationManager.AppSettings["YELP_CONSUMER_KEY"].ToString(),
                        ConsumerSecret = WebConfigurationManager.AppSettings["YELP_CONSUMER_SECRET"].ToString() 
                    };

                    if (String.IsNullOrEmpty(_options.AccessToken) ||
                        String.IsNullOrEmpty(_options.AccessTokenSecret) ||
                        String.IsNullOrEmpty(_options.ConsumerKey) ||
                        String.IsNullOrEmpty(_options.ConsumerSecret))
                    {
                        throw new InvalidOperationException("No OAuth info available.  Please modify Config.cs to add your YELP API OAuth keys");
                    }
                }
                return _options;
            }
        }
    }
}