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
    private bool isOpen = true;

    public ParkingController()
    {
        logService = new LogService($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log");
        widthdrawTimer = new TimerService();
        logTimer = new TimerService();
        _parkingService = new(widthdrawTimer, logTimer, logService);
    }

    public void Start()
    {
        Hello();
        
        while (isOpen)
        {  
            string command = getCommand();
            switch (command)
            {
                case "/balance":
                    GetFullBalance();
                    break;
                case "/available":
                    ShowAvailable();
                    break;
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
                case "/curtransactions":
                    ShowCurTransactions();
                    break;
                case "/take":
                    TakeOut();
                    break;
                case "/help":
                    Info();
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
            PrintVehicle(vehicle);
        }
    }
    private void AddVehicle()
    {
        VehicleType type;
        decimal balance;
        Console.Write("Choose type of your vehicle (PassengerCar,\r\n    Truck,\r\n    Bus,\r\n    Motorcycle): ");
        var typestr = Console.ReadLine();

        switch (typestr)
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

        Console.Write("Enter balance for your vehicle: ");
        var balancestr = Console.ReadLine();
        
        if (!decimal.TryParse(balancestr, out balance))
        {
            Console.WriteLine("Error while parsing balance");
            return;
        }
        try
        {
            var vehicle = new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), type, balance);
            _parkingService.AddVehicle(vehicle);
            Console.Write("Your vehicle: ");
            PrintVehicle(vehicle);
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

    private void ShowCurTransactions()
    {
        var transactions = _parkingService.GetLastParkingTransactions();
        Console.WriteLine("Last Transactions:");
        transactions.ToList().ForEach(a => Console.WriteLine(a));
    }

    private void ShowAvailable()
    {
        var freePlaces = _parkingService.GetFreePlaces();
        var allCount = _parkingService.GetCapacity();
        Console.WriteLine($"Available {freePlaces} places of {allCount}");
    }

    private void TakeOut()
    {
        Console.Write("Enter your vehicleId: ");
        var vehicleId = Console.ReadLine();
        try
        {
            _parkingService.RemoveVehicle(vehicleId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void GetFullBalance()
    {
        Console.WriteLine($"Full balance: {_parkingService.GetBalanceFromFile()}");
    }

    private void PrintVehicle(Vehicle vehicle)
    {
        Console.WriteLine($"{vehicle.Id}: {vehicle.Balance}|{vehicle.VehicleType}");
    }

    private string getCommand()
    {
        Console.Write("Enter here: ");
        var command = Console.ReadLine();
        return command;
    }

    private void Hello()
    {
        Console.WriteLine("Welcome to Cool Parking. Enter commands to do some operations.\n" +
            "To show a list of commands enter /help. To exit enter /exit. ");
    }

    private void Info()
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
}
