using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FMS.Data.Models;
using FMS.Data.Services;

using FMS.Web.Models;

namespace FMS.Web.Controllers;

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

    //GET //Vehicles/details/{vehicle id}

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


    




}