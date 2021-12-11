using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineSystem.Models
{
    
    public class Plane
    {

        public int ID { get; set; }

        [Required]
        [RegularExpression("[A-Z]{3}[0-9]{3}")]
        [Display(Name = "Самолёт")]
        public string PlaneNum { get; set; }

        [Required]

        [EnumDataType(typeof(Category))]
        [Display(Name = "Класс")]
        public Category Category { get; set; }

        [Required]
        [Range(1,60)]
        [Display(Name = "Емкость")]
        public int Capacity{ get; set; }

        //Navigation Properities
        public virtual List<Trip> Trips { get; set; }
    }
}
