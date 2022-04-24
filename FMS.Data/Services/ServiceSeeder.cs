using System;
using System.Text;
using System.Collections.Generic;
using FMS.Data.Models;

namespace FMS.Data.Services
{
    public static class FleetServiceSeeder
    {
        // use this class to seed the database with dummy test data using an IFleetService
        public static void Seed(IFleetService svc)
        {
            svc.Initialise();

            // add seed data

            //Add vehicle
            var check = svc.AddVehicle("Toyota", "Camry", 2020, "4444555UY", "Petrol", "Manual", 330, 4, Convert.ToDateTime("2023-04-23"), "https://th.bing.com/th/id/R.1ccc5e38e2df2cad5b3a506b4577ddd4?rik=BtrULevOZ2a1lw&pid=ImgRaw&r=0");
            var check1 = svc.AddVehicle("Volkswagen", "Golf", 2009, "84456721QW", "Diesel", "Auto", 220, 4, Convert.ToDateTime("2023-04-23"), "https://th.bing.com/th/id/R.9d116f4b200ab906fc0a232f496f2812?rik=yVowSf2vOH7kDQ&pid=ImgRaw&r=0");
            
            //Add MOT

            svc.CreateMot(check.Id, "All good",  "Donald", "Pass", 56000);

            svc.CreateMot(check1.Id, "Bad brakes",  "Kyle", "Fail", 56000);

            // add users
            var u1 = svc.Register("Guest", "guest@fms.com", "guest", Role.guest);
            var u2 = svc.Register("Administrator", "admin@fms.com", "admin", Role.admin);
            var u3 = svc.Register("Manager", "manager@fms.com", "manager", Role.manager);
  

        }
    }
}
