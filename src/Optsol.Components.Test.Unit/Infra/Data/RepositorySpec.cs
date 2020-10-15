using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Optsol.Components.Domain;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Shared.Logger;

namespace Optsol.Components.Test.Unit.Infra.Data
{
    public class RepositorySpec
    {

        [Fact]
        public void DeveRegistrarLogsNorepository()
        {
            //Given
            var entity = new AggregateRoot();
            
            Mock<DbSet<AggregateRoot>> setMock = new Mock<DbSet<AggregateRoot>>();
            setMock.Setup(set => set.FindAsync(It.IsAny<Guid>())).ReturnsAsync(entity);
            
            Mock<DbContext> dbContextMock = new Mock<DbContext>();
            dbContextMock.Setup(context => context.Set<AggregateRoot>()).Returns(setMock.Object);
            
            var logger = new XunitLogger<Repository<AggregateRoot, Guid>>();
            var repository = new Repository<AggregateRoot, Guid>(dbContextMock.Object, logger);
            
            //When
            repository.GetById(entity.Id);
            repository.GetAllAsync();
            repository.InsertAsync(entity);
            repository.UpdateAsync(entity);
            repository.DeleteAsync(entity.Id).ConfigureAwait(false);
            repository.DeleteAsync(entity).ConfigureAwait(false);
            repository.SaveChanges();

            //Then
            var msgContrutor = $"Inicializando Repository<{ entity.GetType().Name }, { typeof(Guid).Name }>";
            var msgGetById = $"Método: { nameof(repository.GetById) }({{ id:{ entity.Id } }}) Retorno: type { entity.GetType().Name }";
            var msgGetAllAsync = $"Método: { nameof(repository.GetAllAsync) }() Retorno: IAsyncEnumerable<{ entity.GetType().Name }>";
            var msgInsertAsync = $"Método: { nameof(repository.GetById) }({{ id:{ entity.Id } }}) Retorno: type { entity.GetType().Name }";
            var msgUpdateAsync = $"Método: { nameof(repository.UpdateAsync) }({{ id:{ entity.Id } }}) Retorno: type { entity.GetType().Name }";
            var msgDeleteAsync = $"Método: { nameof(repository.DeleteAsync) }({{ id:{ entity.Id } }}) Retorno: type { entity.GetType().Name }";
            var msgSaveChanges = $"Método: { nameof(repository.SaveChanges) }()";

            logger.Logs.Should().HaveCount(8);
            logger.Logs.FindAll(f => f.Contains(nameof(repository.GetById))).Should().HaveCount(1);
            logger.Logs.FindAll(f => f.Contains(msgInsertAsync)).Should().HaveCount(1);
            logger.Logs.FindAll(f => f.Contains(nameof(repository.InsertAsync))).Should().HaveCount(1);
            logger.Logs.FindAll(f => f.Contains(nameof(repository.UpdateAsync))).Should().HaveCount(1);
            logger.Logs.FindAll(f => f.Contains(nameof(repository.DeleteAsync))).Should().HaveCount(2);
            logger.Logs.FindAll(f => f.Contains(nameof(repository.SaveChanges))).Should().HaveCount(1);            

            
        }       
    }
}
