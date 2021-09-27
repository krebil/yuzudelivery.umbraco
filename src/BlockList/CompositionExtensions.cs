﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using YuzuDelivery.Umbraco.BlockList;

#if NETCOREAPP
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models.Blocks;
#else
using Umbraco.Core.Composing;
#endif

namespace YuzuDelivery.Umbraco.Core
{
    public static class CompositionExtensions
    {
#if NETCOREAPP
        public static void RegisterBlockListStrategies(this IUmbracoBuilder builder, Assembly assembly)
        {
            builder.RegisterAll<IGridItem>(assembly);
        }
#else
        public static void RegisterBlockListStrategies(this Composition composition, Assembly assembly)
        {
            composition.RegisterAll<IGridItem>(assembly);
        }
#endif
    }
}
