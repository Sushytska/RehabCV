using System;

namespace RehabCV.Models
{
    public class Child 
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string GroupId { get; set; }
        public Group Group { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Location { get; set; }
        public string Street { get; set; }
        public Rehabilitation Rehabilitation { get; set; }
        public string ReserveId { get; set; }
        public Reserve Reserve { get; set; }
        public DateTime DateOfReserv { get; set; }
    }
}
