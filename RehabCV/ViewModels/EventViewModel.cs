using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.Models;
using System.Collections.Generic;


namespace RehabCV.ViewModels
{
    public class EventViewModel
    {
        public List<SelectListItem> TherapistList { get; set; }
        public List<SelectListItem> ChildList { get; set; }
    }
}
