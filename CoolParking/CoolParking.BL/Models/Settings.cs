// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.


using System.Collections.Generic;

namespace CoolParking.BL.Models;

public static class Settings
{
    public static decimal CurBalance { get => 0; }
    public static int MaxCapacity { get => 10; }
    public static int IntevalPayment { get => 5000; } // in milliseconds
    public static int IntervalLog { get => 60000; }
    public static decimal FineCoefficient { get => 2.5m; }
    public static IDictionary<VehicleType, decimal> Tariffs { get; } = new Dictionary<VehicleType, decimal>()
    {
        { VehicleType.PassengerCar, 2},
        { VehicleType.Motorcycle, 1},
        { VehicleType.Truck, 5},
        { VehicleType.Bus, 3.5m}
    };

}

