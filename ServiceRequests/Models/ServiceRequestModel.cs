using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceRequests.Models
{
    public class ServiceRequestModel
    {
        public enum CurrentStatus
        {
            NotApplicable,
            Created,
            InProgress,
            Complete,
            Canceled
        }

        public Guid Id { get; private set; } = new Guid();
        public string BuildingCode { get; set; }
        public string Description { get; set; }
        public CurrentStatus Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
