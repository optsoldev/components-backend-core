using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optsol.Components.Application.Services
{
    public interface IBaseServiceApplication : IDisposable { }

    public interface IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData> : IBaseServiceApplication
        where TEntity : AggregateRoot
        where TGetByIdDto : BaseDataTransferObject
        where TGetAllDto : BaseDataTransferObject
        where TInsertData : BaseDataTransferObject
        where TUpdateData : BaseDataTransferObject
    {
        Task<TGetByIdDto> GetByIdAsync(Guid id);

        Task<IEnumerable<TGetAllDto>> GetAllAsync();

        Task<SearchResult<TGetAllDto>> GetAllAsync<TSearch>(RequestSearch<TSearch> requestSearch) where TSearch : class;

        Task InsertAsync(TInsertData data);

        Task UpdateAsync(TUpdateData data);

        Task DeleteAsync(Guid id);
    }
}