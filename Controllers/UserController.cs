using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CatCar.Data;
using CatCar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CatCar.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

       
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var displayimages = _context.Car.ToList();
            return View(displayimages);
        }

        [Authorize]
        public IActionResult MyBookings()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var displayimages = _context.Rental.Where(l => l.OwnerId == userId).ToList();
            return View(displayimages);
        }

        [Authorize]
        public async Task<IActionResult> ReturnCar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental.FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }


        // POST: Booking/Delete/5
        [HttpPost, ActionName("ReturnCar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _context.Rental.FindAsync(id);

            var query = (from c in _context.Car
                         where c.CarNo == rental.CarNo
                         select c);
            foreach (Car c in query)
            {
                c.Availability = true;
                rental.TimeReturned = DateTime.Now;
            }

            _context.Rental.Remove(rental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyBookings));
        }


        [Authorize]
        // GET: Booking/Create
        public IActionResult CreateBooking()
        {
            LoadView();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBooking([Bind("Id,CarNo,CarName,Location,RentalFee,StartDate,EndDate,OwnerId")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                var userId = claim.Value;

                rental.OwnerId = userId;
                _context.Add(rental);

                await _context.Rental.AddAsync(rental);

                var query = (from c in _context.Car
                             where c.CarNo == rental.CarNo
                             select c);


                var dayhours = (24*(rental.EndDate.Day - rental.StartDate.Day));


                foreach (Car c in query)
                {
                    c.Availability = false;
                    rental.CarName = c.CarName;
                    rental.Location = c.Location;
                    rental.RentalFee = (c.Price * (dayhours+(rental.EndDate.Hour - rental.StartDate.Hour)));
                }

                await _context.Rental.AddAsync(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyBookings));
            }

            return View(rental);
        }


        private void LoadView()
        {
            var query = from m in _context.Car
                        orderby m.CarNo
                        select m;
            List<Car> allCars = query.Where(x => x.Availability == true).ToList();
            SelectList allcarslist = new SelectList(allCars, "CarNo", "CarNo");
            ViewBag.AllCars = allcarslist;
        }

        

        [Authorize]
        public IActionResult BookingPayment(int? id)
        {
            return View(id);
        }

    }

}