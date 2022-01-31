using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.DTO
{
    public class GroupDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть групу захворювання.")]
        [Display(Name = "Група захворювання")]
        public string NameOfDisease { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть дозволену кількість дітей у відсотках.")]
        [Display(Name = "Кількість дітей у відсотках")]
        public int PercentOfChildren { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть пріоритет групи захворювання.")]
        [Display(Name = "Пріоритет")]
        public int Priority { get; set; }
    }
}
