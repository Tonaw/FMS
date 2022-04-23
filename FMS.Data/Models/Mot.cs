using System;
using System.ComponentModel.DataAnnotations;

namespace FMS.Data.Models
{

    public enum TestStatus { Pass, Fail }

    public class Mot
    {
        public int Id { get; set; }
        
        // suitable mot attributes / relationships

        
        public DateTime DateOfMOT { get; set; } = DateTime.Now;

        public string TesterName { get; set; } 

        [Required]
        public string TestStatus { get; set; }

        [Required]
        public int Mileage { get; set; }

        [Required]
        public string TestReport { get; set; }    

        public string MotClosure {get; set; }   

        public int VehicleId { get; set; }      // foreign key
        public Vehicle Vehicle { get; set; }    // navigation property

        public User User { get; set; }

    }
}
