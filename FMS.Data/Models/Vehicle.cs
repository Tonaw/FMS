using System;
using System.ComponentModel.DataAnnotations;
using FMS.Data.Validators; // allows access to UrlResource
using FMS.Data.Models;

namespace FMS.Data.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        
        // suitable vehicle properties/relationships

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }

       [Required]
        public int RegistrationNo { get; set; }

        [Required]
        public string FuelType { get; set; }

        [Required]
        public string Transmission { get; set; }

        [Required]
        public int CC { get; set; }

        [Required]
        public int NoofDoors { get; set; }

        [Required]
        public DateTime MOTDue { get; set; }
    
        [UrlResource]
        public string CarPhotoUrl { get; set; }

        // Relationship 1-N MOT History
        public IList<Mot> Mot {get; set; } = new List<Mot>();

    
        }

}
