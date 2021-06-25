using AutoMapper;
using Optsol.Components.Infra.Data.Pagination;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Application.Mappers.Cliente
{
    public class ClienteEntityToViewModelMapper : Profile
    {
        public ClienteEntityToViewModelMapper()
        {
            CreateMap<ClientePessoaFisicaEntity, ClienteViewModel>()
                .ForMember(item => item.DataCriacao, item => item.MapFrom(src => src.CreatedDate));

            CreateMap<ClientePessoaFisicaEntity, ClienteComCartoesViewModel>()
                .ForMember(item => item.Sobrenome, item => item.MapFrom(src => src.Nome.SobreNome));

            CreateMap<ClientePessoaFisicaEntity, InsertClienteViewModel>()
                .ForMember(item => item.Nome, item => item.MapFrom(src => src.Nome.Nome))
                .ForMember(item => item.SobreNome, item => item.MapFrom(src => src.Nome.SobreNome))
                .ForMember(item => item.Documento, item => item.MapFrom(src => src.Documento))
                .ForMember(item => item.Email, item => item.MapFrom(src => src.Email.ToString()));

            CreateMap<SearchResult<ClientePessoaFisicaEntity>, SearchResult<ClienteViewModel>>();
        }
    }
}
