using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stripe.Requests.NET
{
    public class StripeHelpers
    {
        public static class IFrame
        {
            private static Dictionary<string, string> GetDict(string[] List)
            {
                var x = new Dictionary<string, string>();
                foreach (var url in List)
                {
                    x.Add(url.Split('=')[0], url.Split('=')[1]);
                }
                return x;
            }

            /// <summary>
            /// Parses an item dictionary for GetItems Function
            /// </summary>
            /// <param name="Doc">The HTML Document of the checkout page, please load checkout/page url page content.</param>
            /// <returns></returns>
            public static Dictionary<string, string> Parse(HtmlDocument Doc)
            {
                var iframes = Doc.DocumentNode.Descendants("iframe").Where(a => a.GetAttributeValue("src", "").Contains("sid") && a.GetAttributeValue("src", "").Contains("guid") && a.GetAttributeValue("src", "").Contains("muid")).ToArray();
                if (iframes.Count() <= 0)
                    return new Dictionary<string, string>();

                var iframe = iframes[0];
                var url = iframe.GetAttributeValue("src", "");
                var parts = url.Split('#')[1].Split('&');
                return GetDict(parts);
            }

            /// <summary>
            /// Returns a StripeItems body needed to checkout
            /// </summary>
            /// <param name="Items">Parse items</param>
            /// <param name="UA">The useragent needed to send requests, leaving null will result in a randomly generated User Agent</param>
            /// <returns></returns>
            public static StripeItems GetItems(Dictionary<string, string> Items, string UA = null)
            {
                var items = new StripeItems()
                {
                    GUID = Items["amp;mids[guid]"],
                    PKLive = Items["authentication[apiKey]"],
                    MUID = Items["amp;mids[muid]"],
                    SID = Items["amp;mids[muid]"]
                };
                if (UA == null)
                    items.GenerateRandomUserAgent();
                else
                    items.UserAgent = UA;
                return items;
            }
        }
    }
}
