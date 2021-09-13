using Microsoft.Extensions.Caching.Memory;
using ServiceRequests.Controllers;
using ServiceRequests.Models;
using ServiceRequests.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class ApiTests
    {
        private readonly MemoryCacheOptions opt = new MemoryCacheOptions();
        private readonly IMemoryCache _memoryCache;

        public ApiTests()
        {
            _memoryCache = new MemoryCache(opt);
        }

        [Fact]
        public void GetAll_ShouldWork()
        {
            List<ServiceRequestModel> requests = new ServiceRequestRepository(_memoryCache).GetAll();
            Assert.Equal(new List<ServiceRequestModel>(), requests);
        }

        [Fact]
        public void GetById_ShouldWork()
        {
            Guid id = Guid.NewGuid();
            ServiceRequestModel request = new ServiceRequestRepository(_memoryCache).GetById(id);
            Assert.Null(request);
        }
    }
}
