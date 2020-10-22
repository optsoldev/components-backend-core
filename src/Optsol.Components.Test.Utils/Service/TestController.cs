using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Service;
using Optsol.Components.Service;
using Optsol.Components.Service.Response;
using Optsol.Components.Test.Shared.Data;
using Optsol.Components.Test.Utils.Application;

namespace Optsol.Components.Test.Utils.Service
{
    [ApiController]
    [Route("api/Test")]
    public class TestController : ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>, 
        IApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>
    {
        public TestController(
            ILogger<ApiControllerBase<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>> logger, 
            IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel> serviceApplication,
            IResponseFactory responseFactory) : base(logger, serviceApplication, responseFactory)
        {
        }
    }
}
