namespace Optsol.Components.Infra.Storage.Queue.Results
{
    public class RetrieveResult<T>
        where T: class
    {
        //public CloudQueueMessage Message { get; set; }
        
        public T Result { get; set; }
    }
}
