using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USFParkingTracker.Data;
using USFParkingTracker.Models;

namespace USFParkingTracker.Controllers
{
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Manage
        public async Task<IActionResult> Index()
        {
            return View(await _context.ParkingLots.ToListAsync());
        }

        // GET: Manage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Type,TotalSpaces,AvailableSpaces,Coordinates")] ParkingLot parkingLot)
        {
            if (ModelState.IsValid)
            {
                // your normal saving logic
                parkingLot.CreatedAt = DateTime.Now;
                parkingLot.UpdatedAt = DateTime.Now;
                parkingLot.Status = ParkingLot.DetermineStatus(parkingLot.AvailableSpaces, parkingLot.TotalSpaces);

                _context.Add(parkingLot);
                await _context.SaveChangesAsync();

                _context.ParkingEvents.Add(new ParkingEvent
                {
                    ParkingLotId = parkingLot.Id,
                    EventType = "creation",
                    Timestamp = DateTime.Now,
                    Details = $"Parking lot {parkingLot.Name} created with {parkingLot.TotalSpaces} total spaces"
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // 🔍 Add this debug print block
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"ModelState error in '{key}': {error.ErrorMessage}");
                    }
                }
                return View(parkingLot);
            }

            // if (ModelState.IsValid)
            // {
            //     parkingLot.CreatedAt = DateTime.Now;
            //     parkingLot.UpdatedAt = DateTime.Now;
            //     parkingLot.Status = ParkingLot.DetermineStatus(parkingLot.AvailableSpaces, parkingLot.TotalSpaces);
                
            //     _context.Add(parkingLot);
            //     await _context.SaveChangesAsync();
                
            //     // Create an event
            //     _context.ParkingEvents.Add(new ParkingEvent
            //     {
            //         ParkingLotId = parkingLot.Id,
            //         EventType = "creation",
            //         Timestamp = DateTime.Now,
            //         Details = $"Parking lot {parkingLot.Name} created with {parkingLot.TotalSpaces} total spaces"
            //     });
            //     await _context.SaveChangesAsync();
                
            //     return RedirectToAction(nameof(Index));
            // }
            // return View(parkingLot);
        }

        // GET: Manage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLot = await _context.ParkingLots.FindAsync(id);
            if (parkingLot == null)
            {
                return NotFound();
            }
            return View(parkingLot);
        }

        // POST: Manage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,TotalSpaces,AvailableSpaces,Coordinates")] ParkingLot parkingLot)
        {
            if (id != parkingLot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing lot to preserve creation date
                    var existingLot = await _context.ParkingLots.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
                    if (existingLot == null)
                    {
                        return NotFound();
                    }
                    
                    parkingLot.CreatedAt = existingLot.CreatedAt;
                    parkingLot.UpdatedAt = DateTime.Now;
                    parkingLot.Status = ParkingLot.DetermineStatus(parkingLot.AvailableSpaces, parkingLot.TotalSpaces);
                    
                    _context.Update(parkingLot);
                    await _context.SaveChangesAsync();
                    
                    // Create an event
                    _context.ParkingEvents.Add(new ParkingEvent
                    {
                        ParkingLotId = parkingLot.Id,
                        EventType = "update",
                        Timestamp = DateTime.Now,
                        Details = $"Parking lot {parkingLot.Name} updated"
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingLotExists(parkingLot.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"ModelState error in '{key}': {error.ErrorMessage}");
                }
            }
            
            return View(parkingLot);
        }

        // GET: Manage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLot = await _context.ParkingLots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkingLot == null)
            {
                return NotFound();
            }

            return View(parkingLot);
        }

        // POST: Manage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingLot = await _context.ParkingLots.FindAsync(id);
            if (parkingLot == null)
            {
                return NotFound();
            }
            
            // Create an event before deleting
            _context.ParkingEvents.Add(new ParkingEvent
            {
                ParkingLotId = id,
                EventType = "deletion",
                Timestamp = DateTime.Now,
                Details = $"Parking lot {parkingLot.Name} deleted"
            });
            
            _context.ParkingLots.Remove(parkingLot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Manage/RefreshData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RefreshData()
        {
            var lots = await _context.ParkingLots.ToListAsync();
            var random = new Random();
            
            foreach (var lot in lots)
            {
                int change = random.Next(-5, 6); // Random change between -5 and +5
                lot.AvailableSpaces = Math.Max(0, Math.Min(lot.TotalSpaces, lot.AvailableSpaces + change));
                lot.Status = ParkingLot.DetermineStatus(lot.AvailableSpaces, lot.TotalSpaces);
                lot.UpdatedAt = DateTime.Now;
                
                // Create an event
                _context.ParkingEvents.Add(new ParkingEvent
                {
                    ParkingLotId = lot.Id,
                    EventType = "update",
                    Timestamp = DateTime.Now,
                    Details = $"Available spaces changed from {lot.AvailableSpaces - change} to {lot.AvailableSpaces}"
                });
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingLotExists(int id)
        {
            return _context.ParkingLots.Any(e => e.Id == id);
        }
    }
}