using Optsol.Components.Application.Service;
using Optsol.Components.Test.Utils.Data;

namespace Optsol.Components.Test.Utils.Application
{
    public interface ITestServiceApplication : IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>
    {
    }
}
