using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMS.Web.Models
{
    public class MotCreateViewModel
    {
        // selectlist of Vehicle (id, make)       
        public SelectList Vehicle { set; get; }

        // Collecting StudentId and Issue in Form
        [Required(ErrorMessage = "Please select a vehicle")]
        [Display(Name = "Select Vehicle")]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 5)]
        public string MotClosure {get; set; }   

        public DateTime DateOfMOT { get; set; } = DateTime.Now;

        public string TesterName { get; set; }

        [Required]
        public string TestStatus { get; set; }

        [Required]
        public int Mileage { get; set; }

        [Required]
        public string TestReport { get; set; }    

    }

}
