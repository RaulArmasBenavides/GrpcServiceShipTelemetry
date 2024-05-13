using Grpc.Core;
using GrpcServiceShipTelemetry;
using GrpcServiceShipTelemetry.Domain.Interfaces;
using GrpcServiceShipTelemetry.Domain.Models;
using GrpcServiceShipTelemetry.Infrastructure.Repository;

namespace GrpcServiceShipTelemetry.Services
{
    public class GreeterService :ShipTelemetryService.ShipTelemetryServiceBase
    {
        private readonly IWorkContainer _workContainer; 
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger, IWorkContainer workContainer)
        {
            _logger = logger;
            _workContainer = workContainer;
        }

        //public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        //{
        //    return Task.FromResult(new HelloReply
        //    {
        //        Message = "Hello " + request.Name
        //   });
        //}


        public override Task<TelemetryResponse> RegisterTelemetryData(TelemetryData request, ServerCallContext context)
        {
            try
            {
                var builder = new TelemetryBuilder(request);
                Telemetry telemetry = builder.Build();
                _workContainer.TelemetryRepository.AddTelemetry(telemetry);
                _workContainer.Commit();

                return Task.FromResult(new TelemetryResponse
                {
                    Success = true,
                    Message = "Datos registrados con éxito"
                });
            }
            catch (Exception ex)
            {
                _workContainer.Rollback();
                return Task.FromResult(new TelemetryResponse
                {
                    Success = false,
                    Message = $"Error al registrar datos: {ex.Message}"
                });
            }
        }
        public override async Task<GetTelemetryResponse> GetTelemetryData(TelemetryRequest request, ServerCallContext context)
        {
            try
            {
                var telemetry = await _workContainer.TelemetryRepository.GetTelemetryAsync(request.ShipId);
                if (telemetry != null)
                {
                    var response = new GetTelemetryResponse
                    {
                        Message = "Datos encontrados con éxito",
                        Data = new TelemetryData
                        {
                            // Asumiendo que TelemetryData tiene estas propiedades
                            ShipId = telemetry.ShipId,
                            Latitude = telemetry.Latitude,
                            Longitude = telemetry.Longitude,
                            Speed = telemetry.Speed
                        }
                    };
                    return response;
                }
                else
                {
                    return new GetTelemetryResponse
                    {
                        Message = "No se encontraron datos de telemetría para el ID proporcionado"
                    };
                }
            }
            catch (Exception ex)
            {
                return new GetTelemetryResponse
                {
                    Message = $"Error al obtener datos: {ex.Message}"
                };
            }
        }

    }
}