using System.ComponentModel.DataAnnotations;

namespace PladoHPComputerSets.Models
{
    public class ComputerOrder
    {

        public int Id { get; set; }
        [Display(Name = "Orderer Name")]
        [DisplayFormat(NullDisplayText = "Anonymous")]
        [StringLength(20)]
        public string? OrdererName { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [Range(100, 10000)]
        public decimal Price { get; set; }

        [Display(Name = "Computer type")]
        public string? Type { get; set; }
        public bool Case { get; set; }
        public bool Monitor { get; set; }
        public bool Packed { get; set; }

    }
}
