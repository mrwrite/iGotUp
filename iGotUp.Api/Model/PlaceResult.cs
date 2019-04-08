using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Maps.Geocoding;

namespace iGotUp.Api.Model
{
    public class PlaceResult
    {
        public string[] html_attributions { get; set; }
        public string next_page_token { get; set; }
        public IEnumerable<RootObject> results { get; set; }
        public string status { get; set; }
    }
}