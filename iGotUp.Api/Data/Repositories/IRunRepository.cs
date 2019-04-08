using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Maps.Geocoding;
using Google.Maps.Places;
using iGotUp.Api.Model;

namespace iGotUp.Api.Data.Repositories
{
    public interface IRunRepository
    {
        Task<List<RootObject>> GetLocationResults(double lat, double lng, string radius);

        IEnumerable<RootObject> GetLocationResults(string address1, string address2, string state, string zip);
    }
}