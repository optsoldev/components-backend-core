using Optsol.Components.Domain.Entities;
using System.Threading.Tasks;

namespace Optsol.Components.Domain.Services.Push
{
    public interface IPushService
    {
        Task SendAsync(PushMessageAggregateRoot entity);
    }
}
