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
            var check = svc.AddVehicle("Toyota", "Camry", 2020, 8, "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");
            var check1 = svc.AddVehicle("Volkswagen", "Golf", 2009, 84, "Diesel", "Auto", 220, 4, new System.DateTime(2022-11-19), "");
            
            //Add MOT

            svc.CreateMot(check.Id, "All good",  "Donald", "Pass", 56000);

            svc.CreateMot(check1.Id, "Bad brakes",  "Kyle", "Fail", 56000);

            // add users
            var u1 = svc.Register("Guest", "guest@sms.com", "guest", Role.guest);
            var u2 = svc.Register("Administrator", "admin@sms.com", "admin", Role.admin);
            var u3 = svc.Register("Manager", "manager@sms.com", "manager", Role.manager);
  

        }
    }
}
