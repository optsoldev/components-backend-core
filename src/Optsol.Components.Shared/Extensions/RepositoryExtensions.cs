using Optsol.Components.Shared.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optsol.Components.Shared.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<IEnumerable<TEntity>> AsyncEnumerableToEnumerable<TEntity>(this IAsyncEnumerable<TEntity> source)
        {
            if (source == null)
                throw new AsyncEnumerableNullException();

            var result = new List<TEntity>();
            await foreach (var entity in source)
            {
                result.Add(entity);
            }

            return result.AsEnumerable();
        }

        public static async Task<IEnumerable<TEntity>> AsyncCursorToAsyncEnumerable<TEntity>(this Task<List<TEntity>> source)
        {
            if (source == null)
                throw new AsyncEnumerableNullException();

            var result = new List<TEntity>();
            foreach (var entity in await source)
            {
                result.Add(entity);
            }

            return result.AsEnumerable();
        }
    }
}
