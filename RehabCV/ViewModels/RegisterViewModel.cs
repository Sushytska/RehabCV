using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.ViewModels
{
    public class RegisterViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть ім'я.")]
        [StringLength(100, ErrorMessage = "В полі ім'я має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Ім'я")]
        public string FirstNameOfUser { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть по батькові.")]
        [StringLength(100, ErrorMessage = "В полі по батькові має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "По батькові")]
        public string MiddleNameOfUser { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть прізвище.")]
        [StringLength(100, ErrorMessage = "В полі прізвище має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Прізвище")]
        public string LastNameOfUser { get; set; }

        [Display(Name = "Чи є у Вас електронна пошта?")]
        public bool IsEmail { get; set; }

        [EmailAddress(ErrorMessage = "Електронна пошта не відповідає стандартним вимогам")]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }

        [Display(Name = "Логін латинськими літерами")]
        [StringLength(100, ErrorMessage = "В полі прізвище має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть номер телефону.")]
        [StringLength(13, ErrorMessage = "Номер телефону має містити 13 символів.", MinimumLength = 13)]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть пароль.")]
        [StringLength(100, ErrorMessage = "Пароль має містити мінімум {2} символів верхнього та нижнього регістру, спеціальні символи та цифри.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Підтвердіть пароль.")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Пароль та пароль підтвердження не збігаються.")]
        public string ConfirmPassword { get; set; }
    }
}
