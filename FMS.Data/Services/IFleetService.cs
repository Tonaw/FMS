using System;
using System.Collections.Generic;
	
using FMS.Data.Models;
	
namespace FMS.Data.Services
{
    // This interface describes the operations that a FleetService class should implement
    public interface IFleetService
    {
        void Initialise();
        
        // add suitable method definitions to implement assignment requirements

        // ---------------- Vehicle Management --------------
        IList<Vehicle> GetVehicles();
        Vehicle GetVehicle(int id);
        Vehicle AddVehicle(string make, string model, int year,
                                    int regNo, string fueltype, string transmission, int cc, 
                                    int noofdoors, DateTime motdue, string carphotourl);
        Vehicle GetVehicleByRegNo(int RegistrationNo);
        bool DeleteVehicle(int id); 
        Vehicle UpdateVehicle(Vehicle updated);
        bool IsDuplicateVehicleReg(int regNo, int vehicleId); 

        //MOT Data

        Mot CreateMot(int vehicleId, string testReport, string testername, string teststatus, int mileage);
        Mot GetMot(int id);
        bool DeleteMot(int id);
        IList<Mot> GetAllMots();
        



        // ------------- User Management -------------------
        User Authenticate(string email, string password);
        User Register(string name, string email, string password, Role role);
        User GetUserByEmail(string email);
    
    }
    
}