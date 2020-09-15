using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CatCar.Models
{
    public class Car
    {
        public string Imgname { get; set; }
        public string Imgpath { get; set; }

        [Key]
        public int Id { get; set; }
        [Required]
        public string CarNo { get; set; }
        public string CarName { get; set; }

        [Display(Name = "Brand")]
        public string Type { get; set; }
        public string Location { get; set; }

        [Display(Name ="Price/Hour(RM)")]
        public decimal Price { get; set; }
        
        [Display(Name ="Available")]
        public bool Availability { get; set; }
        public string OwnerId { get; set; }
        
    }

    
}
