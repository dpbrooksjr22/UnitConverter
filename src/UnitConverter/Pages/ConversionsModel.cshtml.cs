using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnitConverter.Pages;

public class ConversionsModel : PageModel
{
    public void OnGet()
    {
        if (string.IsNullOrWhiteSpace(Input))
        {
            Input = "3.1415";
        }

        if (string.IsNullOrWhiteSpace(ConversionType))
        {
            ConversionType = "MilesToKilometers";
        }

        ViewData["ConversionType"] = ConversionType switch
        {
            "MilesToKilometers" => "Miles to Kilometers",
            "KilometersToMiles" => "Kilometers to Miles",
            "FahrenheitToCelsius" => "Fahrenheit to Celsius",
            "CelsiusToFahrenheit" => "Celsius to Fahrenheit",
            "PoundsToKilograms" => "Pounds to Kilograms",
            "KilogramsToPounds" => "Kilograms to Pounds",
            "FeetToMeters" => "Feet to Meters",
            "MetersToFeet" => "Meters to Feet",
            _ => ConversionType
        };

        ViewData["Title"] = "Conversions";

        double conversion;

        try
        {
            conversion = Convert.ToDouble(Input);
        }
        catch (FormatException)
        {
            ViewData["ErrorMessage"] = "Input must be a valid number.";
            return;
        }
        catch (OverflowException)
        {
            ViewData["ErrorMessage"] = "Input must be a valid number.";
            return;
        }

        double? unit = ConversionType switch
        {
            "MilesToKilometers" => new UnitOf.Length().FromMiles(conversion).ToKilometers(),
            "KilometersToMiles" => new UnitOf.Length().FromKilometers(conversion).ToMiles(),
            "FahrenheitToCelsius" => new UnitOf.Temperature().FromFahrenheit(conversion).ToCelsius(),
            "CelsiusToFahrenheit" => new UnitOf.Temperature().FromCelsius(conversion).ToFahrenheit(),
            "PoundsToKilograms" => new UnitOf.Mass().FromPounds(conversion).ToKilograms(),
            "KilogramsToPounds" => new UnitOf.Mass().FromKilograms(conversion).ToPounds(),
            "FeetToMeters" => new UnitOf.Length().FromFeet(conversion).ToMeters(),
            "MetersToFeet" => new UnitOf.Length().FromMeters(conversion).ToFeet(),
            _ => null
        };

        if (unit == null)
        {
            ViewData["ErrorMessage"] = "Unknown conversion type.";
            return;
        }

        Output = unit.Value.ToString();
    }

    [BindProperty(SupportsGet = true)]
    public string ConversionType { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string Input { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;
}
