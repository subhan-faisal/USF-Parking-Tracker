using System;

namespace USFParkingTracker.Models
{
    public class ParkingStats
    {
        public int TotalSpaces { get; set; }
        public int AvailableSpaces { get; set; }
        public int OccupancyRate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}