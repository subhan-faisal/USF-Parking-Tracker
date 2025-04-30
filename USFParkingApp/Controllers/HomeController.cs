using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USFParkingTracker.Data;
using USFParkingTracker.Models;
using USFParkingTracker.Models.ViewModels;

namespace USFParkingTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get data for dashboard
            var lots = await _context.ParkingLots.ToListAsync();
            
            // Calculate stats
            int totalSpaces = lots.Sum(l => l.TotalSpaces);
            int availableSpaces = lots.Sum(l => l.AvailableSpaces);
            int occupancyRate = totalSpaces > 0 
                ? (int)Math.Round((double)(totalSpaces - availableSpaces) / totalSpaces * 100) 
                : 0;
                
            var viewModel = new DashboardViewModel
            {
                Stats = new ParkingStats
                {
                    TotalSpaces = totalSpaces,
                    AvailableSpaces = availableSpaces,
                    OccupancyRate = occupancyRate,
                    LastUpdated = DateTime.Now
                },
                ParkingLots = lots
            };
            
            return View(viewModel);
        }

        public IActionResult Map()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; } // Make nullable with ?
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}