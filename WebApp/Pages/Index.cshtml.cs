using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ServiceRequests.Controllers;
using ServiceRequests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            ServiceRequestController cont = new ServiceRequests.Controllers.ServiceRequestController(cache);
            ActionResult<List<ServiceRequestModel>> result = cont.GetAll();
            if (((ObjectResult)result.Result).StatusCode.Value == 200)
            {
                if (((ObjectResult)result.Result).Value is List<ServiceRequestModel>)
                {
                    List<ServiceRequestModel> list = (List<ServiceRequestModel>)((ObjectResult)result.Result).Value;
                    foreach (ServiceRequestModel item in list)
                    {
                        
                    }
                }
            }


            if (result.Value.Count > 0)
            {

            }
        }
    }
}
