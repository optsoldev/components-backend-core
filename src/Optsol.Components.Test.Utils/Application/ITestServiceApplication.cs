using Optsol.Components.Application.DataTransferObject;
using Optsol.Components.Application.Service;
using Optsol.Components.Domain;
using Optsol.Components.Service;
using Optsol.Components.Test.Shared.Data;

namespace Optsol.Components.Test.Utils.Application
{
    public interface ITestServiceApplication : IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>
    {
    }
}
