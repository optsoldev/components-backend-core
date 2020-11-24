using AutoMapper;
using Optsol.Playground.Application.ViewModels;
using Optsol.Playground.Domain.ValueObjects;

namespace Optsol.Playground.Application.Mappers.Object
{
    public class ObjectViewModelToValeuObjetc : Profile
    {
        public ObjectViewModelToValeuObjetc()
        {
            CreateMap<NomeObjectViewModel, NomeValueObject>();

            CreateMap<EmailObjetcViewModel, EmailValueObject>();
        }
    }
}
