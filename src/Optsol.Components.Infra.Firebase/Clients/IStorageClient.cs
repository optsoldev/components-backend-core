namespace Optsol.Components.Infra.Firebase.Clients
{
    public interface IStorageClient
    {
        void AddClient(IClient client);

        IClient GetClient(IQueryClient search);
    }
}
