using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SafeAdmin.Data;
using SafeAdmin.Model;
using SafeAdmin.Utility;
using System;
using System.Threading.Tasks;

namespace SafeAdmin.Services
{

    public interface IFindLocation
    {
        decimal[] FindAdressLocation(string country, string state, string locality, string zip, string address);
        decimal[] FindCityLocation(string state, string city);
    }

    public class FindLocation : IFindLocation
    {
        private readonly ILogger<SendMail> _logger;
        private readonly SafeContext _context;
        private readonly IOptions<AppConfig> _config;

        public FindLocation(ILogger<SendMail> logger, SafeContext context, IOptions<AppConfig> config)
        {
            _logger = logger;
            _context = context;
            _config = config;
        }

        public decimal[] FindAdressLocation(string country,string state, string locality, string zip, string address)
        {
            string[] parrams = new string[5] { country,state, locality, zip, address };
            string url = string.Format("http://dev.virtualearth.net/REST/v1/Locations?CountryRegion={0}&adminDistrict={1}&locality={2}&postalCode={3}&addressLine={4}&key=", parrams);
            var client = new RestClient(url+_config.Value.BingKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<BingLocationResult>(response.Content);
            if (jsonResponse!= null && jsonResponse.resourceSets.Length > 0 && jsonResponse.resourceSets[0].resources. Length >0)
            {
                return new decimal[2] { (decimal)jsonResponse.resourceSets[0].resources[0].point.coordinates[0], (decimal)jsonResponse.resourceSets[0].resources[0].point.coordinates[1] };
            }
            else
            {
                return new decimal[2] { 0, 0 };
            }
            
        }

        public decimal[] FindCityLocation(string state, string city)
        {
            string[] parrams = new string[2] {state, city};
            string url = string.Format("http://dev.virtualearth.net/REST/v1/Locations/US/{0}/{1}?o=json&key=", parrams);
            var client = new RestClient(url + _config.Value.BingKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<BingLocationResult>(response.Content);
            if (jsonResponse != null && jsonResponse.resourceSets.Length > 0 && jsonResponse.resourceSets[0].resources.Length > 0)
            {
                return new decimal[2] { (decimal)jsonResponse.resourceSets[0].resources[0].point.coordinates[0], (decimal)jsonResponse.resourceSets[0].resources[0].point.coordinates[1] };
            }
            else
            {
                return new decimal[2] { 0, 0 };
            }
            
        }

        private static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }


    }
}
