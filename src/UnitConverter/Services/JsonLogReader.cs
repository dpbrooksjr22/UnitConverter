using Microsoft.AspNetCore.Hosting;
using UnitConverter.Models;
using System.Text.Json;

namespace UnitConverter.Services;

public class JsonLogReader : ILogReader
{
    private readonly string _logDirectory;

    public JsonLogReader(IWebHostEnvironment environment)
    {
        _logDirectory = Path.Combine(
            environment.ContentRootPath,
            "Logs");
    }

    public IEnumerable<LogEntry> Read()
    {
        string[] logFiles = Directory.GetFiles(_logDirectory, "*.json");

        List<LogEntry> entries = new List<LogEntry>();

        foreach (string logFile in logFiles)
        {
            string[] lines;

            try
            {
                lines = File.ReadAllLines(logFile);
            }
            catch (IOException)
            {
                continue;
            }

            foreach (string line in lines)
            {
                try
                {
                    LogEntry? entry = JsonSerializer.Deserialize<LogEntry>(line);

                    if (entry != null)
                    {
                        if (string.IsNullOrEmpty(entry.Level))
                        {
                            entry.Level = "Information";
                        }

                        entries.Add(entry);
                    }
                }
                catch (JsonException)
                {
                    continue;
                }
            }
        }

        return entries;
    }
}
