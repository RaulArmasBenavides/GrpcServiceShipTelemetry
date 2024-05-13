using GrpcServiceShipTelemetry.Infrastructure.Data;
using GrpcServiceShipTelemetry.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GrpcServiceShipTelemetry.Domain.Interfaces;
namespace GrpcServiceShipTelemetry.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
         IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConexionSQL"),
                  b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)),
                  ServiceLifetime.Scoped);

          

            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //services.AddScoped<IWorkContainer, WorkContainer>();
            return services;
        }
    }
}
