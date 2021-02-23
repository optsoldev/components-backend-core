using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Services;
using Optsol.Components.Service.Controllers;
using Optsol.Components.Service.Responses;
using Optsol.Components.Test.Utils.Application;
using Optsol.Components.Test.Utils.Data;
using Optsol.Components.Test.Utils.Entity;

namespace Optsol.Components.Test.Utils.Service
{
    [ApiController]
    [Route("api/Test")]
    public class TestController : ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel, TestSearchDto>, 
        IApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel, TestSearchDto>
    {
        public TestController(
            ILogger<ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel, TestSearchDto>> logger, 
            IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel> serviceApplication,
            IResponseFactory responseFactory) : base(logger, serviceApplication, responseFactory)
        {
        }
    }
}
