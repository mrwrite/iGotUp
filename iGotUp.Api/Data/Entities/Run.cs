using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iGotUp.Api.Data.Entities
{
    public class Run
    {
        public int id { get; set; }
        public int game_type_id { get; set; }
        public int location_id { get; set; }
        public string day { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public bool is_active { get; set; }
        public bool is_membership_required { get; set; }
    }
}