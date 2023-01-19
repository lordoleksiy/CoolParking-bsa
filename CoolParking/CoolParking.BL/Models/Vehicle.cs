// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CoolParking.BL.Models;


public class Vehicle
{
    [NotNull]
    [RegularExpression(@"[A-Z]{2}-\d{4}-[A-Z]{2}", ErrorMessage = "Incorrect id")]
    public string Id { get; }

    public VehicleType VehicleType { get; }

    [Range(0, double.MaxValue)]
    public decimal Balance { get; internal set; }

    public Vehicle(string Id, VehicleType VehicleType, decimal Balance)
    {
        this.Id = Id;
        this.VehicleType = VehicleType;
        this.Balance = Balance;

        var results = new List<ValidationResult>();
        var context = new ValidationContext(this);

        if (!Validator.TryValidateObject(this, context, results, true))
            throw new ArgumentException("Validation error");
    }

    public static string GenerateRandomRegistrationPlateNumber()
    {
        Random rand = new Random();
        char c1 = (char)rand.Next('A', 'Z' + 1);
        char c2 = (char)rand.Next('A', 'Z' + 1);
        int d1 = rand.Next(0, 10);
        int d2 = rand.Next(0, 10);
        int d3 = rand.Next(0, 10);
        int d4 = rand.Next(0, 10);
        char c3 = (char)rand.Next('A', 'Z' + 1);
        char c4 = (char)rand.Next('A', 'Z' + 1);


        return $"{c1}{c2}-{d1}{d2}{d3}{d4}-{c3}{c4}";
    }

}
