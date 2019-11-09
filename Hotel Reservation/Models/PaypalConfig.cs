using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel_Reservation.Models
{
    public class PaypalConfig
    {  
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        // Constructor  
        static PaypalConfig()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }
        // Getting properties from the web.config  
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }
        // Getting accesstocken from paypal
        private static string GetAccessToken()
        {  
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        // Return apicontext object by invoking it with the accesstoken
        public static APIContext GetAPIContext()
        {  
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}