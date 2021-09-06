using Bogus;
using Microsoft.Extensions.Caching.Memory;
using ServiceRequests.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceRequests.Repositories
{
    public class ServiceRequestRepository
    {
        private static List<ServiceRequestModel> _data;
        private readonly Faker _faker;
        private readonly IMemoryCache _cache;

        public ServiceRequestRepository(IMemoryCache cache)
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
                Id = Guid.NewGuid(),
                BuildingCode = _faker.Company.CompanyName("{{name.firstName}}"),
                Description = _faker.Company.CatchPhrase(),
                Status = ServiceRequestModel.CurrentStatus.Created,
                CreatedBy = _faker.Person.FirstName,
                CreatedDate = _faker.Date.Recent(),
                LastModifiedBy = _faker.Person.LastName,
                LastModifiedDate = DateTime.Now
            };
        }

        public List<ServiceRequestModel> GetAll()
        {
            var serviceRequests = _cache.GetOrCreate("ServiceRequests_GetAll", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(120); // time to cache to be inactive (2 minutes)
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300); // time to cache to expire, from now (5 minutes)
                entry.SetPriority(CacheItemPriority.High);

                return _data;
            });

            return serviceRequests;
        }

        public IEnumerable<ServiceRequestModel> GetById(Guid id)
        {
            var serviceRequest = _cache.GetOrCreate("ServiceRequests_GetById_" + id, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(120);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300);
                entry.SetPriority(CacheItemPriority.High);

                return _data.Where(d => d.Id == id);
            });

            return serviceRequest;
        }

        public void CreateServiceRequest(ServiceRequestModel model)
        {
            _data.Add(model);
        }

        public void DeleteServiceRequest(Guid id)
        {
            var model = _data.Find(f => f.Id == id);

            if (model != null)
            {
                _data.Remove(model);

                model.Status = ServiceRequestModel.CurrentStatus.Canceled;
                _data.Add(model);
            }
        }

        public void UpdateServiceRequest(Guid id)
        {
            var model = _data.Find(f => f.Id == id);

            if (model != null)
            {
                _data.Remove(model);

                model.Status = ServiceRequestModel.CurrentStatus.InProgress;
                _data.Add(model);
            }
        }
    }
}
