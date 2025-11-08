using AutoMapper;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;

namespace DualTechTechnicalTest.Mappers;

public class ClientMapper : Profile
{
    public ClientMapper()
    {
        CreateMap<Client, ClientDataTransferObject>();
        CreateMap<ClientDataTransferObject, Client>();
    }
}