// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;

namespace CoolParking.BL.Services;

public class ParkingService : IParkingService
{
    private ITimerService _withdrawTimer { get; set; }
    private ITimerService _logTimer { get; set; }
    private ILogService _logService { get; set; }
    private Parking _parking;
    private bool disposing = false;

    public ParkingService(ITimerService withdrawTimer, ITimerService logTimer, ILogService logService)
    {
        _parking = Parking.GetInstance;
        _withdrawTimer = withdrawTimer;
        _logTimer = logTimer;
        _logService = logService;
        _withdrawTimer.Interval = Settings.IntevalPayment;
        _withdrawTimer.Elapsed += Withdraw;
        _withdrawTimer.Start();
        _logTimer.Interval = Settings.IntervalLog;
        _logTimer.Elapsed += WriteToFile;
        _logTimer.Start();
    }

    public void AddVehicle(Vehicle vehicle)
    {
        if (_parking.Vehicles.Count >= _parking.MaxCapacity) throw new InvalidOperationException("Parking is full");
        if (_parking.Vehicles.Where(a => a.Id.Equals(vehicle.Id)).ToList().Count > 0) throw new ArgumentException("Such id is already registered");
        _parking.Vehicles.Add(vehicle);
    }

    public void Dispose()
    {
        if (!disposing)
        {
            disposing = true;
            this.Dispose();
            _parking.Vehicles.Clear();
            _parking.Transactions.Clear();
            _parking.Balance = 0;
        }    
    }

    public decimal GetBalance()
    {
        return _parking.Balance;
    }

    public int GetCapacity()
    {
        return _parking.MaxCapacity;
    }

    public int GetFreePlaces()
    {
        return _parking.MaxCapacity - _parking.Vehicles.Count;
    }

    public TransactionInfo[] GetLastParkingTransactions()
    {
        var lastTransactions = (from t in _parking.Transactions
                                select t).Take(5);
        return lastTransactions.ToArray();
    }

    public ReadOnlyCollection<Vehicle> GetVehicles()
    {
        return new ReadOnlyCollection<Vehicle>(_parking.Vehicles);
    }

    public string ReadFromLog()
    {
        var text = _logService.Read();
        if (text == null) return "No transactions history";
        return text;
    }

    public void RemoveVehicle(string vehicleId)
    {
        var vehicle = _parking.Vehicles.Where(a => a.Id.Equals(vehicleId)).FirstOrDefault();
        if (vehicle == null) throw new ArgumentException("No such vehicle");
        if (vehicle.Balance < 0) throw new InvalidOperationException("Balnce is less than zero");
        _parking.Vehicles.Remove(vehicle);
    }

    public void TopUpVehicle(string vehicleId, decimal sum)
    {
        if (sum <= 0) throw new ArgumentException("Sum is below zero");
        foreach (var vehicle in _parking.Vehicles.Where(a => a.Id.Equals(vehicleId)))
        {
            var transaction = new TransactionInfo();
            transaction.Sum = sum;
            transaction.Time = DateTime.Now;
            transaction.VehicleId = vehicleId;
            transaction.Type = "User topUp";

            _parking.Transactions.Add(transaction);
            vehicle.Balance += sum;
            return;
        }
        throw new ArgumentException("No such vehicle");
    }

    private void Withdraw(Object source, System.Timers.ElapsedEventArgs e)
    {
        foreach (var vehicle in _parking.Vehicles)
        {
            var cost = _parking.Tarrifs[vehicle.VehicleType];
            if (vehicle.Balance - cost < 0)
            {
                cost = (cost - vehicle.Balance)*_parking.FineCoefficient + vehicle.Balance;
            }
            var transaction = new TransactionInfo();
            transaction.Sum = cost;
            transaction.Time = DateTime.Now;
            transaction.VehicleId = vehicle.Id;
            transaction.Type = "Withdraw";

            _parking.Transactions.Add(transaction);
            vehicle.Balance -= cost;
            _parking.Balance += cost;
        }
    }

    private void WriteToFile(Object source, System.Timers.ElapsedEventArgs e)
    {
        string data = "";
        foreach (var transaction in _parking.Transactions)
        {
            data += $"{transaction}\n";
        }
        _logService.Write(data);
        _parking.Transactions.Clear();
    }
}
