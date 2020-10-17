using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Utils.Data;
using Optsol.Components.Shared.Extensions;
using System.Linq;

namespace Optsol.Components.Test.Integration.IoC
{
    public class RepositoryExtensionsSpec
    {
        public class RepositoryExtensionSpec
        {
            [Fact]
            public async Task DeveTestarConfiguracaoDoRepositorio()
            {
                //Given
                var services = new ServiceCollection();
                var entity = new TestEntity(
                    new NomeValueObject("Weslley", "Carneiro")
                    , new EmailValueObject("weslley.carneiro@optsol.com.br"));

                //When
                services.AddLogging();
                services.AddRepository<TestContext>(new ContextOptionsBuilder());
                services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");

                var provider = services.BuildServiceProvider();
                IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>(); 
                ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
                ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
                await writeRepository.InsertAsync(entity);
                await unitOfWork.CommitAsync();

                //Then                
                var entityGet = await readRepository.GetByIdAsync(entity.Id);
                entityGet.Id.Should().Be(entity.Id);
                entityGet.ToString().Should().Be(entity.ToString());
            }

            [Fact]
            public async Task DeveBuscarTodosPeloRepositorio()
            {
                //Given
                var services = new ServiceCollection();
                var entity = new TestEntity(
                    new NomeValueObject("Weslley", "Carneiro")
                    , new EmailValueObject("weslley.carneiro@optsol.com.br"));

                var entity1 = new TestEntity(
                    new NomeValueObject("Weslley", "Carneiro")
                    , new EmailValueObject("weslley.carneiro@optsol.com.br"));

                var entity2 = new TestEntity(
                    new NomeValueObject("Weslley", "Carneiro")
                    , new EmailValueObject("weslley.carneiro@optsol.com.br"));

                services.AddLogging();
                services.AddRepository<TestContext>(new ContextOptionsBuilder());
                services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");

                var provider = services.BuildServiceProvider();
                IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>(); 
                ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
                ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
                await writeRepository.InsertAsync(entity);
                await writeRepository.InsertAsync(entity1);
                await writeRepository.InsertAsync(entity2);
                await unitOfWork.CommitAsync();

                //When
                var entityResult = await readRepository.GetAllAsync().AsyncEnumerableToEnumerable();

                //Then
                entityResult.Should().HaveCount(3);
                entityResult.Single(s => s.Id == entity.Id).ToString().Should().Be(entity.ToString());
                entityResult.Single(s => s.Id == entity.Id).Nome.ToString().Should().Be(entity.Nome.ToString());
                entityResult.Single(s => s.Id == entity.Id).Email.ToString().Should().Be(entity.Email.ToString());
            }

            [Fact]
            public async Task DeveBuscarPorIdPeloRepositorio()
            {
                //Given
                var services = new ServiceCollection();
                var entity = new TestEntity(
                    new NomeValueObject("Weslley", "Carneiro")
                    , new EmailValueObject("weslley.carneiro@optsol.com.br"));

                services.AddLogging();
                services.AddRepository<TestContext>(new ContextOptionsBuilder());
                services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");

                var provider = services.BuildServiceProvider();
                IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>(); 
                ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
                ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
                await writeRepository.InsertAsync(entity);
                await unitOfWork.CommitAsync();

                //When
                var entityResult = await readRepository.GetByIdAsync(entity.Id);

                //Then
                entityResult.Should().NotBeNull();
                entityResult.Nome.ToString().Should().Be(entity.Nome.ToString());
                entityResult.Email.ToString().Should().Be(entity.Email.ToString());
            }

            [Fact]
            public async Task DeveInserirRegistroPeloRepositorio()
            {
                //Given
                var services = new ServiceCollection();
                var entity = new TestEntity(
                    new NomeValueObject("Weslley", "Carneiro")
                    , new EmailValueObject("weslley.carneiro@optsol.com.br"));

                services.AddLogging();
                services.AddRepository<TestContext>(new ContextOptionsBuilder());
                services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");

                var provider = services.BuildServiceProvider();
                IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>(); 
                ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
                ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();

                //When
                await writeRepository.InsertAsync(entity);
                await unitOfWork.CommitAsync();

                //Then
                var entityResult = await readRepository.GetByIdAsync(entity.Id);
                entityResult.Invalid.Should().BeFalse();
                entityResult.Notifications.Should().HaveCount(0);
                entityResult.Should().NotBeNull();
                entityResult.Nome.ToString().Should().Be(entity.Nome.ToString());
                entityResult.Email.ToString().Should().Be(entity.Email.ToString());
                entityResult.Ativo.Should().BeFalse();
            }

            [Fact]
            public async Task DeveAtualizarRegistroPeloRepositorio()
            {
                //Given
                var services = new ServiceCollection();
                var entity = new TestEntity(
                    new NomeValueObject("Weslley", "Carneiro")
                    , new EmailValueObject("weslley.carneiro@optsol.com.br"));

                services.AddLogging();
                services.AddRepository<TestContext>(new ContextOptionsBuilder());
                services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");

                var provider = services.BuildServiceProvider();
                IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>(); 
                ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
                ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
                await writeRepository.InsertAsync(entity);
                await unitOfWork.CommitAsync();

                //When
                var updateResult = await readRepository.GetByIdAsync(entity.Id);
                var updateEntity = new TestEntity(updateResult.Id, new NomeValueObject("Weslley", "Atualizado"), updateResult.Email);
                                
                await writeRepository.UpdateAsync(updateEntity);
                await unitOfWork.CommitAsync();
                
                //Then
                var entityResult = await readRepository.GetByIdAsync(entity.Id);
                entityResult.Invalid.Should().BeFalse();
                entityResult.Notifications.Should().HaveCount(0);
                entityResult.Should().NotBeNull();
                entityResult.Nome.ToString().Should().Be(updateEntity.Nome.ToString());
                entityResult.Email.ToString().Should().Be(updateEntity.Email.ToString());
                entityResult.Ativo.Should().BeFalse();
            }

            [Fact]
            public async Task DeveRemoverRegistroPeloIdPeloRepositorio()
            {
                //Given
                var services = new ServiceCollection();
                var entity = new TestEntity(
                    new NomeValueObject("Weslley", "Carneiro")
                    , new EmailValueObject("weslley.carneiro@optsol.com.br"));

                services.AddLogging();
                services.AddRepository<TestContext>(new ContextOptionsBuilder());
                services.RegisterRepositories<ITestReadRepository>("Optsol.Components.Test.Utils");

                var provider = services.BuildServiceProvider();
                IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>(); 
                ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
                ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
                await writeRepository.InsertAsync(entity);
                await unitOfWork.CommitAsync();

                //When
                await writeRepository.DeleteAsync(entity.Id);
                await unitOfWork.CommitAsync();

                //Then
                var entityResult = await readRepository.GetByIdAsync(entity.Id);
                entityResult.Should().BeNull();
            }
        }
    }
}
