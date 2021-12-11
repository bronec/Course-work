using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineSystem.Models
{
    public class Route
    {
        public int ID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Продолжительность")]
        public int Duration { get; set; }

        [Display(Name = "Место прибытия")]
        [ForeignKey("PickUp")]
        public int? PickUpID { get; set; }

        [Display(Name = "Место отбытия")]
        [ForeignKey("DropOff")]
        public int? DropOffID { get; set; }

        //Navigation Properities
        [Display(Name = "Место прибытия")]
        public Station PickUp { get; set; }
        [Display(Name = "Место отбытия")]
        public Station DropOff { get; set; }
        public virtual List<Trip> Trips { get; set; }

        public override string ToString()
        {
            return $"{PickUp} => {DropOff} ";
        }
    }
}
