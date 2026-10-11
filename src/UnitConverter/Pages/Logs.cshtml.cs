using Microsoft.AspNetCore.Mvc.RazorPages;
using UnitConverter.Services;
using UnitConverter.Models;
using Microsoft.AspNetCore.Mvc;

namespace UnitConverter.Pages;

public class Logs : PageModel
{
    private readonly ILogReader _logReader;

    public Logs(ILogReader logReader)
    {
        _logReader = logReader;
    }

    public List<LogEntry> Entries { get; set; } = new List<LogEntry>();

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Level { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ConversionType { get; set; }

    public void OnGet()
    {
        List<LogEntry> filteredEntries = new List<LogEntry>();

        foreach (LogEntry entry in _logReader.Read())
        {
            if (!string.IsNullOrEmpty(Level) && entry.Level != Level)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(ConversionType) &&
                entry.ConversionType != ConversionType)
            {
                continue;
            }

            string message = entry.Message ?? "";
            string exception = entry.Exception ?? "";

            if (!string.IsNullOrWhiteSpace(Search) &&
                !message.Contains(Search, StringComparison.OrdinalIgnoreCase) &&
                !exception.Contains(Search, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            filteredEntries.Add(entry);
        }

        Entries = filteredEntries;
    }
}
