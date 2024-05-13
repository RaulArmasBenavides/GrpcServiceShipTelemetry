namespace GrpcServiceShipTelemetry.Domain.Models
{
    public class Telemetry
    {
        public string ShipId { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }

    }
}
