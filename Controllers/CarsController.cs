using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CatCar.Data;
using CatCar.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace CatCar.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _iweb;

        public CarsController(ApplicationDbContext context, IWebHostEnvironment iweb)
        {
            _context = context;
            _iweb = iweb;
        }

        // GET: Cars
        [Authorize(Policy="adminpolicy")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Car.ToListAsync());
        }

        // GET: Cars/Details/5
        [Authorize(Policy = "adminpolicy")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        [Authorize(Policy = "adminpolicy")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Imgname,Imgpath,Id,CarNo,CarName,Type,Location,Price,Availability,OwnerId")] Car car, IFormFile fileobj)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                var userId = claim.Value;

                var uploadimg = Path.Combine(_iweb.WebRootPath, "Images", fileobj.FileName);
                var stream = new FileStream(uploadimg, FileMode.Create);
                await fileobj.CopyToAsync(stream);
                stream.Close();

                car.OwnerId = userId;
                car.Imgpath = uploadimg;
                car.Imgname = fileobj.FileName;

                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

       
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "adminpolicy")]
        public async Task<IActionResult> Edit([Bind("Id,Imgname,Imgpath,CarNo,CarName,Type,Location,Price,Availability,OwnerId")] IFormFile fileobj, Car c, int id)
        {
            if (ModelState.IsValid)
            {

                var getimagedetails = await _context.Car.FindAsync(id);
                _context.Car.Remove(getimagedetails);
                 
                FileInfo fi = new FileInfo(Path.Combine(_iweb.WebRootPath, "Images", getimagedetails.Imgname));
                System.IO.File.Delete(Path.Combine(_iweb.WebRootPath, "Images", getimagedetails.Imgname));
                fi.Delete();
               

                    var uploadimg = Path.Combine(_iweb.WebRootPath, "Images", fileobj.FileName);
                    var stream = new FileStream(uploadimg, FileMode.Create);
                    await fileobj.CopyToAsync(stream);
                    stream.Close();

                    c.Imgpath = uploadimg;
                    c.Imgname = fileobj.FileName;
                    _context.Update(c);
                    await _context.SaveChangesAsync();
                

            }
            return RedirectToAction("Edit");
        }

        // GET: Cars/Delete/5
        [Authorize(Policy = "adminpolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Car.FindAsync(id);
            _context.Car.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.Id == id);
        }
    }
}
