using AutoMapper;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Data;

namespace Optsol.Components.Test.Utils.Mapper
{
    public class TestEntityToViewModel : Profile
    {
        public TestEntityToViewModel() 
        {
            CreateMap<TestEntity, TestViewModel>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(source => source.Nome.ToString()))
                .ForMember(dest => dest.Contato, opt => opt.MapFrom(source => source.Email.ToString()))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(source => source.Ativo ? "Ativo" : "Inativo"));
        }
    }
}
