using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ServiceRequests.Models;
using System.Threading.Tasks;

namespace ServiceRequests.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public ServiceRequestController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public ActionResult<ServiceRequestModel> GetAll()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {

        }
    }
}
