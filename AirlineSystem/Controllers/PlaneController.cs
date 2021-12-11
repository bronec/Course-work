using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirlineSystem.Data;
using AirlineSystem.Models;
using AirlineSystem.Services;

using Microsoft.AspNetCore.Authorization;


    


namespace AirlineSystem.Controllers
{
    [Authorize(Roles = "Employee")]
    public class PlaneController : Controller
    {
        private readonly IRepository<Plane> planeService;

        public PlaneController(IRepository<Plane> planeService)
        {
            this.planeService = planeService;
        }

        // GET: Planes
        public IActionResult Index(string sortOrder)
        {
            
            var planes = planeService.GetAll();
            /*
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewBag.CapacitySortParm = sortOrder == "capacity" ? "capacity_desc" : "capacity";
            switch (sortOrder)
            {
                case "category_desc":
                   planes = planes.OrderByDescending(s => s.Category).ToList();
                    break;
                case "capacity":
                    planes = planes.OrderBy(s => s.Capacity).ToList();
                    break;
                case "capacity_desc":
                    planes = planes.OrderByDescending(s => s.Capacity).ToList();
                    break;
                default:
                    planes = planes.OrderBy(s => s.Category).ToList();
                    break;
            }*/
            return View(planes);

        }

        // GET: Planes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plane = planeService.Details((int)id);
            if (plane == null)
            {
                return NotFound();
            }

            return View(plane);
        }

        // GET: Planes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Planes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,PlaneNum,Category,Capacity")] Plane plane)
        {
            if (ModelState.IsValid)
            {
                planeService.Add(plane);
                return RedirectToAction(nameof(Index));
            }
            return View(plane);
        }

        // GET: Planes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plane = planeService.Details((int)id);
            if (plane == null)
            {
                return NotFound();
            }
            return View(plane);
        }

        // POST: Planes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,PlaneNum,Category,Capacity")] Plane plane)
        {
            if (id != plane.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    planeService.Update(plane);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!planeExists(plane.ID))
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
            return View(plane);
        }

        // GET: Planes/Delete/5
        public  IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plane = planeService.Details((int)id);
            if (plane == null)
            {
                return NotFound();
            }

            ViewBag.Message = null;


            // if (Routes.checkRoute(station.ID) == null)
            if (plane.Trips.Count != 0)
            {
                ViewBag.Message = " Вы не можете найти этот самолёт, потому что у него есть поездки ";

            }

            return View(plane);
        }

        // POST: Planes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var plane = planeService.Details(id);
            if (plane.Trips.Count == 0)
            {
                planeService.Remove(plane);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool planeExists(int id)
        {
            return planeService.GetAll().Any(e => e.ID == id);
        }
    }
}
