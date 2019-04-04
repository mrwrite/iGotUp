using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iGotUp.Api.Data.Entities
{
    public class Location
    {
        public int id { get; set; }
        public string name { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string plus_code { get; set; }
    }
}