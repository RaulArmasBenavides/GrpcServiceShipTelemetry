# GrpcServiceShipTelemetry

Sistema de tracking de barcos/envíos en tiempo real usando gRPC. Captura datos de telemetría (ubicación GPS, velocidad) de manera eficiente.

## ¿Por qué gRPC?

| Aspecto | REST HTTP | gRPC |
|---------|-----------|------|
| **Payload** | JSON (~2KB por update) | Protocol Buffers (~500B) |
| **Latencia** | ~50-100ms+ | ~5-20ms |
| **Conexión** | Nueva conexión c/request | HTTP/2 multiplexing |
| **Caso de uso** | Barcos enviando c/30seg | Ideal para streaming |
| **Ancho de banda** | Mayor consumo | 7-10x más eficiente |

**Ejemplo**: 1000 barcos enviando datos cada 30 segundos:
- **REST**: ~168MB/día
- **gRPC**: ~20MB/día

En comunicaciones satelitales/4G marino, esto es crítico.

## Arquitectura

```
GrpcServiceShipTelemetry/          # Presentación (gRPC Services)
├── GrpcServiceShipTelemetry.Domain/        # Modelos e interfaces
├── GrpcServiceShipTelemetry.Application/   # Lógica de negocio
└── GrpcServiceShipTelemetry.Infrastructure/ # Acceso a datos (EF Core + SQL Server)
```

## Servicios gRPC

### ShipTelemetryService

- **RegisterTelemetryDataStream**: Stream bidireccional para recibir telemetría continua de barcos
- **GetTelemetryData**: Obtener última posición conocida de un barco

## Requisitos

- .NET 7+
- SQL Server
- gRPC Client (grpcurl, cliente .NET, etc)

## Configuración

Actualiza `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "ConexionSQL": "Server=localhost;Database=ShipTelemetry;Trusted_Connection=true;"
  }
}
```

## Ejecución

```bash
dotnet run
```

## Testing con grpcurl

```bash
# Stream bidireccional (registrar telemetría)
grpcurl -plaintext -d @ localhost:5001 greet.ShipTelemetryService.RegisterTelemetryDataStream <<EOF
{ "shipId": "SHIP001", "latitude": 40.7128, "longitude": -74.0060, "speed": 15.5 }
EOF

# Obtener datos
grpcurl -plaintext -d '{"shipId":"SHIP001"}' localhost:5001 greet.ShipTelemetryService.GetTelemetryData
```
