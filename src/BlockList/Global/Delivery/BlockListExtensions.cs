using YuzuDelivery.Umbraco.BlockList;
using System;

#if NETCOREAPP
using Umbraco.Cms.Core.Models.Blocks;
#else
using Umbraco.Core.Models.Blocks;
#endif


namespace YuzuDelivery.Umbraco.Core
{
    public static class BlockListExtensions
    {

        public static object MapSingleItem(this UmbracoMappingContext context, Type[] types, BlockListModel model, BlockListDataService blockList)
        {
            foreach (var t in types)
            {
                if (!blockList.IsItem(t, model)) continue;
                var item = blockList.CreateItem(model, context);
                item.GetType().GetProperty("_ref")?.SetValue(item, $"par{t.Name}");
                return item;
            }

            return null;
        }

    }
}
