using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.DTO
{
    public class RehabDTO
    {
        public string ChildId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть форму реабілітації.")]
        [Display(Name = "Форма реабілітації")]
        public string Form { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть тривалість реабілітації.")]
        [Display(Name = "Тривалість реабілітації")]
        public string DurationAmbylator { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть тривалість реабілітації.")]
        [Display(Name = "Тривалість реабілітації")]
        public string DurationStatsionar { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть дату початку реабілітації.")]
        [Display(Name = "Дата початку реабілітації")]
        public string DateOfRehabId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть дату комісії на реабілітацію.")]
        [Display(Name = "Дата комісії")]
        public string DateOfCommissionId { get; set; }
    }
}
