using System;
using System.ComponentModel.DataAnnotations;

namespace RehabCV.DTO
{
    public class ChildDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть ім'я.")]
        [StringLength(100, ErrorMessage = "В полі ім'я має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Ім'я дитини")]
        public string FirstNameOfChild { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть по батькові.")]
        [StringLength(100, ErrorMessage = "В полі по батькові має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "По батькові дитини")]
        public string MiddleNameOfChild { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть прізвище.")]
        [StringLength(100, ErrorMessage = "В полі прізвище має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Прізвище дитини")]
        public string LastNameOfChild { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть дату народження.")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата народження дитини")]
        public DateTime BirthdayOfChild { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Виберіть групу захворювання.")]
        [Display(Name = "Виберіть групу захворювання")]
        public string GroupId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть область проживання.")]
        [StringLength(100, ErrorMessage = "В полі область проживання має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Область проживання")]
        public string Region { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть район проживання.")]
        [StringLength(100, ErrorMessage = "В полі район проживання має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Район проживання")]
        public string District { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть населений пункт.")]
        [StringLength(100, ErrorMessage = "В полі населений пункт має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Населений пункт")]
        public string Location { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть вулицю проживання.")]
        [StringLength(100, ErrorMessage = "В полі вулиця проживання має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Вулиця проживання")]
        public string Street { get; set; }
    }
}
