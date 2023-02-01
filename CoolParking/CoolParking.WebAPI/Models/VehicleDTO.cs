using System.Text.RegularExpressions;
using CoolParking.BL.Models;

namespace CoolParking.WebAPI.Models;

public record VehicleDTO(string Id, VehicleTypeDTO VehicleType, decimal Balance){}
