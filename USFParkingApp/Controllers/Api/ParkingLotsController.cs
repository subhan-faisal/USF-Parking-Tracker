using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USFParkingTracker.Data;
using USFParkingTracker.Models;

namespace USFParkingTracker.Controllers.Api
{
    [Route("api/parking-lots")]
    [ApiController]
    public class ParkingLotsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParkingLotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/parking-lots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingLot>>> GetParkingLots()
        {
            return await _context.ParkingLots.ToListAsync();
        }

        // GET: api/parking-lots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLot>> GetParkingLot(int id)
        {
            var parkingLot = await _context.ParkingLots.FindAsync(id);

            if (parkingLot == null)
            {
                return NotFound();
            }

            return parkingLot;
        }

        // POST: api/parking-lots
        [HttpPost]
        public async Task<ActionResult<ParkingLot>> PostParkingLot(ParkingLot parkingLot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            parkingLot.CreatedAt = DateTime.Now;
            parkingLot.UpdatedAt = DateTime.Now;
            parkingLot.Status = ParkingLot.DetermineStatus(parkingLot.AvailableSpaces, parkingLot.TotalSpaces);
            
            _context.ParkingLots.Add(parkingLot);
            await _context.SaveChangesAsync();
            
            // Create an event
            _context.ParkingEvents.Add(new ParkingEvent
            {
                ParkingLotId = parkingLot.Id,
                EventType = "creation",
                Timestamp = DateTime.Now,
                Details = $"Parking lot {parkingLot.Name} created with {parkingLot.TotalSpaces} total spaces"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParkingLot), new { id = parkingLot.Id }, parkingLot);
        }

        // PUT: api/parking-lots/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParkingLot(int id, ParkingLot parkingLot)
        {
            if (id != parkingLot.Id)
            {
                return BadRequest();
            }
            
            var existingLot = await _context.ParkingLots.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            if (existingLot == null)
            {
                return NotFound();
            }
            
            parkingLot.CreatedAt = existingLot.CreatedAt;
            parkingLot.UpdatedAt = DateTime.Now;
            parkingLot.Status = ParkingLot.DetermineStatus(parkingLot.AvailableSpaces, parkingLot.TotalSpaces);

            _context.Entry(parkingLot).State = EntityState.Modified;

            try
            {
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
                if (!ParkingLotExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/parking-lots/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParkingLot(int id)
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

            return NoContent();
        }

        // GET: api/parking-lots/stats
        [HttpGet("stats")]
        public async Task<ActionResult<ParkingStats>> GetParkingStats()
        {
            var lots = await _context.ParkingLots.ToListAsync();
            
            int totalSpaces = lots.Sum(l => l.TotalSpaces);
            int availableSpaces = lots.Sum(l => l.AvailableSpaces);
            int occupancyRate = totalSpaces > 0 
                ? (int)Math.Round((double)(totalSpaces - availableSpaces) / totalSpaces * 100) 
                : 0;
                
            return new ParkingStats
            {
                TotalSpaces = totalSpaces,
                AvailableSpaces = availableSpaces,
                OccupancyRate = occupancyRate,
                LastUpdated = DateTime.Now
            };
        }

        // POST: api/parking-lots/update-all
        [HttpPost("update-all")]
        public async Task<ActionResult<IEnumerable<ParkingLot>>> UpdateAllLots()
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
            return lots;
        }

        private bool ParkingLotExists(int id)
        {
            return _context.ParkingLots.Any(e => e.Id == id);
        }
    }
}