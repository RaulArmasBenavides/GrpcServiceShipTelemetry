using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceShipTelemetry.Domain.Interfaces
{
    public interface IWorkContainer : IDisposable
    {
        ITelemetryRepository TelemetryRepository { get; }

        // Método para completar (commit) todas las operaciones en la transacción.

        void Commit();
        void Complete();

        //// Método para revertir todas las operaciones en caso de error.
        void Rollback();
    }
}
