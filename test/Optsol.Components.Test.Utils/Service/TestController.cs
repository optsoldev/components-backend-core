using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Services;
using Optsol.Components.Service.Controllers;
using Optsol.Components.Service.Responses;
using Optsol.Components.Test.Utils.Entity.Entities;
using Optsol.Components.Test.Utils.ViewModels;

namespace Optsol.Components.Test.Utils.Service
{
    [ApiController]
    [Route("api/Test")]
    public class TestController : ApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto>,
        IApiControllerBase<TestEntity, TestRequestDto, TestResponseDto, TestSearchDto>
    {
        public TestController(
            ILoggerFactory logger,
            IBaseServiceApplication<TestEntity> serviceApplication,
            IResponseFactory responseFactory) : base(logger, serviceApplication, responseFactory)
        {
        }
    }
}
