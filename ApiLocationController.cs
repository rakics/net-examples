using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using SafeAdmin.Model.DTOModels;
using SafeAdmin.Model.ZipCodesModels;
using SafeAdmin.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeAdmin.Controllers
{
    [Route("location")]
    [Produces("application/json")]
    public class ApiLocationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOptions<AppConfig> _config;

        public ApiLocationController(IMapper mapper, IOptions<AppConfig> config)
        {
            _mapper = mapper;
            _config = config;
        }

        [Route("location")]
        [HttpGet]
        public IEnumerable<AutocompleteResult> GetLocations(string search)
        {
            var client = new RestClient("https://www.zip-codes.com/content/site-search/site-search-suggestions.asp?q=" + search);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<ZipCodesAutocompleteResult[]>(response.Content);
            var autoResults = _mapper.Map<ZipCodesAutocompleteResult[], AutocompleteResult[]>(jsonResponse);
            return autoResults.ToList();
        }

        [Route("GetCreditInfo")]
        [HttpGet]
        public CreditInfo GetGetCreditInfo()
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/GetCreditInfo?key=" + _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<CreditInfo>(response.Content);
            return jsonResponse;
        }

        [Route("QuickGetZipCodeDetails")]
        [HttpGet]
        public QuickZipInfo QuickGetZipCodeDetails(string zip)
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/QuickGetZipCodeDetails/"+zip+"?key=" + _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<QuickZipInfo>(response.Content);
            return jsonResponse;
        }

        [Route("GetZipCodeDetails")]
        [HttpGet]
        public ZipInfo GetZipCodeDetails(string zip)
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/GetZipCodeDetails/" + zip + "?key=" + _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<ZipInfo>(response.Content);
            return jsonResponse;
        }

        [Route("FindZipCodesInRadius")]
        [HttpGet]
        public ZipCodesInRadius FindZipCodesInRadius(string zip, string max_radius, string min_radius)
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/FindZipCodesInRadius?zipcode=" + zip + "&maximumradius="+ max_radius + "&minimumradius="+ min_radius + "&key=" + _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<ZipCodesInRadius>(response.Content);
            return jsonResponse;
        }

        [Route("FindZipCodesInRadiusLatLong")]
        [HttpGet]
        public ZipCodesInRadius FindZipCodesInRadiusLatLong(string lat, string @long, string max_radius, string min_radius)
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/FindZipCodesInRadius/ByLatLon?latitude="+lat+"&longitude="+@long+"&maximumradius=" + max_radius + "&minimumradius=" + min_radius + "&key=" + _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<ZipCodesInRadius>(response.Content);
            return jsonResponse;
        }

        [Route("CalculateDistance")]
        [HttpGet]
        public DistanceCalculation CalculateDistance(string fromlatitude, string fromlongitude, string tolatitude, string tolongitude)
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/CalculateDistance?fromlat="+fromlatitude+"&fromlon="+fromlongitude+"&tolat="+tolatitude+"&tolon="+tolongitude+"&key=" + _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<DistanceCalculation>(response.Content);
            return jsonResponse;
        }

        [Route("CalculateDistanceByZip")]
        [HttpGet]
        public DistanceCalculationZip CalculateDistanceByZip(string fromzipcode, string tozipcode)
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/CalculateDistance/ByZip?fromzipcode="+fromzipcode+"&tozipcode="+tozipcode+"&key=" + _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<DistanceCalculationZip>(response.Content);
            return jsonResponse;
        }

        [Route("CalculateDistanceZipToLatLon")]
        [HttpGet]
        public DistanceCalculationZip CalculateDistanceZipToLatLon(string fromzipcode, string tolatitude, string tolongitude)
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/CalculateDistance/ZipToLatLon?fromzipcode=" + fromzipcode + "&tolat=" + tolatitude + "&tolon=" + tolongitude + "&key=" + _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<DistanceCalculationZip>(response.Content);
            return jsonResponse;
        }

        [Route("GetZipCodes")]
        [HttpGet]
        public string[] GetZipCodes(string country, string state)
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/GetAllZipCodes?country="+country+"&state="+state+"&key=" + _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<string[]>(response.Content);
            return jsonResponse;
        }

        [Route("ZipCodeOfAddress")]
        [HttpGet]
        public ZipOfAddress ZipCodeOfAddress(string address, string address1, string city, string state,string zip)
        {
            var client = new RestClient("http://api.zip-codes.com/ZipCodesAPI.svc/1.0/ZipCodeOfAddress?address="+address+"&address1="+address1+"&city="+city+"&state="+state+"&zipcode="+zip+"&key="+ _config.Value.ZipCodesKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<ZipOfAddress>(response.Content);
            return jsonResponse;
        }

        

        private static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
