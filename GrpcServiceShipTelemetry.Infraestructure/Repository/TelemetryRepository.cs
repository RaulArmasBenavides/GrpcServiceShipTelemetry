﻿using GrpcServiceShipTelemetry.Domain.Interfaces;
using GrpcServiceShipTelemetry.Domain.Models;
using GrpcServiceShipTelemetry.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace GrpcServiceShipTelemetry.Infrastructure.Repository
{
    public class TelemetryRepository : ITelemetryRepository
    {
 
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
            var _connectionString = _bd.GetConnectionString();
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
                    };
                }
            }

            // Retorna null si no se encuentran datos
            return null;
        }
    }
}
