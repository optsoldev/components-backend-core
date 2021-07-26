using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Firebase.Clients
{
    public interface IQueryClient
    {
        Expression<Func<T, bool>> Query<T>() where T : IClient;
    }
}
