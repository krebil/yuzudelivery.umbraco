﻿using System.Collections.Generic;

#if NETCOREAPP
using Skybrud.Umbraco.GridData.Models;
#else
using Skybrud.Umbraco.GridData;
#endif

namespace YuzuDelivery.Umbraco.Grid
{
    public class GridItemData
    {
        public GridControl Control { get; set; }
        public IDictionary<string, object> ContextItems { get; set; }
        public Dictionary<string, object> RowConfig { get; set; }
        public Dictionary<string, object> ColConfig { get; set; }
    }
}
