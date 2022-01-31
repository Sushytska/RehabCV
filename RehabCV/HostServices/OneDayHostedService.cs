using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RehabCV.Working;

namespace RehabCV.HostServices
{
    public class OneDayHostedService : BackgroundService
    {
        private readonly IWorker _worker;

        public OneDayHostedService(IWorker worker) 
        {
            _worker = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _worker.CheckReserv(stoppingToken);
        }
    }
}
