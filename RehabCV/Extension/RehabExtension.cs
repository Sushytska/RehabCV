using RehabCV.Interfaces;
using RehabCV.Models;
using RehabCV.Repositories;
using RehabCV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Extension
{
    public static class RehabExtension
    {
        public static async Task<IEnumerable<RehabViewModel>> GetRehabViewModel(this IEnumerable<Child> children,
                                                                                IGroup<Group> _group,
                                                                                IRehabilitation<Rehabilitation> _rehabilitation)
        {
            var rehabs = new List<RehabViewModel>();

            if (children != null)
            {
                foreach (var child in children)
                {
                    var rehabilitation = await _rehabilitation.FindByChildId(child.Id);

                    var group = await _group.FindById(child.GroupId);

                    var rehab = new RehabViewModel
                    {
                        FirstNameOfChild = child.FirstName,
                        MiddleNameOfChild = child.MiddleName,
                        LastNameOfChild = child.LastName,
                        NameOfDisease = group.NameOfDisease,
                        Form = rehabilitation.Form,
                        Duration = rehabilitation.Duration,
                        DateOfRehab = rehabilitation.DateOfRehab,
                        DateOfCommission = rehabilitation.DateOfCommission
                    };

                    rehabs.Add(rehab);
                }
            }

            return rehabs;
        }

        public static Group DivisionChildrenInGroups(this Group group, int numberOfChildren)
        {
            var percent = (double)group.PercentOfChildren / 100;
            group.NumberOfChildren = (int)Math.Round(numberOfChildren * percent, MidpointRounding.AwayFromZero);
            
            return group;
        }

        public static async Task<bool> AddChildToQueue(this Group group, 
                                                 IQueue<Queue> _queue, 
                                                 Rehabilitation rehabilitation, 
                                                 IGroup<Group> _group,
                                                 IClild<Child> _child,
                                                 IReserve<Reserve> _reserve,
                                                 IRehabilitation<Rehabilitation> _rehabilitation)
        {
            var queue = new Queue
            {
                Id = Guid.NewGuid().ToString(),
                RehabilitationId = rehabilitation.Id,
                GroupOfDisease = group.NameOfDisease
            };

            if (await _group.AreSeats(group.NameOfDisease, null, rehabilitation.DateOfRehab, _rehabilitation))
            {
                await _queue.AddToQueue(queue);
                return true;
            }
            else
            {
                await _reserve.AddToReserve(_child, rehabilitation.ChildId, queue.GroupOfDisease);
                return false;
            }
            
        }

        public static async Task<bool> AreSeats(this IGroup<Group> _group, string nameOfDisease, string reserv,
                                                DateTime dateOfRehab, IRehabilitation<Rehabilitation> _rehabilitation)
        {
            var group = await _group.FindByName(nameOfDisease);

            var numberOfAllSeats = group.NumberOfChildren;
            var children = group.Children;
            var numberOfBusySeats = 0;

            foreach (var child in children)
            {
                var rehab = await _rehabilitation.FindByChildId(child.Id);
                if (rehab.DateOfRehab == dateOfRehab)
                {
                    numberOfBusySeats++;
                }
            }

            if (reserv != null)
            {
                return numberOfAllSeats > numberOfBusySeats;
            }

            return numberOfAllSeats >= numberOfBusySeats;
        }

        public static async Task ChangeDisease(this IGroup<Group> _group, 
                                               IClild<Child> _child,
                                               Child child,
                                               string nameOfDisease)
        {
            var group = await _group.FindByName(nameOfDisease);

            child.GroupId = group.Id;
            child.Reserve = null;
            child.ReserveId = null;

            await _child.UpdateAsync(child.Id, child);
        }

        public static async Task AddToReserve(this IReserve<Reserve> _reserve, 
                                             IClild<Child> _child,
                                             string childId,
                                             string groupOfDisease)
        {
            var child = await _child.FindById(childId);

            var reserve =  await _reserve.GetReserve();

            if (reserve == null)
            {
                reserve = new Reserve()
                {
                    Id = Guid.NewGuid().ToString(),
                    NumberInReserv = 1
                };
                
                await _reserve.CreateAsync(reserve);

                
            }
            else
            {
                reserve.NumberInReserv = reserve.Children.Count + 1;

                await _reserve.UpdateAsync(reserve.Id, reserve);
            }

            child.ReserveId = reserve.Id;
            child.DateOfReserv = DateTime.Now;

            await _child.UpdateAsync(childId, child);
        }
    }
}
