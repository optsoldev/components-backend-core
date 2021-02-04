using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Service.Controllers
{
    public interface IApiControllerBase
    {
    }

    public interface IApiControllerBase<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData> : IApiControllerBase
        where TEntity : AggregateRoot
        where TGetByIdDto : BaseDataTransferObject
        where TGetAllDto : BaseDataTransferObject
        where TInsertData : BaseDataTransferObject
        where TUpdateData : BaseDataTransferObject
    {
        Task<IActionResult> GetAllAsync();
        Task<IActionResult> GetByIdAsync(Guid id);
        Task<IActionResult> InsertAsync(TInsertData data);
        Task<IActionResult> UpdateAsync(TUpdateData data);
        Task<IActionResult> DeleteAsync(Guid id);
    }
}