using System;
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
            var check = svc.AddVehicle("Toyota", "Camry", 2020, "8778997UI", "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");
            var check1 = svc.AddVehicle("Volkswagen", "Golf", 2009, "8778997UI", "Diesel", "Auto", 220, 4, new System.DateTime(2022-11-19), "");
            
            //assert
            Assert.NotNull(check); // this vehicle should have been added correctly
            Assert.Null(check1); // this vehicle should NOT have been added due to the regNo duplicate
        }


        [Fact]
        public void Vehicle_Input_Check() //Testing for Vehicle Creation
        {
            //act
            var add = svc.AddVehicle("Toyota", "Camry", 2020, "8778997UI", "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");
            
            //retrieve
            var s = svc.GetVehicle(add.Id);

            //assert - checks that Vehicle isn't null
            Assert.NotNull(s);

            // now assert that the properties were set properly
            Assert.Equal(s.Id, s.Id);
            Assert.Equal("Toyota", s.Make);
            Assert.Equal("Camry", s.Model);
            Assert.Equal(2020, s.Year);
            Assert.Equal("8778997UI", s.RegistrationNo);
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
            var exist = svc.AddVehicle("Toyota", "Camry", 2020, "8778997UI", "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");
            
            // act - create a copy and update any vehiclle properties (except Id) 
            var u = new Vehicle{
                Id = exist.Id,
                Make = "Toyota",
                Model = "Corolla",
                Year = 2019,
                RegistrationNo = "98988888YU",
                FuelType = "Petrol",
                Transmission = "Auto",
                CC = 220,
                NoofDoors = 4,
                MOTDue = new System.DateTime(2020-09-29),
                CarPhotoUrl = ""
            };

            // save updated Vehicle
            svc.UpdateVehicle(u);

            // reload updated Vehicle from database into us
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
            var exist = svc.AddVehicle("Toyota", "Camry", 2020, "8778997UI", "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");
            var exist2 = svc.AddVehicle("Merc", "S-Class", 2021, "8778997UI", "Diesel", "Auto", 3890, 4, new System.DateTime(2020-09-19), "");
            
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
            var s = svc.AddVehicle("Toyota", "Camry", 2020, "8778997UI", "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");


            var ns = svc.GetVehicle(s.Id);

            // assert
            Assert.NotNull(ns);
            Assert.Equal(1, ns.Id);
        }

        [Fact]
        public void Vehicle_GetVehicle_FindVehicleWithReg_ShouldReturnNotNull()
        {
            // act 
            var s = svc.AddVehicle("Toyota", "Camry", 2020, "8778997UI", "Petrol", "Manual", 330, 4, new System.DateTime(2020-09-29), "");

            var vehicle = svc.GetVehicleByRegNo(s.RegistrationNo);

            //assert
            Assert.NotNull(vehicle);
        }

        [Fact]
        public void Vehicle_CheckVehicleExists_DeleteAndConfirm_ShouldReturnNull()
        {
            //act
            var s = svc.AddVehicle("Toyota", "Camry", 2020, "8778997UI", "Petrol", "Manual", 330, 4, Convert.ToDateTime(2020-09-29), "");

            var deleted = svc.DeleteVehicle(s.Id);

            var check = svc.GetVehicle(s.Id);

            //assert
            Assert.True(deleted); //Confirm deletion
            Assert.Null(check); //Check that vehicle is not longer in Vehicle List

        }

        //=====Tests for MOT cases======

       
        [Fact]
        public void Mot_CheckViewOfMultipleMots_ShouldReturnTheCount()
        {
            var vehicle = svc.AddVehicle("Toyota", "Camry", 2020, "86557YT", "Petrol", "Manual", 330, 4, Convert.ToDateTime(2020-09-29), "");
            
            var mot = svc.CreateMot(vehicle.Id, "All good", "Donald", "Pass", 56000);
            var mot2 = svc.CreateMot(vehicle.Id, "Wrong injectors",  "Donald", "Pass", 56000);

            //Assert
            Assert.Equal(2, vehicle.Mot.Count);

        }

        [Fact]
        public void Mot_CheckValueInputs_ShouldReturn()
        {
            var vehicle = svc.AddVehicle("Toyota", "Camry", 2020, "86557YT", "Petrol", "Manual", 330, 4, Convert.ToDateTime(2020-09-29), "");
            
            var mot = svc.CreateMot(vehicle.Id, "All good", "Donald", "Pass", 56000);
            var mot2 = svc.CreateMot(vehicle.Id, "Wrong injectors",  "Donald", "Pass", 56000);

            var s = svc.GetMot(vehicle.Id);

            //Assert
            Assert.Equal("All good", mot.TestReport);
            Assert.Equal("Donald", mot.TesterName);
            Assert.Equal("Pass", mot.TestStatus);
            Assert.Equal(56000, mot.Mileage);
        }


        
        [Fact] 
        public void Mot_DeleteMot_WhenExists_ShouldReturnTrue()
        {
            // arrange
            var s = svc.AddVehicle("Toyota", "Camry", 2020, "86557YT", "Petrol", "Manual", 330, 4, Convert.ToDateTime(2020-09-29), "");
            var t = svc.CreateMot(s.Id, "Wrong injectors",  "Donald", "Pass", 56000);

            // act
            var deleted = svc.DeleteMot(s.Id);     // delete mot    
            
            // assert
            Assert.True(deleted);                    // mot should be deleted
        }   

        [Fact] 
        public void Mot_DeleteMot_WhenNonExistant_ShouldReturnFalse()
        {
            // arrange
            //No Vehicle or Mot created

            // act
            var deleted = svc.DeleteMot(1);     // delete non-existent mot    
            
            // assert
            Assert.False(deleted);                  // mot should not be deleted
        }  

        
        //  ================= Different User Profile Tests ===========================
        

        
        [Fact] // --- Register Valid User test
        public void User_Register_WhenValid_ShouldReturnUser()
        {
            // arrange 
            var reg = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
            
            // act
            var user = svc.GetUserByEmail(reg.Email);
            
            // assert
            Assert.NotNull(reg);
            Assert.NotNull(user);
        } 

        [Fact] // --- Register Duplicate Test
        public void User_Register_WhenDuplicateEmail_ShouldReturnNull()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
            
            // act
            var s2 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);

            // assert
            Assert.NotNull(s1);
            Assert.Null(s2);
        } 

        [Fact] // --- Authenticate Invalid Test
        public void User_Authenticate_WhenInValidCredentials_ShouldReturnNull()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "guest");
            // assert
            Assert.Null(user);

        } 

        [Fact] // --- Authenticate Valid Test
        public void User_Authenticate_WhenValidCredentials_ShouldReturnUser()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "admin");
            
            // assert
            Assert.NotNull(user);
        } 



    }
}
