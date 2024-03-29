﻿namespace HotRS.Tools.Core.Utilities.JSON;

/// <summary>
/// Provides a property resolver to hide the vale of the target property.
/// Usage: var safeMsg = JsonConvert.SerializeObject(objectinstance, new JsonSerializerSettings() { ContractResolver = new ObfuscatedPropertyResolver(new[] { "an array names of properties to obfuscate"}) });
/// </summary>
[ExcludeFromCodeCoverage]
public class ObfuscatedPropertyResolver : DefaultContractResolver
{
    private readonly HashSet<string> ignoreProps;
    public ObfuscatedPropertyResolver(IEnumerable<string> propNamesToIgnore)
    {
        this.ignoreProps = new HashSet<string>(propNamesToIgnore);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        if (this.ignoreProps.Contains(property.PropertyName))
        {
            property.Converter = new ObfuscatingConverter();
        }
        return property;
    }
}
