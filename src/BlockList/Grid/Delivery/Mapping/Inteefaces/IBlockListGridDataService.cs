using System.Collections.Generic;
using YuzuDelivery.Umbraco.Core;

#if NETCOREAPP
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

#else
using Umbraco.Core.Models.Blocks;
using Umbraco.Core.Models.PublishedContent;
#endif


namespace YuzuDelivery.Umbraco.BlockList
{
    public interface IBlockListGridDataService
    {
        vmBlock_DataGridRowItem CreateContentAndConfig(GridItemData data);
        vmBlock_DataRows CreateRows(BlockListModel grid, UmbracoMappingContext context);
        vmBlock_DataGrid CreateRowsColumns(BlockListModel grid, UmbracoMappingContext context);
        object CreateVm(IPublishedElement model, IDictionary<string, object> context);
        object GetContentSettingsVm(GridItemData data);
    }
}