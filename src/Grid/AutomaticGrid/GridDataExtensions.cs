using System.Collections.Generic;

#if NETCOREAPP
using Skybrud.Umbraco.GridData.Models;
#else
using Skybrud.Umbraco.GridData;
#endif

namespace YuzuDelivery.Umbraco.Grid
{
    public static class GridDataExtensions
    {

        public static Dictionary<string, object> AddCalculatedAreaSettings(this Dictionary<string, object> settings, GridRow row, GridArea currentArea, IEnumerable<IAutomaticGridConfig> config)
        {
            foreach (var i in config)
            {
                i.AddAreaSettings(settings, row, currentArea);
            }
            return settings;
        }

        public static Dictionary<string, object> AddCalculatedRowConfig(this Dictionary<string, object> settings, GridRow row, IEnumerable<IAutomaticGridConfig> config)
        {
            foreach (var i in config)
            {
                i.AddRowConfig(settings, row);
            }
            return settings;
        }
        public static Dictionary<string, object> ToDictionary(this GridDictionary values, bool hasValues)
        {
            if(values != null && hasValues)
            {
                return values.JObject.ToObject<Dictionary<string, object>>();
            }
            return new Dictionary<string, object>();
        }

    }
}
