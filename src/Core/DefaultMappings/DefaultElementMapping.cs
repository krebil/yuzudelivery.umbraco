using System.Collections.Generic;
using System.Linq;
using YuzuDelivery.Core;

#if NETCOREAPP
using Umbraco.Cms.Core.Models.PublishedContent;
#else
using Umbraco.Core.Models.PublishedContent;
#endif

namespace YuzuDelivery.Umbraco.Core
{

    public class DefaultElementMapping : YuzuMappingConfig
    {
        public DefaultElementMapping()
        {
            ManualMaps.AddTypeReplace<DefaultPublishedElementCollectionConvertor>(false);
            ManualMaps.AddTypeReplace<DefaultPublishedElementCollectionToSingleConvertor>(false);
        }
    }

    public class DefaultPublishedElementCollectionConvertor : IYuzuTypeConvertor<IEnumerable<IPublishedElement>, IEnumerable<object>>
    {
        private readonly IDefaultPublishedElement[] defaultItems;

        public DefaultPublishedElementCollectionConvertor(IDefaultPublishedElement[] defaultItems)
        {
            this.defaultItems = defaultItems;
        }

        public IEnumerable<object> Convert(IEnumerable<IPublishedElement> elements, UmbracoMappingContext context)
        {
            var output = new List<object>();

            var element = elements.FirstOrDefault();
            if (element != null)
            {
                output.AddRange(from i in defaultItems where i.IsValid(element) select i.Apply(element, context));
            }

            return output;
        }
    }

    public class DefaultPublishedElementCollectionToSingleConvertor : IYuzuTypeConvertor<IEnumerable<IPublishedElement>, object>
    {
        private readonly IDefaultPublishedElement[] defaultItems;

        public DefaultPublishedElementCollectionToSingleConvertor(IDefaultPublishedElement[] defaultItems)
        {
            this.defaultItems = defaultItems;
        }

        public object Convert(IEnumerable<IPublishedElement> elements, UmbracoMappingContext context)
        {
            var element = elements.FirstOrDefault();
            return element != null ? (from i in defaultItems where i.IsValid(element) select i.Apply(element, context)).FirstOrDefault() : null;
        }
    }

}
