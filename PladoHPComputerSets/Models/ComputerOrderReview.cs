namespace PladoHPComputerSets.Models
{
    public class ComputerOrderReview
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ReviewerName { get; set; } = "Anonymous";
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
    }
}
