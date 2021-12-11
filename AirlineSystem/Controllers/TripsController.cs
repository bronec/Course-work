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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using Microsoft.AspNetCore.Authorization;


namespace AirlineSystem.Controllers
{
    [Authorize(Roles = "Employee")]

    public class TripsController : Controller
    {
        private readonly IRepository<Trip> _tripService;
        private readonly IRepository<Plane> _planeService;
        private readonly IRepository<Route> _routeService;

        public TripsController(IRepository<Trip> tripService, IRepository<Plane> planeService, IRepository<Route> routeService)
        {
            _tripService = tripService;
            _planeService = planeService;
            _routeService = routeService;
        }

        #region CRUD

        public IActionResult Index()
        {
            return View(_tripService.GetAll());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var trip = _tripService.Details((int)id);
            if (trip == null)
                return NotFound();

            return View(trip);
        }

        public IActionResult Create()
        {
            this.Get_ForeignObjects();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,StartDateTime,Price,AvailableSeats,RouteID,PlaneID")] Trip trip)
        {
            this.ValidateTrip(trip);
            if (ModelState.IsValid)
            {
                
                Trip.GenerateAvailableSeats(trip, _planeService.Details(trip.PlaneID).Capacity);
                _tripService.Add(trip);

                return RedirectToAction(nameof(Index));
            }

            this.Get_ForeignKey(trip);
            return View(trip);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var trip = _tripService.Details((int)id);
            if (trip == null)
                return NotFound();

            this.CanEdit(trip);
            this.Get_ForeignKey(trip);
            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,StartDateTime,Price,AvailableSeats,RouteID,PlaneID")] Trip trip)
        {
            if (id != trip.ID)
                return NotFound();

            /*
             if (_tripService.Details(id).PlaneID != trip.PlaneID)
             this.ValidatePlane(trip.PlaneID, trip.StartDateTime);*/

            this.ValidateDate(trip.StartDateTime);

            if (ModelState.IsValid)
            {
                try
                {
                    //Trip.GenerateAvailableSeats(trip, _PlaneService.Details(trip.PlaneID).Capacity);


                    _tripService.Update(trip);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            this.Get_ForeignKey(trip);
            return View(trip);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var trip = _tripService.Details((int)id);
            if (trip == null)
                return NotFound();

            this.CanEdit(trip);
            return View(trip);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Trip trip = _tripService.Details(id);
            this.ValidateDate(trip.StartDateTime);

            if (ModelState.IsValid)
            {
                if (trip.Tickets != null)
                {
                    _tripService.Remove(trip);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(trip);
        }

        #endregion

        #region Methods

        #region ViewBags

        private void Get_ForeignKey(Trip trip)
        {
            ViewData["PlaneID"] = new SelectList(_planeService.GetAll(), "ID", "PlaneNum", trip.PlaneID);
            ViewData["RouteID"] = new SelectList(_routeService.GetAll(), "ID", "", trip.RouteID);
        }

        private void Get_ForeignObjects()
        {
            ViewData["PlaneID"] = new SelectList(_planeService.GetAll(), "ID", "PlaneNum");
            ViewData["RouteID"] = new SelectList(_routeService.GetAll(), "ID", "");
        }

        #endregion

        #region Validation

        private void ValidateTrip(Trip trip)
        {
            this.ValidateDate(trip.StartDateTime);
            this.ValidatePlane(trip.PlaneID, trip.StartDateTime);
        }




        private void ValidateDate(DateTime tripDate)
        {
            if (tripDate <= DateTime.Now)
            {
                ModelState.AddModelError("StartDateTime", "Дата не применима");
            }
        }

        private void ValidatePlane(int PlaneID, DateTime tripDate)
        {
            Plane _plane = _planeService.Details(PlaneID);
            if (_plane.Trips.Where(t => t.StartDateTime <= tripDate && t.ReturnDateTime >= tripDate).Count() != 0)
            {
                ModelState.AddModelError("PlaneID", "Автобус в это время недоступен");
            }
        }

        #endregion

        private void CanEdit(Trip trip)
        {
            string msg = "Вы не можете отредактировать или удалить эту поездку";
            ViewBag.Message = null;

            if (trip.Tickets != null)
            {
                ViewBag.Message = msg;
            }

            if (trip.AvailableSeatsCount != null)
            {
                int AvailableSeats = Convert.ToInt32(trip.AvailableSeatsCount);
                if (AvailableSeats != trip.Plane.Capacity)
                {
                    ViewBag.Message = msg;
                }
            }

            if (trip.StartDateTime <= DateTime.Now)
            {
                ViewBag.Message = msg;
            }
        }

        #endregion
    }
}
