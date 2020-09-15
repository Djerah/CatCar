using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CatCar.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Display(Name="Car No.")]
        public string CarNo { get; set; }

        [Display(Name = "Car Name")]
        public string CarName { get; set; }

        public string Location { get; set; }

        [Display(Name = "Booking Fee(RM)")]
        public decimal RentalFee { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Owner Id")]
        public string OwnerId { get; set; }

        [Display(Name ="Time Returned")]
        public DateTime TimeReturned { get; set; }
    }
}
