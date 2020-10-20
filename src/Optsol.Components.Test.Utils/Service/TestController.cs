using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
using Optsol.Components.Service.Response;
using Optsol.Components.Test.Shared.Data;

namespace Optsol.Components.Test.Utils.Service
{
    [ApiController]
    [Route("api/Test")]
    public class TestController : ApiControllerBase<TestEntity, Guid>, IApiControllerBase<TestEntity, Guid>
    {
        public TestController(ILogger<ApiControllerBase<TestEntity, Guid>> logger, IResponseFactory responseFactory, IBaseServiceApplication<TestEntity, Guid> applicationService) 
            : base(logger, responseFactory, applicationService)
        {

        }
    }
}
