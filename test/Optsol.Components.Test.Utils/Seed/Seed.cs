using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Test.Utils.Data.Contexts;
using Optsol.Components.Test.Utils.Data.Entities;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Test.Utils.Seed
{
    public static class Seed
    {
        public static List<TestEntity> TestEntityList()
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

        public static List<TestDeletableEntity> TestDeletableEntityList()
        {
            return TestEntityList()
                .Select(entity => new TestDeletableEntity(entity.Id, entity.Nome, entity.Email))
                .ToList();
        }

        public static List<TestTenantEntity> TestTenantEntityList(List<TenantEntity> tenants)
        {
            var result = new List<TestTenantEntity>();

            result.AddRange(TestEntityList().Take(94).Select(entity => new TestTenantEntity(entity.Id, tenants[0].Id, entity.Nome, entity.Email)));
            result.AddRange(TestEntityList().Skip(94).Take(1).Select(entity => new TestTenantEntity(entity.Id, tenants[1].Id, entity.Nome, entity.Email)));
            result.AddRange(TestEntityList().Take(95).Take(5).Select(entity => new TestTenantEntity(entity.Id, tenants[2].Id, entity.Nome, entity.Email)));

            return result;
        }

        public static List<TenantEntity> TenantEntityList()
        {
            return new List<TenantEntity>
            {
                new TenantEntity ("http://domain.tenant.one.com", "one"),
                new TenantEntity ("http://domain.tenant.two.com", "two"),
                new TenantEntity ("http://domain.tenant.three.com", "three"),
                new TenantEntity ("http://domain.tenant.four.com", "four")
            };
        }

        public static ServiceProvider CreateTestEntitySeedInContext(this ServiceProvider provider, int take = 1, Action<IEnumerable<TestEntity>> afterInsert = null)
        {
            var context = provider.GetRequiredService<Context>();
            var entities = TestEntityList().Take(take);

            afterInsert?.Invoke(entities);

            context.AddRange(entities);
            context.SaveChanges();

            return provider;
        }

        public static ServiceProvider CreateTestEntitySeedInMongoContext(this ServiceProvider provider, int take = 1, Action<IEnumerable<TestEntity>> afterInsert = null)
        {
            var context = provider.GetRequiredService<MongoContext>();
            var entities = TestEntityList().Take(take);

            var set = context.GetCollection<TestEntity>(typeof(TestEntity).Name);
            set.Database.DropCollection(typeof(TestEntity).Name);

            afterInsert?.Invoke(entities);

            set.InsertMany(entities);

            context.SaveChangesAsync().GetAwaiter().GetResult();

            return provider;
        }

        public static ServiceProvider CreateDeletableTestEntitySeedInContext(this ServiceProvider provider, int take = 1, Action<IEnumerable<TestDeletableEntity>> afterInsert = null)
        {
            var context = provider.GetRequiredService<DeletableContext>();
            var entities = TestDeletableEntityList().Take(take);

            afterInsert?.Invoke(entities);

            context.AddRange(entities);
            context.SaveChanges();

            return provider;
        }

        public static ServiceProvider CreateTenantTestEntitySeedInContext(this ServiceProvider provider, int take = 1, Action<IEnumerable<TestTenantEntity>, IEnumerable<TenantEntity>> afterInsert = null)
        {
            var tenants = TenantEntityList();
            var entities = TestTenantEntityList(tenants).Take(take);

            afterInsert?.Invoke(entities, tenants);

            var tenantContext = provider.GetRequiredService<TenantDbContext>();
            tenantContext.AddRange(tenants);
            tenantContext.SaveChanges();

            var context = provider.GetRequiredService<MultiTenantContext>();
            context.AddRange(entities);
            context.SaveChanges();

            return provider;
        }
    }
}
