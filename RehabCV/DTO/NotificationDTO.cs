using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.DTO
{
    public class NotificationDTO
    {
        public bool AddedToGroup { get; set; }
        public string Notification { get; set; }

        [Display(Name = "Дати проходження реабілітації")]
        public string DateId { get; set; }

        [Display(Name = "Додати в резерв?")]
        public bool AddToReserve { get; set; }

        [Display(Name = "Записати на іншу дату?")]
        public bool AddToAnotherDate { get; set; }
        public string ChildId { get; set; }
    }
}
