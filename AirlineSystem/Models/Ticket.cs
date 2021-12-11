using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineSystem.Models
{
    public class Ticket
    {
        public int ID { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name = "Количество пассажиров")]
        public int PassengerCount { get; set; }

        [Required]
        [Display(Name = "Забронированные места")]
        public string BookedSeats { get; set; }

        [Required]
        [ForeignKey("Trip")]
        [Display(Name = "Клиент ID")]
        public int TripID { get; set; }

        [Required]
        [ForeignKey("Client")]
        [Display(Name = "Поездки ID")]
        public string ClientID { get; set; }


        //Navigation Properties
        [Display(Name = "Клиент")]
        public virtual ApplicationUser Client { get; set; }
        [Display(Name = "Поездки")]
        public virtual Trip Trip { get; set; }

        
    }
}
