using System.Threading;
using System.Threading.Tasks;

namespace RehabCV.Working
{
    public interface IWorker
    {
        Task CheckReserv(CancellationToken cancellationToken);
    }
}