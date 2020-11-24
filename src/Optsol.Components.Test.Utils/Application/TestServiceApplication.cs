using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.DataTransferObject;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;
using Optsol.Components.Domain;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Shared.Data;

namespace Optsol.Components.Test.Utils.Application
{
    public class TestServiceApplication : BaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>, ITestServiceApplication
    {
        public TestServiceApplication(
            IMapper mapper, 
            ILogger<BaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>> logger, 
            IServiceResultFactory serviceResultFactory, 
            IUnitOfWork unitOfWork, 
            IReadRepository<TestEntity, Guid> readRepository, 
            IWriteRepository<TestEntity, Guid> writeRepository) 
            : base(mapper, logger, serviceResultFactory, unitOfWork, readRepository, writeRepository)
        {
        }
    }
}
