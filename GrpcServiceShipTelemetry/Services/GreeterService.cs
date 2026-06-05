using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceShipTelemetry;
using GrpcServiceShipTelemetry.Domain.Interfaces;
using GrpcServiceShipTelemetry.Domain.Models;
using GrpcServiceShipTelemetry.Infrastructure.Repository;

namespace GrpcServiceShipTelemetry.Services
{
    public class GreeterService : ShipTelemetryService.ShipTelemetryServiceBase
    {
        private readonly IWorkContainer _workContainer;
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger, IWorkContainer workContainer)
        {
            _logger = logger;
            _workContainer = workContainer;
        }

        public override async Task RegisterTelemetryDataStream(
            IAsyncStreamReader<TelemetryData> requestStream,
            IServerStreamWriter<TelemetryResponse> responseStream,
            ServerCallContext context)
        {
            try
            {
                await foreach (var telemetryData in requestStream.ReadAllAsync())
                {
                    try
                    {
                        var telemetry = new Telemetry
                        {
                            ShipId = telemetryData.ShipId,
                            Latitude = telemetryData.Latitude,
                            Longitude = telemetryData.Longitude,
                            Speed = telemetryData.Speed,
                            Timestamp = telemetryData.Timestamp?.ToDateTime() ?? DateTime.UtcNow
                        };

                        _workContainer.TelemetryRepository.AddTelemetry(telemetry);
                        _workContainer.Commit();

                        await responseStream.WriteAsync(new TelemetryResponse
                        {
                            Success = true,
                            Message = $"Datos de {telemetryData.ShipId} registrados exitosamente",
                            ReceivedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                        });

                        _logger.LogInformation($"Telemetria registrada para barco {telemetryData.ShipId}");
                    }
                    catch (Exception ex)
                    {
                        _workContainer.Rollback();
                        _logger.LogError($"Error procesando datos: {ex.Message}");

                        await responseStream.WriteAsync(new TelemetryResponse
                        {
                            Success = false,
                            Message = $"Error: {ex.Message}",
                            ReceivedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                        });
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Stream de telemetria cancelado");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en stream: {ex.Message}");
                throw;
            }
        }

        public override async Task<GetTelemetryResponse> GetTelemetryData(TelemetryRequest request, ServerCallContext context)
        {
            try
            {
                var telemetry = await _workContainer.TelemetryRepository.GetTelemetryAsync(request.ShipId);
                if (telemetry != null)
                {
                    return new GetTelemetryResponse
                    {
                        Message = "Datos encontrados",
                        Data = new TelemetryData
                        {
                            ShipId = telemetry.ShipId,
                            Latitude = telemetry.Latitude,
                            Longitude = telemetry.Longitude,
                            Speed = telemetry.Speed,
                            Timestamp = Timestamp.FromDateTime(telemetry.Timestamp)
                        }
                    };
                }

                return new GetTelemetryResponse
                {
                    Message = "No se encontraron datos para el barco especificado"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error obteniendo datos: {ex.Message}");
                return new GetTelemetryResponse
                {
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
