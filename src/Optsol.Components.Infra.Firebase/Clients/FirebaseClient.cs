using Optsol.Components.Infra.Firebase.Models.Request;
using Optsol.Components.Infra.Firebase.Models.Response;
using Refit;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Firebase.Clients
{
    public interface FirebaseClient
    {
        [Post("/fcm/send")]
        [Headers("Authorization: Key", "Content-Type: application/json")]
        Task<CloudMessagingResponse> Send<T>([Body] CloudMessagingRequest<T> cloudMessagingRequest);
    }
}
