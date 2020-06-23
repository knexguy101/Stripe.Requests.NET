using System;
using System.Collections.Generic;
using System.Text;

namespace Stripe.Requests.NET
{
    public class Profile
    {
        //TODO make items more specific, docs will show how to enter values for now
        public string Title { get; set; }
        public string CCNum { get; set; }
        public string CVV { get; set; }
        public string ExpMonth { get; set; } //01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12
        public string ExpYear { get; set; } //20, 21, 22, 23
        public string Name { get; set; } //full name
        public string Email { get; set; }
        public string AddressCountry { get; set; } //US
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; } //CA, CO, NY
        public string Zip { get; set; }
    }
}
