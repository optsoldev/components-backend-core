using System;
using System.Linq.Expressions;

namespace Optsol.Components.Infra.Firebase.Clients
{
    public interface IQueryClient
    {
        Expression<Func<T, bool>> Query<T>() where T : IClient;
    }
}
