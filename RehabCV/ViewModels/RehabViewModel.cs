using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.ViewModels
{
    public class RehabViewModel
    {
        [Display(Name = "Ім'я дитини")]
        public string FirstNameOfChild { get; set; }

        [Display(Name = "По батькові дитини")]
        public string MiddleNameOfChild { get; set; }

        [Display(Name = "Прізвище дитини")]
        public string LastNameOfChild { get; set; }

        [Display(Name = "Форма реабілітації")]
        public string Form { get; set; }

        [Display(Name = "Тривалість реабілітації")]
        public string Duration { get; set; }

        [Display(Name = "Дата початку реабілітації")]
        public DateTime DateOfRehab { get; set; }

        [Display(Name = "Дата комісії")]
        public DateTime DateOfCommission { get; set; }

        [Display(Name = "Група захворювання")]
        public string NameOfDisease { get; set; }
    }
}
