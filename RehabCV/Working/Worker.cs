using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RehabCV.Extension;
using RehabCV.Interfaces;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RehabCV.Working
{
    public class Worker : IWorker
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task CheckReserv(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var _events = scope.ServiceProvider.GetService<IEvent<Event>>();
                var _reserve = scope.ServiceProvider.GetService<IReserve<Reserve>>();
                var _rehabilitation = scope.ServiceProvider.GetService<IRehabilitation<Rehabilitation>>();
                var _queue = scope.ServiceProvider.GetService<IQueue<Queue>>();
                var _group = scope.ServiceProvider.GetService<IGroup<Group>>();
                var _child = scope.ServiceProvider.GetService<IChild<Child>>();

                var dates = await _events.FindAll();

                foreach (var item in dates)
                {
                    if (DateTime.Now.AddDays(7) >= item.Start && item.Subject == "Реабілітація")
                    {
                        var reserve = await _reserve.GetReserve();

                        if (reserve == null)
                        {
                            break;
                        }

                        var children = reserve.Children.OrderBy(x => x.DateOfReserv).ToList();

                        foreach (var child in children)
                        {
                            var rehab = await _rehabilitation.FindByChildId(child.Id);

                            var group = await _group.FindById(child.GroupId);

                            var groups = await _group.FindAll();

                            var groupOrdered = groups.OrderBy(x => x.Priority).ToList();

                            if (rehab.DateOfRehab == item.Start)
                            {
                                var queue = new Queue
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    RehabilitationId = rehab.Id,
                                    GroupOfDisease = group.NameOfDisease
                                };

                                foreach (var g in groupOrdered)
                                {
                                    if (await _group.AreSeats(g.NameOfDisease, "reserv", rehab.DateOfRehab, _rehabilitation))
                                    {
                                        queue.GroupOfDisease = g.NameOfDisease;

                                        await _group.ChangeDisease(_child, child, queue.GroupOfDisease);

                                        await _queue.AddToQueue(queue);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                
                await Task.Delay(TimeSpan.FromDays(1), cancellationToken);
            }   
        }
    }
}
