using CoolParking.WebAPI.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

namespace CoolParking.WebAPI;

public class Program{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddConfig(builder.Configuration);
        //builder.Services.AddMyDependencyGroup();  //won't work because of concrete params in constructors

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

    }
}
