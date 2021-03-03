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
            CreateMap<TestViewModel, TestEntity>()
                .ConstructUsing((viewModel, entity) => 
                {
                    return new TestEntity(
                        new NomeValueObject(viewModel.Nome.Split(' ').First(), viewModel.Nome.Split(' ').Last()),
                        new EmailValueObject(viewModel.Contato));
                });

            CreateMap<InsertTestViewModel, TestEntity>()
                .ForMember(dest => dest.Nome, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ConstructUsing((viewModel, context) => 
                {
                    return new TestEntity(
                        new NomeValueObject(viewModel.Nome.Split(' ').First(), viewModel.Nome.Split(' ').Last()),
                        new EmailValueObject(viewModel.Contato));
                });

            
            CreateMap<UpdateTestViewModel, TestEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Nome, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ConstructUsing((viewModel, context) => 
                {
                    return new TestEntity(
                        viewModel.Id,
                        new NomeValueObject(viewModel.Nome.Split(' ').First(), viewModel.Nome.Split(' ').Last()),
                        new EmailValueObject(viewModel.Contato));
                });

            CreateMap<TestEntity, TestEntity>()
               .ConstructUsing((viewModel, entity) =>
               {
                   return new TestEntity(
                       new NomeValueObject(viewModel.Nome.Nome, viewModel.Nome.SobreNome),
                       new EmailValueObject(viewModel.Email.Email));
               });
        }
    }
}
