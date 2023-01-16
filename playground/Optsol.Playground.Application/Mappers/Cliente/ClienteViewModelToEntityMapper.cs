using AutoMapper;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entities;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Application.Mappers.Cliente
{
    public class ClienteViewModelToEntityMapper : Profile
    {
        public ClienteViewModelToEntityMapper()
        {
            CreateMap<ClienteRequest, ClientePessoaFisica>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(source => new ClientePessoaFisica(
                    new NomeValueObject(source.Nome, source.SobreNome),
                    new EmailValueObject(source.Email),
                    source.Documento));
        }
    }
}
