using System.Text.RegularExpressions;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.WebAPI.Infrastructure;
using CoolParking.WebAPI.Models;
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
    public IEnumerable<VehicleDTO> GetAll() => new MyAutoMapper<Vehicle, VehicleDTO>().Map<IEnumerable<Vehicle>, IEnumerable<VehicleDTO>>(_parkingService.GetVehicles());

    [HttpGet("{id}")]
    public ActionResult<VehicleDTO> GetVehicle(string id)
    {
        if (!IsValidId(id))
            return BadRequest("Invalid id");

        try
        {
            var vehicle = new MyAutoMapper<Vehicle, VehicleDTO>().Map(_parkingService.GetVehicle(id));
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult<VehicleDTO> PostVehicle([FromBody]VehicleDTO vehicleDTO)
    {
        try
        {
            var vehicle = new MyAutoMapper<VehicleDTO, Vehicle>().Map(vehicleDTO);
            _parkingService.AddVehicle(vehicle);
            return Created($"vehicles/{vehicle.Id}", vehicleDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteVehicle(string id)
    {
        if (!IsValidId(id))
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

    private static bool IsValidId(string id)
    {
        if (Regex.IsMatch(id, @"[A-Z]{2}-\d{4}-[A-Z]{2}"))
        {
            return true;
        }
        return false;
    }

}
