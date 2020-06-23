using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Stripe.Requests.NET
{
    public class StripeFullCheckout
    {
        private StripeItems _StripeItems { get; set; }
        private Profile _Profile { get; set; }
        private HttpClient _Client { get; set; }
        private string _CS_LIVE { get; set; }

        /// <summary>
        /// Init for a Full Page Stripe Checkout
        /// </summary>
        /// <param name="Client">HttpClient that will be used to send requests</param>
        /// <param name="SI">The Items used to send requests, use StripeHelpers.GetItems to create this</param>
        /// <param name="Profile">The data submitted at checkout</param>
        /// <param name="CS_LIVE">The checkout token, THIS IS GENERATED ON SITES AND IS DIFFERENT FOR EACH SITE</param>
        public StripeFullCheckout(HttpClient Client, StripeItems SI, Profile Profile, string CS_LIVE)
        {
            this._StripeItems = SI;
            this._Profile = Profile;
            this._Client = Client;
            this._CS_LIVE = CS_LIVE;
        }

        /// <summary>
        /// Submits the checkout of the StripeFullCheckout object
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage SubmitCheckout()
        {
            GetCheckoutID();
            GetPPage();
            return SubmitID();
        }

        /// <summary>
        /// Submits the order, using GetPPage and GetCheckoutID
        /// </summary>
        /// <returns></returns>
        private HttpResponseMessage SubmitID()
        {
            HttpRequestMessage x = new HttpRequestMessage(HttpMethod.Post, $"https://api.stripe.com/v1/payment_pages/{_StripeItems.PPage}/confirm");
            x.Headers.TryAddWithoutValidation("Accept", "application/json");
            x.Headers.TryAddWithoutValidation("Host", "api.stripe.com");
            x.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
            x.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");
            var dict = new Dictionary<string, string>();
            dict.Add("payment_method", _StripeItems.ID);
            dict.Add("key", _StripeItems.PKLive);
            x.Content = new FormUrlEncodedContent(dict);

            var res = _Client.SendAsync(x).Result;
            return res;
        }

        /// <summary>
        /// Gets the PPage, finalizing all variables to be submitted
        /// </summary>
        private void GetPPage()
        {
            HttpRequestMessage x = new HttpRequestMessage(HttpMethod.Get, $"https://api.stripe.com/v1/payment_pages?key={_StripeItems.PKLive}&guid={_StripeItems.GUID}&muid={_StripeItems.MUID}&sid={_StripeItems.SID}&session_id={_CS_LIVE}");
            x.Headers.TryAddWithoutValidation("Accept", "application/json");
            x.Headers.TryAddWithoutValidation("Host", "api.stripe.com");
            x.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
            x.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");

            var res = _Client.SendAsync(x).Result;
            var parseRes = JObject.Parse(res.Content.ReadAsStringAsync().Result);
            if (!parseRes.ContainsKey("id"))
                throw new Exception("No PPage found in GetPPage request");

            this._StripeItems.PPage = parseRes["id"].Value<string>();
        }

        /// <summary>
        /// Gets the checkout ID, creating a new checkout session
        /// </summary>
        private void GetCheckoutID()
        {
            HttpRequestMessage x = new HttpRequestMessage(HttpMethod.Post, "https://api.stripe.com/v1/payment_methods");
            x.Headers.TryAddWithoutValidation("Accept", "application/json");
            x.Headers.TryAddWithoutValidation("Host", "api.stripe.com");
            x.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
            x.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");
            var dict = new Dictionary<string, string>();
            dict.Add("type", "card");
            dict.Add("card[number]", _Profile.CCNum);
            dict.Add("card[cvc]", _Profile.CVV);
            dict.Add("card[exp_month]", _Profile.ExpMonth);
            dict.Add("card[exp_year]", _Profile.ExpYear);
            dict.Add("billing_details[name]", _Profile.Name);
            dict.Add("billing_details[email]", _Profile.Email);
            dict.Add("billing_details[address][country]", _Profile.AddressCountry);
            dict.Add("billing_details[address][line1]", _Profile.Address1);
            dict.Add("billing_details[address][city]", _Profile.City);
            dict.Add("billing_details[address][postal_code]", _Profile.Zip);
            dict.Add("billing_details[address][state]", _Profile.State);
            dict.Add("guid", _StripeItems.GUID);
            dict.Add("muid", _StripeItems.MUID);
            dict.Add("sid", _StripeItems.SID);
            dict.Add("key", _StripeItems.PKLive);
            dict.Add("payment_user_agent", _StripeItems.UserAgent);
            x.Content = new FormUrlEncodedContent(dict);

            var res = _Client.SendAsync(x).Result;
            var parseRes = JObject.Parse(res.Content.ReadAsStringAsync().Result);
            if (!parseRes.ContainsKey("id"))
                throw new Exception("No ID found in CheckoutID Request.");

            this._StripeItems.ID = parseRes["id"].Value<string>();
        }
    }
}
