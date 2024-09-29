using AutoMapper;
using test_tesk_rest_solution.Data.Entities;
using test_tesk_rest_solution.ViewModels;

namespace test_tesk_rest_solution.AutomapperProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderEntity, OrderViewModel>();
    }
}