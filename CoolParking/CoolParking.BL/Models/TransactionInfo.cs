using System;

namespace CoolParking.BL.Models;

public struct TransactionInfo
{
    public decimal Sum { get; set; }
    public DateTime Time { get; set; }
    public string Type { get; set; }
    public string VehicleId { get; set; }
    public override string ToString()
    {
        return $"{Type}|{Sum}|${Time}|{VehicleId}";
    }
}