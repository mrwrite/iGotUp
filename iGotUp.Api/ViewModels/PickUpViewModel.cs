using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iGotUp.Api.Data.Entities;

namespace iGotUp.Api.ViewModels
{
    public class PickUpViewModel
    {
        public string name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public IEnumerable<Run> runs { get; set; }
    }
}