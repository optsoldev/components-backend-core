using AutoMapper;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Application.Mappers.Cliente
{
    public class ClienteViewModelToEntityMapper : Profile
    {
        public ClienteViewModelToEntityMapper()
        {
            CreateMap<InsertClienteViewModel, ClienteEntity>()
                .ForMember(item => item.Nome, item => item.MapFrom(src => src.Nome))
                .ForMember(item => item.Email, item => item.MapFrom(src => src.Email));

            CreateMap<UpdateClienteViewModel, ClienteEntity>()
                .ForMember(item => item.Nome, item => item.MapFrom(src => src.Nome))
                .ForMember(item => item.Email, item => item.MapFrom(src => src.Email));
        }
    }
}
