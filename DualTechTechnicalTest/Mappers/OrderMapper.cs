using AutoMapper;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;

namespace DualTechTechnicalTest.Mappers;

public class OrderMapper : Profile
{
    public OrderMapper()
    {
        CreateMap<Order, OrderDataTransferObject>()
            .ForCtorParam(nameof(OrderDataTransferObject.Details),opt => opt.MapFrom(o => o.OrderDetails));
    }
}
