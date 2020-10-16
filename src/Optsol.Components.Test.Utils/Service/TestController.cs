using System;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
using Optsol.Components.Test.Shared.Data;

namespace Optsol.Components.Test.Utils.Service
{
    public class TestController : ApiControllerBase<TestEntity, Guid>, IApiControllerBase<TestEntity, Guid>
    {
        public TestController(ILogger<ApiControllerBase<TestEntity, Guid>> logger, IBaseServiceApplication<TestEntity, Guid> applicationService) 
            : base(logger, applicationService)
        {

        }
    }
}
