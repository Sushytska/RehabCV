using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.Database;
using RehabCV.DTO;
using RehabCV.Interfaces;
using RehabCV.Models;
using RehabCV.Repositories;
using RehabCV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Controllers
{
    public class ChildController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IChild<Child> _repository;
        private readonly IGroup<Group> _group;
        private const string policy = "RequireAdminRole";

        public ChildController(UserManager<User> userManager, 
                               IChild<Child> repository,
                               IGroup<Group> group)
        {
            _userManager = userManager;
            _repository = repository;
            _group = group;
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<IActionResult> Index(string id)
        {
            var children = await _repository.FindByParentId(id);

            return View(children);
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<IActionResult> Parent()
        {
            var parents = await _userManager.GetUsersInRoleAsync("Parent");

            return View(parents);
        }

        [HttpGet, Authorize()]
        public async Task<IActionResult> Create()
        {
            var groups = await _group.FindAll();

            ViewBag.groups = new SelectList(groups, "Id", "NameOfDisease");

            return View();
        }

        [HttpPost, Authorize(), ActionName("Create")]
        public async Task<IActionResult> Create(ChildDTO childDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var child = new Child
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    FirstName = childDTO.FirstNameOfChild,
                    MiddleName = childDTO.MiddleNameOfChild,
                    LastName = childDTO.LastNameOfChild,
                    Birthday = childDTO.BirthdayOfChild,
                    GroupId = childDTO.GroupId,
                    Region = childDTO.Region,
                    District = childDTO.District,
                    Location = childDTO.Location,
                    Street = childDTO.Street
                };

                var result = await _repository.CreateAsync(child);

                if (result != null)
                {
                    return RedirectToAction("Create", "Rehab", new {id = child.Id});
                }
                
            }

            return View(childDTO);
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<ActionResult> Update(string id)
        {
            var child = await _repository.FindById(id);

            if (child == null )
            {
                return BadRequest();
            }

            var childDTO = new ChildDTO
            {
                FirstNameOfChild = child.FirstName,
                LastNameOfChild = child.LastName,
                MiddleNameOfChild = child.MiddleName,
                BirthdayOfChild = child.Birthday,
                GroupId = child.GroupId,
                Region = child.Region,
                District = child.District,
                Location = child.Location,
                Street = child.Street
            };

            var groups = await _group.FindAll();

            ViewBag.groups = new SelectList(groups, "Id", "NameOfDisease");

            return View(childDTO);
        }

        [HttpPost, Authorize(Policy = policy), ActionName("Update")]
        public async Task<IActionResult> Update(string id, ChildDTO childDTO)
        {
            if (ModelState.IsValid)
            {
                var child = await _repository.FindById(id);

                child.FirstName = childDTO.FirstNameOfChild;
                child.LastName = childDTO.LastNameOfChild;
                child.MiddleName = childDTO.MiddleNameOfChild;
                child.Birthday = childDTO.BirthdayOfChild;
                child.GroupId = childDTO.GroupId;
                child.Region = childDTO.Region;
                child.District = childDTO.District;
                child.Location = childDTO.Location;
                child.Street = childDTO.Street;

                await _repository.UpdateAsync(id, child);

                return RedirectToAction("Parent");
            }

            return View(childDTO);
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<ActionResult> Delete(string id)
        {
            var child = await _repository.FindById(id);

            var childDTO = new ChildDTO
            {
                FirstNameOfChild = child.FirstName,
                LastNameOfChild = child.LastName,
                MiddleNameOfChild = child.MiddleName
            };

            return View(childDTO);
        }

        [HttpPost, Authorize(Policy = policy), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _repository.DeleteAsync(id);

            if (result == 0)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Child");
        }
    }
}
