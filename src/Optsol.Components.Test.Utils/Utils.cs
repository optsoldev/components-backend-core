using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Test.Utils.Data;

namespace Optsol.Components.Test.Utils
{
    public static class Utils
    {
        public static IAsyncEnumerable<TestEntity> GetAllAggregateRootAsyncEnumerable(params TestEntity[] entities)
        {
            return Converter(entities);
        }

        public static IAsyncEnumerable<AggregateRoot> GetAllAggregateRootAsyncEnumerable(params AggregateRoot[] entities)
        {   
            return Converter(entities);
        }

        private static async IAsyncEnumerable<T> Converter<T>(params T[] entities)
        {
            foreach (var entity in entities)
                yield return entity;

            await Task.CompletedTask;
        }
    }
}
