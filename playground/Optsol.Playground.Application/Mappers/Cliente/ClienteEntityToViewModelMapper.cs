using AutoMapper;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Application.Mappers.Cliente
{
    public class ClienteEntityToViewModelMapper : Profile
    {
        public ClienteEntityToViewModelMapper()
        {
            CreateMap<ClienteEntity, ClienteViewModel>()
                                .ForMember(item => item.DataCriacao, item => item.MapFrom(src => src.CreatedDate));

            CreateMap<ClienteEntity, ClienteComCartoesViewModel>()
                .ForMember(item => item.Sobrenome, item => item.MapFrom(src => src.Nome.SobreNome));
        }
    }
}
