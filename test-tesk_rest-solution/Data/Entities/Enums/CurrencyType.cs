using System.ComponentModel;

namespace test_tesk_rest_solution.Data.Entities.Enums;

public enum CurrencyType
{
    [Description("USD")]
    Usd = 0,
    
    [Description("EUR")]
    Eur  = 1
}