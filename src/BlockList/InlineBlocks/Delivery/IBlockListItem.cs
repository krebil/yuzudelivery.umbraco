using System;
using YuzuDelivery.Umbraco.Core;

#if NETCOREAPP
using Umbraco.Cms.Core.Models.Blocks;
#else
using Umbraco.Core.Models.Blocks;
#endif


namespace YuzuDelivery.Umbraco.BlockList
{
    public interface IBlockListItem
    {
        bool IsValid(BlockListItem i);
        V CreateVm<V>(BlockListItem i, UmbracoMappingContext context);

        object CreateVm(BlockListItem i, Type destinationType, UmbracoMappingContext context);
    }
}