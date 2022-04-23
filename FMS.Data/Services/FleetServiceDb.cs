using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using FMS.Data.Models;
using FMS.Data.Repository;
using FMS.Data.Security;

namespace FMS.Data.Services
{
    public class FleetServiceDb : IFleetService
    {
        private readonly DataContext db;

        public FleetServiceDb()
        {
            db = new DataContext();
        }

        public void Initialise()
        {
            db.Initialise(); // recreate database
        }
  

        // ==================== Fleet Management ==================
       
        // implement IFleetService methods here

        // All needed operations for Vehicle Management

                // retrieve list of Vehicles
        public IList<Vehicle> GetVehicles()
        {
            return db.Vehicles.ToList();
        }

        // Retrive vehicle by Id and related MOT
        public Vehicle GetVehicle(int id)
        {
            return db.Vehicles
                     .Include(s => s.Mot)
                     .FirstOrDefault(s => s.Id == id);
        }

        

        // Add a new vehicle checking registration number is unique
        public Vehicle AddVehicle(string make, string model, int year,
                                    string regNo, string fueltype, string transmission, int cc, 
                                    int noofdoors, DateTime motdue, string carphotourl)
        {
            
            // check if Vehicle with email exists            
            var exists = GetVehicleByRegNo(regNo);
            if (exists != null)
            {
                return null;
            }


            // create new vehicle
            var s = new Vehicle
            {
                Make = make,
                Model = model,
                Year = year,
                RegistrationNo = regNo,
                FuelType = fueltype,
                Transmission = transmission,
                CC = cc,
                NoofDoors = noofdoors,
                MOTDue = motdue,
                CarPhotoUrl = carphotourl
            };

            db.Vehicles.Add(s); // add Vehicle to the list
            db.SaveChanges();
            return s; // return newly added Vehicle
        }

        public Vehicle GetVehicleByRegNo(string registrationNo)
        {
            return db.Vehicles.FirstOrDefault(s => s.RegistrationNo == registrationNo);
        }


        // Delete the Vehicle identified by Id returning true if 
        // deleted and false if not found
        public bool DeleteVehicle(int id)
        {
            var s = GetVehicle(id);
            if (s == null)
            {
                return false;
            }

            db.Vehicles.Remove(s); 
            db.SaveChanges();
            return true;
        }


        // Update the Vehicle with the details in updated 
        public Vehicle UpdateVehicle(Vehicle updated)
        {
            

            // verify the Vehicle exists
            var vehicle = GetVehicle(updated.Id);
            if (vehicle == null)
            {
                return null;
            }
            // update the details of the Vehicle retrieved and save
                vehicle.Make = updated.Make;
                vehicle.Year = updated.Year;
                vehicle.Model = updated.Model;
                vehicle.RegistrationNo = updated.RegistrationNo;
                vehicle.FuelType = updated.FuelType;
                vehicle.Transmission = updated.Transmission;
                vehicle.CC = updated.CC;
                vehicle.NoofDoors = updated.NoofDoors;
                vehicle.MOTDue = updated.MOTDue;
                vehicle.CarPhotoUrl = updated.CarPhotoUrl;
                
            db.SaveChanges();
            return vehicle;
        }

        //Checking for Vehicle Duplicates by Vehicle Registration Number
        public bool IsDuplicateVehicleReg(string regNo, int vehicleId) 
        {
            var existing = GetVehicleByRegNo(regNo);
            // if a Vehicle with email exists and the Id does not match
            return existing != null && vehicleId != existing.Id;           
        }


        // All needed operations for MOT Management


        //Creation of new MOT
        public Mot CreateMot(int id, string testReport, string testername, string teststatus, int mileage)
        {

        
            var vehicle = GetVehicle(id);
            if (vehicle == null) return null;



            var mot = new Mot
            {
                // Id created by Database
                TestReport = testReport,        
                VehicleId = id,
                TesterName = testername,
                TestStatus = teststatus,
                Mileage = mileage,
                // set by default in model but we can override here if required
                DateOfMOT = DateTime.Now,
            };

            db.Mots.Add(mot);
            db.SaveChanges(); // write to database
            return mot;
        }

    
        //Retrival of MOT with its parent vehicle
        public Mot GetMot(int id)
        {
            // return Mot and related Vehicle or null if not found
            return db.Mots
                     .Include(t => t.Vehicle)
                     .FirstOrDefault(t => t.Id == id);
        }

        //Deletion of MOT from its parent vehicle and dB
        public bool DeleteMot(int id)
        {
            // find Mot
            var mot = GetMot(id);
            if (mot == null) return false;
            
            // remove MOT 
            var result = db.Mots.Remove(mot);

            db.SaveChanges();
            return true;
        }

        // Retrieve all MOTs and their corresponding vehicles
        public IList<Mot> GetAllMots()
        {
            return db.Mots
                     .Include(t => t.Vehicle)
                     .ToList();
        }

        // perform a search of the MOTs based on a query and
        // perform a search of the vehicles based on a query



        // ==================== User Authentication/Registration Management ==================
        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
        }

        public User Register(string name, string email, string password, Role role)
        {
            // check that the user does not already exist (unique user name)
            var exists = GetUserByEmail(email);
            if (exists != null)
            {
                return null;
            }

            // Custom Hasher used to encrypt the password before storing in database
            var user = new User 
            {
                Name = name,
                Email = email,
                Password = Hasher.CalculateHash(password),
                Role = role   
            };
   
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }

        public User GetUserByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

    }
}
