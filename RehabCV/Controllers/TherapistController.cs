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
    }
}
