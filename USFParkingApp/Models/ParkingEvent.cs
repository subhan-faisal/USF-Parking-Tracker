using System;
using System.ComponentModel.DataAnnotations;

namespace USFParkingTracker.Models
{
    public class ParkingEvent
    {
        public int Id { get; set; }
        
        [Required]
        public int ParkingLotId { get; set; }
        
        public ParkingLot? ParkingLot { get; set; }
        
        [Required]
        [StringLength(50)]
        public string EventType { get; set; } = string.Empty;
        
        public DateTime Timestamp { get; set; }
        
        [StringLength(255)]
        public string? Details { get; set; }
    }
}