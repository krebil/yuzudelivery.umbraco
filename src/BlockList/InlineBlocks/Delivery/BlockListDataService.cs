using System;
using System.Linq;
using System.Collections.Generic;
using YuzuDelivery.Core;
using YuzuDelivery.Umbraco.Core;

#if NETCOREAPP
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
#else
using Umbraco.Core.Models.Blocks;
#endif


namespace YuzuDelivery.Umbraco.BlockList
{
    public class BlockListDataService
    {
        private readonly IMapper mapper;
        private readonly IYuzuConfiguration config;
        private readonly IEnumerable<IBlockListItem> blockListItems;
#if NETCOREAPP
        private readonly IPublishedValueFallback publishedValueFallback;
#endif

        private IEnumerable<Type> viewmodelTypes;

        public BlockListDataService(IMapper mapper, IYuzuConfiguration config, IEnumerable<IBlockListItem> blockListItems
#if NETCOREAPP
            , IPublishedValueFallback publishedValueFallback
#endif
            )
        {
            this.mapper = mapper;
            this.config = config;
            this.blockListItems = blockListItems;
#if NETCOREAPP
            this.publishedValueFallback = publishedValueFallback;
#endif

            viewmodelTypes = config.ViewModels.Where(x => x.Name.StartsWith(YuzuConstants.Configuration.BlockPrefix) || x.Name.StartsWith(YuzuConstants.Configuration.SubPrefix));
        }

        public bool IsItem<V>(BlockListModel model)
        {
            return IsItem(typeof(V), model);
        }

        public bool IsItem(Type type, BlockListModel model)
        {
            var alias = model.FirstOrDefault()?.Content.ContentType.Alias.FirstCharacterToUpper();
            var modelType = config.CMSModels.FirstOrDefault(x => x.Name == alias);

            return modelType == type;
        }

        public V CreateItem<V>(BlockListModel model, UmbracoMappingContext context)
        {
            return ((V)CreateItem(model, context));
        }

        public object CreateItem(BlockListModel model, UmbracoMappingContext context)
        {
            if (model != null && model.Any())
            {
                return ConvertToVm(model.FirstOrDefault(), context);
            }
            else
                return null;
        }

        public List<V> CreateList<V>(BlockListModel model, UmbracoMappingContext context)
        {
            return model.Select(i => ConvertToVm<V>(i, context)).Where(vm => vm != null).ToList();
        }

        public List<object> CreateList(BlockListModel model, UmbracoMappingContext context)
        {
            return model.Select(i => ConvertToVm(i, context)).Where(vm => vm != null).ToList();
        }

        private V ConvertToVm<V>(BlockListItem i, UmbracoMappingContext context)
        {
            return ((V)ConvertToVm(i, context));
        }

        private object ConvertToVm(BlockListItem i, UmbracoMappingContext context)
        {
            var alias = i.Content.ContentType.Alias.FirstCharacterToUpper();
            var modelType = config.CMSModels.FirstOrDefault(x => x.Name == alias);
            var vmType = viewmodelTypes.FirstOrDefault(x => x.Name.EndsWith(alias));

#if NETCOREAPP
            var o = i.Content.ToElement(modelType, publishedValueFallback);
#else
            var o = i.Content.ToElement(modelType);
#endif
            if (o == null) return null;
            {
                var custom = blockListItems.FirstOrDefault(x => x.IsValid(i));

                return custom != null ? custom.CreateVm(i, vmType, context) : mapper.Map(o, modelType, vmType, context.Items);
            }
        }
    }
}