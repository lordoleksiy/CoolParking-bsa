using System.Diagnostics.Contracts;
using System.Reflection;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.BL.Services;
using CoolParking.WebAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoolParking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingController: ControllerBase
{
    private readonly IParkingService _parkingService;
    public ParkingController()
    {
        _parkingService = Data.ParkingService;
    }

    [HttpGet("balance")] // api/balance
    public ActionResult<decimal> GetBalance() => Ok(_parkingService.GetBalance());

    [HttpGet("capacity")]
    public ActionResult<int> GetCapacity() => Ok(_parkingService.GetCapacity());

    [HttpGet("freePlaces")]
    public ActionResult<int> GetFreePlaces() => Ok(_parkingService.GetFreePlaces());

    [HttpGet("fullbalance")]
    public ActionResult<decimal> GetFullBalance() => Ok(_parkingService.GetBalanceFromFile());

}
