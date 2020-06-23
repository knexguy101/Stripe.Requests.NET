# Stripe.Requests.NET


Stripe.Requests.NET provides a full auto checkout module for Stripe pages. All that is needed is the checkout page html, checkout token, and the profile for the data to enter at checkout.

Why use this?
  - Gives a request based response with more data, great for debugging/logging.
  - Stripe request based checkout will ignore extra fields, sort of a one-does-all solution
  - HttpClient supports proxies and cookies, and is used within this module.

If I dont have a solution for CF and loading JS, why use this, I can just autofill?
 - As stated above, extra fields on requests will be ignored, meaning that the module will work on any site containing stripe and no extra auto fill code has to be written for the browser to perform.

# Warning
Stripe checkout has cloudflare protection to load as well as JS detection, this module does not support this and is intended to assist browser that can get past these issues.

# Use Cases
 - Listening for stripe checkout pages on a PuppeteerSharp browser or Selenium Browser and automatically checking out.


# Example
```cs
    class Program
    {
        static void Main(string[] args)
        {
            //Create our httpclient
            CookieContainer cookiejar = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                CookieContainer = cookiejar,
                Proxy = null
            };
            HttpClient client = new HttpClient(handler);

            //new html doc
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml("checkout html here");

            //get our items
            var list = StripeHelpers.IFrame.Parse(doc);
            var items = StripeHelpers.IFrame.GetItems(list);
            var cs_live = "cs_live_token";

            //init new checkout client
            var stripeCheckout = new StripeFullCheckout(client, items, new Profile()
            {
                State = "CA",
                Address1 = "test address",
                AddressCountry = "US",
                CCNum = "1234123412341234",
                City = "test",
                CVV = "101",
                Email = "testing@gmail.com",
                ExpMonth = "04",
                ExpYear = "26",
                Name = "test name",
                Title = "test proifile",
                Zip = "12345"
            }, cs_live);

            //get checkout response
            var res = stripeCheckout.SubmitCheckout();
        }
    }
```

