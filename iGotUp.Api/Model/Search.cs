using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iGotUp.Api.Model
{
    public class Search
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public string radius { get; set; }
    }
}