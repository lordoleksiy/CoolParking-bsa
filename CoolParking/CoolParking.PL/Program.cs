using System.Reflection;
using System.Runtime.CompilerServices;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.BL.Services;
using CoolParking.PL;


ParkingController parkingController = new();
parkingController.Start();
//try
//{
    //Vehicle vehicle = new (Vehicle.GenerateRandomRegistrationPlateNumber(), VehicleType.Truck, 100);

    //parking.AddVehicle(vehicle);
    //var vehicles = parking.GetVehicles();    
    //foreach (var v in vehicles)
    //{
    //    printVehicle(v);
    //}
//}
//catch (ArgumentException e)
//{
//    Console.WriteLine(e.Message);
//}
