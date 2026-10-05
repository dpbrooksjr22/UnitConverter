using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnitConverter.Models;
using UnitConverter.Services;

namespace UnitConverter.Pages;

public class QuickConversions: PageModel
{
    private readonly IConversionService _UnitOfConversionService;

    public decimal? Output { get; set; }

    public QuickConversions (IConversionService unitOfConversionService)
    {
        _UnitOfConversionService = unitOfConversionService;
    }

    private IActionResult PerformConversion(string input, string conversionType)
    {
        if (!decimal.TryParse(input, out decimal output))
        {

            ViewData["ErrorMessage"] = "Must be a valid number";
            return Page();
        }
        else
        {
            Output = _UnitOfConversionService.Convert(output, conversionType);

            return Page();
        }
    }

    public IEnumerable<SelectListItem> PoundOptions =>
    [
        new("1 pound", "1"),
        new("5 pounds", "5"),
        new("10 pounds", "10"),
        new("25 pounds", "25"),
        new("50 pounds", "50")
    ];

    public IActionResult OnGetMilesToKilometers(string input)
    {
        return PerformConversion(input, ConversionTypes.MilesToKilometers);
    }

    public IActionResult OnGetKilometersToMiles(string input)
    {
        return PerformConversion(input, ConversionTypes.KilometersToMiles);
    }

    public IActionResult OnGetFahrenheitToCelsius(string input)
    {
        return PerformConversion(input, ConversionTypes.FahrenheitToCelsius);
    }

    public IActionResult OnGetCelsiusToFahrenheit(string input)
    {
        return PerformConversion(input, ConversionTypes.CelsiusToFahrenheit);
    }

    public IActionResult OnGetPoundsToKilograms(string input)
    {
        return PerformConversion(input, ConversionTypes.PoundsToKilograms);
    }

    public IActionResult OnGetKilogramsToPounds(string input)
    {
        return PerformConversion(input, ConversionTypes.KilogramsToPounds);
    }

    public IActionResult OnGetFeetToMeters(string input)
    {
        return PerformConversion(input, ConversionTypes.FeetToMeters);
    }

    public IActionResult OnGetMetersToFeet(string input)
    {
        return PerformConversion(input, ConversionTypes.MetersToFeet);
    }

    private IActionResult RedirectToConversion(string conversionType, string input)
    {
        return RedirectToPage(
            "/Conversions",
            new
            {
                ConversionType = conversionType,
                Input = input
            });
    }
}
