using System.ComponentModel;

namespace test_tesk_rest_solution.Data.Entities.Enums;

public enum StatusType
{
    [Description("Pending")]
    Pending = 0,
    
    [Description("Processing")]
    Processing  = 1,
    
    [Description("Completed")]
    Completed = 2,
    
    [Description("Cancelled")]
    Cancelled = 3
}