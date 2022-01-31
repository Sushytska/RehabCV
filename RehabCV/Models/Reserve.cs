using System.Collections.Generic;

namespace RehabCV.Models
{
    public class Reserve
    {
        public string Id { get; set; }
        public int NumberInReserv { get; set; }
        public List<Child> Children { get; set; }
    }
}
