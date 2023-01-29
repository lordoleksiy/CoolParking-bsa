using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolParking.BL.Models;

namespace CoolParking.PL.Models;


// decided to create to avoid trouble while put request
internal class VehicleViewModel
{
    public string Id { get; set; }
    public VehicleType VehicleType { get; set; }
    public decimal Balance { get; set; }

    public override string ToString()
    {
        return $"{Id}: {VehicleType}| {Balance}";
    }
}
