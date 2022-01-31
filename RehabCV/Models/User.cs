using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace RehabCV.Models
{
    public class User : IdentityUser
    {
        public List<Child> Child { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
