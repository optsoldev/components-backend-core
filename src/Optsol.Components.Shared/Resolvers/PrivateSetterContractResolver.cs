﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Optsol.Components.Shared.Resolvers
{
    public class PrivateSetterContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jProperty = base.CreateProperty(member, memberSerialization);
            if (jProperty.Writable)
                return jProperty;

            jProperty.Writable = member.IsPropertyWithSetter();

            return jProperty;
        }
    }

    internal static class MemberInfoExtensions
    {
        internal static bool IsPropertyWithSetter(this MemberInfo member)
        {
            var property = member as PropertyInfo;

            return property?.SetMethod != null;
        }
    }
}
