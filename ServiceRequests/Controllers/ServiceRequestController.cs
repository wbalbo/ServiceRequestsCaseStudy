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

            if (requests.Count == 0)
                return NoContent();
            
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public ActionResult<ServiceRequestModel> GetById(Guid id)
        {
            IEnumerable<ServiceRequestModel> requests = new ServiceRequestRepository(_memoryCache).GetById(id);

            if (requests == null)
                return NotFound();

            return Ok(requests);
        }

        [HttpPost]
        public ActionResult CreateServiceRequest(ServiceRequestModel model)
        {
            var requests = new ServiceRequestRepository(_memoryCache);
            try
            {                
                bool wasCreated = requests.CreateServiceRequest(model);

                if (!wasCreated)
                    return BadRequest();
            }
            catch
            {
                return BadRequest();
            }

            return Created("New service request", model.Id);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateServiceRequest(Guid id)
        {
            var requests = new ServiceRequestRepository(_memoryCache);
            try
            {
                bool wasUpdated = requests.UpdateServiceRequest(id);

                if (!wasUpdated)
                    return NotFound();
            }
            catch
            {
                return BadRequest();
            }

            return Ok(requests);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteServiceRequest(Guid id)
        {
            var requests = new ServiceRequestRepository(_memoryCache);
            bool wasDeleted = requests.DeleteServiceRequest(id);

            if (!wasDeleted)
                return NotFound();

            return Created("Service request removed", id);
        }
    }
}
