using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть електронну пошту/логін.")]
        [Display(Name = "Електронна пошта/Логін")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть пароль.")]
        [StringLength(100, ErrorMessage = "Пароль має містити мінімум {2} символів верхнього та нижнього регістру, спеціальні символи та цифри.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
