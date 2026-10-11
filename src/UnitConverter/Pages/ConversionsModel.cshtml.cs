using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnitConverter.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace UnitConverter.Pages;

public class ConversionsModel : PageModel
{
    private readonly ILogger<ConversionsModel> _logger;

    public ConversionsModel() : this(NullLogger<ConversionsModel>.Instance)
    {
    }

    [ActivatorUtilitiesConstructor]
    public ConversionsModel(ILogger<ConversionsModel> logger)
    {
        _logger = logger;
    }
    public void OnGet()
    {
        if (string.IsNullOrWhiteSpace(Conversion.Input))
        {
            Conversion.Input = "3.1415";
        }

        if (string.IsNullOrWhiteSpace(Conversion.ConversionType))
        {
            Conversion.ConversionType = "MilesToKilometers";
        }

        ViewData["ConversionType"] = Conversion.ConversionType switch
        {
            "MilesToKilometers" => "Miles to Kilometers",
            "KilometersToMiles" => "Kilometers to Miles",
            "FahrenheitToCelsius" => "Fahrenheit to Celsius",
            "CelsiusToFahrenheit" => "Celsius to Fahrenheit",
            "PoundsToKilograms" => "Pounds to Kilograms",
            "KilogramsToPounds" => "Kilograms to Pounds",
            "FeetToMeters" => "Feet to Meters",
            "MetersToFeet" => "Meters to Feet",
            _ => Conversion.ConversionType
        };

        ViewData["Title"] = "Conversions";

        double conversion;

        try
        {
            conversion = Convert.ToDouble(Conversion.Input);
        }
        catch (FormatException)
        {
            _logger.LogWarning("Invalid conversion input {Input} for {ConversionType}",
                Conversion.Input, Conversion.ConversionType);

            ViewData["ErrorMessage"] = "Input must be a valid number.";
            return;
        }
        catch (OverflowException)
        {
            _logger.LogWarning("Invalid conversion input {Input} for {ConversionType}",
                Conversion.Input, Conversion.ConversionType);

            ViewData["ErrorMessage"] = "Input must be a valid number.";
            return;
        }

        double? unit;

        try
        {
            unit = Conversion.ConversionType switch
        {
            ConversionTypes.MilesToKilometers => new UnitOf.Length().FromMiles(conversion).ToKilometers(),
            ConversionTypes.KilometersToMiles => new UnitOf.Length().FromKilometers(conversion).ToMiles(),
            ConversionTypes.FahrenheitToCelsius => new UnitOf.Temperature().FromFahrenheit(conversion).ToCelsius(),
            ConversionTypes.CelsiusToFahrenheit => new UnitOf.Temperature().FromCelsius(conversion).ToFahrenheit(),
            ConversionTypes.PoundsToKilograms => new UnitOf.Mass().FromPounds(conversion).ToKilograms(),
            ConversionTypes.KilogramsToPounds => new UnitOf.Mass().FromKilograms(conversion).ToPounds(),
            ConversionTypes.FeetToMeters => new UnitOf.Length().FromFeet(conversion).ToMeters(),
            ConversionTypes.MetersToFeet => new UnitOf.Length().FromMeters(conversion).ToFeet(),
            _ => null
        };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Conversion failed for {ConversionType} with input {Input}",
                Conversion.ConversionType, Conversion.Input);

            ViewData["ErrorMessage"] = "Conversion failed.";
            return;
        }

        if (unit == null)
        {
            _logger.LogWarning("Unknown conversion type {ConversionType}",
                Conversion.ConversionType);

            ViewData["ErrorMessage"] = "Unknown conversion type.";
            return;
        }

        Conversion.Output = unit.Value.ToString();

        _logger.LogInformation(
            "Converted {Input} using {ConversionType} with result {Result}",
            conversion,
            Conversion.ConversionType,
            unit.Value);
    }

    [BindProperty(SupportsGet = true)]
    public ConversionModel Conversion { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string ConversionType
    {
        get => Conversion.ConversionType;
        set => Conversion.ConversionType = value;
    }

    [BindProperty(SupportsGet = true)]
    public string Input
    {
        get => Conversion.Input;
        set => Conversion.Input = value;
    }

    public string Output
    {
        get => Conversion.Output;
        set => Conversion.Output = value;
    }
}

