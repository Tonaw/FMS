
using Xunit;
using FMS.Data.Models;
using FMS.Data.Services;

namespace FMS.Test
{

    public class ServiceTests
    {
        private readonly IFleetService svc;


        public ServiceTests()
        {
            // general arrangement
            svc = new FleetServiceDb();
          
            // ensure data source is empty before each test
            svc.Initialise();
        }

        // ========================== Fleet Tests =========================

        [Fact]
        public void VehicleCreate_Check_If_Duplicate()
        {
        
            // ensure data source is empty before each test
            //act
            var check = svc.AddVehicle("Toyota", "Camry", 2020, 8, "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");
            var check1 = svc.AddVehicle("Volkswagen", "Golf", 2009, 84, "Diesel", "Auto", 220, 4, new System.DateTime(2022-11-19), "");
            
            //assert
            Assert.NotNull(check); // this vehicle should have been added correctly
            Assert.Null(check1); // this vehicle should NOT have been added due to the regNo duplicate
        }


        [Fact]
        public void Vehicle_Input_Check()
        {
            //act
            var add = svc.AddVehicle("Toyota", "Camry", 2020, 8, "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");
            
            //retrieve
            var s = svc.GetVehicle(add.Id);

            //assert - checks that student isn't null
            Assert.NotNull(s);

            // now assert that the properties were set properly
            Assert.Equal(s.Id, s.Id);
            Assert.Equal("Toyota", s.Make);
            Assert.Equal("Camry", s.Model);
            Assert.Equal(2020, s.Year);
            Assert.Equal(8, s.RegistrationNo);
            Assert.Equal("Petrol", s.FuelType);
            Assert.Equal("Manual", s.Transmission);
            Assert.Equal(330, s.NoofDoors);
            Assert.Equal(new System.DateTime(2020-09-29), s.MOTDue);
            Assert.Equal("", s.CarPhotoUrl);

        }

        // write suitable tests to verify operation of the fleet service
        
        [Fact]
        public void Vehicle_Update_Existing()
        {
            // arrange - create test vehicle
            var exist = svc.AddVehicle("Toyota", "Camry", 2020, 8, "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");
            
            // act - create a copy and update any vehiclle properties (except Id) 
            var u = new Vehicle{
                Id = exist.Id,
                Make = "Merc",
                Model = "S-class",
                Year = 2019,
                RegistrationNo = 7,
                FuelType = "Petrol",
                Transmission = "Auto",
                CC = 220,
                NoofDoors = 4,
                MOTDue = new System.DateTime(2020-09-29),
                CarPhotoUrl = ""
            };

            // save updated Vehicle
            svc.UpdateVehicle(u);

            // reload updated student from database into us
            var us = svc.GetVehicle(exist.Id);

            // assert
            Assert.NotNull(u);

            // now assert that the properties were set properly           
            Assert.Equal(u.Id, us.Id);
            Assert.Equal(u.Make, us.Make);
            Assert.Equal(u.Model, us.Model);
            Assert.Equal(u.Year, us.Year);
            Assert.Equal(u.RegistrationNo, us.RegistrationNo);
            Assert.Equal(u.FuelType, us.FuelType);
            Assert.Equal(u.Transmission, us.Transmission);
            Assert.Equal(u.CC, us.CC);
            Assert.Equal(u.NoofDoors, us.NoofDoors);
            Assert.Equal(u.MOTDue, us.MOTDue);
            Assert.Equal(u.CarPhotoUrl, us.CarPhotoUrl);
        }

        [Fact] 
        public void Vehicle_GetAllVehicle_SinceNone_ShouldReturn0()
        {
            // act 
            var vehicles = svc.GetVehicles();
            var count = vehicles.Count; //counts existing vehicles

            // assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void Vehicle_GetAllVehicles_SinceTwoExists_ShouldReturn2()
        {
            // arrange
            var exist = svc.AddVehicle("Toyota", "Camry", 2020, 87789977, "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");
            var exist2 = svc.AddVehicle("Merc", "S-Class", 2021, 77776768, "Diesel", "Auto", 3890, 4, new System.DateTime(2020-09-19), "");
            
            // act
            var vehicles = svc.GetVehicles();
            var count = vehicles.Count;

            // assert
            Assert.Equal(2, count);
        }

         [Fact] 
        public void Vehicle_GetVehicle_WhenNonExistent_ShouldReturnNull()
        {
            // act 
            var vehicle = svc.GetVehicle(1); // non existent vehicle

            // assert
            Assert.Null(vehicle);
        }

        [Fact]
         public void Vehicle_GetVehicle_WhenNonExistent_ShouldReturnVehicle()
        {
            // act 
            var s = svc.AddVehicle("Toyota", "Camry", 2020, 87789977, "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");


            var ns = svc.GetVehicle(s.Id);

            // assert
            Assert.NotNull(ns);
            Assert.Equal(1, ns.Id);
        }
        
            



    }
}
