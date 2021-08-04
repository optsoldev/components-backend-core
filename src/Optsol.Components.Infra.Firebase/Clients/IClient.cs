using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Firebase.Clients
{
    public interface IClient : IEntity
    {
        public string Token { get; }
    }
}
