using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Shared.Data;

namespace Optsol.Components.Test.Utils.Application
{
    public class ServiceApplication : BaseServiceApplication<TestEntity, Guid>, IServiceApplication
    {
        public ServiceApplication(
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IServiceResultFactory serviceResultFactory,
            ILogger<BaseServiceApplication<TestEntity, Guid>> logger,
            IReadRepository<TestEntity, Guid> readRepository, 
            IWriteRepository<TestEntity, Guid> writeRepository) 
            : base(mapper, logger, serviceResultFactory, unitOfWork, readRepository, writeRepository)
        {
            
        }
    }
}
