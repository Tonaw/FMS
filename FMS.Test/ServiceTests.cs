
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

        

    }
}
