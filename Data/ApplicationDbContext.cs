using System;
using System.Collections.Generic;
using System.Text;
using CatCar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatCar.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       
        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define composite key.
            builder.Entity<Car>()
                .HasKey(lc => new { lc.Id, lc.CarNo });
        }*/

        public DbSet<Car> Car { get; set; }
        public DbSet<Rental> Rental { get; set; }
    }
}
