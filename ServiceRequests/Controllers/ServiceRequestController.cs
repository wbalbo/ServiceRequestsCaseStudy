using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ServiceRequests.Models;
using ServiceRequests.Repositories;
using System;
using System.Collections.Generic;

namespace ServiceRequests.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ServiceRequestController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public ActionResult<List<ServiceRequestModel>> GetAll()
        {
            List<ServiceRequestModel> requests = new ServiceRequestRepository(_memoryCache).GetAll();
            
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public ActionResult<ServiceRequestModel> GetById(Guid id)
        {
            IEnumerable<ServiceRequestModel> requests = new ServiceRequestRepository(_memoryCache).GetById(id);
                        
            return Ok(requests);
        }

        [HttpPost]
        public ActionResult CreateServiceRequest(ServiceRequestModel model)
        {
            var requests = new ServiceRequestRepository(_memoryCache);
            requests.CreateServiceRequest(model);

            return Created("New service request", model);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateServiceRequest(Guid id)
        {
            var requests = new ServiceRequestRepository(_memoryCache);
            requests.UpdateServiceRequest(id);

            return Ok(requests);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteServiceRequest(Guid id)
        {
            var requests = new ServiceRequestRepository(_memoryCache);
            requests.DeleteServiceRequest(id);

            return Ok(requests);
        }
    }
}
