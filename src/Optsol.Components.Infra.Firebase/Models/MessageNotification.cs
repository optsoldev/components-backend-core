using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Firebase.Models
{
    public class MessageNotification : MessageData
    {
        //
        // Summary:
        //     Gets or sets the title of the notification.
        public string Title { get; set; }
        //
        // Summary:
        //     Gets or sets the body of the notification.
        public string Body { get; set; }
        //
        // Summary:
        //     Gets or sets the URL of the image to be displayed in the notification.
        public string ImageUrl { get; set; }
    }
}
