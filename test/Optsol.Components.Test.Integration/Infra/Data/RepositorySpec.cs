using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Test.Utils.Data;
using Optsol.Components.Test.Utils.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Optsol.Components.Test.Utils.Seed.Seed;

namespace Optsol.Components.Test.Integration.Infra.Data
{
    public class RepositorySpec
    {

        [Fact]
        public async Task Deve_Buscar_Todos_Pelo_Repositorio()
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
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
            await writeRepository.InsertAsync(entity);
            await writeRepository.InsertAsync(entity1);
            await writeRepository.InsertAsync(entity2);
            await unitOfWork.CommitAsync();

            //When
            var entityResult = await readRepository.GetAllAsync();

            //Then
            entityResult.Should().HaveCount(3);
            entityResult.Single(s => s.Id == entity.Id).ToString().Should().Be(entity.ToString());
            entityResult.Single(s => s.Id == entity.Id).Nome.ToString().Should().Be(entity.Nome.ToString());
            entityResult.Single(s => s.Id == entity.Id).Email.ToString().Should().Be(entity.Email.ToString());
        }

        [Fact]
        public async Task Deve_Buscar_Por_Id_Pelo_Repositorio()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            services.AddLogging();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

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
        public async Task Deve_Inserir_Registro_Pelo_Repositorio()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            services.AddLogging();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

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
        public async Task Deve_Atualizar_Registro_Pelo_Repositorio()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            services.AddLogging();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

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
        public async Task Deve_Remover_Registro_Pelo_Id_Pelo_Repositorio()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            services.AddLogging();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

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

        [Fact]
        public async Task Nao_Deve_Remover_Se_Id_For_Invalido()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            services.AddLogging();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();
            await writeRepository.InsertAsync(entity);
            await unitOfWork.CommitAsync();

            //When
            await writeRepository.DeleteAsync(Guid.NewGuid());
            await unitOfWork.CommitAsync();

