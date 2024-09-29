using System;
using test_tesk_rest_solution.Data.Entities.Enums;

namespace test_tesk_rest_solution.ViewModels;

public class OrderViewModel
{
    public int Id { get; set; }
    
    public string CustomerName { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public CurrencyType Currency { get; set; }
    
    public StatusType Status { get; set; }
    
    public int Priority { get; set; }
    
    public decimal TotalAmountInBaseCurrency { get; set; }
}