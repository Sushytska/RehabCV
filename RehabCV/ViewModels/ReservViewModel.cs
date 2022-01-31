using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.ViewModels
{
    public class ReservViewModel
    {
        [Display(Name = "Ім'я дитини")]
        public string FirstName { get; set; }

        [Display(Name = "По батькові дитини")]
        public string MiddleName { get; set; }

        [Display(Name = "Прізвище дитини")]
        public string LastName { get; set; }

        [Display(Name = "Група захворювання")]
        public string NameOfDisease { get; set; }

        [Display(Name = "Дата реабілітації")]
        public DateTime DateOfRehab { get; set; }

        [Display(Name = "Дата запису в резерв")]
        public DateTime DateOfReserv { get; set; }
    }
}
