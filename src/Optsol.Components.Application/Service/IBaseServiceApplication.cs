using System;
using System.Threading.Tasks;
using Optsol.Components.Application.DataTransferObject;
using Optsol.Components.Application.Result;
using Optsol.Components.Domain;

namespace Optsol.Components.Application.Service
{
    public interface IBaseServiceApplication<TEntity, TGetByIdDto, TGetAllDto, TInsertData, TUpdateData> : IDisposable
        where TEntity: AggregateRoot
        where TGetByIdDto: BaseDataTransferObject
        where TGetAllDto: BaseDataTransferObject
        where TInsertData: BaseDataTransferObject
        where TUpdateData: BaseDataTransferObject
    {        
        Task<ServiceResult<TGetByIdDto>> GetByIdAsync(Guid id);
        Task<ServiceResultList<TGetAllDto>> GetAllAsync();
        Task<ServiceResult> InsertAsync(TInsertData data);  
        Task<ServiceResult> UpdateAsync(TUpdateData data);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}