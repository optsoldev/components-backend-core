using Newtonsoft.Json;
using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Infra.Firebase.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Firebase.Models
{
    public abstract class MessageBase : PushMessage, IClient
    {
        //
        // Summary:
        //     Gets or sets the registration token of the device to which the message should
        //     be sent.
        public string Token { get; set; }
        //
        // Summary:
        //     Gets or sets the name of the FCM topic to which the message should be sent. Topic
        //     names may contain the /topics/ prefix.
        public string Topic { get; set; }
        //
        // Summary:
        //     Gets or sets the FCM condition to which the message should be sent. Must be a
        //     valid condition string such as "'foo' in topics".
        public string Condition { get; set; }
    }
}
