using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class ConnectionStringNullException: Exception
    {  
        public ConnectionStringNullException(ILoggerFactory logger = null)
            : base ("A string de conexão não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(ConnectionStringNullException));
            _logger?.LogCritical(
@$"{nameof(ConnectionStringNullException)}:
""ConnectionStrings"": {{
    ""DefaultConnection"": ""Server=ip-server,1433;Database=InstanceData;..."",
    ""IdentityConnection"":""Server=ip-server,1433;Database=InstanceIdentity;..."",
  }}
"
            );
        }
    }
}
