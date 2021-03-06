
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

using FMS.Data.Models;
using FMS.Data.Services;

namespace FMS.Web.Controllers
{
    [Authorize]
    public class VehicleController : BaseController
    {
        // provide suitable controller actions

        private IFleetService svc;

        public VehicleController()
        {
            svc = new FleetServiceDb();
        }


        //GET/ Vehicle

        public IActionResult Index()
        {
            var vehicles = svc.GetVehicles();

            return View(vehicles);
        }

    // GET //Vehicles/details/{vehicle id}

        public IActionResult Details(int id)
        {
            var s = svc.GetVehicle(id);

            if (s == null)
            {
                Alert($"Vehicle {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            //pass vehicle to view
            return View(s);
        }


        // GET: /vehicle/create

        [Authorize(Roles="admin")]
        public IActionResult Create()
        {
            //display blank form to create a vehicle
            return View();
        }


        // POST /vehicle/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="admin")]

        public IActionResult Create([Bind("Make, Model, Year, RegistrationNo, FuelType, Transmission, CC, NoofDoors, MOTDue, CarPhotoUrl")]  Vehicle s)
        {
            // check registraion number is unique for this vehicle
            if (svc.IsDuplicateVehicleReg(s.RegistrationNo, s.Id))
            {
                // add manual validation error
                ModelState.AddModelError(nameof(s.RegistrationNo),"The registration number is already in use");
            }

            // complete POST action to add vehicle
            if (ModelState.IsValid)
            {
                // pass data to service to store 
                s = svc.AddVehicle(s.Make, s.Model, s.Year, s.RegistrationNo, s.FuelType, s.Transmission, s.CC, s.NoofDoors, s.MOTDue, s.CarPhotoUrl);
                Alert($"Vehicle created successfully", AlertType.success);

                return RedirectToAction(nameof(Details), new { Id = s.Id});
            }
            
            // redisplay the form for editing as there are validation errors
            return View(s);
        }


        // GET /vehicle/edit/{id}
        [Authorize(Roles="admin,manager")]
        public IActionResult Edit(int id)
        {        
            // load the Vehicle using the service
            var s = svc.GetVehicle(id);

            // check if s is null and if so alert
            if (s == null)
            {
                Alert($"Vehicle {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }   

            // pass vehicle to view for editing
            return View(s);
        }


        // POST /vehicle/edit/{id}
        [HttpPost]
       // [ValidateAntiForgeryToken]
        [Authorize(Roles="admin,manager")]
        public IActionResult Edit(int id, [Bind("Id, Make, Model, Year, RegistrationNo, FuelType, Transmission, CC, NoofDoors, MOTDue, CarPhotoUrl")] Vehicle s)
        {
            // check email is unique for this vehicle  
            if (svc.IsDuplicateVehicleReg(s.RegistrationNo, s.Id)) {
                // add manual validation error
                ModelState.AddModelError("RegNo", "This Registration number is already registered");
            }

            // validate and complete POST action to save vehicle changes
            if (ModelState.IsValid)
            {
                // pass data to service to update
                svc.UpdateVehicle(s);      
                Alert("Vehicle updated successfully", AlertType.info);

                return RedirectToAction(nameof(Details), new { Id = s.Id });
            }

            // redisplay the form for editing as validation errors
            return View(s);
        }

                // GET / Vehicle/delete/{id}
        [Authorize(Roles="admin")]      
        public IActionResult Delete(int id)
        {       
            // load the Vehicle using the service
            var s = svc.GetVehicle(id);
            // check the returned Vehicle is not null and if so return NotFound()
            if (s == null)
            {
                // TBC - Display suitable warning alert and redirect
                Alert($"Vehicle {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }     
            
            // pass Vehicle to view for deletion confirmation
            return View(s);
        }

        // POST /Vehicle/delete/{id}
        [HttpPost]
        [Authorize(Roles="admin")]
        [ValidateAntiForgeryToken]              
        public IActionResult DeleteConfirm(int id)
        {
            // TBC delete Vehicle via service
            svc.DeleteVehicle(id);

            Alert("Vehicle deleted successfully", AlertType.info);
            // redirect to the index view
            return RedirectToAction(nameof(Index));
        }


        // ============== Vehicle with mot management ==============

        // GET /Vehicle/createMot/{id}
        public IActionResult MotCreate(int id)
        {     
            var s = svc.GetVehicle(id);
            // check the returned vehicle is not null and if so alert
            if (s == null)
            {
                Alert($"Vehicle {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            // create a mot view model and set VehicleId (foreign key)
            var mot = new Mot { VehicleId = id };

            return View( mot );
        }

        //id, string testReport, string testername, string teststatus, int mileage

        // POST /vehicle/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MotCreate([Bind("VehicleId, TestReport, TesterName, TestStatus, Mileage")] Mot t)
        {
            if (ModelState.IsValid)
            {                
                var mot = svc.CreateMot(t.VehicleId, t.TestReport, t.TesterName, t.TestStatus, t.Mileage);
                Alert($"MOT created successfully for vehicle {t.VehicleId}", AlertType.info);
                return RedirectToAction(nameof(Details), new { Id = mot.VehicleId });
            }
            // redisplay the form for editing
            return View(t);
        }

        // GET /vehicle/motdelete/{id}
        public IActionResult MotDelete(int id)
        {
            // load the Mot using the service
            var mot = svc.GetMot(id);
            // check the returned Mot is not null and if so return NotFound()
            if (mot == null)
            {
                Alert($"MOT {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }     
            
            // pass Mot to view for deletion confirmation
            return View(mot);
        }

        // POST /Vehicle/Motdeleteconfirm/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MotDeleteConfirm(int id, int VehicleId)
        {
            // delete Vehicle via service
            svc.DeleteMot(id);
            Alert($"MOT deleted successfully for vehicle {VehicleId}", AlertType.info);
            
            // redirect to the Mot index view
            return RedirectToAction(nameof(Details), new { Id = VehicleId });
        }

    }
}
        


