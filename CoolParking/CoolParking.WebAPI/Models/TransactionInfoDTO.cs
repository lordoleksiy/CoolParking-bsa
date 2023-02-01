namespace CoolParking.WebAPI.Models;

public record TransactionInfoDTO(decimal Sum, DateTime Time, string Type, string VehicleId)
{}
