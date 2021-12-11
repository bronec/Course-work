using AirlineSystem.Data;
using AirlineSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineSystem.Services
{
    public class TripService : IRepository<Trip>
    {
        private readonly ApplicationDbContext _context;

        public TripService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Trip> GetAll()
        {
            return _context.Trips.Include(t => t.Plane)
                                 .Include("Route")
                                 .Include("Route.DropOff")
                                 .Include("Route.PickUp")
                                 .ToList();//.Include("Tickets")
        }

        public Trip Details(int id)
        {
            return _context.Trips.Include("Plane")
                                 .Include("Tickets")
                                 .Include("Route")
                                 .Include("Route.DropOff")
                                 .Include("Route.PickUp")
                                 .FirstOrDefault(m => m.ID == id);

        }

        public void Add(Trip trip)
        {
            _context.Add(trip);
            _context.SaveChanges();
        }

        public void Update(Trip trip)
        {
            _context.Update(trip);
            _context.SaveChanges();
        }

        public void Remove(Trip trip)
        {
            _context.Trips.Remove(trip);
            _context.SaveChanges();
        }

        /*public string GetSeats(int PlaneID)
        {
            int seatsCount = _context.Planes.Where(b => b.ID == PlaneID).FirstOrDefault().Capacity;

            string seats = null;
            if (seatsCount >= 1)
            {
                seats = "1";
                for (int i = 2; i <= seatsCount; i++)
                    seats += "," + i;
            }
            return seats;
        }*/
    }
}
