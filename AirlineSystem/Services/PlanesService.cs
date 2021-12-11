using AirlineSystem.Data;
using AirlineSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineSystem.Services
{
    public class PlanesService : IRepository<Plane>
    {
        private readonly ApplicationDbContext context;

        public PlanesService(ApplicationDbContext context)
        {
            this.context = context;
        }


        public List<Plane> GetAll()
        {
            return context.Planes.ToList();
        }

        public Plane Details(int id)
        {
            return context.Planes.Include(b=>b.Trips)
                                .ThenInclude(t=>t.Route)
                                .Include("Trips.Route.PickUp")
                                .Include("Trips.Route.DropOff")
                                .Include("Trips")
                                .FirstOrDefault(b => b.ID == id);
        }
        public void Add(Plane entity)
        {
            context.Planes.Add(entity);
            context.SaveChanges();
        }

        public void Update(Plane entity)
        {
            context.Planes.Update(entity);
            context.SaveChanges();
        }
        public void Remove(Plane entity)
        {
            context.Planes.Remove(entity);
            context.SaveChanges();
        }
    }
}
