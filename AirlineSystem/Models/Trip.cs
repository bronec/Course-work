﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.Models
{
    public class Trip
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Дата отбытия")]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "Стоимость")]
        [Range(0, Double.MaxValue)]
        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        [Display(Name = "Доступные места")]
        public string AvailableSeats { get; set; }

        [Required]
        [ForeignKey("Route")]
        [Display(Name = "Маршрут ID")]
        public int RouteID { get; set; }

        [Required]
        [ForeignKey("Plane")]
        [Display(Name = "Самолёт ID")]
        public int PlaneID { get; set; }

        //Navigation Properties
        [Display(Name = "Маршрут")]
        public Route Route { get; set; }
        [Display(Name = "Самолёт")]
        public virtual Plane Plane { get; set; }
        public virtual List<Ticket> Tickets { get; set; }

        public override string ToString()
        {
            return $"{ID} {Route} {StartDateTime}";
        }

        [NotMapped]
        public string tostringprop
        {
            get => $"{ID} {Route} {StartDateTime}";

        }

        [NotMapped]
        public string AvailableSeatsCount
        {
            get
            {
                if (AvailableSeats != null)
                    return $"{AvailableSeats.Split(",").Count()}";
                else
                    return AvailableSeats;
            }
        }

        [NotMapped]
        public string RouteToString
        {
            get
            {
                if (Route != null)
                    return Route.ToString();
                else
                    return null;
            }
        }

        [NotMapped]
        public string[] AvailableSeatsArray
        {
            get
            {
                if (AvailableSeats != null)
                    return AvailableSeats.Split(",");
                else
                    return null;
            }
        }

        [NotMapped]
        public DateTime? ReturnDateTime
        {
            get
            {
                if (this.Route != null)
                {
                    DateTime? dt = this.StartDateTime + 2 * (new TimeSpan(0, Route.Duration, 0));
                    return dt;
                }
                else
                    return null;
            }
        }


        public static void GenerateAvailableSeats(Trip trip, int capacity)
        {
            StringBuilder sb = new();
            for (int i = 1; i <= capacity; i++)
            {
                sb.Append(i);
                if(i != capacity )
                    sb.Append(',');
            }
            trip.AvailableSeats = sb.ToString();
        }

    }
}
