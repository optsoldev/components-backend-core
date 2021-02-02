using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Shared.Logger;
using Optsol.Components.Shared.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Optsol.Components.Test.Utils.Entity;

namespace Optsol.Components.Test.Unit.Infra.Data
{
    public class RepositorySpec
    {
        [Fact]
        public void Deve_Registrar_Logs_No_Repositorio()
        {
            //Given
            var entity = new TestEntity();

            var setMock = new Mock<DbSet<TestEntity>>();
            setMock.Setup(setup => setup.FindAsync(It.IsAny<Guid>())).ReturnsAsync(entity);
            setMock.Setup(setup => setup.Attach(It.IsAny<TestEntity>()))
                .Returns<EntityEntry<TestEntity>>(entry =>
                {
                    var entityEntryMock = new Mock<EntityEntry<TestEntity>>();
                    entityEntryMock.SetupProperty(p => p.State, EntityState.Added);

                    return entityEntryMock.Object;
                });

            var coreContextMock = new Mock<CoreContext>();
            coreContextMock.Setup(context => context.Set<TestEntity>()).Returns(setMock.Object);
            var logger = new XunitLogger<Repository<TestEntity, Guid>>();
            var repositoryMock = new Repository<TestEntity, Guid>(coreContextMock.Object, logger);

            //When
            repositoryMock.GetByIdAsync(entity.Id);
            repositoryMock.GetAllAsync();
            repositoryMock.InsertAsync(entity);
            repositoryMock.UpdateAsync(entity);
            repositoryMock.DeleteAsync(entity.Id).ConfigureAwait(false);
            repositoryMock.DeleteAsync(entity).ConfigureAwait(false);
            repositoryMock.SaveChangesAsync();

            //Then
            var msgContructor = "Inicializando Repository<TestEntity, Guid>";
            var msgGetById = $"Método: GetByIdAsync( {{ id:{ entity.Id } }} ) Retorno: type TestEntity";
            var msgGetAllAsync = "Método: GetAllAsync() Retorno: IAsyncEnumerable<TestEntity>";
            var msgInsertAsync = $"Método: InsertAsync( {{entity:{ entity.ToJson() }}} )";
            var msgUpdateAsync = $"Método: UpdateAsync( {{entity:{ entity.ToJson() }}} )";
            var msgDeleteAsync = $"Método: DeleteAsync( {{entity:{ entity.ToJson() }}} )";
            var msgSaveChanges = "Método: SaveChangesAsync()";

            logger.Logs.Should().HaveCount(8);
            logger.Logs.Any(a => a.Equals(msgGetById)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgContructor)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgGetAllAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgInsertAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgUpdateAsync)).Should().BeTrue();
            logger.Logs.Any(a => a.Equals(msgSaveChanges)).Should().BeTrue();
            logger.Logs.Where(a => a.Equals(msgDeleteAsync)).Should().HaveCount(2);
        }
    }
}
