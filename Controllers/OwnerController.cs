using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatCar.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;
using CatCar.Models;

namespace CatCar.Controllers
{
    public class OwnerController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _iweb;


        public OwnerController(ApplicationDbContext context, IWebHostEnvironment iweb)
        {
            _context = context;
            _iweb = iweb;
        }

        [Authorize(Policy = "writepolicy")]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var displayimages = _context.Car.Where(l => l.OwnerId == userId).ToList();
            return View(displayimages);
        }

        [Authorize(Policy = "writepolicy")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
    }
}
