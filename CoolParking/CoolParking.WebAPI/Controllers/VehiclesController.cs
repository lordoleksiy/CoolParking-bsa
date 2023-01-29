using System.Text.RegularExpressions;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.WebAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoolParking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController: ControllerBase
{
    private readonly IParkingService _parkingService;

    public VehiclesController()
    {
        _parkingService = Data.ParkingService;
    }

    [HttpGet]
    public IEnumerable<Vehicle> GetAll() =>_parkingService.GetVehicles();

    [HttpGet("{id}")]
    public ActionResult<Vehicle> GetVehicle(string id)
    {
        if (!Regex.IsMatch(id, @"[A-Z]{2}-\d{4}-[A-Z]{2}"))
            return BadRequest("Invalid id");

        try
        {
            var vehicle = _parkingService.GetVehicle(id);
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult<Vehicle> PostVehicle([FromBody]Vehicle vehicle)
    {
        if (vehicle == null || !vehicle.IsValid())
            return BadRequest("Invalid body of request");
        try
        {
            _parkingService.AddVehicle(vehicle);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Created($"vehicles/{vehicle.Id}", vehicle);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteVehicle(string id)
    {
        if (!Regex.IsMatch(id, @"[A-Z]{2}-\d{4}-[A-Z]{2}"))
            return BadRequest("Invalid id");

        try
        {
            _parkingService.RemoveVehicle(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return StatusCode(204);
    }

}
