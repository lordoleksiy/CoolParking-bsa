using System.Diagnostics;
using System.Reflection;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.BL.Services;

namespace CoolParking.PL;

public class ParkingController
{
    private ILogService logService;
    private ITimerService widthdrawTimer;
    private ITimerService logTimer;
    private ParkingService _parkingService;
    private CommandController _commandController;
    private bool isOpen = true;

    public ParkingController()
    {
        logService = new LogService($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log");
        widthdrawTimer = new TimerService();
        logTimer = new TimerService();
        _parkingService = new(widthdrawTimer, logTimer, logService);
        _commandController = new CommandController();
    }

    public void Start()
    {
        _commandController.Hello();
        
        while (isOpen)
        {  
            string command = _commandController.getCommand();
            switch (command)
            {
                case "/history":
                    ShowHistory();
                    break;
                case "/topup":
                    TopUpVehicle();
                    break;
                case "/curbalance":
                    GetBalance();
                    break;
                case "/vehicles":
                    ShowVehicles();
                    break;
                case "/add":
                    AddVehicle();
                    break;
                case "/help":
                    _commandController.Info();
                    break;
                case "/exit":
                    isOpen = false;
                    break;
            }
        }
    }

    private void GetBalance()
    {
        Console.WriteLine($"Balance of parking during current session: {_parkingService.GetBalance()}");
    }
    private void ShowVehicles()
    {
        var vehicles = _parkingService.GetVehicles();
        if (vehicles.Count == 0)
        {
            Console.WriteLine("No vehicles in parking.");
            return;
        }
        Console.WriteLine("Vehicles: ");
        foreach ( var vehicle in vehicles )
        {
            _commandController.PrintVehicle(vehicle);
        }
    }
    private void AddVehicle()
    {
        var dataVehicle = _commandController.parseVehicle();
        VehicleType type;
        decimal balance;
        switch (dataVehicle.VehicleType)
        {
            case "PassengerCar":
                type = VehicleType.PassengerCar; break;
            case "Truck":
                type = VehicleType.Truck; break;
            case "Bus":
                type = VehicleType.Bus; break;
            case "Motorcycle":
                type = VehicleType.Motorcycle; break;
            default:
                Console.WriteLine("No such type of vehicle");
                return;
        }
        if (!decimal.TryParse(dataVehicle.Balance, out balance))
        {
            Console.WriteLine("Error while parsing balance");
            return;
        }
        try
        {
            var vehicle = new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), type, balance);
            _parkingService.AddVehicle(vehicle);
            Console.Write("Your vehicle: ");
            _commandController.PrintVehicle(vehicle);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void TopUpVehicle()
    {
        Console.Write("Enter id of vehicle: ");
        var id = Console.ReadLine();
        Console.Write("Enter amount of topup: ");
        var money = Console.ReadLine();
        try
        {
            _parkingService.TopUpVehicle(id, decimal.Parse(money));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ShowHistory()
    {
        var text = _parkingService.ReadFromLog();
        Console.WriteLine("History of transactions:");
        Console.WriteLine(text);
    }

}
