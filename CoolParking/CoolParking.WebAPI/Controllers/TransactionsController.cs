using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.WebAPI.Infrastructure;
using CoolParking.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoolParking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController: ControllerBase
{
    private readonly IParkingService _parkingService;
    public TransactionsController(IParkingService service)
    {
        _parkingService = service;
    }

    [HttpGet("last")]
    public ActionResult<TransactionInfoDTO[]> GetLast() => Ok(new MyAutoMapper<TransactionInfo, TransactionInfoDTO>().Map<TransactionInfo[], TransactionInfoDTO[]>(_parkingService.GetLastParkingTransactions()));

    [HttpGet("all")]
    public ActionResult<string> GetAll() 
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
    public ActionResult<VehicleDTO> TopUp([FromBody]TopUpVehicleDTO vehicleDTO)
    {
        try
        {
            _parkingService.TopUpVehicle(vehicleDTO.Id, vehicleDTO.Balance);
            return Ok(new MyAutoMapper<Vehicle, VehicleDTO>().Map(_parkingService.GetVehicle(vehicleDTO.Id)));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }


}
