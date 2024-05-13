using GrpcServiceShipTelemetry.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceShipTelemetry.Domain.Interfaces
{
    public interface ITelemetryRepository
    {
        void AddTelemetry(Telemetry telemetry);
        Task<Telemetry> GetTelemetryAsync(string shipId);
    }
}
