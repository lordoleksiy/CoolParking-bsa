// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.

using System;
using System.IO;
using System.Reflection;
using System.Text;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using static System.Net.Mime.MediaTypeNames;

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
            return null;
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
