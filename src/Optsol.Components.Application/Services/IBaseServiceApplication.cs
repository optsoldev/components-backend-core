using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Pagination;
using Optsol.Components.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes { get; set; }

        Task<TGetByIdDto> GetByIdAsync(Guid id);

        Task<IEnumerable<TGetByIdDto>> GetByIdsAsync(IEnumerable<Guid> id);

        Task<IEnumerable<TGetAllDto>> GetAllAsync();

        Task<SearchResult<TGetAllDto>> GetAllAsync<TSearch>(ISearchRequest<TSearch> requestSearch) where TSearch : class;

        Task<BaseDataTransferObject> InsertAsync(TInsertData data);

        Task<TCustom> InsertAsync<TCustom>(TInsertData data) where TCustom : BaseDataTransferObject;

        Task<BaseDataTransferObject> UpdateAsync(TUpdateData data);

        Task<TCustom> UpdateAsync<TCustom>(TUpdateData data) where TCustom : BaseDataTransferObject;

        Task DeleteAsync(Guid id);
    }
}