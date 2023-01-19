﻿// TODO: implement struct TransactionInfo.
//       Necessarily implement the Sum property (decimal) - is used in tests.
//       Other implementation details are up to you, they just have to meet the requirements of the homework.


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