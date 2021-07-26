using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Firebase.Models
{
    public class MessageData : MessageBase
    {
        public IDictionary<string, string> Data { get; set; }
    }
}
