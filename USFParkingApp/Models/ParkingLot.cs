using System;
using System.ComponentModel.DataAnnotations;

namespace USFParkingTracker.Models
{
    public class ParkingLot
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        [Range(1, 10000)]
        public int TotalSpaces { get; set; }
        
        [Required]
        [Range(0, 10000)]
        public int AvailableSpaces { get; set; }
        
        [Required]
        public string Status { get; set; } = string.Empty;  // "open", "limited", "full", "closed"
        
        public Coordinates? Coordinates { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        // Helper method to determine status based on available spaces
        public static string DetermineStatus(int availableSpaces, int totalSpaces)
        {
            if (availableSpaces == 0) return "full";
            
            double percentage = (double)availableSpaces / totalSpaces * 100;
            
            if (percentage < 5) return "full";
            if (percentage <= 25) return "limited";
            return "open";
        }
    }
}