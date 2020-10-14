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
        public void DeveRegistrarLogsNoRepositorio()
        {
            //Given
            var entity = new AggregateRoot();
            
            Mock<DbSet<AggregateRoot>> setMock = new Mock<DbSet<AggregateRoot>>();
            setMock.Setup(set => set.FindAsync(It.IsAny<Guid>())).ReturnsAsync(entity);
            
            Mock<DbContext> dbContextMock = new Mock<DbContext>();
            dbContextMock.Setup(context => context.Set<AggregateRoot>()).Returns(setMock.Object);
            
            var logger = new XunitLogger<Repository<AggregateRoot, Guid>>();
            var repositorio = new Repository<AggregateRoot, Guid>(dbContextMock.Object, logger);
            
            //When
            repositorio.GetById(entity.Id);
            repositorio.GetAllAsync();
            repositorio.InsertAsync(entity);
            repositorio.UpdateAsync(entity);
            repositorio.DeleteAsync(entity.Id).ConfigureAwait(false);
            repositorio.DeleteAsync(entity).ConfigureAwait(false);
            repositorio.SaveChanges();

            //Then
            var msgContrutor = $"Inicializando Repository<{ entity.GetType().Name }, { typeof(Guid).Name }>";
            var msgGetById = $"Método: { nameof(repositorio.GetById) }({{ id:{ entity.Id } }}) Retorno: type { entity.GetType().Name }";
            var msgGetAllAsync = $"Método: { nameof(repositorio.GetAllAsync) }() Retorno: IAsyncEnumerable<{ entity.GetType().Name }>";
            var msgInsertAsync = $"Método: { nameof(repositorio.GetById) }({{ id:{ entity.Id } }}) Retorno: type { entity.GetType().Name }";
            var msgUpdateAsync = $"Método: { nameof(repositorio.UpdateAsync) }({{ id:{ entity.Id } }}) Retorno: type { entity.GetType().Name }";
            var msgDeleteAsync = $"Método: { nameof(repositorio.DeleteAsync) }({{ id:{ entity.Id } }}) Retorno: type { entity.GetType().Name }";
            var msgSaveChanges = $"Método: { nameof(repositorio.SaveChanges) }()";

            logger.Logs.Should().HaveCount(8);
            logger.Logs.FindAll(f => f.Contains(nameof(repositorio.GetById))).Should().HaveCount(1);
            logger.Logs.FindAll(f => f.Contains(msgInsertAsync)).Should().HaveCount(1);
            logger.Logs.FindAll(f => f.Contains(nameof(repositorio.InsertAsync))).Should().HaveCount(1);
            logger.Logs.FindAll(f => f.Contains(nameof(repositorio.UpdateAsync))).Should().HaveCount(1);
            logger.Logs.FindAll(f => f.Contains(nameof(repositorio.DeleteAsync))).Should().HaveCount(2);
            logger.Logs.FindAll(f => f.Contains(nameof(repositorio.SaveChanges))).Should().HaveCount(1);            

            
        }       
    }
}
