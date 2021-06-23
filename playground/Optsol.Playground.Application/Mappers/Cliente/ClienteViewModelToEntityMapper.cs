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
            CreateMap<InsertClienteViewModel, ClientePessoaFisicaEntity>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(source => new ClientePessoaFisicaEntity(
                    new NomeValueObject(source.Nome, source.SobreNome),
                    new EmailValueObject(source.Email),
                    source.Documento));

            CreateMap<UpdateClienteViewModel, ClientePessoaFisicaEntity>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(source => new ClientePessoaFisicaEntity(
                    source.Id, 
                    new NomeValueObject(source.Nome, source.SobreNome), 
                    new EmailValueObject(source.Email), 
                    source.Documento));
        }
    }
}
