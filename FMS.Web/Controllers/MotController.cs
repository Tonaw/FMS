using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using FMS.Data.Models;
using FMS.Data.Services;
using FMS.Web.Models;

namespace FMS.Web.Controllers
{
    [Authorize]
    public class MotController : BaseController
    {
        private readonly IFleetService svc;
        public MotController()
        {
            svc = new FleetServiceDb();
        } 

        // GET /Mot/index
        public IActionResult Index()
        {
            // return open Mots
            var mots = svc.GetAllMots();

            return View(mots);
        }
     
        
        // public IActionResult Search(MotSearchViewModel m)
        // {                  
        //     // TBC - perform query using values in view model and assign 
        //     //       results to viewmodel Mots property
        //     m.Mots = svc.SearchMots(m.Range, m.Query);

        //     // TBC -- return the View and pass the viewmodel as a param
        //     return View(m);
        // }        
             
        // GET/mot/{id}
        public IActionResult Details(int id)
        {
            var mot = svc.GetMot(id);
            if (mot == null)
            {
                Alert("MOT Not Found", AlertType.warning);  
                return RedirectToAction(nameof(Index));             
            }

            return View(mot);
        }

        // POST /Mot/close/{id}
        [HttpPost]
        [Authorize(Roles="admin,manager")]
        public IActionResult Delete(int id)
        {
            // close Mot via service
            var mot = svc.DeleteMot(id); // TBC add resolution from the model */ ;
            if (mot == null)
            {
                Alert("MOT Not Found", AlertType.warning);                               
            }
            else
            {
                Alert($"MOT {id } Deleted", AlertType.info);  
            }

            // redirect to the index view
            return RedirectToAction(nameof(Index));
        }
       
        //GET /Mot/create
        [Authorize(Roles="admin,manager")]
        public IActionResult Create()
        {
            var vehicles = svc.GetVehicles();
            // populate viewmodel select list property
            var mvm = new MotCreateViewModel {
               Vehicle  = new SelectList(vehicles,"Id","Make") 
            };
            
            // render blank form
            return View( mvm );
        }
       
        // POST /Mot/create
        [HttpPost]
        [Authorize(Roles="admin,manager")]
        public IActionResult Create(MotCreateViewModel mvm)
        {
            if (ModelState.IsValid)
            {
                svc.CreateMot(mvm.VehicleId, mvm.TestReport, mvm.TesterName, mvm.TestStatus, mvm.Mileage);
     
                Alert($"MOT Created", AlertType.info);  
                return RedirectToAction(nameof(Index));
            }
            
            // redisplay the form for editing
            return View(mvm);
        }

    }
}
