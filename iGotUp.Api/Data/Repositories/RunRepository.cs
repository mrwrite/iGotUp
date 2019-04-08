using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Google.Maps;
using Google.Maps.Geocoding;
using Google.Maps.Places;
using Google.Maps.Places.Details;
using iGotUp.Api.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace iGotUp.Api.Data.Repositories
{
    public class RunRepository : IRunRepository
    {
        private readonly IConfiguration configuration;
        private readonly GotUpContext ctx;
        private string next_token;

        public RunRepository(IConfiguration configuration, GotUpContext ctx)
        {
            this.configuration = configuration;
            this.ctx = ctx;
        }

        public async Task<List<RootObject>> GetLocationResults(double lat, double lng, string radius)
        {
            List<RootObject> results_to_return = new List<RootObject>();
            var key = this.configuration["GOOGLE_MAP_API_KEY"];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://maps.googleapis.com");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var requestUri =
                $"maps/api/place/nearbysearch/json?location={lat},{lng}&radius={radius}&types=gym&name=basketball&key={key}";
            var response = await client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsAsync<PlaceResult>();

                var locations = this.ctx.Locations;
                next_token = results.next_page_token;
                results_to_return.AddRange(results.results);
                await Task.Delay(1350);

                while (next_token != null && next_token != "")
                {
                    var nextRequestUri = $"maps/api/place/nearbysearch/json?location={lat},{lng}&radius={radius}&types=gym&name=basketball&key={key}&pagetoken={next_token}";
                    var next_response = await client.GetAsync(nextRequestUri);
                    if (next_response.IsSuccessStatusCode)
                    {
                        var next_results = await next_response.Content.ReadAsAsync<PlaceResult>();
                        results_to_return.AddRange(next_results.results);
                        next_token = next_results.next_page_token;
                    }
                }

                next_token = null;
                return results_to_return.Where(x => locations.Any(y => y.address1 == x.vicinity.Substring(0, x.vicinity.IndexOf(",")))).ToList();
            }

            return null;
        }

        public IEnumerable<RootObject> GetLocationResults(string address1, string address2, string state, string zip)
        {
            throw new NotImplementedException();
        }
    }
}