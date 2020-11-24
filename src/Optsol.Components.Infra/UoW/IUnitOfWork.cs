using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Optsol.Components.Infra.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        Task<bool> CommitAsync();
    }
}
