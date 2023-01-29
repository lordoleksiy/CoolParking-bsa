using System;
using System.IO;
using CoolParking.BL.Interfaces;

namespace CoolParking.BL.Services;

public class LogService : ILogService
{
    public string LogPath { get; }

    public LogService(string LogPath)
    {
        this.LogPath = LogPath;
    }

    public string Read()
    {
        string text;
        try
        {
            using (var file = new StreamReader(LogPath))
            {
                text = file.ReadToEnd();
            }
        }
        catch
        {
            throw new InvalidOperationException();
        }
        return text;
    }

    public void Write(string logInfo)
    {
        if (logInfo.Length < 1) return;
        using (var file = new StreamWriter(LogPath, true))
        {
            file.WriteLine(logInfo);
        }
    }
}
