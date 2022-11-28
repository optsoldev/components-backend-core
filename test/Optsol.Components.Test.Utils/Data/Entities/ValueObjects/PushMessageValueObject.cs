using Optsol.Components.Infra.PushNotification.Firebase.Models;
using System.Collections.Generic;

namespace Optsol.Components.Test.Utils.Data.Entities.ValueObjects
{
    public class PushMessageValueObject : PushMessage
    {
        public PushMessageValueObject(string title, string body) 
            : base(title, body)
        {
            
        }
    }

    public class PushMessageDataValueObject : PushMessageData
    {
        public PushMessageDataValueObject(string title, string body, 
            IDictionary<string, string> data)
            : base(title, body, data)
        {

        }
    }
}
