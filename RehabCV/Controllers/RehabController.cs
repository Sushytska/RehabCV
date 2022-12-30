using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.DTO;
using RehabCV.Extension;
using RehabCV.Interfaces;
using RehabCV.Models;
using RehabCV.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Controllers
{
    public class RehabController : Controller
    {
        private readonly IRehabilitation<Rehabilitation> _rehabilitation;
        private readonly UserManager<User> _userManager;
        private readonly IChild<Child> _child;
        private readonly IQueue<Queue> _queue;
        private readonly IGroup<Group> _group;
        private readonly IReserve<Reserve> _reserve;
        private readonly IEvent<Event> _event;
        private const string policy = "RequireAdminRole";

        public RehabController(IRehabilitation<Rehabilitation> rehabilitation,
                               UserManager<User> userManager,
                               IChild<Child> child,
                               IQueue<Queue> queue,
                               IGroup<Group> group,
                               IReserve<Reserve> reserve,
                               IEvent<Event> @event)
        {
            _rehabilitation = rehabilitation;
            _userManager = userManager;
            _child = child;
            _queue = queue;
            _group = group;
            _reserve = reserve;
            _event = @event;
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<IActionResult> Index(DateTime searchDate)
        {
            var children = await _child.FindAll();

            var childList = children.Where(x => x.ReserveId == null).ToList();

            var rehabViewModel = await childList.GetRehabViewModel(_group, _rehabilitation);

            var dt = new DateTime();

            return View(rehabViewModel.Where(x => x.DateOfRehab == searchDate || x.DateOfCommission == searchDate
                                             || searchDate == dt).ToList());
        }

        [HttpGet, Authorize()]
        public async Task<IActionResult> Create(string id)
        {
            var @event = await _event.FindAll();
            var datesRehab = new List<Event>();
            var datesCommiss = new List<Event>();

            foreach (var item in @event)
            {
                if (DateTime.UtcNow.AddDays(7) < item.Start && item.Subject == "Реабілітація")
                {
                    datesRehab.Add(item);
                }
                if (DateTime.UtcNow < item.Start && item.Subject == "Комісія")
                {
                    datesCommiss.Add(item);
                }
            }

            ViewBag.datesRehab = new SelectList(datesRehab, "Id", "Start");

            ViewBag.datesCommiss = new SelectList(datesCommiss, "Id", "Start");

            ViewBag.children = id;

            return View();
        }

        [HttpPost, Authorize(), ActionName("Create")]
        public async Task<IActionResult> Create(RehabDTO rehabDTO)
        {
            if (ModelState.IsValid)
            {
                var duration = rehabDTO.Form == "Амбулаторна" ? rehabDTO.DurationAmbylator : rehabDTO.DurationStatsionar;

                var dateOfRehab = await _event.FindById(rehabDTO.DateOfRehabId);

                var dateOfCommis = await _event.FindById(rehabDTO.DateOfCommissionId);

                var rehab = new Rehabilitation
                {
                    Id = Guid.NewGuid().ToString(),
                    ChildId = rehabDTO.ChildId,
                    Form = rehabDTO.Form,
                    Duration = duration,
                    DateOfRehab = dateOfRehab.Start,
                    DateOfCommission = dateOfCommis.Start
                };

                var resultRehab = await _rehabilitation.CreateAsync(rehab);

                var child = await _child.FindById(rehabDTO.ChildId);

                var group = await _group.FindById(child.GroupId);

                var addedToGroup = await group.AddChildToQueue(_queue, rehab, _group, _child, _reserve, _rehabilitation);

                if (resultRehab != null)
                {
                    return RedirectToAction("Notification", new { addedToGroup, dateTime = rehab.DateOfRehab, childId = rehab.ChildId });
                }

                await _child.DeleteAsync(rehabDTO.ChildId);
            }

            return View(rehabDTO);
        }

        [HttpGet, Authorize()]
        public async Task<IActionResult> Notification(bool addedToGroup, DateTime dateTime, string childId)
        {
            var notification = new NotificationDTO();
            if (addedToGroup)
            {
                notification.Notification = $"Запис на реаблітацію пройшов успішно, дата реабілітації {dateTime}";
                notification.AddedToGroup = true;
            }
            else
            {
                var @event = await _event.FindAll();
                var dates = new List<Event>();

                foreach (var item in @event)
                {
                    if (DateTime.UtcNow.AddDays(7) < item.Start && item.Subject == "Реабілітація")
                    {
                        dates.Add(item);
                    }
                }

                ViewBag.dates = new SelectList(@event, "Id", "Start");

                notification.Notification = $"В даній групі на дату {dateTime} вже немає місць, " +
                                            $"записатись можна в резерв, або на наступну дату";
                notification.AddedToGroup = false;

                ViewBag.children = childId;
            }

            return PartialView(notification);
        }

        [HttpPost, Authorize(), ActionName("Notification")]
        public async Task<IActionResult> Notification(NotificationDTO notificationDTO)
        {
            if (notificationDTO.AddToAnotherDate)
            {
                var dateOfRehab = await _event.FindById(notificationDTO.DateId);

                var child = await _child.FindById(notificationDTO.ChildId);

                var rehab = await _rehabilitation.FindByChildId(notificationDTO.ChildId);

                var group = await _group.FindById(child.GroupId);

                var areSeats = await _group.AreSeats(group.NameOfDisease, "anoherDate", dateOfRehab.Start, _rehabilitation);

                if (areSeats)
                {
                    rehab.DateOfRehab = dateOfRehab.Start;

                    await _rehabilitation.UpdateAsync(rehab.Id, rehab);

                    child.Reserve = null;
                    child.ReserveId = null;

                    await _child.UpdateAsync(child.Id, child);
                }

                return RedirectToAction("Notification", new { addedToGroup = areSeats, dateTime = dateOfRehab.Start, rehab });
            }

            if (notificationDTO.AddToReserve)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(notificationDTO);
        }
    }
}
