using System.Threading.Tasks;

namespace Optsol.Components.Domain.Services.Push
{
    public interface IPushService
    {
        Task SendAsync(PushMessage pushMessage);
    }
}
