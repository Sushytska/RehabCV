using System;

namespace RehabCV.Models
{
    public class Rehabilitation
    {
        public string Id { get; set; }
        public string ChildId { get; set; }
        public Child Child { get; set; }
        public string Form { get; set; }
        public string Duration { get; set; }
        public DateTime DateOfRehab { get; set; }
        public DateTime DateOfCommission { get; set; }
        public Queue Queue { get; set; }
    }
}
