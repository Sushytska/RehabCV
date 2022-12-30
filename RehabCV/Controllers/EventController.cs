using Microsoft.AspNetCore.Mvc;
using RehabCV.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using RehabCV.Models;
using RehabCV.Repositories;
using RehabCV.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RehabCV.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RehabCV.Controllers
{
    public class EventController : Controller
    {
        private readonly IEvent<Event> _eventRepository;
        private readonly IChild<Child> _childRepository;
        private readonly ITherapist<Therapist> _therapistRepository;
        private const string policy = "RequireAdminRole";
        public EventController(IEvent<Event> eventRepository, IChild<Child> childRepository,
            ITherapist<Therapist> therapistRepository)
        {
            _eventRepository = eventRepository;
            _childRepository = childRepository;
            _therapistRepository = therapistRepository;
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<ActionResult> Index()
        {
            var viewModel = new EventViewModel
            {
                TherapistList = await GetTherapistItems(),
                ChildList = await GetClildItems()
            };
            return View(viewModel);
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<JsonResult> GetEvents()
        {
            var events = await _eventRepository.FindAll();

            return Json(events, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        }

        [HttpPost, Authorize(Policy = policy)]
        public async Task<IActionResult> SaveEvent(Event e)
        {
            if (!String.IsNullOrEmpty(e.Id))
            {
                var v = await _eventRepository.FindById(e.Id);

                v.Subject = e.Subject;
                v.Start = e.Start;
                v.End = e.End;
                v.Description = e.Description;
                v.IsFullDay = e.IsFullDay;
                v.ThemeColor = e.ThemeColor;

                await _eventRepository.UpdateAsync(e.Id, v);
                return Ok();
            }
            e.Id = Guid.NewGuid().ToString();

            var createdId = await _eventRepository.CreateAsync(e);
            return createdId == null
                ? BadRequest()
                : new StatusCodeResult(StatusCodes.Status201Created);
        }

        [HttpPost, Authorize(Policy = policy)]
        public async Task<JsonResult> DeleteEvent(string id)
        {
            var status = false;
            if (_eventRepository != null)
            {
                var v = await _eventRepository.FindById(id);
                if (v != null)
                {
                    var result = await _eventRepository.DeleteAsync(id);

                    if (result != 0)
                    {
                        status = true;
                    }
                }
            }

            return Json(status, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        }

        private async Task<List<SelectListItem>> GetTherapistItems()
        {
            var therapistList = (await _therapistRepository.FindAll()).ToList();
            return therapistList.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = $"{x.FirstName} {x.MiddleName} {x.LastName}"
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetClildItems()
        {
            var childList = (await _childRepository.FindAll()).ToList();
            return childList.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = $"{x.FirstName} {x.MiddleName} {x.LastName}"
            }).ToList();
        }
    }
}
