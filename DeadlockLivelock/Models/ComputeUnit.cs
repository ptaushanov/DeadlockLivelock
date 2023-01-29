using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.Models
{
    public class ComputeUnit
    {
        public int? ComputeUnitId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Payload { get; set; }
        public string UUID { get;  set; }

        public ComputeUnit(int? computeUnitId, string name, string status, string payload, string uUID)
        {
            ComputeUnitId = computeUnitId;
            Name = name;
            Status = status;
            Payload = payload;
            UUID = uUID;
        }

        public ComputeUnit(int? computeUnitId, string name, string status, string payload)
        {
            ComputeUnitId = computeUnitId;
            Name = name;
            Status = status;
            Payload = payload;
            UUID = Guid.NewGuid().ToString();
        }

        public ComputeUnit(int? computeUnitId, string name, string status)
        {
            ComputeUnitId = computeUnitId;
            Name = name;
            Status = status;
            Payload = "";
            UUID = Guid.NewGuid().ToString();
        }

    }
}
