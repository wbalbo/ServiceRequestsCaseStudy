using Bogus;
using Microsoft.Extensions.Caching.Distributed;
using ServiceRequests.Models;
using System.Collections.Generic;

namespace ServiceRequests.Repositories
{
    public class ServiceRequestRepository
    {
        private static List<ServiceRequestModel> _data;
        private readonly Faker _faker;
        private readonly IDistributedCache _cache;

        public ServiceRequestRepository(IDistributedCache cache)
        {
            _cache = cache;

            if (_data != null) 
                return;

            _faker = new Faker();
            _data = new List<ServiceRequestModel>();

            for (int i = 0; i < 10; i++)
            {
                _data.Add(AddFakeData());
            }
        }

        private ServiceRequestModel AddFakeData()
        {
            return new ServiceRequestModel()
            {
                BuildingCode = _faker.Company.CompanyName("name.firstName"),
                Description = _faker.Rant.ToString(),
                Status = ServiceRequestModel.CurrentStatus.Created,
                CreatedBy = _faker.Person.FirstName,
                CreatedDate = _faker.Date.Recent(),
                LastModifiedBy = _faker.Person.FirstName,
                LastModifiedDate = _faker.Date.Recent()
            };
        }
    }
}
