using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FR.Infrastructure.PaypalAPI
{
    public class Constants
    {
        /// <summary>
        /// Modify these values if you want to use your own profile.
        /// </summary>

        /* 
         *                                                                         *
         * WARNING: Do not embed plaintext credentials in your application code.   *
         * Doing so is insecure and against best practices.                        *
         *                                                                         *
         * Your API credentials must be handled securely. Please consider          *
         * encrypting them for use in any production environment, and ensure       *
         * that only authorized individuals may view or modify them.               *
         *                                                                         *
         */




        public const string ENVIRONMENT = "sandbox";
        public const string PAYPAL_URL = "https://www.sandbox.paypal.com";
        public const string ECURLLOGIN = "https://developer.paypal.com";
        public const string SUBJECT = "";


        public const string PROFILE_KEY = "Profile";
        public const string PAYMENT_ACTION_PARAM = "paymentAction";
        public const string PAYMENT_TYPE_PARAM = "paymentType";


        public const string STANDARD_EMAIL_ADDRESS = "sdk-seller@sdk.com";
        public const string WEBSCR_URL = PAYPAL_URL + "/cgi-bin/webscr";

        //Permission
        public const string OAUTH_SIGNATURE = "";
        public const string OAUTH_TOKEN = "";
        public const string OAUTH_TIMESTAMP = "";

    }
}
