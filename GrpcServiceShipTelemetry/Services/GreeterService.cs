using Grpc.Core;
using GrpcServiceShipTelemetry;
using GrpcServiceShipTelemetry.Infraestructure.Repository;

namespace GrpcServiceShipTelemetry.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly IWorkContainer _workContainer;
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger, IWorkContainer workContainer)
        {
            _logger = logger;
            _workContainer = workContainer;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }


        public override Task<TelemetryResponse> RegisterTelemetryData(TelemetryData request, ServerCallContext context)
        {
            try
            {
                _workContainer.TelemetryRepository.AddTelemetry(request);
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
    }
}