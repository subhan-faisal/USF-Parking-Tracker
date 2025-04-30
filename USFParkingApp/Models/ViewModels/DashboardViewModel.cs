using System.Collections.Generic;

namespace USFParkingTracker.Models.ViewModels
{
    public class DashboardViewModel
    {
        public ParkingStats Stats { get; set; } = new ParkingStats();
        public IEnumerable<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();
    }
}