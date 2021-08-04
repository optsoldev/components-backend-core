using Optsol.Components.Infra.Firebase.Models;

namespace Optsol.Components.Test.Utils.Data.Entities.ValueObjects
{
    public class PushMessageValueObject : PushMessage
    {
        public PushMessageValueObject(string title, string body) 
            : base(title, body)
        {
            
        }
    }
}
