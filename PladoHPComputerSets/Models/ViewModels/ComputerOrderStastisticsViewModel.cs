using System.Collections;
using System.Collections.Generic;

namespace PladoHPComputerSets.Models.ViewModels
{
    public class ComputerOrderStastisticsViewModel
    {
        public ICollection<ComputerOrder> CompletedOrders { get; set; }
        public ICollection<ComputerOrder> NotCompletedOrders { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCompletedOrders { get; set; }
    }
}
