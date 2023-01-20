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
            _parking.Vehicles.Clear();
            _parking.Transactions.Clear();
            _parking.Balance = 0;
            _logTimer.Dispose();
            _withdrawTimer.Dispose();
            this.Dispose();
        }    
    }

    public decimal GetBalance()
    {
        return _parking.Balance;
    }

    public decimal GetBalanceFromFile()
    {
        var amount = _parking.Balance;
        var text = _logService.Read();
        if (text.Length == 0) return amount;
        foreach (var t in text.Split("\n"))
        {
            var dataArr = t.Split("|");
            if (dataArr[0].Equals("Withdraw"))
            {
                amount += decimal.Parse(dataArr[1]);
            }
        }
        return amount;
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
        return _parking.Transactions.ToArray();
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
                if (vehicle.Balance < 0) 
                {
                    cost *= _parking.FineCoefficient;
                }
                else
                {
                    cost = (cost - vehicle.Balance)*_parking.FineCoefficient + vehicle.Balance;
                }
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
        //if (_parking.Transactions.Count > 0){ 
        //}
        // writing to file must be surrounded by if to avoid writing of empty strings but test won't pass(((((
    }
}
