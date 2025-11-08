using AutoMapper;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;

namespace DualTechTechnicalTest.Mappers;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductDataTransferObject>();
        CreateMap<ProductDataTransferObject, Product>();
    }
    
}