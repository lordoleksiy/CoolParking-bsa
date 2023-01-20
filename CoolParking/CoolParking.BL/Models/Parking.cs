using System.Collections.Generic;

namespace CoolParking.BL.Models;
public class Parking {
    private static Parking _instance;
    public IList<Vehicle> Vehicles { get; set; }
    public IList<TransactionInfo> Transactions { get; set; }
    public decimal Balance { get; set; }
    public int MaxCapacity { get; }
    public IDictionary<VehicleType, decimal> Tarrifs { get; }
    public decimal FineCoefficient { get ; }

    public Parking() {
        Vehicles = new List<Vehicle>();
        Transactions = new List<TransactionInfo>();
        Balance = Settings.CurBalance;
        MaxCapacity = Settings.MaxCapacity;
        Tarrifs = Settings.Tariffs;
        FineCoefficient= Settings.FineCoefficient;
    }
    public static Parking GetInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Parking();
            }
            return _instance;
        }
    }
}
