using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceShipTelemetry.Infraestructure.Repository
{
    public class WorkContainer : IWorkContainer
    {
        private readonly TelemetryRepository _telemetryRepository;
        // Otros repositorios si es necesario
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;

        public WorkContainer(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            //_telemetryRepository = new TelemetryRepository(_connection, _transaction);
            // Inicializar otros repositorios aquí
        }

        public TelemetryRepository TelemetryRepository => _telemetryRepository;

        ITelemetryRepository IWorkContainer.TelemetryRepository => throw new NotImplementedException();

        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }
    }
}
