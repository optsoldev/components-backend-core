using Optsol.Components.Application.Services;
using Optsol.Components.Test.Utils.Entity;

namespace Optsol.Components.Test.Utils.Application
{
    public interface ITestServiceApplication : IBaseServiceApplication<TestEntity, TestViewModel, TestViewModel, InsertTestViewModel, UpdateTestViewModel>
    {
    }
}
