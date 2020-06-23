using System;
using System.Collections.Generic;
using System.Text;

namespace Stripe.Requests.NET
{
    public class StripeItems
    {
        public string ID { get; set; }
        public string PKLive { get; set; }
        public string GUID { get; set; }
        public string MUID { get; set; }
        public string SID { get; set; }
        public string PPage { get; set; }
        public string UserAgent { get; set; }

        private string GetRandomV3Name()
        {
            var rand = new Random();
            var items = "123456789abcdefABCDEF".ToCharArray();
            var temp = "";
            for(int x = 0; x < 8; x++)
            {
                temp += items[rand.Next(0, items.Length)].ToString();
            }
            return temp;
        }

        public void GenerateRandomUserAgent()
        {
            var v3name = GetRandomV3Name();
            UserAgent = $"stripe.js%2F{v3name}%3B+stripe-js-v3%2F{v3name}%3B+checkout";
        }
    }
}
