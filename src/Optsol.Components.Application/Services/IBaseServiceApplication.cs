using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optsol.Components.Application.Services
{
    public interface IBaseServiceApplication { };

    public interface IBaseServiceApplication<TEntity> : IBaseServiceApplication
        where TEntity : AggregateRoot
    {
        Task<TGetByIdDto> GetByIdAsync<TGetByIdDto>(Guid id)
            where TGetByIdDto : BaseDataTransferObject;

        Task<IEnumerable<TGetByIdDto>> GetByIdsAsync<TGetByIdDto>(IEnumerable<Guid> ids)
            where TGetByIdDto : BaseDataTransferObject;

        Task<IEnumerable<TGetAllDto>> GetAllAsync<TGetAllDto>()
            where TGetAllDto : BaseDataTransferObject;

        Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes { get; set; }

        Task<ISearchResult<TGetAllDto>> GetAllAsync<TGetAllDto, TSearch>(ISearchRequest<TSearch> requestSearch)
            where TSearch : class
            where TGetAllDto : BaseDataTransferObject;

        Task<TResponseInsertData> InsertAsync<TInsertData, TResponseInsertData>(TInsertData data)
            where TInsertData : BaseDataTransferObject
            where TResponseInsertData : class;

        Task<TResponseUpdateData> UpdateAsync<TUpdateData, TResponseUpdateData>(TUpdateData data)
            where TUpdateData : BaseDataTransferObject
            where TResponseUpdateData : class;

        Task DeleteAsync(Guid id);
    }
}