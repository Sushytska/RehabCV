using System.Collections.Generic;

namespace RehabCV.Models
{
    public class Group
    {
        public string Id { get; set; }
        public string NameOfDisease { get; set; }
        public int NumberOfChildren { get; set; }
        public int PercentOfChildren { get; set; }
        public int Priority { get; set; }
        public List<Child> Children { get; set; }
    }
}
