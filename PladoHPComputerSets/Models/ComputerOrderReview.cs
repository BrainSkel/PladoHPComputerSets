using System.ComponentModel.DataAnnotations;

namespace PladoHPComputerSets.Models
{
    public class ComputerOrderReview
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        [Display(Name = "Reviewer Name")]

        [StringLength(30)]
        public string ReviewerName { get; set; } = "Anonymous";

        [StringLength(15)]
        public string? Title { get; set; }

        [StringLength(100)]
        public string? Comment { get; set; }

        [Range(0, 10)]
        public int Rating { get; set; }

        public ICollection<ComputerOrderReview> ComputerOrderReviews { get; set; }
    }
}
