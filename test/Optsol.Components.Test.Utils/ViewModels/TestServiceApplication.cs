using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Data;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;
using Optsol.Components.Domain.Repositories;

namespace Optsol.Components.Test.Utils.ViewModels
{
    public class TestServiceApplication : BaseServiceApplication<TestEntity>, ITestServiceApplication
    {
        public TestServiceApplication(
            IMapper mapper,
            ILoggerFactory logger,
            IUnitOfWork unitOfWork,
            IReadRepository<TestEntity, Guid> readRepository,
            IWriteRepository<TestEntity, Guid> writeRepository,
            NotificationContext notificationContext)
            : base(mapper, logger, unitOfWork, readRepository, writeRepository, notificationContext)
        {
        }
    }
}
