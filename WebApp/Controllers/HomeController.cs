using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceRequests.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<ServiceRequestModel> serviceRequestsList = new List<ServiceRequestModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:33710/api/servicerequest"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    serviceRequestsList = JsonConvert.DeserializeObject<List<ServiceRequestModel>>(apiResponse);
                }   
            }

            return View(serviceRequestsList);
        }
    }
}
