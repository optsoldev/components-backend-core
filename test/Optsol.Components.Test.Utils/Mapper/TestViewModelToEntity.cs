using AutoMapper;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Entity.Entities;
using Optsol.Components.Test.Utils.ViewModels;
using System.Linq;

namespace Optsol.Components.Test.Utils.Mapper
{
    public class TestViewModelToEntity : Profile
    {
        public TestViewModelToEntity()
        {
            CreateMap<TestResponseDto, TestEntity>()
                .ConstructUsing((viewModel, context) => 
                {
                    return new TestEntity(
                        new NomeValueObject(viewModel.Nome.Split(' ').First(), viewModel.Nome.Split(' ').Last()),
                        new EmailValueObject(viewModel.Contato));
                });

            CreateMap<TestRequestDto, TestEntity>()
                .ForMember(dest => dest.Nome, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ConstructUsing((viewModel, context) => 
                {
                    return new TestEntity(
                        new NomeValueObject(viewModel.Nome.Split(' ').First(), viewModel.Nome.Split(' ').Last()),
                        new EmailValueObject(viewModel.Contato));
                });

            
            CreateMap<TestEntity, TestEntity>()
               .ConstructUsing((viewModel, context) =>
               {
                   return new TestEntity(
                       new NomeValueObject(viewModel.Nome.Nome, viewModel.Nome.SobreNome),
                       new EmailValueObject(viewModel.Email.Email));
               });
        }
    }
}
