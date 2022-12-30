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
    public class TherapistController : Controller
    {
        private readonly ITherapist<Therapist> _repository;

        private const string policy = "RequireAdminRole";

        public TherapistController(
                        ITherapist<Therapist> repository)
        {
            _repository = repository;
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<IActionResult> Index()
        {
            var therapists = await _repository.FindAll();

            return View(therapists);
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost, Authorize(Policy = policy), ActionName("Create")]
        public async Task<IActionResult> Create(TherapistDTO therapistDTO)
        {
            if (ModelState.IsValid)
            {
                var therapist = new Therapist
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = therapistDTO.FirstNameOfTherapist,
                    MiddleName = therapistDTO.MiddleNameOfTherapist,
                    LastName = therapistDTO.LastNameOfTherapist,
                    Birthday = therapistDTO.BirthdayOfTherapist,
                    Region = therapistDTO.Region,
                    District = therapistDTO.District,
                    Location = therapistDTO.Location,
                    Street = therapistDTO.Street,
                    Post = therapistDTO.Post
                };

                var result = await _repository.CreateAsync(therapist);

                if (result != null)
                {
                    return RedirectToAction("Index", "Therapist");
                }

            }
            return View(therapistDTO);
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<ActionResult> Update(string id)
        {
            var therapist = await _repository.FindById(id);

            if (therapist == null)
            {
                return NotFound();
            }

            var therapistDTO = new TherapistDTO
            {
                FirstNameOfTherapist = therapist.FirstName,
                LastNameOfTherapist = therapist.LastName,
                MiddleNameOfTherapist = therapist.MiddleName,
                BirthdayOfTherapist = therapist.Birthday,
                Region = therapist.Region,
                District = therapist.District,
                Location = therapist.Location,
                Street = therapist.Street,
                Post = therapist.Post
            };

            return View(therapistDTO);
        }

        [HttpPost, Authorize(Policy = policy), ActionName("Update")]
        public async Task<IActionResult> Update(string id, TherapistDTO therapistDTO)
        {
            if (ModelState.IsValid)
            {
                var therapist = await _repository.FindById(id);

                therapist.FirstName = therapistDTO.FirstNameOfTherapist;
                therapist.LastName = therapistDTO.LastNameOfTherapist;
                therapist.MiddleName = therapistDTO.MiddleNameOfTherapist;
                therapist.Birthday = therapistDTO.BirthdayOfTherapist;
                therapist.Region = therapistDTO.Region;
                therapist.District = therapistDTO.District;
                therapist.Location = therapistDTO.Location;
                therapist.Street = therapistDTO.Street;
                therapist.Post = therapistDTO.Post;

                await _repository.UpdateAsync(id, therapist);
            }
            return View(therapistDTO);
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<ActionResult> Delete(string id)
        {
            var therapist = await _repository.FindById(id);

            var therapistDTO = new TherapistDTO
            {
                FirstNameOfTherapist = therapist.FirstName,
                LastNameOfTherapist = therapist.LastName,
                MiddleNameOfTherapist = therapist.MiddleName
            };

            return View(therapistDTO);
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

            return RedirectToAction("Index", "Therapist");
        }
    }
}
