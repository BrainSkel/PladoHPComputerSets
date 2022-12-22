namespace PladoHPComputerSets.Models.ViewModels
{
    public class ComputerOrderStastisticsViewModel
    {
        public ICollection<ComputerOrder> CompletedOrders { get; set; }
        public ICollection<ComputerOrder> NotCompletedOrders { get; set; }
    }
}
