using System.Linq;
using System.ComponentModel.DataAnnotations;
using System;
using AutoMapper;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Shared.Data;

namespace Optsol.Components.Test.Utils.Mapper
{
    public class TestViewModelToEntity : Profile
    {
        public TestViewModelToEntity()
        {
            CreateMap<TestViewModel, TestEntity>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .AfterMap((viewModel, entity) => {
                    entity = new TestEntity(
                        new NomeValueObject(viewModel.Nome.Split(' ').First(), viewModel.Nome.Split(' ').Last()),
                        new EmailValueObject(viewModel.Contato));
                });
        }
    }
}
