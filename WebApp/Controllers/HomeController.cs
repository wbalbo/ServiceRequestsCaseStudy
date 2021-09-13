using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceRequests.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        public ViewResult AddServiceRequest() => View();

        [HttpPost]
        public async Task<IActionResult> AddServiceRequest(ServiceRequestModel model)
        {
            model = SetDefaultValues(model);

            ServiceRequestModel modelFromApi = new ServiceRequestModel();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
 
                using (var response = await httpClient.PostAsync("http://localhost:33710/api/servicerequest", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    modelFromApi = JsonConvert.DeserializeObject<ServiceRequestModel>(apiResponse);
                }
            }
            return View(modelFromApi);
        }

        public async Task<IActionResult> GetServiceRequest(Guid id)
        {
            ServiceRequestModel serviceRequestModel;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:33710/api/servicerequest/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    serviceRequestModel = JsonConvert.DeserializeObject<ServiceRequestModel>(apiResponse);
                }
            }

            return View(serviceRequestModel);
        }

        private ServiceRequestModel SetDefaultValues(ServiceRequestModel model)
        {
            model.Id = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            model.Status = ServiceRequestModel.CurrentStatus.Created;

            return model;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:33710/api/servicerequest/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
