using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Optsol.Components.Service.Transformers
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
