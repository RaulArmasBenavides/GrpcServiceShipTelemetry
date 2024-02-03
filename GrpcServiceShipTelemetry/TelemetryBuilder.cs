using GrpcServiceShipTelemetry.Domain;

namespace GrpcServiceShipTelemetry
{
    public class TelemetryBuilder
    {
        private readonly TelemetryData _telemetryData;

        public TelemetryBuilder(TelemetryData telemetryData)
        {
            _telemetryData = telemetryData;
        }

        public Telemetry Build()
        {
            return new Telemetry
            {
                ShipId = _telemetryData.ShipId,
                Latitude = _telemetryData.Latitude,
                Longitude = _telemetryData.Longitude,
                Speed = _telemetryData.Speed
                // Asumiendo que estas propiedades existen en ambas clases
                // Agrega aquí cualquier otra propiedad necesaria
            };
        }
    }
}
