using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;

namespace Optsol.Components.Test.Utils.ViewModels
{
    public class TestServiceApplication : BaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>, ITestServiceApplication
    {
        public TestServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>> logger,
            IUnitOfWork unitOfWork,
            IReadRepository<TestEntity, Guid> readRepository,
            IWriteRepository<TestEntity, Guid> writeRepository,
            NotificationContext notificationContext)
            : base(mapper, logger, unitOfWork, readRepository, writeRepository, notificationContext)
        {
        }
    }
}
