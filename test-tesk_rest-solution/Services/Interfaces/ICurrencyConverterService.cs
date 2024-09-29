using System.Threading.Tasks;
using test_tesk_rest_solution.Data.Entities.Enums;

namespace test_tesk_rest_solution.Services.Interfaces;

public interface ICurrencyConverterService
{
    Task<decimal?> GetConversionRateAsync(CurrencyType fromCurrency, CurrencyType toCurrency);
}