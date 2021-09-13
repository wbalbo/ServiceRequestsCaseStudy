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
        private readonly IMemoryCache _cache;

        public ServiceRequestRepository(IMemoryCache cache)
        {
            _cache = cache;

            if (_data != null)
                return;

            _data = new List<ServiceRequestModel>();
        }

        public List<ServiceRequestModel> GetAll()
        {
            var serviceRequests = _cache.GetOrCreate("ServiceRequests_GetAll", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(120); // time to cache to be inactive (2 minutes)
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300); // time to cache to expire, from now (5 minutes)
                entry.SetPriority(CacheItemPriority.High);

                return _data.Where(d=> d.Status != ServiceRequestModel.CurrentStatus.Canceled).ToList();
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

                return _data.Where(d => d.Id == id && d.Status != ServiceRequestModel.CurrentStatus.Canceled);
            });

            return serviceRequest;
        }

        public bool CreateServiceRequest(ServiceRequestModel model)
        {
            int _listCount = _data.Count;            

            _data.Add(model);

            return _data.Count > _listCount;
        }

        public bool DeleteServiceRequest(Guid id)
        {
            var model = _data.Find(f => f.Id == id);

            if (model != null)
            {
                _data.Remove(model);

                model.Status = ServiceRequestModel.CurrentStatus.Canceled;
                _data.Add(model);
                return true;
            }
            else
                return false;
        }

        public bool UpdateServiceRequest(Guid id)
        {
            var model = _data.Find(f => f.Id == id);

            if (model != null)
            {
                _data.Remove(model);

                model.Status = ServiceRequestModel.CurrentStatus.InProgress;
                _data.Add(model);
                return true;
            }
            else
                return false;
        }
    }
}
