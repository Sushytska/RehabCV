﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.DTO;
using RehabCV.Extension;
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
        private readonly IRepository<Child> _child;
        private readonly IQueue<Queue> _queue;

        public RehabController(IRehabilitation<Rehabilitation> rehabilitation, 
                               UserManager<User> userManager,
                               IRepository<Child> child,
                               IQueue<Queue> queue)
        {
            _rehabilitation = rehabilitation;
            _userManager = userManager;
            _child = child;
            _queue = queue;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var rehabilitations = await _rehabilitation.FindAllByParentId(user.Id);

            var children = await _child.FindByParentId(user.Id);

            var rehabViewModel = rehabilitations.GetRehabViewModel(children);

            return View(rehabViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var children = await _child.FindByParentId(user.Id);

            ViewBag.children = new SelectList(children, "Id", "FirstName");

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(RehabDTO rehabDTO)
        {
            if (ModelState.IsValid)
            {
                var rehab = new Rehabilitation
                {
                    Id = Guid.NewGuid().ToString(),
                    ChildId = rehabDTO.ChildId,
                    Form = rehabDTO.Form,
                    Duration = rehabDTO.Duration,
                    DateOfRehab = rehabDTO.DateOfRehab
                };

                var resultRehab = await _rehabilitation.CreateAsync(rehab);

                var lastNumberInQueue = _queue.GetLastNumberOfQueue();

                var queue = new Queue
                {
                    Id = Guid.NewGuid().ToString(),
                    RehabilitationId = rehab.Id,
                    TypeOfRehab = rehab.Form,
                    NumberInQueue = lastNumberInQueue + 1
                };

                var resultQueue = await _queue.AddToQueue(queue);

                if (resultRehab != null && resultQueue != null)
                {
                    return RedirectToAction("Index", "Rehab");
                }
            }
    
            return View(rehabDTO);
        }
    }
}