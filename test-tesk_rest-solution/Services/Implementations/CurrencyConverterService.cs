using System.Collections.Generic;
using System.Threading.Tasks;
using test_tesk_rest_solution.Data.Entities.Enums;
using test_tesk_rest_solution.Services.Interfaces;

namespace test_tesk_rest_solution.Services.Implementations;

public class CurrencyConverterService : ICurrencyConverterService
{
    private readonly Dictionary<CurrencyType, decimal> _conversionRates = new Dictionary<CurrencyType, decimal>
    {
        { CurrencyType.Usd, 1m },
        { CurrencyType.Eur, 1.1m }
    };
    
    public async Task<decimal?> GetConversionRateAsync(CurrencyType fromCurrency, CurrencyType toCurrency)
    {
        await Task.Delay(100);

        if (fromCurrency == toCurrency)
        {
            return 1m;
        }

        if (_conversionRates.TryGetValue(fromCurrency, out var fromRate) &&
            _conversionRates.TryGetValue(toCurrency, out var toRate))
        {
            return toRate / fromRate;
        }

        return null;
    }
}