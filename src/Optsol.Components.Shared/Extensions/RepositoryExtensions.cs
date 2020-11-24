using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Optsol.Components.Shared.Exceptions;

namespace Optsol.Components.Shared.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<IEnumerable<TEntity>> AsyncEnumerableToEnumerable<TEntity>(this IAsyncEnumerable<TEntity> source)
        {
            if(source == null)
                throw new AsyncEnumerableNullException();

            var result = new List<TEntity>();
            await foreach (var entity in source)
            {
                result.Add(entity);
            }

            return result.AsEnumerable();
        }    
    }
}
