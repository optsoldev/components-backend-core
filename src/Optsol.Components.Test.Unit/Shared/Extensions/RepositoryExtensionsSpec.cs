using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Test.Shared.Logger;
using Xunit;
using System.Linq;
using static Optsol.Components.Test.Utils.Utils;

namespace Optsol.Components.Test.Unit.Shared.Extensions
{
    public class RepositoryExtensionsSpec
    {

        [Fact]
        public async Task DeveConverterIAsyncEnumerableEmIEnumerable()
        {
            var entity = new AggregateRoot();
            var entity2 = new AggregateRoot();

            //Given
            var entitiesAsync = GetAllAggregateRootAsyncEnumerable(entity, entity2);

            Mock<DbSet<AggregateRoot>> setMock = new Mock<DbSet<AggregateRoot>>();
            
            setMock.Setup(set => set.AsAsyncEnumerable()).Returns(entitiesAsync);
            
            Mock<DbContext> dbContextMock = new Mock<DbContext>();
            dbContextMock.Setup(context => context.Set<AggregateRoot>()).Returns(setMock.Object);
            
            var logger = new XunitLogger<Repository<AggregateRoot, Guid>>();
            var repository = new Repository<AggregateRoot, Guid>(dbContextMock.Object, logger);

            //When
            var entities = await repository.GetAllAsync().AsyncEnumerableToEnumerable();

            //Then
            entities.Should().BeOfType<List<AggregateRoot>>();
            entities.Should().HaveCount(2);
            entities.FirstOrDefault(f => f.Id == entity.Id).Should().NotBeNull();
            entities.FirstOrDefault(f => f.Id == entity2.Id).Should().NotBeNull();
        }
    }
}
