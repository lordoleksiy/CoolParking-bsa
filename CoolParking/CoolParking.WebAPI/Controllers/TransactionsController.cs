using System.Text.RegularExpressions;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.WebAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoolParking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController: ControllerBase
{
    private readonly IParkingService _parkingService;
    public TransactionsController()
    {
        _parkingService = Data.ParkingService;
    }

    [HttpGet("last")]
    public ActionResult<TransactionInfo[]> GetLast() => Ok(_parkingService.GetLastParkingTransactions());

    [HttpGet("all")]
    public ActionResult<TransactionInfo[]> GetAll() 
    {
        try
        {
            var result = _parkingService.ReadFromLog();
            return Ok(result);
        }
        catch
        {
            return NotFound();
        }  
    }

    [HttpPut("topUpVehicle")]
    public ActionResult<Vehicle> TopUp([FromBody]Vehicle? vehicle)
    {
        if (vehicle == null || !vehicle.IsValid())
            return BadRequest("Invalid body of request");

        try
        {
            _parkingService.TopUpVehicle(vehicle.Id, vehicle.Balance);
            return Ok(_parkingService.GetVehicle(vehicle.Id));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
