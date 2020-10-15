using System;
using Optsol.Components.Application.Service;
using Optsol.Components.Test.Shared.Data;

namespace Optsol.Components.Test.Utils.Application
{
    public interface IServiceApplication : IBaseServiceApplication<TestEntity, Guid>
    {
    }
}
