using System.Timers;
using CoolParking.BL.Interfaces;

namespace CoolParking.BL.Services;

public class TimerService : ITimerService
{
    private Timer _timer;
    public double Interval { get; set; }

    public event ElapsedEventHandler Elapsed;

    public void Dispose()
    {
        _timer?.Dispose();
        this.Dispose();
    }

    public void Start()
    {
        _timer = new Timer();
        _timer.Interval = Interval;
        _timer.Elapsed += Elapsed;
        _timer.Enabled = true;
    }

    public void Stop()
    {
        _timer.Stop();
    }
}
