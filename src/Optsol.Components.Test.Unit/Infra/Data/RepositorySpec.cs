using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Optsol.Components.Domain;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Shared.Logger;
using Optsol.Components.Shared.Extensions;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Optsol.Components.Test.Unit.Infra.Data
{
    public class RepositorySpec
    {

        [Fact]
        public void Deve_Registrar_Logs_No_Repositorio()
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
            repository.GetByIdAsync(entity.Id);
            repository.GetAllAsync();
            repository.InsertAsync(entity);
            repository.UpdateAsync(entity);
            repository.DeleteAsync(entity.Id).ConfigureAwait(false);
            repository.DeleteAsync(entity).ConfigureAwait(false);
            repository.SaveChanges();

            //Then
            var msgContructor = "Inicializando Repository<AggregateRoot, Guid>";
            var msgGetById = $"Método: GetByIdAsync({{ id:{ entity.Id } }}) Retorno: type AggregateRoot";
            var msgGetAllAsync = "Método: GetAllAsync() Retorno: IAsyncEnumerable<AggregateRoot>";
            var msgInsertAsync = $"Método: InsertAsync({{ entity:{ entity.ToJson() } }})";
            var msgUpdateAsync = $"Método: UpdateAsync({{ entity:{ entity.ToJson() } }})";
            var msgDeleteAsync = $"Método: DeleteAsync({{ entity:{ entity.ToJson() } }})";
            var msgSaveChanges = "Método: SaveChanges()";

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
