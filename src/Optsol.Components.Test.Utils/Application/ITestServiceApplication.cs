using Optsol.Components.Application.Services;
using Optsol.Components.Test.Utils.Data;

namespace Optsol.Components.Test.Utils.Application
{
    public interface ITestServiceApplication : IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>
    {
    }
}
