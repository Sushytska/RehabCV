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

namespace RehabCV.Controllers
{
    public class EventController : Controller
    {
        private readonly IEvent<Event> _eventRepository;
        private const string policy = "RequireAdminRole";
        public EventController(IEvent<Event> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet, Authorize(Policy = policy)]
        public ActionResult Index()
        {
            return View();
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
        public async Task<JsonResult> SaveEvent(Event e)
        {
            var status = false;
            if (_eventRepository != null)
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
                }
                else
                {
                    e.Id = Guid.NewGuid().ToString();

                    var result = await _eventRepository.CreateAsync(e);

                    if (result != null)
                    {
                        status = true;
                    }
                }
            }
            return Json(status);
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
    }
}
