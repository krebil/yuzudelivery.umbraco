using System.Collections.Generic;
using System.Linq;
using YuzuDelivery.Umbraco.Core;
using YuzuDelivery.Umbraco.Import;

#if NETCOREAPP
using Umbraco.Extensions;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
#else
using Umbraco.Core.Models.Blocks;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
#endif


namespace YuzuDelivery.Umbraco.BlockList
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class BlockListGridDataService : IBlockListGridDataService
    {
        private readonly string[] sectionAliases;

        private readonly IEnumerable<IGridItem> gridItems;
        private readonly IEnumerable<IGridItemInternal> gridItemsInternal;
#if NETCOREAPP
        private readonly IPublishedValueFallback publishedValueFallback;
#endif

        public BlockListGridDataService(IYuzuDeliveryImportConfiguration importConfig, IEnumerable<IGridItem> gridItems, IEnumerable<IGridItemInternal> gridItemsInternal
#if NETCOREAPP
            , IPublishedValueFallback publishedValueFallback
#endif
            )
        {
            sectionAliases = importConfig.GridRowConfigs.Select(x => x.Name.FirstCharacterToLower()).ToArray();

            this.gridItems = gridItems;
            this.gridItemsInternal = gridItemsInternal;
#if NETCOREAPP
            this.publishedValueFallback = publishedValueFallback;
#endif
        }

        public vmBlock_DataRows CreateRows(BlockListModel grid, UmbracoMappingContext context)
        {
            if (grid != null)
            {
                return new vmBlock_DataRows()
                {
                    Rows = grid.Any() ? grid.Where(x => sectionAliases.Contains(x.Content.ContentType.Alias)).Select(rowBlockList =>
                    {
                        var rowContent = rowBlockList.Content;
                        var rowSettingsVm = GetRowSettingsVm(rowBlockList, context);

                        var columnProperty = rowContent.Properties.FirstOrDefault();
                        var columnContent = rowContent.Value<BlockListModel>(columnProperty.Alias);

                        var row = new vmSub_DataRowsRow()
                        {
                            Config = rowSettingsVm,
                            Items = columnContent?
                                .Select(cell => CreateContentAndConfig(new GridItemData(cell, context.Items)))
                                .Where(x => x != null).ToList()
                        };

                        context.Items[BlockListConstants.ContextRow] = row;

                        return row;

                    }).ToList() : new List<vmSub_DataRowsRow>()
                };
            }

            return null;
        }

        public vmBlock_DataGrid CreateRowsColumns(BlockListModel grid, UmbracoMappingContext context)
        {
            if (grid != null)
            {
                return new vmBlock_DataGrid()
                {
                    Rows = grid.Any() ? grid.Where(x => sectionAliases.Contains(x.Content.ContentType.Alias)).Select(rowBlockList =>
                    {
                        var row = new vmSub_DataGridRow();
                        context.Items[BlockListConstants.ContextRow] = row;

                        var rowContent = rowBlockList.Content;
                        var columns = rowContent.Properties.Where(y => !y.Alias.EndsWith("Settings")).ToList();

                        row.Config = GetRowSettingsVm(rowBlockList, context);

                        row.Columns = columns.Select(columnProperty =>
                        {
                            var column = new vmSub_DataGridColumn()
                            {
                                GridSize = 12 / columns.Count,
                            };

                            context.Items[BlockListConstants.ContextColumn] = column;
#if NETCOREAPP
                            var columnContent = columnProperty.Value<BlockListModel>(publishedValueFallback);
#else
                            var columnContent = columnProperty.Value<BlockListModel>();
#endif
                            GetColumnSettingsVm(rowContent, columnProperty, context);

                            column.Config = GetColumnSettingsVm(rowContent, columnProperty, context);
                            column.Items = columnContent?
                                    .Select(cell => CreateContentAndConfig(new GridItemData(cell, context.Items)))
                                    .Where(x => x != null).ToList();

                            return column;

                        }).ToList();

                        return row;

                    }).ToList() : new List<vmSub_DataGridRow>()
                };
            }
            else
                return null;
        }

        private object GetRowSettingsVm(BlockListItem rowBlockList, UmbracoMappingContext context)
        {
            context.Items.Remove(BlockListConstants.RowSettings);

            var rowConfig = rowBlockList.Settings;

            var vm = CreateVm(rowConfig, context.Items);
            if (vm != null)
                context.Items[BlockListConstants.RowSettings] = vm;

            return vm;
        }

        private object GetColumnSettingsVm(IPublishedElement rowContent, IPublishedProperty columnProperty, UmbracoMappingContext context)
        {
            context.Items.Remove(BlockListConstants.ColumnSettings);

            var columnConfig = rowContent.Value<BlockListModel>(columnProperty.Alias + "Settings")
           .Select(x => x.Content).FirstOrDefault();

            var vm = CreateVm(columnConfig, context.Items);
            if (vm != null)
                context.Items[BlockListConstants.ColumnSettings] = vm;

            return vm;
        }

        public object GetContentSettingsVm(GridItemData data)
        {
            data.ContextItems.Remove(BlockListConstants.ContentSettings);

            var vm = CreateVm(data.Config, data.ContextItems);
            if (vm != null)
                data.ContextItems[BlockListConstants.ContentSettings] = vm;
            return vm;
        }

        public virtual vmBlock_DataGridRowItem CreateContentAndConfig(GridItemData data)
        {
            var settingsVm = GetContentSettingsVm(data);
            var contentVm = CreateVm(data.Content, data.ContextItems);

            return new vmBlock_DataGridRowItem()
            {
                Content = contentVm,
                Config = settingsVm
            };
        }


        public virtual object CreateVm(IPublishedElement model, IDictionary<string, object> context)
        {
            foreach (var i in gridItems)
            {
                if (i.IsValid(model))
                    return i.Apply(model, context);
            }

            foreach (var i in gridItemsInternal)
            {
                if (i.IsValid(model))
                    return i.Apply(model, context);
            }

            return null;
        }


    }

    public class GridContext
    {
        public List<vmSub_DataGridRow> Rows { get; set; }
        
        public vmSub_DataGridRow CurrentRow { get; set; }

        public vmSub_DataGridColumn CurrentColumns {  get; set; }
    }

}