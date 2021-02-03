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
            CreateMap<InsertClienteViewModel, ClienteEntity>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(source => new ClienteEntity(
                    new NomeValueObject(source.Nome, source.SobreNome),
                    new EmailValueObject(source.Email)));

            CreateMap<UpdateClienteViewModel, ClienteEntity>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(source => new ClienteEntity(
                    source.Id, 
                    new NomeValueObject(source.Nome, source.SobreNome), 
                    new EmailValueObject(source.Email)));
        }
    }
}
