using GrpcServiceShipTelemetry.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceShipTelemetry.Infraestructure.Repository
{
    public interface ITelemetryRepository
    {
        void AddTelemetry(Telemetry telemetry);
        Telemetry GetTelemetry(string shipId);
    }
}
