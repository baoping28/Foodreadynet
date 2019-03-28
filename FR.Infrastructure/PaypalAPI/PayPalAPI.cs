using System;
using com.paypal.sdk.services;

namespace FR.Infrastructure.PaypalAPI
{
    public class PayPalAPI
    {
        public PayPalAPI()
        {
        }

        public static NVPCallerServices PayPalAPIInitialize()
        {
            NVPCallerServices caller = new NVPCallerServices();
            caller.APIProfile = SetProfile.SessionProfile;
            return caller;

        }

    }
}