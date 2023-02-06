using CoolParking.BL.Models;
using CoolParking.WebAPI.Models;

namespace CoolParking.PL;

public class ParkingController
{
    
    private bool isOpen = true;
    private readonly Client client = new();

    public void Start()
    {
        Hello();
        
        while (isOpen)
        {  
            string command = GetCommand();
            switch (command)
            {
                case "/get":
                    GetVehicle();
                    break;
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
                default:
                    Console.WriteLine("No such command");
                    break;
            }
        }
    }

    private void GetVehicle()
    {
        Console.Write("Enter vehicle id: ");
        var id = Console.ReadLine();
        var res = client.Get<VehicleDTO>($"vehicles/{id}");
        Console.WriteLine(res);
    }

    private void GetBalance()
    {
        var res = client.Get<decimal>("parking/balance");
        Console.WriteLine($"Current parking balance: {res}");
    }

    private void ShowVehicles()
    {
        var vehicles = client.Get<IEnumerable<VehicleDTO>>("vehicles");
        if (vehicles == null || !vehicles.Any())
        {
            Console.WriteLine("No vehicles...");
            return;
        }

        Console.WriteLine("List of vehicles: ");
        vehicles.ToList().ForEach(v => Console.WriteLine(v));
    }

    private void AddVehicle()
    {
        VehicleTypeDTO type;
        decimal balance;
        Console.Write("Choose type of your vehicle (PassengerCar,\r\n    Truck,\r\n    Bus,\r\n    Motorcycle): ");
        var typestr = Console.ReadLine();

        switch (typestr)
        {
            case "PassengerCar":
                type = VehicleTypeDTO.PassengerCar; break;
            case "Truck":
                type = VehicleTypeDTO.Truck; break;
            case "Bus":
                type = VehicleTypeDTO.Bus; break;
            case "Motorcycle":
                type = VehicleTypeDTO.Motorcycle; break;
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

        var vehicle = new VehicleDTO(Vehicle.GenerateRandomRegistrationPlateNumber(), type, balance);
        var res = client.Post("vehicles", vehicle);
        Console.Write("Your vehicle: ");
        Console.WriteLine(res);

    }

    private void TopUpVehicle()
    {
        Console.Write("Enter id of vehicle: ");
        var id = Console.ReadLine();
        Console.Write("Enter amount of topup: ");
        var money = Console.ReadLine();

        if (!decimal.TryParse(money, out decimal amount))
        {
            Console.WriteLine("Wrong Input!");
            return;
        }
        var model = new VehicleDTO(id!, null, amount);
   
        var res = client.Put("transactions/topUpVehicle", model);
        Console.WriteLine(res);
    }

    private void ShowHistory()
    {
        var transactions = client.Get("transactions/all");
        Console.WriteLine("History of transactions:");
        Console.WriteLine(transactions);
    }

    private void ShowCurTransactions()
    {
        var transactions = client.Get<IEnumerable<TransactionInfo>>("transactions/last");
        if (transactions == null || !transactions.Any())
        {
            Console.WriteLine("No Transactions...");
            return;
        }
        Console.WriteLine("Last Transactions:");
        transactions.ToList().ForEach(a => Console.WriteLine(a));
    }

    private void ShowAvailable()
    {
        var freePlaces = client.Get<int>("parking/freePlaces");
        var allCount = client.Get<int>("parking/capacity");
        Console.WriteLine($"Available {freePlaces} places of {allCount}");
    }

    private void TakeOut()
    {
        Console.Write("Enter Id of vehicle: ");
        var id = Console.ReadLine();
        var res = client.Delete($"vehicles/{id}");
        Console.WriteLine(res);
    }

    private void GetFullBalance()
    {
        var res = client.Get<decimal>("parking/fullbalance");
        Console.WriteLine($"Full balance: {res}");
    }

    private static string GetCommand()
    {
        Console.Write("Enter here: ");
        var command = Console.ReadLine();
        return command!;
    }

    private static void Hello()
    {
        Console.WriteLine("Welcome to Cool Parking. Enter commands to do some operations.\n" +
            "To show a list of commands enter /help. To exit enter /exit. ");
    }

    private static void Info()
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
            "\t/topup - Top up vehicle balance\n" +
            "\t/get - Get a single vehicle by id");
    }
}
