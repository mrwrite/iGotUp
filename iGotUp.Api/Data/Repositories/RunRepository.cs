using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Google.Maps;
using Google.Maps.Geocoding;
using Google.Maps.Places;
using Google.Maps.Places.Details;
using iGotUp.Api.Data.Entities;
using iGotUp.Api.Model;
using iGotUp.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace iGotUp.Api.Data.Repositories
{
    public class RunRepository : IRunRepository
    {
        private readonly IConfiguration configuration;
        private readonly GotUpContext ctx;
        private readonly ILogger<IRunRepository> logger;
        private string next_token;
        private SqlConnection sqlConnection;

        public RunRepository(IConfiguration configuration, GotUpContext ctx, ILogger<IRunRepository> logger)
        {
            this.configuration = configuration;
            this.ctx = ctx;
            this.logger = logger;
            sqlConnection = new SqlConnection(this.configuration.GetConnectionString("iGotUpConnectionString"));
        }

        public void addRun(Run run)
        {
            run.is_active = true;

            if (this.ctx.Runs.Any(x => x.location_id == run.location_id && x.game_type_id == run.game_type_id && x.start_time == run.start_time && x.end_time == run.end_time))
            {
            }
            else
            {
                this.ctx.Runs.Add(run);
            }
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

        public RunDetailViewModel get_run_detail(int run_id)
        {
            var jsonResult = new StringBuilder();

            try
            {
                using (SqlConnection dbConnection = sqlConnection)
                {
                    dbConnection.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.get_run_details", dbConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@run_id", SqlDbType.Int);
                        cmd.Parameters["@run_id"].Value = run_id;
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            jsonResult.Append(reader.GetValue(0).ToString());
                        }

                        return JsonConvert.DeserializeObject<RunDetailViewModel>(jsonResult.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<PickUpViewModel> get_pickup_games()
        {
            var jsonResult = new StringBuilder();

            try
            {
                using (SqlConnection dbConnection = sqlConnection)
                {
                    dbConnection.Open();

                    using (SqlCommand cmd = new SqlCommand("dbo.get_pickup_games", dbConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var reader = cmd.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            jsonResult.Append("[]");
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                jsonResult.Append(reader.GetValue(0).ToString());
                            }
                        }

                        return JsonConvert.DeserializeObject<IEnumerable<PickUpViewModel>>(jsonResult.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                this.logger.LogError($"Failed to get Pickup Games: {e}");
                return null;
            }
        }

        public void reserveRun(int user_id, int run_id)
        {
            var userRun = new RunPlayers();
            userRun.run_id = run_id;
            userRun.user_id = user_id;

            if (!this.ctx.RunPlayers.Any(x => x.run_id == userRun.run_id && x.user_id == userRun.user_id))
            {
                this.ctx.RunPlayers.Add(userRun);
            }
        }
    }
}