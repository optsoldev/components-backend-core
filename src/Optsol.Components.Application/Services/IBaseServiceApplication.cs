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
        Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes { get; set; }

        Task<TResponse> GetByIdAsync<TResponse>(Guid id)
            where TResponse : BaseModel;

        Task<IEnumerable<TResponse>> GetByIdsAsync<TResponse>(IEnumerable<Guid> ids)
            where TResponse : BaseModel;

        Task<IEnumerable<TResponse>> GetAllAsync<TResponse>()
            where TResponse : BaseModel;

        Task<ISearchResult<TResponse>> GetAllAsync<TResponse, TSearch>(ISearchRequest<TSearch> requestSearch)
            where TSearch : class
            where TResponse : BaseModel;

        Task<TResponse> InsertAsync<TRequest, TResponse>(TRequest data)
            where TRequest : BaseModel
            where TResponse : BaseModel;

        Task<TResponse> UpdateAsync<TRequest, TResponse>(Guid id, TRequest data)
            where TRequest : BaseModel
            where TResponse : BaseModel;

        Task DeleteAsync(Guid id);
    }
}