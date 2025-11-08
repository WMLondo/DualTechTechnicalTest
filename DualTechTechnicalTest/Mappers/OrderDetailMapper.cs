using AutoMapper;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;

namespace DualTechTechnicalTest.Mappers;

public class OrderDetailMapper:Profile
{
    public OrderDetailMapper()
    {
        CreateMap<OrderDetail, OrderDetailDataTransferObject>();
    }
}