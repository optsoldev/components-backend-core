using System;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
    }
}
