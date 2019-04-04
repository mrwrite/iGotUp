using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace iGotUp.Api.Data.Entities
{
    public class GotUpUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Alias { get; set; }

        public bool initialLogin { get; set; }
    }
}