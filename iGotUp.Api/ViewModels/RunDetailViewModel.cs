using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace iGotUp.Api.ViewModels
{
    public class RunDetailViewModel
    {
        public string name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string game_name { get; set; }
        public IEnumerable<UserViewModel> players { get; set; }
    }
}