            //Then
            var entityResult = await readRepository.GetByIdAsync(entity.Id);
            entityResult.Should().NotBeNull();
        }

        [Fact]
        public async Task Deve_Obter_Registros_Paginados()
        {
            //Given
            var searchDto = new TestSearchDto();
            var requestSearchPage1 = new RequestSearch<TestSearchDto>
            {
                Search = searchDto,
                Page = 0,
                PageSize = 10
            };

            var requestSearchPage2 = new RequestSearch<TestSearchDto>
            {
                Search = searchDto,
                Page = 2,
                PageSize = 10
            };
            var testEntityList = ObterListaEntidade()
                .OrderBy(o => o.Nome.Nome)
                .ToList();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();

            var tasks = testEntityList.Select(entity => writeRepository.InsertAsync(entity));
            await Task.WhenAll(tasks);

            await unitOfWork.CommitAsync();

            //When
            var testEntityPage1 = await readRepository.GetAllAsync(requestSearchPage1);
            var testEntityPage2 = await readRepository.GetAllAsync(requestSearchPage2);

            //Then
            //Paginação 1
            testEntityPage1.Should().NotBeNull();
            testEntityPage1.Page.Should().Be(requestSearchPage1.Page);
            testEntityPage1.PageSize.Should().Be(requestSearchPage1.PageSize);
            testEntityPage1.Items.Should().HaveCount(requestSearchPage1.PageSize.Value.ToInt());
            testEntityPage1.TotalItems.Should().Be(requestSearchPage1.PageSize.Value);

            var skip1 = requestSearchPage1.Page <= 0 ? 1 : --requestSearchPage1.Page * (requestSearchPage1.PageSize ?? 0);

            testEntityPage1.Items.First().Id.Should().Be(testEntityList.Skip(skip1.ToInt()).First().Id);
            testEntityPage1.Items.Last().Id.Should().Be(testEntityList.Skip(skip1.ToInt()).Take(requestSearchPage1.PageSize.Value.ToInt()).Last().Id);
            testEntityPage1.Total.Should().Be(testEntityList.Count);

            //Paginação 2
            testEntityPage2.Should().NotBeNull();
            testEntityPage2.Page.Should().Be(requestSearchPage2.Page);
            testEntityPage2.PageSize.Should().Be(requestSearchPage2.PageSize);
            testEntityPage2.Items.Should().HaveCount(requestSearchPage2.PageSize.Value.ToInt());
            testEntityPage2.TotalItems.Should().Be(requestSearchPage2.PageSize.Value);

            var skip2 = requestSearchPage2.Page <= 0 ? 1 : --requestSearchPage2.Page * (requestSearchPage2.PageSize ?? 0);

            testEntityPage2.Items.First().Id.Should().Be(testEntityList.Skip(skip2.ToInt()).First().Id);
            testEntityPage2.Items.Last().Id.Should().Be(testEntityList.Skip(skip2.ToInt()).Take(requestSearchPage2.PageSize.Value.ToInt()).Last().Id);
            testEntityPage2.Total.Should().Be(testEntityList.Count);
        }

        [Fact]
        public async Task Deve_Obter_Registros_Paginados_Usando_Somente_Filtro()
        {
            //Given
            var searchDto = new TestSearchOnlyDto();
            var requestSearchPage1 = new RequestSearch<TestSearchOnlyDto>
            {
                Search = searchDto,
                Page = 0,
                PageSize = 10
            };

            var requestSearchPage2 = new RequestSearch<TestSearchOnlyDto>
            {
                Search = searchDto,
                Page = 2,
                PageSize = 10
            };
            var testEntityList = ObterListaEntidade()
                .OrderBy(o => o.Nome.Nome)
                .ToList();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            ITestReadRepository readRepository = provider.GetRequiredService<ITestReadRepository>();
            ITestWriteRepository writeRepository = provider.GetRequiredService<ITestWriteRepository>();

            var tasks = testEntityList.Select(entity => writeRepository.InsertAsync(entity));
            await Task.WhenAll(tasks);

            await unitOfWork.CommitAsync();

            //When
            var testEntityPage1 = await readRepository.GetAllAsync(requestSearchPage1);
            var testEntityPage2 = await readRepository.GetAllAsync(requestSearchPage2);

            //Then
            //Paginação 1
            testEntityPage1.Should().NotBeNull();
            testEntityPage1.Page.Should().Be(requestSearchPage1.Page);
            testEntityPage1.PageSize.Should().Be(requestSearchPage1.PageSize);
            testEntityPage1.Items.Should().HaveCount(requestSearchPage1.PageSize.Value.ToInt());
            testEntityPage1.TotalItems.Should().Be(requestSearchPage1.PageSize.Value);

            var skip1 = requestSearchPage1.Page <= 0 ? 1 : --requestSearchPage1.Page * (requestSearchPage1.PageSize ?? 0);

            testEntityPage1.Items.First().Id.Should().Be(testEntityList.Skip(skip1.ToInt()).First().Id);
            testEntityPage1.Items.Last().Id.Should().Be(testEntityList.Skip(skip1.ToInt()).Take(requestSearchPage1.PageSize.Value.ToInt()).Last().Id);
            testEntityPage1.Total.Should().Be(testEntityList.Count);

            //Paginação 2
            testEntityPage2.Should().NotBeNull();
            testEntityPage2.Page.Should().Be(requestSearchPage2.Page);
            testEntityPage2.PageSize.Should().Be(requestSearchPage2.PageSize);
            testEntityPage2.Items.Should().HaveCount(requestSearchPage2.PageSize.Value.ToInt());
            testEntityPage2.TotalItems.Should().Be(requestSearchPage2.PageSize.Value);

            var skip2 = requestSearchPage2.Page <= 0 ? 1 : --requestSearchPage2.Page * (requestSearchPage2.PageSize ?? 0);

            testEntityPage2.Items.First().Id.Should().Be(testEntityList.Skip(skip2.ToInt()).First().Id);
            testEntityPage2.Items.Last().Id.Should().Be(testEntityList.Skip(skip2.ToInt()).Take(requestSearchPage2.PageSize.Value.ToInt()).Last().Id);
            testEntityPage2.Total.Should().Be(testEntityList.Count);
        }

        [Fact]
        public async Task Nao_Deve_Buscar_Registros_Excluidos_Logicamente()
        {
            //Given
            var services = new ServiceCollection();
            var entity = new TestDeletableEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));
            entity.Delete();

            var entity1 = new TestDeletableEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            var entity2 = new TestDeletableEntity(
                new NomeValueObject("Weslley", "Carneiro")
                , new EmailValueObject("weslley.carneiro@optsol.com.br"));

            services.AddLogging();
            services.AddContext<TestContext>(new ContextOptionsBuilder());
            services.AddRepository<ITestReadRepository, TestReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            ITestDeletableReadRepository readRepository = provider.GetRequiredService<ITestDeletableReadRepository>();
            ITestDeletableWriteRepository writeRepository = provider.GetRequiredService<ITestDeletableWriteRepository>();
            await writeRepository.InsertAsync(entity);
            await writeRepository.InsertAsync(entity1);
            await writeRepository.InsertAsync(entity2);
            await unitOfWork.CommitAsync();

            await writeRepository.DeleteAsync(entity.Id);
            await unitOfWork.CommitAsync();

            //When
            var entityResult = await readRepository.GetAllAsync();

            //Then
            entityResult.Should().HaveCount(2);
        }
    }
}