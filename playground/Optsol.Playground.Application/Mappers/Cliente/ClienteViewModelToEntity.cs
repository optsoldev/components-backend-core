using AutoMapper;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application.Mappers.Cliente
{
    public class ClienteViewModelToClienteEntity : Profile
    {
        public ClienteViewModelToClienteEntity()
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
