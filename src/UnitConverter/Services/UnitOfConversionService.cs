using UnitConverter.Models;

namespace UnitConverter.Services;

public class UnitOfConversionService : IConversionService
{
    public decimal Convert(decimal value, string conversionType)
    {
        double? unit = conversionType switch
        {
        ConversionTypes.MilesToKilometers => new UnitOf.Length().FromMiles((double)value).ToKilometers(),
        ConversionTypes.KilometersToMiles => new UnitOf.Length().FromKilometers((double)value).ToMiles(),
        ConversionTypes.FahrenheitToCelsius => new UnitOf.Temperature().FromFahrenheit((double)value).ToCelsius(),
        ConversionTypes.CelsiusToFahrenheit => new UnitOf.Temperature().FromCelsius((double)value).ToFahrenheit(),
        ConversionTypes.PoundsToKilograms => new UnitOf.Mass().FromPounds((double)value).ToKilograms(),
        ConversionTypes.KilogramsToPounds => new UnitOf.Mass().FromKilograms((double)value).ToPounds(),
        ConversionTypes.FeetToMeters => new UnitOf.Length().FromFeet((double)value).ToMeters(),
        ConversionTypes.MetersToFeet => new UnitOf.Length().FromMeters((double)value).ToFeet(),
            _ => null
        };

        return (decimal) unit;
    }
}
