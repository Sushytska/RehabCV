using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RehabCV.DTO
{
    public class NumberOfChildrenDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть кількість дітей в групі.")]
        [Display(Name = "Кількість дітей в групі")]
        public int NumberOfChildren { get; set; }
    }
}
