using Microsoft.AspNetCore.Mvc;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data.Pagination;
using System;
using System.Threading.Tasks;
using Optsol.Components.Application;

namespace Optsol.Components.Service.Controllers
{
    public interface IApiControllerBase { }

    public interface IApiControllerBase<TEntity, TRequest, TResponse> : IApiControllerBase
        where TEntity : AggregateRoot
        where TRequest : BaseModel
        where TResponse : BaseModel
    {
        Task<IActionResult> GetAllAsync();

        Task<IActionResult> GetByIdAsync([FromRoute] Guid id);

        Task<IActionResult> InsertAsync([FromBody] TRequest data);

        Task<IActionResult> UpdateAsync([FromRoute] Guid id, TRequest data);

        Task<IActionResult> DeleteAsync([FromRoute] Guid id);
    }

    public interface IApiControllerBase<TEntity, TRequest, TResponse, TSearch> :
        IApiControllerBase<TEntity, TRequest, TResponse>
        where TEntity : AggregateRoot
        where TRequest : BaseModel
        where TResponse : BaseModel
        where TSearch : class
    {
        Task<IActionResult> GetAllAsync(SearchRequest<TSearch> search);
    }
}