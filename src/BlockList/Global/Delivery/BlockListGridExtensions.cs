using YuzuDelivery.Core;
using YuzuDelivery.Umbraco.BlockList;


namespace YuzuDelivery.Umbraco.Core
{
    public static class BlockListGridContextExtensions
    {
        
        public static bool IsInPreview(this UmbracoMappingContext context)
        {
            return context.Items.ContainsKey(BlockListConstants.IsInPreview) && context.Items[BlockListConstants.IsInPreview].ToBool();
        }

        public static vmSub_DataRowsRow ContextRow(this UmbracoMappingContext context)
        {
            return context.Items[BlockListConstants.ContextRow] as vmSub_DataRowsRow;
        }

        public static vmSub_DataGridRow ContextGridRow(this UmbracoMappingContext context)
        {
            return context.Items[BlockListConstants.ContextRow] as vmSub_DataGridRow;
        }

        public static vmSub_DataGridColumn ContextColumn(this UmbracoMappingContext context)
        {
            return context.Items[BlockListConstants.ContextColumn] as vmSub_DataGridColumn;
        }

        public static T RowSettings<T>(this UmbracoMappingContext context)
            where T : class
        {
            return GetSettings<T>(context, BlockListConstants.RowSettings);
        }

        public static T ColumnsSettings<T>(this UmbracoMappingContext context)
            where T : class
        {
            return GetSettings<T>(context, BlockListConstants.ColumnSettings);
        }

        public static T ContentSettings<T>(this UmbracoMappingContext context)
            where T : class
        {
            return GetSettings<T>(context, BlockListConstants.ContentSettings);
        }

        public static T GetSettings<T>(UmbracoMappingContext context, string name)
            where T : class
        {
            return context.Items.ContainsKey(name) ? context.Items[name] as T : null;
        }

    }
}
