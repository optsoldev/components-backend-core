using System;
using System.Collections.Generic;

namespace Optsol.Components.Infra.PushNotification.Firebase.Models
{
    public class PushMessageData : PushMessage
    {
        public PushMessageData(string title, string body, IDictionary<string, string> data)
            : base(title, body)
        {
            Data = data;
        }

        public IDictionary<string, string> Data { get; private set; }

        public override void Validate()
        {
            var dataIsNull = Data is null;
            if (dataIsNull)
            {
                throw new ArgumentException(nameof(Data));
            }
        }
    }
}
