using GrpcServiceShipTelemetry.Domain.Interfaces;
using GrpcServiceShipTelemetry.Domain.Models;
using GrpcServiceShipTelemetry.Infraestructure.Data;
using System.Data.SqlClient;

namespace GrpcServiceShipTelemetry.Infraestructure.Repository
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly string _connectionString;
        private readonly ApplicationDbContext _bd;
        public TelemetryRepository(ApplicationDbContext bd) 
        {
            _bd = bd;
        }

 

        public void AddTelemetry(Telemetry telemetry)
        {
            // Lógica para añadir datos de telemetría a la base de datos
        }

        public async Task<Telemetry> GetTelemetryAsync(string shipId)
        {
            var query = "SELECT * FROM Telemetry WHERE ShipId = @shipId";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@shipId", shipId);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
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

            // Retorna null si no se encuentran datos
            return null;
        }
    }
}
