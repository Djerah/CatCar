using CatCar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatCar.ViewModels
{
    public class BookingViewModel
    {
        public IEnumerable<SelectListItem> Cars { get; set; }
        public int SelectedCar { get; set; }
    }
}
