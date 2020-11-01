using AutoMapper;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application.Mappers
{
    public class ClienteDomainToViewModelMapper : Profile
    {
        public ClienteDomainToViewModelMapper()
        {
            CreateMap<ClienteEntity, ClienteViewModel>()
                .ForMember(item => item.Sobrenome, item => item.MapFrom(src => src.Nome.SobreNome));

        }
    }
}
