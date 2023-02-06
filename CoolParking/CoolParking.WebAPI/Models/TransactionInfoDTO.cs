namespace CoolParking.WebAPI.Models;

public record class TransactionInfoDTO(decimal Sum, DateTime Time, string Type, string VehicleId)
{}
