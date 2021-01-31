using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Services
{
    public interface IServiceApplication : IBaseServiceApplication<AggregateRoot, BaseDataTransferObject, BaseDataTransferObject, BaseDataTransferObject, BaseDataTransferObject>
    {
    }
}
