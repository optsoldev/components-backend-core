using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Shared.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Optsol.Components.Test.Utils.Utils;

namespace Optsol.Components.Test.Unit.Shared.Extensions
{
    public class RepositoryExtensionsSpec
    {

        [Trait("Extensions", "RepositoryExtensions")]
        [Fact(DisplayName = "Deve converter IAsyncEnumerable em IEnumerable")]
        public async Task Deve_Converter_IAsyncEnumerable_IEnumerable()
        {
            var entity = new AggregateRoot();
            var entity2 = new AggregateRoot();

            //Given
            var entitiesAsync = GetAllAggregateRootAsyncEnumerable(entity, entity2);

            var setMock = new Mock<DbSet<AggregateRoot>>();
            setMock.Setup(set => set.AsAsyncEnumerable()).Returns(entitiesAsync);
            
            var coreContextMock = new Mock<CoreContext>();
            coreContextMock.Setup(context => context.Set<AggregateRoot>()).Returns(setMock.Object);
            
            var logger = new XunitLogger<Repository<AggregateRoot, Guid>>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            var repository = new Repository<AggregateRoot, Guid>(coreContextMock.Object, loggerFactoryMock.Object);

            //When
            var entities = await repository.GetAllAsync();

            //Then
            entities.Should().BeOfType<List<AggregateRoot>>();
            entities.Should().HaveCount(2);
            entities.FirstOrDefault(f => f.Id == entity.Id).Should().NotBeNull();
            entities.FirstOrDefault(f => f.Id == entity2.Id).Should().NotBeNull();
        }
    }
}
