using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class NumberOfChController : Controller
    {
        private readonly INumberOfCh<NumberOfChildren> _numberOfCh;
        private readonly IGroup<Group> _group;
        private const string policy = "RequireAdminRole";

        public NumberOfChController(INumberOfCh<NumberOfChildren> numberOfCh,
                                   IGroup<Group> group)
        {
            _numberOfCh = numberOfCh;
            _group = group;
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<IActionResult> Index()
        {
            var numberOfCh = await _numberOfCh.GetNumber();

            return View(numberOfCh);
        }

        [HttpGet, Authorize(Policy = policy)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Authorize(Policy = policy), ActionName("Create")]
        public async Task<IActionResult> Create(NumberOfChildrenDTO numberOfChildrenDTO)
        {
            var numberOfCh = await _numberOfCh.GetNumber();

            if (numberOfCh != null)
            { 
                return RedirectToAction("Index", "NumberOfCh");
            }

            if (ModelState.IsValid)
            {
                var numberOfChildren = new NumberOfChildren
                {
                    Id = Guid.NewGuid().ToString(),
                    NumberOfChildrenInGroup = numberOfChildrenDTO.NumberOfChildren
                };

                var result = await _numberOfCh.CreateAsync(numberOfChildren);

                if (result != null)
                {
                    return RedirectToAction("Index", "NumberOfCh");
                }
            }

            return View(numberOfChildrenDTO);
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<ActionResult> Update()
        {
            var numberOfChildren = await _numberOfCh.GetNumber();

            if (numberOfChildren == null)
            {
                return BadRequest();
            }

            var numberOfChildrenDTO = new NumberOfChildrenDTO
            {
                NumberOfChildren = numberOfChildren.NumberOfChildrenInGroup
            };

            return View(numberOfChildrenDTO);
        }

        [HttpPost, Authorize(Policy = policy), ActionName("Update")]
        public async Task<IActionResult> Update(NumberOfChildrenDTO numberOfChildrenDTO)
        {
            if (ModelState.IsValid)
            {
                var numberOfChildren = await _numberOfCh.GetNumber();

                numberOfChildren.NumberOfChildrenInGroup = numberOfChildrenDTO.NumberOfChildren;

                await _numberOfCh.UpdateAsync(numberOfChildren);

                var groups = await _group.FindAll();

                foreach (var item in groups)
                {
                    var group = item.DivisionChildrenInGroups(numberOfChildrenDTO.NumberOfChildren);

                    await _group.UpdateAsync(group.Id, group);
                }

                return RedirectToAction("Index", "NumberOfCh");
            }

            return View(numberOfChildrenDTO);
        }
    }
}
