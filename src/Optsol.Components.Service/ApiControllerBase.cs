using Microsoft.AspNetCore.Mvc;
using Optsol.Components.Application;

namespace Optsol.Components.Service
{
    public class ApiControllerBase<TApplicationService> : ControllerBase
        //where TApplicationService: class, IBaseServiceApplication<
    {
        // protected readonly TApplicationService _service { get; set; }

        // public ApiControllerBase(TApplicationService service)
        // {
        //     _service = service;
        // }


    }
}
