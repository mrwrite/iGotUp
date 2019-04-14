using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Maps.Geocoding;
using Google.Maps.Places;
using iGotUp.Api.Data.Entities;
using iGotUp.Api.Model;
using iGotUp.Api.ViewModels;

namespace iGotUp.Api.Data.Repositories
{
    public interface IRunRepository
    {
        Task<List<RootObject>> GetLocationResults(double lat, double lng, string radius);

        IEnumerable<RootObject> GetLocationResults(string address1, string address2, string state, string zip);

        void addRun(Run run);

        void reserveRun(int user_id, int run_id);

        IEnumerable<PickUpViewModel> get_pickup_games();

        RunDetailViewModel get_run_detail(int run_id);
    }
}