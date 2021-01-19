using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Utils.Data;
using Xunit;

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
            var entityResult = await readRepository.GetAllAsync().AsyncEnumerableToEnumerable();

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

        private List<TestEntity> ObterListaEntidade()
        {
            return new List<TestEntity>
            {
                new TestEntity (new NomeValueObject("Isaiah", "Sosa"), new EmailValueObject("justo.eu.arcu@Integervitaenibh.net")),
                new TestEntity (new NomeValueObject("Hop", "Gross"), new EmailValueObject("Integer@magna.co.uk")),
                new TestEntity (new NomeValueObject("Armand", "Villarreal"), new EmailValueObject("lorem.tristique@posuerevulputatelacus.ca")),
                new TestEntity (new NomeValueObject("Alan", "Barry"), new EmailValueObject("placerat@acnulla.ca")),
                new TestEntity (new NomeValueObject("Octavius", "Wheeler"), new EmailValueObject("tristique@mollisIntegertincidunt.edu")),
                new TestEntity (new NomeValueObject("Arthur", "Wallace"), new EmailValueObject("amet.lorem.semper@Suspendissealiquet.co.uk")),
                new TestEntity (new NomeValueObject("Wade", "Snow"), new EmailValueObject("nisl.sem@est.org")),
                new TestEntity (new NomeValueObject("Benedict", "Day"), new EmailValueObject("magna.Nam@iaculis.com")),
                new TestEntity (new NomeValueObject("Daquan", "Dalton"), new EmailValueObject("enim@velarcueu.edu")),
                new TestEntity (new NomeValueObject("Preston", "Higgins"), new EmailValueObject("Proin@nullaante.net")),
                new TestEntity (new NomeValueObject("Aaron", "Newton"), new EmailValueObject("diam.Sed@DonecnibhQuisque.co.uk")),
                new TestEntity (new NomeValueObject("Keith", "Allen"), new EmailValueObject("nisl.Quisque@rutrumurnanec.edu")),
                new TestEntity (new NomeValueObject("Luke", "Huber"), new EmailValueObject("vel@estcongue.ca")),
                new TestEntity (new NomeValueObject("Fuller", "Yang"), new EmailValueObject("eget.volutpat.ornare@Aeneanmassa.com")),
                new TestEntity (new NomeValueObject("Robert", "Landry"), new EmailValueObject("amet.faucibus@dictummagna.org")),
                new TestEntity (new NomeValueObject("Rudyard", "Hayes"), new EmailValueObject("sapien.Aenean@est.org")),
                new TestEntity (new NomeValueObject("Seth", "Diaz"), new EmailValueObject("mattis@rutrum.edu")),
                new TestEntity (new NomeValueObject("Demetrius", "Shaffer"), new EmailValueObject("eros.non@molestiein.ca")),
                new TestEntity (new NomeValueObject("Walter", "Case"), new EmailValueObject("egestas.lacinia@Proin.edu")),
                new TestEntity (new NomeValueObject("Drake", "Stanley"), new EmailValueObject("fringilla.est.Mauris@at.org")),
                new TestEntity (new NomeValueObject("Avram", "Price"), new EmailValueObject("Donec.sollicitudin.adipiscing@nibhPhasellus.edu")),
                new TestEntity (new NomeValueObject("Dustin", "Fernandez"), new EmailValueObject("consequat@Donec.net")),
                new TestEntity (new NomeValueObject("Odysseus", "Sykes"), new EmailValueObject("adipiscing@Nullam.net")),
                new TestEntity (new NomeValueObject("Xanthus", "Nolan"), new EmailValueObject("dolor.Nulla@laoreet.co.uk")),
                new TestEntity (new NomeValueObject("Preston", "Bruce"), new EmailValueObject("Phasellus@consectetuer.org")),
                new TestEntity (new NomeValueObject("Noah", "Andrews"), new EmailValueObject("dolor.sit.amet@Donec.ca")),
                new TestEntity (new NomeValueObject("Owen", "Lee"), new EmailValueObject("varius.ultrices@Cum.com")),
                new TestEntity (new NomeValueObject("Sebastian", "Kidd"), new EmailValueObject("semper.egestas@egetmassaSuspendisse.net")),
                new TestEntity (new NomeValueObject("Castor", "Allison"), new EmailValueObject("dui@aaliquetvel.edu")),
                new TestEntity (new NomeValueObject("Alan", "Sparks"), new EmailValueObject("libero.Integer.in@pharetrautpharetra.ca")),
                new TestEntity (new NomeValueObject("Tate", "Oneal"), new EmailValueObject("urna@nisinibhlacinia.org")),
                new TestEntity (new NomeValueObject("Thor", "Pratt"), new EmailValueObject("Fusce.feugiat.Lorem@CurabiturdictumPhasellus.ca")),
                new TestEntity (new NomeValueObject("Kennedy", "Conway"), new EmailValueObject("Suspendisse.ac@arcu.ca")),
                new TestEntity (new NomeValueObject("August", "Mercado"), new EmailValueObject("at.fringilla.purus@iderat.co.uk")),
                new TestEntity (new NomeValueObject("Keaton", "Osborn"), new EmailValueObject("nec.luctus.felis@gravidanuncsed.org")),
                new TestEntity (new NomeValueObject("Abdul", "Mcknight"), new EmailValueObject("gravida.nunc@diamProin.edu")),
                new TestEntity (new NomeValueObject("Axel", "Knowles"), new EmailValueObject("commodo@Mauriseuturpis.edu")),
                new TestEntity (new NomeValueObject("Levi", "Sims"), new EmailValueObject("amet@a.ca")),
                new TestEntity (new NomeValueObject("Fritz", "York"), new EmailValueObject("Nam.nulla.magna@euneque.edu")),
                new TestEntity (new NomeValueObject("Dale", "Moore"), new EmailValueObject("orci.luctus.et@Namconsequatdolor.co.uk")),
                new TestEntity (new NomeValueObject("Barry", "Fry"), new EmailValueObject("facilisis@maurisutmi.co.uk")),
                new TestEntity (new NomeValueObject("Tobias", "Phillips"), new EmailValueObject("lacus.Aliquam.rutrum@orciquis.com")),
                new TestEntity (new NomeValueObject("Uriel", "James"), new EmailValueObject("non.lacinia@vulputateposuerevulputate.com")),
                new TestEntity (new NomeValueObject("Grady", "Bauer"), new EmailValueObject("vestibulum@consectetuercursuset.com")),
                new TestEntity (new NomeValueObject("Ivor", "Everett"), new EmailValueObject("posuere.vulputate@nequeseddictum.com")),
                new TestEntity (new NomeValueObject("Reuben", "Sweet"), new EmailValueObject("erat.semper@nibh.ca")),
                new TestEntity (new NomeValueObject("Timothy", "Alston"), new EmailValueObject("pede.ac@dictumultriciesligula.co.uk")),
                new TestEntity (new NomeValueObject("Marsden", "Key"), new EmailValueObject("Duis@risusDonec.co.uk")),
                new TestEntity (new NomeValueObject("Tiger", "Vance"), new EmailValueObject("fringilla.purus.mauris@pedeet.edu")),
                new TestEntity (new NomeValueObject("Jacob", "Gentry"), new EmailValueObject("Donec@semmagna.ca")),
                new TestEntity (new NomeValueObject("Levi", "Wiggins"), new EmailValueObject("orci@nibh.edu")),
                new TestEntity (new NomeValueObject("Nehru", "Pittman"), new EmailValueObject("Phasellus.dolor.elit@euelit.net")),
                new TestEntity (new NomeValueObject("Micah", "Ayers"), new EmailValueObject("odio.vel@elitEtiamlaoreet.org")),
                new TestEntity (new NomeValueObject("Dennis", "Mcdowell"), new EmailValueObject("cursus.a.enim@facilisisSuspendissecommodo.ca")),
                new TestEntity (new NomeValueObject("Walter", "Gregory"), new EmailValueObject("Sed.et@necorciDonec.ca")),
                new TestEntity (new NomeValueObject("Mufutau", "Everett"), new EmailValueObject("vel.faucibus.id@Donec.edu")),
                new TestEntity (new NomeValueObject("Elton", "Arnold"), new EmailValueObject("Duis@aliquetlibero.com")),
                new TestEntity (new NomeValueObject("Cyrus", "Hampton"), new EmailValueObject("In.nec@odioEtiamligula.edu")),
                new TestEntity (new NomeValueObject("Dalton", "Roman"), new EmailValueObject("ut@Praesentinterdumligula.org")),
                new TestEntity (new NomeValueObject("Lyle", "Horton"), new EmailValueObject("nunc.Quisque@anteipsumprimis.com")),
                new TestEntity (new NomeValueObject("Zachary", "Levine"), new EmailValueObject("Phasellus@Nulla.co.uk")),
                new TestEntity (new NomeValueObject("Emerson", "Patel"), new EmailValueObject("sit.amet@accumsan.net")),
                new TestEntity (new NomeValueObject("Judah", "Robbins"), new EmailValueObject("Integer.mollis.Integer@feugiat.edu")),
                new TestEntity (new NomeValueObject("Gavin", "Branch"), new EmailValueObject("in.felis.Nulla@et.org")),
                new TestEntity (new NomeValueObject("Magee", "Bryant"), new EmailValueObject("lectus.convallis.est@auctor.com")),
                new TestEntity (new NomeValueObject("Alexander", "Knowles"), new EmailValueObject("magnis@nullaanteiaculis.ca")),
                new TestEntity (new NomeValueObject("Ivan", "Travis"), new EmailValueObject("malesuada.fames@laoreetliberoet.org")),
                new TestEntity (new NomeValueObject("Gary", "Daniels"), new EmailValueObject("adipiscing.elit@tinciduntadipiscing.com")),
                new TestEntity (new NomeValueObject("Roth", "Bond"), new EmailValueObject("vitae@elitafeugiat.com")),
                new TestEntity (new NomeValueObject("Jamal", "Strickland"), new EmailValueObject("lacus@luctusaliquetodio.co.uk")),
                new TestEntity (new NomeValueObject("Ferdinand", "Castaneda"), new EmailValueObject("pretium.neque.Morbi@Quisqueimperdiet.ca")),
                new TestEntity (new NomeValueObject("Samson", "Porter"), new EmailValueObject("Vivamus@nequeSed.co.uk")),
                new TestEntity (new NomeValueObject("Jasper", "Blackwell"), new EmailValueObject("sollicitudin@arcuvel.ca")),
                new TestEntity (new NomeValueObject("Fuller", "Snyder"), new EmailValueObject("Donec.egestas.Duis@orciconsectetuer.co.uk")),
                new TestEntity (new NomeValueObject("Marshall", "Mullins"), new EmailValueObject("Pellentesque.tincidunt@auctorvelitAliquam.ca")),
                new TestEntity (new NomeValueObject("Devin", "Hampton"), new EmailValueObject("Quisque.nonummy.ipsum@orci.edu")),
                new TestEntity (new NomeValueObject("Tarik", "Oneal"), new EmailValueObject("semper.pretium.neque@malesuada.net")),
                new TestEntity (new NomeValueObject("Jackson", "Marsh"), new EmailValueObject("Integer.eu.lacus@mauris.co.uk")),
                new TestEntity (new NomeValueObject("Baxter", "Sexton"), new EmailValueObject("lacus@Aliquamgravida.co.uk")),
                new TestEntity (new NomeValueObject("Edward", "Hart"), new EmailValueObject("elementum.dui.quis@ut.ca")),
                new TestEntity (new NomeValueObject("Orlando", "Pearson"), new EmailValueObject("arcu.ac.orci@et.net")),
                new TestEntity (new NomeValueObject("Porter", "Terry"), new EmailValueObject("Nam.porttitor@amet.edu")),
                new TestEntity (new NomeValueObject("Jared", "Blankenship"), new EmailValueObject("risus.Donec.nibh@acmattisvelit.org")),
                new TestEntity (new NomeValueObject("Kenneth", "Obrien"), new EmailValueObject("imperdiet.ullamcorper.Duis@montesnascetur.ca")),
                new TestEntity (new NomeValueObject("Quinlan", "Sloan"), new EmailValueObject("tincidunt@felis.co.uk")),
                new TestEntity (new NomeValueObject("Josiah", "Morin"), new EmailValueObject("dolor@Suspendissealiquet.ca")),
                new TestEntity (new NomeValueObject("Orson", "Johnston"), new EmailValueObject("elit.Etiam.laoreet@nequeetnunc.edu")),
                new TestEntity (new NomeValueObject("Zeph", "Pacheco"), new EmailValueObject("nibh.enim@pedeacurna.net")),
                new TestEntity (new NomeValueObject("Daquan", "Hendrix"), new EmailValueObject("arcu.imperdiet@vitaealiquameros.net")),
                new TestEntity (new NomeValueObject("Micah", "Patel"), new EmailValueObject("rutrum.eu.ultrices@Maecenas.edu")),
                new TestEntity (new NomeValueObject("Lane", "Horne"), new EmailValueObject("aliquam@sem.net")),
                new TestEntity (new NomeValueObject("Chandler", "Miles"), new EmailValueObject("nunc.ullamcorper@Donecluctus.net")),
                new TestEntity (new NomeValueObject("Kennan", "Riggs"), new EmailValueObject("Phasellus.dolor@sem.co.uk")),
                new TestEntity (new NomeValueObject("Richard", "Burris"), new EmailValueObject("Phasellus@enimEtiam.com")),
                new TestEntity (new NomeValueObject("Vincent", "Kaufman"), new EmailValueObject("neque@vestibulumneceuismod.org")),
                new TestEntity (new NomeValueObject("Evan", "Pittman"), new EmailValueObject("diam@vel.co.uk")),
                new TestEntity (new NomeValueObject("Harding", "Cochran"), new EmailValueObject("ut.quam@ligulaDonec.com")),
                new TestEntity (new NomeValueObject("Beau", "Herman"), new EmailValueObject("Sed.nunc.est@acfeugiatnon.net")),
                new TestEntity (new NomeValueObject("Beau", "Harmon"), new EmailValueObject("gravida@elit.org")),
                new TestEntity (new NomeValueObject("Ignatius", "Decker"), new EmailValueObject("Donec.porttitor.tellus@risusDonec.ca"))

            };
        }
    }
}