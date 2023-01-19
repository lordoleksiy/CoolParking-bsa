using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolParking.BL.Models;

namespace CoolParking.PL
{
    public class CommandController
    {
        private string _command;
         public void Info()
         {
            Console.WriteLine("Choose command from the list:\n\n" +
                "\t/balance - Parking Balance\n" +
                "\t/curbalance - Parking balance during current session\n" +
                "\t/available - Amount of available places\n" +
                "\t/curtransactions - Parking transactions during session\n" +
                "\t/history - History of parking transactions\n" +
                "\t/vehicles - List of vehicles on parking\n" +
                "\t/add - Add vehicle to parking\n" +
                "\t/take - Take vehicle from parking\n" +
                "\t/topup - Top up vehicle balance");
         }

        public void Hello()
        {
            Console.WriteLine("Welcome to Cool Parking. Enter commands to do some operations.\n" +
                "To show a list of commands enter /help. To exit enter /exit. ");
        }

        public string getCommand() {
            Console.Write("Enter here: ");
            _command = Console.ReadLine();
            return _command;
        }


        public void PrintVehicle(Vehicle vehicle)
        {
            Console.WriteLine($"{vehicle.Id}: {vehicle.Balance}|{vehicle.VehicleType}");
        }

        public (string Balance, string VehicleType) parseVehicle()
        {

            Console.Write("Choose type of your vehicle (PassengerCar,\r\n    Truck,\r\n    Bus,\r\n    Motorcycle): ");
            var type = Console.ReadLine();
            Console.Write("Enter balance for your vehicle: ");
            var balance = Console.ReadLine();
            return (balance, type);
        }
    }
}
