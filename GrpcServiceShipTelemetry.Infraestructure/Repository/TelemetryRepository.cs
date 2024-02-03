using GrpcServiceShipTelemetry.Domain;
using System.Data.SqlClient;

namespace GrpcServiceShipTelemetry.Infraestructure.Repository
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly string _connectionString;

        public TelemetryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddTelemetry(Telemetry telemetry)
        {
            // Lógica para añadir datos de telemetría a la base de datos
        }

        public Telemetry GetTelemetry(string shipId)
        {
            // Aquí iría la lógica para consultar la base de datos
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Telemetry WHERE ShipId = '{shipId}'", connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Telemetry
                        {
                            ShipId = shipId,
                            Latitude = (double)reader["Latitude"],
                            Longitude = (double)reader["Longitude"],
                            Speed = (double)reader["Speed"]
                            // ... otros campos
                        };
                    }
                }
            }

            // Retorna null o maneja la situación en la que no se encuentran datos
            return null;
        }
    }
}
