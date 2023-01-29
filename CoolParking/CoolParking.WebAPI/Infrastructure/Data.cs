using System.Reflection;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Services;

namespace CoolParking.WebAPI.Infrastructure
{
    public class Data
    {
        public static ILogService LogService { get; }
        public static ITimerService WidthdrawTimer { get; }
        public static ITimerService LogTimer { get; }
        public static ParkingService ParkingService { get; }
        static Data()
        {
            LogService = new LogService($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log");
            WidthdrawTimer = new TimerService();
            LogTimer = new TimerService();
            ParkingService = new(WidthdrawTimer, LogTimer, LogService);
        }

    }
}
