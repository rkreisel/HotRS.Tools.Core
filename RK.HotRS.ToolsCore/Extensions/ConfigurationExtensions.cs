namespace HotRS.Tools.Core.Extensions;

/// <summary>
/// Three extension methods to manage a populated Configuration instance.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public static class ConfigurationExtensions
{
    /// <summary>
    /// Deletes duplicate references to config files
    /// </summary>
    /// <param name="source">A populated IConfiguration instance</param>
    /// <param name="keepWhich">An enum to tell the method which instance of config file to keep. Default = first</param>
    public static void CleanUpJSONConfigs(this IConfiguration source, KeepWhich keepWhich = KeepWhich.First)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        var configProviders = source.GetType().GetProperty("Providers").GetValue(source) as List<IConfigurationProvider>;
        var itemsToSkip = new List<int>();

        for (int ndx = 0; ndx < configProviders.Count; ndx++)
        {
            if (configProviders[ndx] is ChainedConfigurationProvider)
            {
                throw new NotImplementedException(Properties.Resources.CANNOTHANDLECHAINEDCONFIGURATIONS);
            }
            else
            {
                if (configProviders[ndx].GetType().GetProperty("Source") != null)
                {
                    var cfp = (JsonConfigurationProvider)configProviders[ndx];

                    var matches = new List<int>();
                    for (int srchndx = 0; srchndx < configProviders.Count; srchndx++)
                    {
                        if (configProviders[srchndx].GetType().GetProperty("Source") != null)
                        {
                            var tmp = (JsonConfigurationProvider)configProviders[srchndx];
                            if (tmp.Source.Path.Equals(cfp.Source.Path, StringComparison.InvariantCultureIgnoreCase))
                            {
                                matches.Add(srchndx);
                            }
                        }
                    }
                    if (matches.Count > 1)
                    {
                        var itemToKeep = keepWhich == KeepWhich.First ? matches.First() : matches.Last();
                        foreach (var match in matches)
                        {
                            if (match != itemToKeep)
                            {
                                itemsToSkip.Add(match);
                            }
                        }
                    }
                }
            }
        }
        itemsToSkip = itemsToSkip.Distinct().OrderBy(i => i).ToList();
        for (var loopNdx = itemsToSkip.Count - 1; loopNdx >= 0; loopNdx--)
        {
            configProviders.Remove(configProviders[itemsToSkip[loopNdx]]);
        }
    }

    /// <summary>
    /// Deletes instances of config sources that are found in the list of items.
    /// </summary>
    /// <param name="source">A populated IConfiguration instance</param>
    /// <param name="items">A list of items to remove from the config sources.</param>
    public static void CleanUpJSONConfigs(this IConfiguration source, IList<ConfigItem> items)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(items,nameof(items));

        var configProviders = source.GetType().GetProperty("Providers").GetValue(source) as List<IConfigurationProvider>;
        var itemsToSkip = new List<int>();
        for (int ndx = 0; ndx < configProviders.Count; ndx++)
        {
            if (configProviders[ndx].GetType().GetProperty("Source") != null)
            {
                var cfp = (JsonConfigurationProvider)configProviders[ndx];
                //this is one to check
                foreach (var item in items)
                {
                    if (cfp.Source.Path.Equals(item.Path, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //collect a list of matches
                        var matches = new List<int>();
                        for (int srchndx = 0; srchndx < configProviders.Count; srchndx++)
                        {
                            if (configProviders[srchndx].GetType().GetProperty("Source") != null)
                            {
                                var tmp = (JsonConfigurationProvider)configProviders[srchndx];
                                if (tmp.Source.Path.Equals(item.Path, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    matches.Add(srchndx);
                                }
                            }
                        }
                        if (matches.Count > 1)
                        {
                            var itemToKeep = item.KeepWhich == KeepWhich.First ? matches.First() : matches.Last();
                            foreach (var match in matches)
                            {
                                if (match != itemToKeep)
                                {
                                    itemsToSkip.Add(match);
                                }
                            }
                        }
                    }
                }
            }
        }
        itemsToSkip = itemsToSkip.Distinct().ToList();
        for (var loopNdx = itemsToSkip.Count - 1; loopNdx >= 0; loopNdx--)
        {
            configProviders.Remove(configProviders[itemsToSkip[loopNdx]]);
        }
    }
    /// <summary>
    /// If present in the set of configuration sources, user secrets is moved to the end,  
    /// thus overriding any other configurations (including those from the YAML files).
    /// </summary>
    /// <param name="source"></param>
    public static void PreferUserSecrets(this IConfiguration source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        var configProviders = source.GetType().GetProperty("Providers").GetValue(source) as List<IConfigurationProvider>;
        JsonConfigurationProvider cfp = null;
        int secretsIndex = -1;
        for (int ndx = 0; ndx < configProviders.Count; ndx++)
        {
            var item = configProviders[ndx].GetType().GetProperty("Source");
            if (item != null)
            {
                var tmp = (JsonConfigurationProvider)configProviders[ndx];
                var sourceName = tmp.Source.Path;
                if (sourceName.Equals("secrets.json", StringComparison.OrdinalIgnoreCase))
                {
                    cfp = (JsonConfigurationProvider)configProviders[ndx];
                    secretsIndex = ndx;
                    break;
                }
            }
        }
        if (secretsIndex >= 0)
        {
            configProviders.Add(cfp);
            configProviders.Remove(configProviders[secretsIndex]);
        }
    }

    #region Dump
    public static IDictionary<string, string> Dump(this IConfiguration source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        var result = new Dictionary<string, string>();
        var configProviders = source.GetType().GetProperty("Providers").GetValue(source) as List<IConfigurationProvider>;
        for (int ndx = 0; ndx < configProviders.Count; ndx++)
        {
            if (configProviders[ndx] is ChainedConfigurationProvider cp)
            {
                var configType = cp.GetType().GetField("_config", BindingFlags.Instance | BindingFlags.NonPublic);
                var configs = (ConfigurationRoot)configType.GetValue(cp);
                foreach (var config in configs.Providers)
                {
                    if (config is ChainedConfigurationProvider provider)
                    {
                        result.MergeDictionary(HandleChainedProvider(provider));
                    }
                    else
                    {
                        var tmp = config.AsProvider();
                        if (tmp != null)
                        {
                            result.MergeDictionary(ParseProvider(tmp));
                        }
                    }
                }
            }
            else
            {
                if (configProviders[ndx].GetType().GetProperty("Source") != null)
                {
                    var tmp = configProviders[ndx].AsProvider();
                    if (tmp != null)
                    {
                        result.MergeDictionary(ParseProvider(tmp));
                    }
                }
            }
        }
        return result;
    }

    private static IDictionary<string, string> HandleChainedProvider(ChainedConfigurationProvider provider)
    {
        var result = new Dictionary<string, string>();
        var cp = (ChainedConfigurationProvider) provider;
        var configType = cp.GetType().GetField("_config", BindingFlags.Instance | BindingFlags.NonPublic);
        var configs = (ConfigurationRoot)configType.GetValue(cp);
        foreach (var config in configs.Providers)
        {
            if (config is ChainedConfigurationProvider provider1) //handle multiple chained configurations with a resursive call
            {
                result.MergeDictionary(HandleChainedProvider(provider1));
            }
            else
            {
                var tmp = config.AsProvider();
                if (tmp != null)
                {
                    var workingDictionary = ParseProvider(tmp);
                    result.MergeDictionary(workingDictionary);
                }
            }
        }
        return result;
    }

    //[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "There is nothing to do if the type cannot be converted. The calling method must handle a null return.")]
    private static ConfigurationProvider AsProvider(this IConfigurationProvider sourceConfig)
    {
        try
        {                
            return (ConfigurationProvider)sourceConfig;
        }
        catch 
        {
            return null;
        }
    }

    private static IDictionary<string, string> ParseProvider(ConfigurationProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));
        var result = new Dictionary<string, string>();
        var dataType = provider.GetType().GetProperty("Data", BindingFlags.Instance | BindingFlags.NonPublic);
        var dataItems = (IDictionary<string, string>)dataType.GetValue(provider);
        foreach (var dataItem in dataItems)
        {
            if (result.ContainsKey(dataItem.Key))
            {
                _ = result[dataItem.Key] = dataItem.Value;
            }
            else
                result.Add(dataItem.Key, dataItem.Value);
        }
        return result;
    }

    private static void MergeDictionary(this IDictionary<string, string> oldItems, IDictionary<string, string> newItems)
    {
        foreach (var item in newItems)
        {
            if (oldItems.ContainsKey(item.Key))
                oldItems[item.Key] = item.Value;
            else
                oldItems.Add(item.Key, item.Value);
        }
    }
    #endregion
}

[ExcludeFromCodeCoverage]
public sealed class ConfigItem
{
    public string Path { get; set; }
    public KeepWhich KeepWhich { get; set; }
}

public enum KeepWhich
{
    First,
    Last
}

