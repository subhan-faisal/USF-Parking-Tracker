using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using USFParkingTracker.Models;

namespace USFParkingTracker.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                // Create the database if it doesn't exist
                context.Database.EnsureCreated();

                // Look for any parking lots
                if (context.ParkingLots.Any())
                {
                    return;   // DB has been seeded
                }

                var parkingLots = new ParkingLot[]
                {
                    new ParkingLot
                    {
                        Name = "Collins Parking Facility",
                        Type = "Student",
                        TotalSpaces = 300,
                        AvailableSpaces = 75,
                        Status = "limited",
                        Coordinates = new Coordinates { Lat = 28.0617, Lng = -82.4127 },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new ParkingLot
                    {
                        Name = "Laurel Drive Garage",
                        Type = "Faculty & Staff",
                        TotalSpaces = 250,
                        AvailableSpaces = 112,
                        Status = "open",
                        Coordinates = new Coordinates { Lat = 28.0634, Lng = -82.4156 },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new ParkingLot
                    {
                        Name = "Crescent Hill Garage",
                        Type = "Student & Event",
                        TotalSpaces = 400,
                        AvailableSpaces = 23,
                        Status = "limited",
                        Coordinates = new Coordinates { Lat = 28.0659, Lng = -82.4105 },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new ParkingLot
                    {
                        Name = "Richard A. Beard Garage",
                        Type = "Staff & Student",
                        TotalSpaces = 320,
                        AvailableSpaces = 0,
                        Status = "full",
                        Coordinates = new Coordinates { Lat = 28.0608, Lng = -82.4088 },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new ParkingLot
                    {
                        Name = "Holly Drive Garage",
                        Type = "Visitor",
                        TotalSpaces = 190,
                        AvailableSpaces = 82,
                        Status = "open",
                        Coordinates = new Coordinates { Lat = 28.0672, Lng = -82.4132 },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new ParkingLot
                    {
                        Name = "Sycamore Drive Lot",
                        Type = "Staff",
                        TotalSpaces = 300,
                        AvailableSpaces = 150,
                        Status = "open",
                        Coordinates = new Coordinates { Lat = 28.0589, Lng = -82.4167 },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                };

                foreach (ParkingLot lot in parkingLots)
                {
                    context.ParkingLots.Add(lot);
                }
                context.SaveChanges();

                // Add some initial events
                foreach (var lot in context.ParkingLots.ToList())
                {
                    context.ParkingEvents.Add(new ParkingEvent
                    {
                        ParkingLotId = lot.Id,
                        EventType = "creation",
                        Timestamp = DateTime.Now,
                        Details = $"Parking lot {lot.Name} created with {lot.TotalSpaces} total spaces"
                    });
                }
                context.SaveChanges();
            }
        }
    }
}