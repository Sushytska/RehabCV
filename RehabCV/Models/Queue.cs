
namespace RehabCV.Models
{
    public class Queue
    {
        public string Id { get; set; }
        public string RehabilitationId { get; set; }
        public Rehabilitation Rehabilitation { get; set; }
        public string GroupOfDisease { get; set; }
        public int NumberInQueue { get; set; }
    }
}
