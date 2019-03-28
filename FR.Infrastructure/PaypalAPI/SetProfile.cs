using System.Web;
using com.paypal.sdk.profiles;
using FR.Infrastructure.Helpers;

namespace FR.Infrastructure.PaypalAPI
{
    public class SetProfile
    {
        public const string PROFILE_KEY = "PayPalProfile";

        /// <summary>
        /// Required designer variable.
        /// </summary
        public static readonly IAPIProfile DefaultProfile = SetProfile.CreateAPIProfile(Globals.Settings.PayPalSettings.PayPalAPIUsername, Globals.Settings.PayPalSettings.PayPalAPIPassword, Globals.Settings.PayPalSettings.PayPalAPISignature, Globals.Settings.PayPalSettings.Environment);

        public static IAPIProfile CreateAPIProfile(string apiUsername, string apiPassword, string signature, string environment)
        {
            IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
            profile.APIUsername = apiUsername;
            profile.APIPassword = apiPassword;
            profile.APISignature = signature;
            profile.Environment = environment;
            return profile;

        }
        public static void SetDefaultProfile()
        {
            SessionProfile = DefaultProfile;
        }

        public static IAPIProfile SessionProfile
        {
            get
            {
                if (HttpContext.Current.Session[PROFILE_KEY] == null)
                {
                    HttpContext.Current.Session[PROFILE_KEY] = DefaultProfile;
                }
                return (IAPIProfile)HttpContext.Current.Session[PROFILE_KEY];
            }
            set
            {
                HttpContext.Current.Session[PROFILE_KEY] = value;
            }
        }
    }
}
